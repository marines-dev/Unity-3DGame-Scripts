using System;
using System.Collections;
using System.Collections.Generic;
using Table;
using UnityEngine;
using UnityEngine.AI;
using static Define;

public interface ITarget_Temp
{
    public ExistenceState ExistenceStateType { get; }
    public SurvivalState SurvivalStateType { get; }

    public Vector3 Position { get; }
    public Vector3 Rotation { get; }

    public void OnEnableTargetOutline();
    public void OnDisableTargetOutline();

    public void OnDamage(int pValue);
}

public abstract class BaseActor : MonoBehaviour, ITarget_Temp
{
    private Actor actorType = Actor.None;
    protected int actorID = 0;

    [Obsolete] private ExistenceState existenceStateType = ExistenceState.Despawn;
    public ExistenceState ExistenceStateType
    {
        get { return existenceStateType; }
        private set
        {
            existenceStateType = value;

            switch (existenceStateType)
            {
                case ExistenceState.Despawn:
                    { gameObject.SetActive(false); }
                    break;
                case ExistenceState.Spawn:
                    { gameObject.SetActive(true); }
                    break;
            }
        }
    }

    public SurvivalState SurvivalStateType { get; private set; } = SurvivalState.Dead;

    public Vector3 Position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    public Vector3 Rotation
    {
        get { return transform.rotation.eulerAngles; }
        set { transform.rotation = Quaternion.Euler(value); }
    }

    public struct ActorStat //액터마다 Stat구성 다름
    {
        public int maxHp;
        public int currentHp;
        public int attack;
        public int defense;
        //public float runSpeed;
    }
    private ActorStat stat;
    public ActorStat Stat { get { return stat; } }

    private BaseAnim baseAnimType = BaseAnim.Idle;
    public BaseAnim BaseAnimType
    {
        get { return baseAnimType; }
        set
        {
            if (value == baseAnimType)
                return;

            baseAnimType = value;

            switch (baseAnimType)
            {
                case BaseAnim.Die:
                    {
                        if (temp_isDying == false)
                        {
                            SetDead();
                        }
                    }
                    break;
                case BaseAnim.Idle:
                    {
                        SetIdle();
                    }
                    break;
                case BaseAnim.Run:
                    {
                        SetRun();
                    }
                    break;
            }
        }
    }

    private UpperAnim upperAnimType = UpperAnim.None;
    public UpperAnim UpperAnimType
    {
        get { return upperAnimType; }
        set
        {
            if (value == upperAnimType || value == UpperAnim.None)
            {
                Util.LogWarning();
                return;
            }

            upperAnimType = value;

            switch (upperAnimType)
            {
                case UpperAnim.Ready:
                    {
                        SetReady();
                    }
                    break;
                case UpperAnim.Attack:
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

    readonly public string temp_baseLayerName = "Base Layer";
    readonly public string temp_upperLayerName = "Upper Layer";

    protected float temp_scanRange = 6f; //임시
    protected float temp_hitTime = 0.4f; //플레이어
    private float temp_startTime = 1f;
    private float temp_andTime = 0f;
    private float temp_fadeOutTime = 0f;
    private bool temp_isDying = false;
    protected bool temp_isAttacking = false;
    protected bool temp_isHit = false;

    protected string Temp_Tag { set { gameObject.tag = value; } }

    private Collider collider = null;
    private Shader_Util shader = null;
    public Animator_Util Anim { get; private set; } = null;
    public NavMeshAgent NevAgent { get; private set; } = null;

    HPBarUI hpBarUI = null;
    private IWeapon weap = null;
    protected IWeapon Weap => weap ?? (weap = new NullObject());
    
    //private Action<GameObject> despawnAction = null;


    private void Awake()
    {
        /// Collider
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
        }

        Anim = gameObject.GetOrAddComponent<Animator_Util>();
        shader = gameObject.GetOrAddComponent<Shader_Util>();

        NevAgent = gameObject.GetOrAddComponent<NavMeshAgent>();
        NevAgent.radius = 0.3f;
    }

    protected virtual void Update()
    {
        if (BaseAnimType == BaseAnim.Die)
        {
            UpdateDying();
        }
        else if (UpperAnimType == UpperAnim.Attack)
        {
            UpdateAttacking();
        }
    }

    void FixedUpdate()
    {
        if (hpBarUI != null)
        {
            hpBarUI.SetHPBar(transform, Stat.currentHp, Stat.maxHp);
        }
    }

    public virtual void Spawn(Actor pActorType, int pActorID)
    {
        CharacterTable.Data characterData = null;
        StatTable.Data statData = null;
        WeaponTable.Data weaponData = null;

        switch (pActorType)
        {
            case Actor.Player:
                {
                    characterData = GlobalScene.Instance.CharacterTable.GetTableData(1); //임시
                    statData = GlobalScene.Instance.StatTable.GetTableData(1); //임시 
                    weaponData = GlobalScene.Instance.WeaponTable.GetTableData(2); //임시
                }
                break;
            case Actor.Enemy:
                {
                    EnemyTable.Data enemeyData = GlobalScene.Instance.EnemyTable.GetTableData(pActorID);
                    characterData = GlobalScene.Instance.CharacterTable.GetTableData(enemeyData.CharacterID);
                    statData = GlobalScene.Instance.StatTable.GetTableData(enemeyData.StatID); //임시
                    weaponData = GlobalScene.Instance.WeaponTable.GetTableData(enemeyData.WeaponID); //임시
                }
                break;
        }

        bool isCalNavMeshPath = CalculateNavMeshPath(Position);
        if (characterData == null || statData == null || weaponData == null || ! isCalNavMeshPath)
        {
            Util.LogError();
            Despawn();
        }

        ///
        actorType = pActorType;
        actorID = pActorID;

        ExistenceStateType = ExistenceState.Spawn;
        SurvivalStateType = SurvivalState.Alive;

        ///
        shader.SetMateriasColorAlpha(1f, false);

        Anim.SetAnimatorController(characterData.Animator, characterData.Avatar);
        Anim.SwapAnimationClip("Humanoid_Ready_Hand", characterData.UpperReadyClip);
        Anim.SwapAnimationClip("Humanoid_Attack_Hand", characterData.UpperAttackClip);

        /// Stat
        {
            stat.maxHp = statData.MaxHp;
            stat.currentHp = statData.MaxHp;
            stat.attack = statData.Attack;
            stat.defense = statData.Defense;
            //stat.runSpeed = statData.moveSpeed;
            //this.statData.maxExp = statData.maxExp;
        }

        /// HPBarUI
        if (hpBarUI == null)
            CreateHPBarUI();
        hpBarUI?.Open();

        /// Weapon
        if(weap == null)
        {
            weap = Weapon.CreateWeapon(transform, weaponData.ID);
            weap.SetReadyPos();
        }

        /// State
        baseAnimType = BaseAnim.None;
        BaseAnimType = BaseAnim.Idle;

        upperAnimType = UpperAnim.None;
        UpperAnimType = UpperAnim.Ready;

        temp_isAttacking = false;
        temp_isHit = false;
        temp_isDying = false;

        //despawnAction = pDespawnAction;
    }

    public virtual void Despawn()
    {
        //if (despawnAction != null)
        //    despawnAction.Invoke(gameObject);

        ExistenceStateType = ExistenceState.Despawn;

        WorldScene.Instance.SetDespawnSpawning(gameObject, actorType);
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

    protected virtual void SetDead()
    {
        SurvivalStateType = SurvivalState.Dead;

        if (hpBarUI != null)
            hpBarUI.Close(); //임시

        weap.SetEquiped(false);

        NevAgent.isStopped = true;
        NevAgent.velocity = Vector3.zero;
        Anim.SetRebind();

        PlayBaseAnimation(BaseAnimType, 0.03f, 1f);
        temp_startTime = 1f;
        temp_andTime = 0f;
        temp_fadeOutTime = 0f;
        temp_isDying = true;
    }

    protected virtual void SetIdle()
    {
        NevAgent.isStopped = true;
        NevAgent.velocity = Vector3.zero;

        PlayBaseAnimation(BaseAnimType, 0.03f, 1f);
    }

    protected virtual void SetRun()
    {
        NevAgent.isStopped = false;
        //navMeshAgent.speed = stat.moveSpeed;

        PlayBaseAnimation(BaseAnimType, 0.03f, 1f);
    }

    protected virtual void SetReady()
    {
        temp_isAttacking = false;

        PlayUpperAnimation(UpperAnimType, 0.1f, 1f);
        Weap.SetReadyPos();
    }

    protected virtual void SetAttack()
    {
        temp_isAttacking = true;
        temp_isHit = false;

        PlayUpperAnimation(UpperAnimType, 0.1f, 1f);
        Weap.SetAttackPos();
    }

    private void UpdateDying()
    {
        if (temp_isDying == false)
            return;

        temp_fadeOutTime = Mathf.Lerp(temp_startTime, temp_andTime, Anim.GetAnimatorStateNormalizedTime(temp_baseLayerName));
        SetMateriasColorAlpha(temp_fadeOutTime);

        if (Anim.GetAnimatorStateNormalizedTime(temp_baseLayerName) >= 1.0f)
        {
            SetMateriasColorAlpha(0f);
            Anim.SetRebind();

            Despawn();

            temp_startTime = 1f;
            temp_andTime = 0f;
            temp_fadeOutTime = 0f;
            temp_isDying = false;
        }
    }

    private void UpdateAttacking()
    {
        if (temp_isAttacking == false)
            return;

        int truncValue = (int)Anim.GetAnimatorStateNormalizedTime(temp_upperLayerName);
        float animTime = Anim.GetAnimatorStateNormalizedTime(temp_upperLayerName) - truncValue;

        if (temp_isHit && animTime < temp_hitTime)
        {
            temp_isHit = false; // Reset
        }
        else if (temp_isHit == false && animTime >= temp_hitTime)
        {
            temp_isHit = true;
            OnHit();
        }

        if (animTime >= 1.0f)
        {
            temp_isAttacking = false;
            temp_isHit = false;
        }
    }

    protected virtual void OnHit() 
    {
        Weap.PlayHit();
    }

    public void OnDamage(int pValue)
    {
        int attackValue = pValue + Weap.AttackValue;
        int damage = Mathf.Max(0, attackValue - Stat.defense);
        int hp = Stat.currentHp - damage;
        SetHP(hp);

        /// Die
        if (Stat.currentHp <= 0)
        {
            BaseAnimType = BaseAnim.Die;
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

    private void SetMateriasColorAlpha(float pAlpha)
    {
        shader.SetMateriasColorAlpha(pAlpha, true);
    }

    private void PlayBaseAnimation(BaseAnim pBaseAnimType, float pTransDuration, float pSpeed)
    {
        string animName = pBaseAnimType.ToString();
        Anim.SetCrossFade(temp_baseLayerName, animName, pTransDuration, pSpeed);
    }

    protected void PlayUpperAnimation(UpperAnim pUpperAnimType, float pTransDuration, float pSpeed)
    {
        string animName = pUpperAnimType.ToString();
        Anim.SetCrossFade(temp_upperLayerName, animName, pTransDuration, pSpeed);
    }

    private HPBarUI CreateHPBarUI()
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
        return hpBarUI;
    }

    protected bool CalculateNavMeshPath(Vector3 _randPos)
    {
        NavMeshPath path = new NavMeshPath();
        return NevAgent.CalculatePath(_randPos, path);
    }
}
