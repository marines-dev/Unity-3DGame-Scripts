using System;
using UnityEngine;
using UnityEngine.AI;

public interface ITargetHandler_Temp
{
    public SurvivalState SurvivalStateType { get; }

    public Vector3 Position { get; }
    public Vector3 Rotation { get; }
    //public Quaternion   transRotation { get; set; }

    public void OnEnableTargetOutline();
    public void OnDisableTargetOutline();

    public void OnDamage(int pValue);
}

public enum SurvivalState
{
    Dead,
    Alive,
}

public class Character : BaseWorldObject, ITargetHandler_Temp
{
    private Table.CharacterTable.Data characterData = null;
    //int characterID = 0;

    protected struct ActorStat //액터마다 Stat구성 다름
    {
        public int maxHp;
        public int currentHp;
        public int attack;
        public int defense;
        public float runSpeed;
    }
    private ActorStat stat;
    protected ActorStat Stat { get { return stat; } }

    public SurvivalState SurvivalStateType { get; private set; }

    private Define.BaseAnim baseAnimType = Define.BaseAnim.Idle;
    public Define.BaseAnim BaseAnimType
    {
        get { return baseAnimType; }
        set
        {
            if (value == baseAnimType)
                return;

            baseAnimType = value;

            switch (baseAnimType)
            {
                case Define.BaseAnim.Die:
                    {
                        if (temp_isDying == false)
                        {
                            SetDead();
                        }
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
                Util.LogWarning();
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
                        if (temp_isAttacking == false)
                        {
                            SetAttack();
                        }
                    }
                    break;
            }
        }
    }

    protected string temp_baseLayerName = "Base Layer";
    protected string temp_upperLayerName = "Upper Layer";
    protected float temp_scanRange = 6f; //임시
    protected float temp_hitTime = 0.4f; //플레이어
    private float temp_startTime = 1f;
    private float temp_andTime = 0f;
    private float temp_fadeOutTime = 0f;
    private bool temp_isDying = false;
    private bool temp_isAttacking = false;
    private bool temp_isHit = false;

    protected string Temp_Tag { set { gameObject.tag = value; } }

    Collider collider = null;
    protected NavMeshAgent navMeshAgent { get; private set; } = null;
    protected Animator_Util animator { get; private set; } = null;
    protected Shader_Util shader { get; private set; } = null;

    HPBarUI hpBarUI = null;
    protected Weapon weapon { get; private set; } = null;


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
        animator = gameObject.GetOrAddComponent<Animator_Util>();
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
        if(hpBarUI != null)
        {
            hpBarUI.SetHPBar(transform, Stat.currentHp, Stat.maxHp);
        }   
    }

    public override void SetWorldObject(Define.WorldObject pWorldObjType, int pWorldObjID, Action<GameObject> pDespawnAction)
    {
        base.SetWorldObject(pWorldObjType, pWorldObjID, pDespawnAction);

        characterData = WorldScene.Instance.CharacterTable.GetTableData(pWorldObjID);
    }

    public override void Spawn(Vector3 pPos, Vector3 pRot)
    {
        base.Spawn(pPos, pRot);

        //
        SurvivalStateType = SurvivalState.Alive;

        //
        shader.SetMateriasColorAlpha(1f, false);

        animator.SetAnimatorController(characterData.animator, characterData.avatar);
        animator.SwapAnimationClip("Humanoid_Ready_Hand", characterData.upperReadyClip);
        animator.SwapAnimationClip("Humanoid_Attack_Hand", characterData.upperAttackClip);

        // Stat
        {
            Table.StatTable.Data statData = WorldScene.Instance.StatTable.GetTableData(1); //임시

            stat.maxHp = statData.maxHp;
            stat.currentHp = statData.maxHp;
            stat.attack = statData.attack;
            stat.defense = statData.defense;
            stat.runSpeed = statData.moveSpeed;
            //this.statData.maxExp = statData.maxExp;
        }

        if (hpBarUI == null)
            CreateHPBarUI();

        hpBarUI.Open();

        // Weapon(Temp)
        if (characterData.characterType == Define.Character.Human)
        {
            if (weapon == null)
                CreateWeapon(1);

            weapon.InitWeapon();
            SetWeaponPos(Vector3.zero, Quaternion.identity, false);
        }

        /// State
        baseAnimType = Define.BaseAnim.None;
        BaseAnimType = Define.BaseAnim.Idle;

        upperAnimType = Define.UpperAnim.None;
        UpperAnimType = Define.UpperAnim.Ready;

        temp_isAttacking = false;
        temp_isHit = false;
        temp_isDying = false;
    }

    protected virtual void SetDead()
    {
        SurvivalStateType = SurvivalState.Dead;

        if (hpBarUI != null)
            hpBarUI.Close(); //임시

        navMeshAgent.isStopped = true;
        navMeshAgent.velocity = Vector3.zero;
        animator.SetRebind();

        //
        string animName = BaseAnimType.ToString();
        animator.SetCrossFade(temp_baseLayerName, animName, 0.03f, 1f);

        temp_startTime = 1f;
        temp_andTime = 0f;
        temp_fadeOutTime = 0f;
        temp_isDying = true;
    }

    protected virtual void SetIdle()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.velocity = Vector3.zero;

        string animName = BaseAnimType.ToString();
        animator.SetCrossFade(temp_baseLayerName, animName, 0.03f, 1f);
    }

    protected virtual void SetRun()
    {
        navMeshAgent.isStopped = false;
        //navMeshAgent.speed = stat.moveSpeed;

        string animName = BaseAnimType.ToString();
        animator.SetCrossFade(temp_baseLayerName, animName, 0.03f, 1f);
    }

    protected virtual void SetReady()
    {
        temp_isAttacking = false;
        // Weapon
        Vector3 pos = new Vector3(-0f, 0f, -0.3f);
        Quaternion rot = Quaternion.Euler(0f, 0f, 180f);
        SetWeaponPos(pos, rot, true);

        string animName = UpperAnimType.ToString();
        animator.SetCrossFade(temp_upperLayerName, animName, 0.1f, 1f);
    }

    protected virtual void SetAttack()
    {
        temp_isAttacking = true;
        temp_isHit = false;

        // Weapon
        Vector3 pos = new Vector3(0.14f, 0.14f, -0.35f);
        Quaternion rot = Quaternion.Euler(19f, -37f, 166f);
        SetWeaponPos(pos, rot, true);

        string animName = UpperAnimType.ToString();
        animator.SetCrossFade(temp_upperLayerName, animName, 0f, 1f);
    }

    protected void SetHP(int pHpValue)
    {
        if (pHpValue < 0)
            stat.currentHp = 0;
        else if (pHpValue > stat.maxHp)
            stat.currentHp = stat.maxHp;
        else
            stat.currentHp = pHpValue;
    }

    void UpdateAttacking()
    {
        if (temp_isAttacking == false)
            return;

        int truncValue = (int)animator.GetAnimatorStateNormalizedTime(temp_upperLayerName);
        float animTime = animator.GetAnimatorStateNormalizedTime(temp_upperLayerName) - truncValue;

        if (temp_isHit && animTime < temp_hitTime)
        {
            temp_isHit = false; // Reset
        }
        else if (temp_isHit == false && animTime >= temp_hitTime)
        {
            temp_isHit = true;
            OnHitEvent();
        }

        if (animTime >= 1.0f)
        {
            temp_isAttacking = false;
            temp_isHit = false;
        }
    }

    void UpdateDying()
    {
        if (temp_isDying == false)
            return;

        temp_fadeOutTime = Mathf.Lerp(temp_startTime, temp_andTime, animator.GetAnimatorStateNormalizedTime(temp_baseLayerName));
        SetMateriasColorAlpha(temp_fadeOutTime);

        if (animator.GetAnimatorStateNormalizedTime(temp_baseLayerName) >= 1.0f)
        {
            SetMateriasColorAlpha(0f);
            animator.SetRebind();

            Despawn();

            temp_startTime = 1f;
            temp_andTime = 0f;
            temp_fadeOutTime = 0f;
            temp_isDying = false;
        }
    }

    protected virtual void OnHitEvent() { }

    public void OnDamage(int pValue)
    {
        int damage = Mathf.Max(0, pValue - Stat.defense);
        int hp = Stat.currentHp - pValue;
        SetHP(hp);

        //
        if (Stat.currentHp <= 0)
        {
            BaseAnimType = Define.BaseAnim.Die;
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

    void CreateHPBarUI()
    {
        /// Destroy
        {
            if (hpBarUI != null)
            {
                GlobalScene.Instance.DestroyGameObject(hpBarUI.gameObject);
                hpBarUI = null;
            }
        }

        hpBarUI = WorldScene.Instance.CreateBaseSpaceUI<HPBarUI>(transform);
    }

    void CreateWeapon(int pWeaponID)
    {
        if (pWeaponID == 0)
            return;

        /// Destroy
        {
            if (weapon != null)
            {
                GlobalScene.Instance.DestroyGameObject(weapon.gameObject);
                weapon = null;
            }
        }


        Transform[] children = GetComponentsInChildren<Transform>();
        Transform weponTrans = Array.Find(children, x => x.name == "Hand_L");
        if (weponTrans == null)
        {
            Util.LogWarning();
            return;
        }

        string tempPath = $"Prefabs/Weapon/SM_Wep_Watergun_02";
        GameObject go = GlobalScene.Instance.InstantiateResource(tempPath, weponTrans);
        if (go == null)
        {
            Util.LogWarning();
            return;
        }

        weapon = go.GetOrAddComponent<Weapon>();
    }

    void SetWeaponPos(Vector3 pPos, Quaternion pRot, bool pEnable)
    {
        if (weapon == null)
            return;

        weapon.SetPosition(pPos);
        weapon.SetRotation(pRot);
        weapon.SetEnable(pEnable);
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
}

