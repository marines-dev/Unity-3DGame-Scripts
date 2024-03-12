using System;
using System.Collections;
using UnityEngine;

public class Enemy : Character
{
    float scanRange = 6f;
    float attackRange = 1.5f;
    Vector3 destPos = Vector3.zero;

    protected override void Update()
    {
        base.Update();

        if (BaseAnimType == Define.BaseAnim.Die)
            return;

        UpdateBaseState(BaseAnimType);
        UpdateUpperState(UpperAnimType);
    }

    //public override void ResetCharacter()
    //{
    //    SetStateType(Define.State.Idle);
    //}

    protected override void SetDie()
    {
        //// 플레이어 Exp 증가
        //if (Manager.World.IsGamePlay)
        //    Manager.World.playerCtrl.IncreaseExp(stat.maxExp);

        shader.OnDisableOutline();

        base.SetDie();
    }

    //protected override void OnHitEvent()
    //{
    //    if (Manager.World.IsGamePlay)
    //    {
    //        Player target = null;
    //        Collider[] hitColliders = Physics.OverlapBox(transform.position + (transform.forward / 2f) + transform.up, transform.localScale, Quaternion.identity);
    //        foreach (var hitCollider in hitColliders)
    //        {
    //            if (hitCollider.tag == "Player")
    //            {
    //                target = hitCollider.GetComponent<Player>();
    //                if (target != null)
    //                    target.OnDamage(stat.attack);

    //                break;
    //            }
    //        }

    //        //if (target != null && target.StateType != Define.State.Die)
    //        //{
    //        //    float distance = (this.target.transform.position - transform.position).magnitude;
    //        //    if (distance <= attackRange)
    //        //        SetStateType(Define.State.Attack);
    //        //    else
    //        //        SetStateType(Define.State.Run);
    //        //}
    //        //else
    //        //{
    //        //    SetStateType(Define.State.Idle);
    //        //}
    //    }
    //}

    void UpdateBaseState(Define.BaseAnim pBaseState)
    {
        if (BaseAnimType == Define.BaseAnim.Die)
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

        if (GameManagerEX.Instance.IsGamePlay)
        {
            if (UpperAnimType == Define.UpperAnim.Attack)
            {
                return;
            }

            destPos = GameManagerEX.Instance.playerCtrl.Position;
            distance = (destPos - Position).magnitude;
            if (distance <= scanRange && CalculateNavMeshPath(destPos))
            {
                //target = Managers.Game.GamePlayer.gameObject;
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
    }

    void UpdateRun()
    {
        float distance = 0f;
        //행동 변경 검사 : 공격 범위 이내이면 -> Attack
        if (GameManagerEX.Instance.IsGamePlay /*&& target != null*/)
        {
            distance = (GameManagerEX.Instance.playerCtrl.Position - Position).magnitude;
            if (distance <= attackRange) // 플레이어와의 거리가 공격 범위보다 가까우면 공격
            {
                navMeshAgent.SetDestination(transform.position);
                BaseAnimType = Define.BaseAnim.Idle;
                //SetUpperStateType(Define.UpperState.Attack);
                return;
            }
        }

        Vector3 dir = destPos - transform.position; //방향
        if (dir.magnitude < 1f)
        {
            BaseAnimType = Define.BaseAnim.Idle;
            return;
        }

        //navMeshAgent.speed = StatData.moveSpeed;
        navMeshAgent.SetDestination(destPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
    }

    void UpdateUpperReady()
    {
        if (GameManagerEX.Instance.IsGamePlay)
        {
            float distance = (GameManagerEX.Instance.playerCtrl.Position - Position).magnitude;
            if (distance <= attackRange) // 플레이어와의 거리가 공격 범위보다 가까우면 공격
            {
                UpperAnimType = Define.UpperAnim.Attack;
                //return;
            }
        }
    }

    void UpdateUpperAttack()
    {
        if (GameManagerEX.Instance.IsGamePlay)
        {
            float distance = (GameManagerEX.Instance.playerCtrl.Position - Position).magnitude;
            if (distance > attackRange)
            {
                UpperAnimType = Define.UpperAnim.Ready;
                return;
            }

            Vector3 dir = GameManagerEX.Instance.playerCtrl.Position - Position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
        }
        else
        {
            UpperAnimType = Define.UpperAnim.Ready;
        }
    }
}
