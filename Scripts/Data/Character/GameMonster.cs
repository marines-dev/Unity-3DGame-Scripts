using System;
using System.Collections;
using UnityEngine;

public class GameMonster : BaseCharacter
{
    float   scanRange   = 6f;
    float   attackRange = 1.5f;
    Vector3 destPos     = Vector3.zero;

    void Update()
    {
        if (baseStateType == Define.BaseState.Die)
            return;

        UpdateBaseState(baseStateType);
        UpdateUpperState(upperStateType);
    }

    protected override void InitSpawnCharacter()
    {
        gameObject.tag = "Monster"; //임시
        hitTime = 0.55f; //임시
    }
    //public override void ResetCharacter()
    //{
    //    SetStateType(Define.State.Idle);
    //}
    protected override IEnumerator BaseDieStateProcecssCoroutine()
    {
        if (baseStateType != Define.BaseState.Die)
        {
            Debug.LogWarning("Failed : ");
            yield break;
        }

        // 플레이어 Exp 증가
        if (Managers.Game.IsGamePlay)
            Managers.Game.playerCtrl.IncreaseExp(stat.maxExp);

        navMeshAgent.isStopped = true;
        navMeshAgent.velocity = Vector3.zero;

        shader.OnDisableOutline();
        animatorOverride.SetRebind();
        yield return null;

        string animName = baseStateType.ToString();
        //animatorOverride.CrossFade(animName, 0.03f);
        animatorOverride.SetCrossFade(baseLayerName, animName, 0.03f, 1.5f);
        yield return new WaitForEndOfFrame();

        //yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(stateType.ToString()) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        float startTime = 1f;
        float andTime = 0f;
        float fadeOutTime = Mathf.Lerp(startTime, andTime, animatorOverride.GetAnimatorStateNormalizedTime(baseLayerName));
        SetMateriasColorAlpha(fadeOutTime);

        while (animatorOverride.GetAnimatorStateNormalizedTime(baseLayerName) < 1f)
        {
            fadeOutTime = Mathf.Lerp(startTime, andTime, animatorOverride.GetAnimatorStateNormalizedTime(baseLayerName));
            SetMateriasColorAlpha(fadeOutTime);

            //Debug.Log($"fadeOutTime : {fadeOutTime}");
            yield return null;
        }

        Debug.Log($"{animName} 애니메이션 종료!");
        SetMateriasColorAlpha(0f);
        animatorOverride.SetRebind();

        yield return new WaitForEndOfFrame();

        //
        SetMateriasColorAlpha(1f);
        DespawnCharacter();
    }
    protected override IEnumerator BaseIdleStateProcecssCoroutine()
    {
        if (baseStateType != Define.BaseState.Idle)
        {
            Debug.LogError("");
            yield break;
        }

        //
        navMeshAgent.isStopped = true;
        navMeshAgent.velocity = Vector3.zero;

        string animName = baseStateType.ToString();
        animatorOverride.SetCrossFade(baseLayerName, animName, 0.03f, 1f);
        //animatorOverride.CrossFade(animName, 0.03f);
        //yield return new WaitUntil(() => animatorOverride.GetAnimatorStateNormalizedTime(0) >= 1.0f);
        yield return null;
    }
    protected override IEnumerator BaseRunStateProcecssCoroutine()
    {
        if (baseStateType != Define.BaseState.Run)
        {
            Debug.LogError("");
            yield break;
        }

        //
        navMeshAgent.isStopped = false;

        //
        string animName = baseStateType.ToString();
        animatorOverride.SetCrossFade(baseLayerName, animName, 0.03f, 1f);
        //animatorOverride.CrossFade(animName, 0.03f);
        //yield return new WaitUntil(() => animatorOverride.GetAnimatorStateNormalizedTime(0) >= 1.0f);
    }
    protected override IEnumerator UpperReadyStateProcecssCoroutine()
    {
        if (upperStateType != Define.UpperState.Ready)
        {
            Debug.LogWarning("Failed : ");
            yield break;
        }

        //
        string animName = upperStateType.ToString();
        animatorOverride.SetCrossFade(upperLayerName, animName, 0.1f, 1f);
        //animatorOverride.CrossFade(animName, 0f, -1, 0f);
        //animator.CrossFade("animName", 0.1f, -1, 0);
        //yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(stateType.ToString()) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        yield return null;
    }
    protected override IEnumerator UpperAttackStateProcecssCoroutine()
    {
        if (upperStateType != Define.UpperState.Attack)
        {
            Debug.LogError("");
            yield break;
        }

        string animName = upperStateType.ToString();
        animatorOverride.SetCrossFade(upperLayerName, animName, 0.1f, 2f);
        //animatorOverride.CrossFade(animName, 0f, -1, 0f);
        //animator.CrossFade("animName", 0.1f, -1, 0);
        //yield return new WaitUntil(() => animatorOverride.GetAnimatorStateNormalizedTime(1) >= 1.0f);
        yield return null;

        int truncValue = 0;
        float animTime = 0f;
        bool isHit = false;
        while (upperStateType == Define.UpperState.Attack)
        {
            truncValue = (int)animatorOverride.GetAnimatorStateNormalizedTime(upperLayerName);
            animTime = animatorOverride.GetAnimatorStateNormalizedTime(upperLayerName) - truncValue;

            if (isHit && animTime < hitTime)
            {
                isHit = false; // Reset
            }
            else if (isHit == false && animTime >= hitTime)
            {
                isHit = true;
                OnHitEvent();
                //Debug.Log($"Hit! : {animTime}(총 {truncValue}회)");
            }
            yield return null;
        }
        yield return null;
    }
    protected override void OnHitEvent()
    {
        if (Managers.Game.IsGamePlay)
        {
            GamePlayer target = null;
            Collider[] hitColliders = Physics.OverlapBox(transform.position + (transform.forward / 2f) + transform.up, transform.localScale, Quaternion.identity);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.tag == "Player")
                {
                    target = hitCollider.GetComponent<GamePlayer>();
                    if (target != null)
                        target.OnDamage(stat.attack);

                    break;
                }
            }

            //if (target != null && target.StateType != Define.State.Die)
            //{
            //    float distance = (this.target.transform.position - transform.position).magnitude;
            //    if (distance <= attackRange)
            //        SetStateType(Define.State.Attack);
            //    else
            //        SetStateType(Define.State.Run);
            //}
            //else
            //{
            //    SetStateType(Define.State.Idle);
            //}
        }
    }

    void UpdateBaseState(Define.BaseState pBaseState)
    {
        if (baseStateType == Define.BaseState.Die)
            return;

        switch (pBaseState)
        {
            case Define.BaseState.Die:
                //UpdateDie();
                break;
            case Define.BaseState.Idle:
                UpdateIdle();
                break;
            case Define.BaseState.Run:
                UpdateRun();
                break;
        }
    }
    void UpdateUpperState(Define.UpperState pUpperState)
    {
        if (baseStateType == Define.BaseState.Die)
            return;

        switch (pUpperState)
        {
            case Define.UpperState.Ready:
                {
                    UpdateUpperReady();
                }
                break;
            case Define.UpperState.Attack:
                {
                    UpdateUpperAttack();
                }
                break;
        }
    }

    void UpdateIdle()
    {
        float distance = 0f;

        if (Managers.Game.IsGamePlay)
        {
            if(upperStateType == Define.UpperState.Attack)
            {
                return;
            }

            destPos = Managers.Game.playerCtrl.transPosition;
            distance = (destPos - transPosition).magnitude;
            if (distance <= scanRange && CalculateNavMeshPath(destPos))
            {
                //target = Managers.Game.GamePlayer.gameObject;
                SetBaseStateType(Define.BaseState.Run);
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
        if (Managers.Game.IsGamePlay /*&& target != null*/)
        {
            distance = (Managers.Game.playerCtrl.transPosition - transPosition).magnitude;
            if (distance <= attackRange) // 플레이어와의 거리가 공격 범위보다 가까우면 공격
            {
                navMeshAgent.SetDestination(transform.position);
                SetBaseStateType(Define.BaseState.Idle);
                //SetUpperStateType(Define.UpperState.Attack);
                return;
            }
        }

        Vector3 dir = destPos - transform.position; //방향
        if (dir.magnitude < 1f)
        {
            SetBaseStateType(Define.BaseState.Idle);
            return;
        }

        navMeshAgent.speed = stat.moveSpeed;
        navMeshAgent.SetDestination(destPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
    }

    void UpdateUpperReady()
    {
        if (Managers.Game.IsGamePlay)
        {
            float distance = (Managers.Game.playerCtrl.transPosition - transPosition).magnitude;
            if (distance <= attackRange) // 플레이어와의 거리가 공격 범위보다 가까우면 공격
            {
                SetUpperStateType(Define.UpperState.Attack);
                //return;
            }
        }
    }

    void UpdateUpperAttack()
    {
        if (Managers.Game.IsGamePlay)
        {
            float distance = (Managers.Game.playerCtrl.transPosition - transPosition).magnitude;
            if (distance > attackRange)
            {
                SetUpperStateType(Define.UpperState.Ready);
                return;
            }

            Vector3 dir = Managers.Game.playerCtrl.transPosition - transPosition;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
        }
        else
        {
            SetUpperStateType(Define.UpperState.Ready);
        }
    }

    [Obsolete("테스트")]
    Vector3 RandomPos()
    {
        Table.Spawner.Data spawnerData = Managers.Table.GetTable<Table.Spawner>().GetTableData(2);
        Vector3 randDir = UnityEngine.Random.insideUnitSphere * UnityEngine.Random.Range(0, spawnerData.spawnRadius);
        randDir.y       = 0;
        Vector3 randPos = spawnerData.spawnPos + randDir;

        return randPos;
    }
}
