using Interface;
using UnityEngine;
using static Define;


public class Enemy : BaseActor
{
    private float   temp_attackRange = 1.5f;
    private Vector3 temp_destPos     = Vector3.zero;


    protected override void Update()
    {
        base.Update();

        if (BaseAnimType == BaseAnim.Die)
            return;

        UpdateBaseState(BaseAnimType);
        UpdateUpperState(UpperAnimType);
    }


    public override void Spawn(Actor pActorType, int pActorID = 0)
    {
        base.Spawn(pActorType, pActorID);

        // Temp
        Temp_Tag = "Monster";
    }

    protected override void SetDead()
    {
        // 플레이어 Exp 증가
        EnemyTable.Data enemeyData = GlobalScene.Instance.EnemyTable.GetTableData(actorID);
        WorldScene.Instance.PlayerCtrl.OnIncreaseExp(enemeyData.RewardExp);

        OnDisableTargetOutline();

        base.SetDead();
    }

    protected override void OnHit()
    {
        base.OnHit();

        Collider[] hitColliders = Physics.OverlapBox(transform.position + (transform.forward / 2f) + transform.up, transform.localScale, Quaternion.identity);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag == "Player")
            {
                ITarget player = WorldScene.Instance.GetTargetCharacter(hitCollider.gameObject);
                if (player != null)
                {
                    player.OnDamage(Stat.attack);
                }
                break;
            }
        }
    }

    void UpdateBaseState(Define.BaseAnim pBaseState)
    {
        if (BaseAnimType == Define.BaseAnim.Die || UpperAnimType == Define.UpperAnim.Attack)
            return;

        switch (pBaseState)
        {
            case Define.BaseAnim.Die:
                //UpdateDie();
                break;
            case Define.BaseAnim.Idle:
                UpdateIdle();
                break;
            case Define.BaseAnim.Run:
                UpdateRun();
                break;
        }
    }

    void UpdateUpperState(Define.UpperAnim pUpperState)
    {
        if (BaseAnimType == Define.BaseAnim.Die)
            return;

        switch (pUpperState)
        {
            case Define.UpperAnim.Ready:
                {
                    UpdateUpperReady();
                }
                break;
            case Define.UpperAnim.Attack:
                {
                    UpdateUpperAttack();
                }
                break;
        }
    }

    void UpdateIdle()
    {
        float distance = 0f;

        NevAgent.isStopped = true;
        NevAgent.velocity = Vector3.zero;

        temp_destPos = WorldScene.Instance.PlayerCtrl.Position;
        distance = (temp_destPos - Position).magnitude;
        if (distance <= temp_scanRange && CalculateNavMeshPath(temp_destPos))
        {
            BaseAnimType = Define.BaseAnim.Run;
            return;
        }

        //// 랜덤 이동
        //Vector3 randomPos = RandomPos();
        //distance = (randomPos - transPosition).magnitude;
        //if (distance <= scanRange && CalculateNavMeshPath(destPos))
        //{
        //    destPos = randomPos;
        //    SetBaseStateType(Define.BaseState.Run);
        //    return;
        //}
    }

    void UpdateRun()
    {
        float distance = 0f;
        //행동 변경 검사 : 공격 범위 이내이면 -> Attack
        distance = (WorldScene.Instance.PlayerCtrl.Position - Position).magnitude;
        if (distance <= temp_attackRange)
        {
            NevAgent.SetDestination(transform.position);
            BaseAnimType = Define.BaseAnim.Idle;
            return;
        }

        Vector3 dir = temp_destPos - transform.position; //방향
        if (dir.magnitude < 1f)
        {
            BaseAnimType = Define.BaseAnim.Idle;
            return;
        }

        NevAgent.isStopped = false;

        EnemyTable.Data     enemeyData    = GlobalScene.Instance.EnemyTable.GetTableData(actorID);
        CharacterTable.Data characterData = GlobalScene.Instance.CharacterTable.GetTableData(enemeyData.CharacterID);

        NevAgent.speed = characterData.RunSpeed_Temp;
        NevAgent.SetDestination(temp_destPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
    }

    void UpdateUpperReady()
    {
        float distance = (WorldScene.Instance.PlayerCtrl.Position - Position).magnitude;
        bool  isAttack = distance <= temp_attackRange;
        if (isAttack) { UpperAnimType = Define.UpperAnim.Attack; }
    }

    void UpdateUpperAttack()
    {
        NevAgent.isStopped = true;
        NevAgent.velocity  = Vector3.zero;

        float distance = (WorldScene.Instance.PlayerCtrl.Position - Position).magnitude;
        if (distance > temp_attackRange)
        {
            UpperAnimType = Define.UpperAnim.Ready;
            return;
        }

        Vector3     dir     = WorldScene.Instance.PlayerCtrl.Position - Position;
        Quaternion  quat    = Quaternion.LookRotation(dir);
        transform.rotation  = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
    }
}
