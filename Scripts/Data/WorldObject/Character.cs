using System;
using System.Collections;
using UnityEngine.AI;
using UnityEngine;

public interface ITargetHandler
{
    public Define.BaseAnim BaseAnimType { get; set; }

    public Vector3 Position { get; }
    public Vector3 Rotation { get; }
    //public Quaternion   transRotation { get; set; }

    public void OnEnableTargetOutline();
    public void OnDisableTargetOutline();

    public void OnDamage(int pValue);
}

// HPBar 기능 이동 필요
public class Character : BaseWorldObject, ITargetHandler
{
    private Table.CharacterTable.Data characterData = null;
    //int characterID = 0;

    #region Actor

    //테이블(enum)
    //액터 종류(임시) : (Player),  Enemy, Boss, NPC
    string animatorControllerType = string.Empty;
    protected struct Stat //액터마다 Stat구성 다름
    {
        public int maxHp;
        public int currentHp;
        public int attack;
        public int defense;
    }
    private Stat statData;
    protected Stat StatData { get { return statData; } }
    
    int weaponID = 0;

    ////스크립트(animatorControllerType에 따라 결정) //스크립트오브젝터블고려
    protected string baseLayerName = "Base Layer"; 
    protected string upperLayerName = "Upper Layer";
    private Define.BaseAnim baseAnimType = Define.BaseAnim.Idle;
    public Define.BaseAnim BaseAnimType
    {
        get { return baseAnimType; }
        set
        {
            if (value == baseAnimType)
            {
                Debug.Log("Failed : ");
                return;
            }

            baseAnimType = value;

            switch (baseAnimType)
            {
                case Define.BaseAnim.Die:
                    {
                        if (isDying == false)
                            SetDie();
                    }
                    break;
                case Define.BaseAnim.Idle:
                    {
                        SetIdle();
                    }
                    break;
                case Define.BaseAnim.Run:
                    {
                        SetRun();
                    }
                    break;
            }
        }
    }

    private Define.UpperAnim upperAnimType = Define.UpperAnim.None;
    public Define.UpperAnim UpperAnimType
    {
        get { return upperAnimType; }
        set
        {
            if (value == upperAnimType || value == Define.UpperAnim.None)
            {
                Debug.Log($"Failed : ");
                return;
            }

            upperAnimType = value;

            // Upper
            switch (upperAnimType)
            {
                case Define.UpperAnim.Ready:
                    {
                        SetReady();
                    }
                    break;
                case Define.UpperAnim.Attack:
                    {
                        if (isAttacking == false)
                        {
                            SetAttack();
                        }
                    }
                    break;
            }
        }
    }

    string baseIdleAnimationClip = "";
    string baseRunAnimationClip = "";
    string upperReadyAnimationClip = "Humanoid_Ready_Gun";
    string upperAttackAnimationClip = "Humanoid_Attack_Gun";
    
    public int attackSpeed; 
    public float runSpeed;
    protected float hitTime = 0.4f; //플레이어
    //배틀액터 //Enemy - hitTime = 0.55f; 
    ////플레이어엑터만 무기에 따라 바뀜
    //string upperReadyAnimationClip1 = "Humanoid_Ready_Hand";
    //string upperAttackAnimationClip1 = "Humanoid_Attack_Hand";


    // 상태값(Test)
    float startTime = 1f;
    float andTime = 0f;
    float fadeOutTime = 0f;
    bool isAttacking = false;
    bool isHit = false;
    bool isDying = false;

    Collider collider = null;
    protected NavMeshAgent navMeshAgent { get; private set; } = null;
    //protected Animator_Util animator { get; private set; } = null;
    protected Shader_Util shader { get; private set; } = null;

    //protected Weapon weapon { get; private set; } = null;
    //HPBarUI hpBarUI = null;

    #endregion Actor


    void Awake()
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

        navMeshAgent = gameObject.GetOrAddComponent<NavMeshAgent>();
        navMeshAgent.radius = 0.3f;
        //animator = gameObject.GetOrAddComponent<Animator_Util>();
        shader = gameObject.GetOrAddComponent<Shader_Util>();
    }

    protected virtual void Update()
    {
        if (BaseAnimType == Define.BaseAnim.Die)
        {
            UpdateDying();
        }
        else if (UpperAnimType == Define.UpperAnim.Attack)
        {
            UpdateAttacking();
        }
    }

    void FixedUpdate()
    {
        //hpBarUI.SetUpdateHPBar(transform, stat_.currentHp, stat_.maxHp);
    }

    public override void SetWorldObject(Define.WorldObject pWorldObjType, int pWorldObjID, Action<GameObject> pDespawnAction)
    {
        base.SetWorldObject(pWorldObjType, pWorldObjID, pDespawnAction);

        characterData = TableManager.Instance.CreateOrGetBaseTable<Table.CharacterTable>().GetTableData(pWorldObjID);
    }

    public override void Spawn(Vector3 pPos, Vector3 pRot)
    {
        base.Spawn(pPos, pRot);

        //
        shader.SetMateriasColorAlpha(1f, false);

        
        //animator.SetAnimatorController(characterData.animatorController, characterData.animatorAvatar);
        //animator.SwapAnimationClip(upperReadyAnimationClip1, upperReadyAnimationClip2);
        //animator.SwapAnimationClip(upperAttackAnimationClip1, upperAttackAnimationClip2);

        // Stat
        {
            Table.StatTable.Data statData = TableManager.Instance.CreateOrGetBaseTable<Table.StatTable>().GetTableData(1); //임시

            this.statData.maxHp = statData.maxHp;
            this.statData.currentHp = statData.maxHp;
            this.statData.attack = statData.attack;
            this.statData.defense = statData.defense;
            
            //this.runSpeed = statData.moveSpeed;
            //this.statData.maxExp = statData.maxExp;
        }

        //if (hpBarUI == null)
        //    CreateHPBarUI();

        //hpBarUI.Open();

        // State
        baseAnimType = Define.BaseAnim.Idle;
        upperAnimType = Define.UpperAnim.Ready;

        //// Weapon
        //if (characterData.weaponID != 0)
        //{
        //    if (weapon == null)
        //        CreateWeapon(characterData.weaponID);

        //    weapon.InitWeapon();
        //    SetWeaponPos(Vector3.zero, Quaternion.identity, false);
        //}

        isAttacking = false;
        isHit = false;
        isDying = false;
    }

    protected override void Despawn()
    {
        base.Despawn();

        SetMateriasColorAlpha(1f);

        baseAnimType = Define.BaseAnim.Die;
        upperAnimType = Define.UpperAnim.None;
    }

    protected virtual void SetDie()
    {
        //hpBarUI.Close(); //임시

        navMeshAgent.isStopped = true;
        navMeshAgent.velocity = Vector3.zero;
        //animator.SetRebind();

        //
        string animName = BaseAnimType.ToString();
        //animator.SetCrossFade(baseLayerName, animName, 0.03f, 1f);

        startTime = 1f;
        andTime = 0f;
        fadeOutTime = 0f;
        isDying = true;
    }

    protected virtual void SetIdle()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.velocity = Vector3.zero;

        string animName = BaseAnimType.ToString();
        //animator.SetCrossFade(baseLayerName, animName, 0.03f, 1f);
    }

    protected virtual void SetRun()
    {
        navMeshAgent.isStopped = false;
        //navMeshAgent.speed = stat_.moveSpeed;

        string animName = BaseAnimType.ToString();
        //animator.SetCrossFade(baseLayerName, animName, 0.03f, 1f);
    }

    protected virtual void SetReady()
    {
        isAttacking = false;
        //// Weapon
        //Vector3 pos = new Vector3(-0f, 0f, -0.3f);
        //Quaternion rot = Quaternion.Euler(0f, 0f, 180f);
        //SetWeaponPos(pos, rot, true);

        string animName = UpperAnimType.ToString();
        //animator.SetCrossFade(upperLayerName, animName, 0.1f, 1f);
    }

    protected virtual void SetAttack()
    {
        isAttacking = true;
        isHit = false;

        //// Weapon
        //Vector3 pos = new Vector3(0.14f, 0.14f, -0.35f);
        //Quaternion rot = Quaternion.Euler(19f, -37f, 166f);
        //SetWeaponPos(pos, rot, true);

        string animName = UpperAnimType.ToString();
        //animator.SetCrossFade(upperLayerName, animName, 0f, 1f);
    }

    //protected abstract void OnHitEvent();

    void UpdateAttacking()
    {
        if (isAttacking == false)
            return;

        //int truncValue = (int)animator.GetAnimatorStateNormalizedTime(upperLayerName);
        //float animTime = animator.GetAnimatorStateNormalizedTime(upperLayerName) - truncValue;

        //if (isHit && animTime < hitTime)
        //{
        //    isHit = false; // Reset
        //}
        //else if (isHit == false && animTime >= hitTime)
        //{
        //    isHit = true;
        //    OnHitEvent();
        //}

        //if (animTime >= 1.0f)
        //{
        //    isAttacking = false;
        //    isHit = false;
        //}
    }

    void UpdateDying()
    {
        //if (isDying == false)
        //    return;

        //fadeOutTime = Mathf.Lerp(startTime, andTime, animator.GetAnimatorStateNormalizedTime(baseLayerName));
        //SetMateriasColorAlpha(fadeOutTime);

        //if (animator.GetAnimatorStateNormalizedTime(baseLayerName) >= 1.0f)
        //{
        //    SetMateriasColorAlpha(0f);
        //    animator.SetRebind();

        //    SetDespawn();

        //    startTime = 1f;
        //    andTime = 0f;
        //    fadeOutTime = 0f;
        //    isDying = false;
        //}
    }

    public void OnDamage(int pValue)
    {
        //int damage = Mathf.Max(0, pValue - stat_.defense);
        //int hp = stat_.currentHp - pValue;
        //SetHP(hp);

        ////
        //if (stat_.currentHp <= 0)
        //{
        //    BaseStateType = Define.BaseState.Die;
        //}
    }

    public void OnEnableTargetOutline()
    {
        shader.OnEnableOutline();
    }

    public void OnDisableTargetOutline()
    {
        shader.OnDisableOutline();
    }

    //[Obsolete("레벨업 스탯 구현 필요")]
    //protected void SetStat()
    //{
    //    Table.StatTable.Data statData = GlobalScene.TableMng.CreateOrGetBaseTable<Table.StatTable>().GetTableData(1); //임시

    //    stat_.maxHp = statData.maxHp;
    //    stat_.currentHp = statData.maxHp;
    //    stat_.attack = statData.attack;
    //    stat_.defense = statData.defense;
    //    stat_.moveSpeed = statData.moveSpeed;
    //    stat_.maxExp = statData.maxExp;
    //}

    protected void SetHP(int pHpValue)
    {
        if (pHpValue < 0)
            statData.currentHp = 0;
        else if (pHpValue > statData.maxHp)
            statData.currentHp = statData.maxHp;
        else
            statData.currentHp = pHpValue;
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

    void SetWeaponPos(Vector3 pPos, Quaternion pRot, bool pEnable)
    {
        //if (weapon == null)
        //    return;

        //weapon.SetPosition(pPos);
        //weapon.SetRotation(pRot);
        //weapon.SetEnable(pEnable);
    }

    #region Load

    void CreateWeapon(int pWeaponID)
    {
        //if (pWeaponID == 0)
        //    return;

        //DestroyWeapon();

        //Transform[] children = GetComponentsInChildren<Transform>();
        //Transform weponTrans = Array.Find(children, x => x.name == "Hand_L");
        //if (weponTrans == null)
        //{
        //    Debug.LogWarning("Failed : ");
        //    return;
        //}

        //string tempPath = $"Prefabs/Weapon/SM_Wep_Watergun_02";
        //UnityEngine.GameObject go = GlobalScene.ResourceMng.Instantiate(tempPath, weponTrans);
        //if (go == null)
        //{
        //    Debug.LogWarning("Failed : ");
        //    return;
        //}

        //weapon = go.GetOrAddComponent<Weapon>();
    }

    void DestroyWeapon()
    {
        //if (weapon != null)
        //{
        //    GlobalScene.ResourceMng.DestroyGameObject(weapon.gameObject);
        //    weapon = null;
        //}
    }

    void CreateHPBarUI()
    {
        //DestroyHPBarUI();
        //hpBarUI = GlobalScene.UIMng.CreateWorldSpaceUI<HPBarUI>(transform);
        //hpBarUI.Load();
    }

    void DestroyHPBarUI()
    {
        //if (hpBarUI != null)
        //{
        //    GlobalScene.ResourceMng.DestroyGameObject(hpBarUI.gameObject);
        //    hpBarUI = null;
        //}
    }

    #endregion Load
}
