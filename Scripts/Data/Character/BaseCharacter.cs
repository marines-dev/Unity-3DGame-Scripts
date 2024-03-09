using System;
using System.Collections;
using UnityEngine.AI;
using UnityEngine;

public interface ITargetHandler
{
    public Define.BaseState baseStateType { get; }

    public Vector3      transPosition { get; set; }
    public Quaternion   transRotation { get; set; }

    public void OnEnableTargetOutline();
    public void OnDisableTargetOutline();

    public void OnDamage(int pValue);
}

// HPBar 기능 이동 필요
public abstract class BaseCharacter : MonoBehaviour, ITargetHandler
{
    // Character
    int characterID_ = 0;
    protected int characterID
    {
        get
        {
            if (characterID_ <= 0)
            {
                Debug.LogWarning("Failed : ");
            }
            return characterID_;
        }
        private set
        {
            if (value <= 0)
            {
                Debug.LogWarning("Failed : ");
                return;
            }
            characterID_ = value;
        }
    }

    // Stat
    protected struct Stat
    {
        public int maxHp;
        public int currentHp;
        public int attack;
        public int defense;
        public float moveSpeed;
        public int maxExp;
        //public int exp = 0;
    }
    protected Stat stat_;
    protected Stat stat { get { return stat_; } }

    // State
    public Define.BaseState     baseStateType { get; private set; }     = Define.BaseState.None;
    protected Define.UpperState upperStateType { get; private set; }    = Define.UpperState.None;
    bool isBaseStateProcess = false;
    bool isUpperStateProcess = false;

    // Transform
    public Vector3 transPosition
    {
        get
        {
            Vector3 pos = transform.localPosition;
            return pos;
        }
        set
        {
            if (value == null)
                value = Vector3.zero;

            transform.localPosition = value;
        }
    }
    public Quaternion transRotation
    {
        get
        {
            Quaternion rot = transform.localRotation;
            return rot;
        }
        set
        {
            if (value == null)
                value = Quaternion.identity;

            transform.localRotation = value;
        }
    }

    // Test
    protected string baseLayerName      = "Base Layer";
    protected string upperLayerName     = "Upper Layer";
    string upperReadyAnimationClip1     = "Humanoid_Ready_Hand";
    string upperReadyAnimationClip2     = "Humanoid_Ready_Gun";
    string upperAttackAnimationClip1    = "Humanoid_Attack_Hand";
    string upperAttackAnimationClip2    = "Humanoid_Attack_Gun";
    protected float hitTime = 0.5f;

    NavMeshAgent        navMeshAgent_ = null;
    public NavMeshAgent navMeshAgent
    {
        get
        {
            if (navMeshAgent_ == null)
            {
                navMeshAgent_ = transform.gameObject.GetOrAddComponent<NavMeshAgent>();
            }
            return navMeshAgent_;
        }
    }
    Animator_Util           animatorOverride_ = null;
    protected Animator_Util animatorOverride
    {
        get
        {
            if (animatorOverride_ == null)
            {
                animatorOverride_ = gameObject.GetOrAddComponent<Animator_Util>();
            }
            return animatorOverride_;
        }
    }
    Shader_Util             shader_ = null;
    protected Shader_Util   shader
    {
        get
        {
            if (shader_ == null)
            {
                shader_ = gameObject.GetOrAddComponent<Shader_Util>();
            }
            return shader_;
        }
    }
    protected Weapon weapon { get; private set; } = null;
    HPBarUI     hPBarUI     = null;
    Collider    collider    = null;

    [Obsolete("임시")]IEnumerator fixedUpdateHPBarCoroutine = null;
    IEnumerator baseStateProcessCoroutine   = null;
    IEnumerator baseStateProcessRoutine     = null;
    IEnumerator upperStateProcessCoroutine  = null;
    IEnumerator upperStateProcessRoutine    = null;

    void Start()
    {
        collider = gameObject.GetComponentInChildren<CapsuleCollider>();
        if (collider == null)
        {
            Renderer renderer = GetComponentInChildren<Renderer>();
            if (renderer != null)
            {
                collider = renderer.gameObject.GetOrAddComponent<MeshCollider>();
            }
        }

        navMeshAgent_ = gameObject.GetOrAddComponent<NavMeshAgent>();
        navMeshAgent_.radius = 0.3f;
        animatorOverride_ = gameObject.GetOrAddComponent<Animator_Util>();
        shader_ = gameObject.GetOrAddComponent<Shader_Util>();
    }

    //void OnDestroy()
    //{
    //}

    protected abstract void InitSpawnCharacter();// 스폰 시 Base.SpawnCharacter() 함수에서 캐릭터를 초기화합니다.
    //public abstract void ResetCharacter();
    protected abstract IEnumerator BaseDieStateProcecssCoroutine();
    protected abstract IEnumerator BaseIdleStateProcecssCoroutine();
    protected abstract IEnumerator BaseRunStateProcecssCoroutine();
    protected abstract IEnumerator UpperReadyStateProcecssCoroutine();
    protected abstract IEnumerator UpperAttackStateProcecssCoroutine();
    protected abstract void OnHitEvent();
    //protected abstract void SetDead();
    //protected abstract void SetIdle();
    //protected abstract void SetRun();
    //protected abstract void SetSkill();
    //protected abstract void SetRespawn();

    [Obsolete("테스트 중")]
    public void SpawnCharacter(int pCharacterID)
    {
        //
        characterID = pCharacterID;

        Table.Character.Data characterData = GlobalScene.TableMng.CreateOrGetBaseTable<Table.Character>().GetTableData(characterID);
        SetStat(characterData.level);

        transPosition = Vector3.zero;
        transRotation = Quaternion.identity;

        shader.SetMateriasColorAlpha(1f, false);
        animatorOverride.SetAnimatorController(characterData.animatorController, characterData.animatorAvatar);
        animatorOverride.SwapAnimationClip(upperReadyAnimationClip1,    upperReadyAnimationClip2);
        animatorOverride.SwapAnimationClip(upperAttackAnimationClip1,   upperAttackAnimationClip2);

        // Weapon
        if (characterData.weaponID != 0)
        {
            if (weapon == null)
                CreateWeapon(characterData.weaponID);

            weapon.InitWeapon();
            SetWeaponPos(Vector3.zero, Quaternion.identity, false);
        }

        // Stat
        if (hPBarUI == null)
            CreateHPBarUI();

        // State
        baseStateType       = Define.BaseState.None; //초기화
        upperStateType      = Define.UpperState.None;
        isBaseStateProcess  = false;
        isUpperStateProcess = false;


        // 자식 클래스에서 초기화할 함수입니다.
        InitSpawnCharacter(); // 임시(순서)

        //
        SetBaseStateType(Define.BaseState.Idle);
        SetUpperStateType(Define.UpperState.Ready);
        SetHPBarUI();
    }

    protected void DespawnCharacter()
    {
        //GlobalScene.SpawnMng.DespawnCharacter(gameObject);
    }

    void SetWeaponPos(Vector3 pPos, Quaternion pRot, bool pEnable)
    {
        if (weapon == null)
            return;

        weapon.SetPosition(pPos);
        weapon.SetRotation(pRot);
        weapon.SetEnable(pEnable);
    }

    void SetHPBarUI()
    {
        hPBarUI.Open();
        FixedUpdateHPBarProcess();
    }

    [Obsolete("레벨업 스탯 구현 필요")]
    protected void SetStat(int pLevel)
    {
        Table.Character.Data    characterData = GlobalScene.TableMng.CreateOrGetBaseTable<Table.Character>().GetTableData(characterID);
        Table.Stat.Data         statData      = GlobalScene.TableMng.CreateOrGetBaseTable<Table.Stat>().GetTableData(characterData.statID);

        stat_.maxHp     = statData.maxHp;
        stat_.currentHp = statData.maxHp;
        stat_.attack    = statData.attack;
        stat_.defense   = statData.defense;
        stat_.moveSpeed = statData.moveSpeed;
        stat_.maxExp    = statData.maxExp;
    }

    protected void SetHP(int pHpValue)
    {
        if (pHpValue < 0)
            stat_.currentHp = 0;
        else if (pHpValue > stat_.maxHp)
            stat_.currentHp = stat_.maxHp;
        else
            stat_.currentHp = pHpValue;
    }

    protected void SetMateriasColorAlpha(float pAlpha)
    {
        shader.SetMateriasColorAlpha(pAlpha, true);
    }

    protected bool CalculateNavMeshPath(Vector3 _randPos)
    {
        NavMeshPath path = new NavMeshPath();
        return navMeshAgent.CalculatePath(_randPos, path);
    }

    protected void SetBaseStateType(Define.BaseState pBaseStateType)
    {
        if (baseStateType == Define.BaseState.Die)
        {
            Debug.Log("");
            return;
        }

        if (pBaseStateType == Define.BaseState.None)
        {
            Debug.Log("");
            return;
        }

        if (pBaseStateType == baseStateType)
        {
            Debug.Log("");
            return;
        }

        //
        baseStateType = pBaseStateType;
        //animatorOverride.speed = 1; //임시

        SetBaseStateProcess();
    }

    protected void SetUpperStateType(Define.UpperState pUpperStateType)
    {
        if (baseStateType == Define.BaseState.Die)
        {
            Debug.Log("");
            return;
        }

        if (pUpperStateType == Define.UpperState.None)
        {
            Debug.Log("");
            return;
        }

        if (pUpperStateType == upperStateType)
        {
            Debug.Log("");
            return;
        }

        //
        upperStateType = pUpperStateType;
        //animatorOverride.speed = 1; //임시

        SetUpperStateProcess();
    }

    public void OnDamage(int pValue)
    {
        int damage  = Mathf.Max(0, pValue - stat_.defense);
        int hp      = stat_.currentHp - pValue;
        SetHP(hp);

        //
        if (stat_.currentHp <= 0)
        {
            Debug.Log($"{gameObject.name} : Dead");
            ClearFixedUpdateHPBar();

            //
            SetUpperStateType(Define.UpperState.Ready);
            SetBaseStateType(Define.BaseState.Die);
        }
    }

    public void OnEnableTargetOutline()
    {
        shader.OnEnableOutline();
    }

    public void OnDisableTargetOutline()
    {
        shader.OnDisableOutline();
    }

    void FixedUpdateHPBarProcess()
    {
        ClearFixedUpdateHPBarPorcess();

        fixedUpdateHPBarCoroutine = FixedUpdateHPBarCoroutine();
        StartCoroutine(fixedUpdateHPBarCoroutine);
    }

    void ClearFixedUpdateHPBar()
    {
        //
        ClearFixedUpdateHPBarPorcess();
        if (hPBarUI != null)
            hPBarUI.Close();
    }

    void ClearFixedUpdateHPBarPorcess()
    {
        if (fixedUpdateHPBarCoroutine != null)
        {
            StopCoroutine(fixedUpdateHPBarCoroutine);
            fixedUpdateHPBarCoroutine = null;
        }
    }

    IEnumerator FixedUpdateHPBarCoroutine()
    {
        if (hPBarUI == null)
        {
            Debug.LogWarning("Failed : ");
            yield break;
        }
        yield return null;

        hPBarUI.SetHPBar(transform, stat_.currentHp, stat_.maxHp);

        while (baseStateType != Define.BaseState.Die)
        {
            hPBarUI.SetHPBar(transform, stat_.currentHp, stat_.maxHp);

            yield return new WaitForFixedUpdate();
        }

        hPBarUI.SetHPBar(transform, 0, stat_.maxHp);
    }

    void SetBaseStateProcess()
    {
        ClearBaseStateProcess();

        switch (baseStateType)
        {
            case Define.BaseState.Die:
                {
                    baseStateProcessRoutine = BaseDieStateProcecssCoroutine();
                }
                break;
            case Define.BaseState.Idle:
                {
                    //// WeaponPos
                    //Vector3 pos = new Vector3(-0.0319f, 0.0585f, 0.1015f);
                    //Quaternion rot = Quaternion.Euler(0f, 0f, 0f);
                    //SetWeaponPos(pos, rot, true);

                    baseStateProcessRoutine = BaseIdleStateProcecssCoroutine();
                }
                break;
            case Define.BaseState.Run:
                {
                    //// WeaponPos
                    //Vector3 pos = new Vector3(-0.139f, 0.025f, 0.043f);
                    //Quaternion rot = Quaternion.Euler(17.513f, -57.14f, -4.325f);
                    //SetWeaponPos(pos, rot, true);

                    baseStateProcessRoutine = BaseRunStateProcecssCoroutine();
                }
                break;
        }

        baseStateProcessCoroutine = BaseStateProcessCoroutine();
        StartCoroutine(baseStateProcessCoroutine);
    }

    void ClearBaseStateProcess()
    {
        // Coroutine
        if (baseStateProcessCoroutine != null)
        {
            StopCoroutine(baseStateProcessCoroutine);
            baseStateProcessCoroutine = null;
        }

        // Routine
        if (baseStateProcessRoutine != null)
        {
            StopCoroutine(baseStateProcessRoutine);
            baseStateProcessRoutine = null;
        }

        isBaseStateProcess = false;
    }

    IEnumerator BaseStateProcessCoroutine()
    {
        //
        isBaseStateProcess = true;
        yield return null;

        if (baseStateProcessRoutine != null)
            yield return baseStateProcessRoutine;

        isBaseStateProcess = false;
    }

    void SetUpperStateProcess()
    {
        ClearUpperStateProcess();

        switch (upperStateType)
        {
            case Define.UpperState.Ready:
                {
                    // WeaponPos
                    Vector3 pos = new Vector3(-0f, 0f, -0.3f);
                    Quaternion rot = Quaternion.Euler(0f, 0f, 180f);
                    SetWeaponPos(pos, rot, true);

                    upperStateProcessRoutine = UpperReadyStateProcecssCoroutine();
                }
                break;
            case Define.UpperState.Attack:
                {
                    // WeaponPos
                    Vector3 pos = new Vector3(0.14f, 0.14f, -0.35f);
                    Quaternion rot = Quaternion.Euler(19f, -37f, 166f);
                    SetWeaponPos(pos, rot, true);

                    upperStateProcessRoutine = UpperAttackStateProcecssCoroutine();
                }
                break;
        }

        upperStateProcessCoroutine = UpperStateProcessCoroutine();
        StartCoroutine(upperStateProcessCoroutine);
    }

    void ClearUpperStateProcess()
    {
        // Coroutine
        if (upperStateProcessCoroutine != null)
        {
            StopCoroutine(upperStateProcessCoroutine);
            upperStateProcessCoroutine = null;
        }

        // Routine
        if (upperStateProcessRoutine != null)
        {
            StopCoroutine(upperStateProcessRoutine);
            upperStateProcessRoutine = null;
        }

        isUpperStateProcess = false;
    }

    IEnumerator UpperStateProcessCoroutine()
    {
        //
        isUpperStateProcess = true;
        yield return null;

        if (upperStateProcessRoutine != null)
            yield return upperStateProcessRoutine;

        isUpperStateProcess = false;
    }

    #region Load

    void CreateWeapon(int pWeaponID)
    {
        if (pWeaponID == 0)
            return;

        DestroyWeapon();

        Transform[] children = GetComponentsInChildren<Transform>();
        Transform weponTrans = Array.Find(children, x => x.name == "Hand_L");
        if (weponTrans == null)
        {
            Debug.LogWarning("Failed : ");
            return;
        }

        string tempPath = $"Prefabs/Weapon/SM_Wep_Watergun_02";
        GameObject go = GlobalScene.ResourceMng.Instantiate(tempPath, weponTrans);
        if(go == null)
        {
            Debug.LogWarning("Failed : ");
            return;
        }

        weapon = go.GetOrAddComponent<Weapon>();
    }

    void DestroyWeapon()
    {
        if (weapon != null)
        {
            GlobalScene.ResourceMng.DestroyGameObject(weapon.gameObject);
            weapon = null;
        }
    }

    void CreateHPBarUI()
    {
        DestroyHPBarUI();

        hPBarUI = GlobalScene.UIMng.CreateBaseSpaceUI<HPBarUI>(transform);
        hPBarUI.Open();
    }

    void DestroyHPBarUI()
    {
        if (hPBarUI != null)
        {
            GlobalScene.ResourceMng.DestroyGameObject(hPBarUI.gameObject);
            hPBarUI = null;
        }
    }

    #endregion Load
}
