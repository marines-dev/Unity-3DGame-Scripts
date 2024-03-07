using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IControllHandler
{
    public Vector3 transPosition { get; set; }
    public Quaternion transRotation { get; set; }
    //public Define.BaseState baseStateType { get; }

    public void OnMove(Vector2 pRot);
    public void OnStop();
    public void OnAttack();
    public void OnReady();
    public Transform GetTranform(); //임시
    public void IncreaseExp(int pAddExpValue);
}

public class GamePlayer : BaseCharacter, IControllHandler
{
    int     userLevelValue  = 0;
    int     userExpValue    = 0;
    float   scanRange       = 6f; //임시

    ITargetHandler target = null;


    void Update()
    {
        if (target != null)
        {
            //
            Vector3 target_dir = target.transPosition - transPosition;
            if(target_dir.magnitude > scanRange)
            {
                target.OnDisableTargetOutline();
                target = null;
            }
            else if(target.baseStateType == Define.BaseState.Die)
            {
                target.OnDisableTargetOutline();
                target = null;
            }
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, scanRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag == "Monster")
            {
                ITargetHandler tempTarget = GlobalScene.GameMng.GetTargetCharacter(hitCollider.gameObject);
                if(tempTarget != null && tempTarget.baseStateType != Define.BaseState.Die)
                {
                    if (target != null) //가까운 타겟팅
                    {
                        float targetDist        = (target.transPosition - transPosition).magnitude;
                        float tempTargetDist    = (tempTarget.transPosition - transPosition).magnitude;
                        if (targetDist > tempTargetDist) //타겟의 거리가 더 멀면
                        {
                            target.OnDisableTargetOutline();
                            target = tempTarget;
                        }
                    }
                    else
                    {
                        target = tempTarget;
                    }

                    target.OnEnableTargetOutline();
                }
            }
        }

        if(upperStateType == Define.UpperState.Attack && target != null)
        {
            Vector3 dir = (target.transPosition - transPosition).normalized;
            transRotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 30 * Time.deltaTime);
        }
    }


    [Obsolete("수정 필요")] protected override void InitSpawnCharacter()
    {
        gameObject.tag = "Player"; //임시
        hitTime = 0.4f; //임시

        userExpValue = GlobalScene.UserMng.ExpValue;
        userLevelValue = GlobalScene.UserMng.LevelValue;
        int currentHp = GlobalScene.UserMng.HpValue;

        SetStat(userLevelValue);
        SetHP(currentHp);

        //handler.StartController();
    }
    protected override IEnumerator BaseDieStateProcecssCoroutine()
    {
        if (baseStateType != Define.BaseState.Die)
        {
            Debug.LogWarning("Failed : ");
            yield break;
        }

        // maxHP로 초기화 합니다.
        int currentHp = GlobalScene.UserMng.SetUserHP(stat.maxHp);
        SetHP(currentHp);

        navMeshAgent.isStopped = true;
        navMeshAgent.velocity = Vector3.zero;

        animatorOverride.SetRebind();
        //weapon.SetEnable(false);
        yield return null;

        string animName = baseStateType.ToString();
        animatorOverride.SetCrossFade(baseLayerName, animName, 0.03f, 1f);
        //animatorOverride.CrossFade(animName, 0.03f);
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
        GlobalScene.GameMng.SetPlayerRespawn();
    }
    protected override IEnumerator BaseIdleStateProcecssCoroutine()
    {
        if (baseStateType != Define.BaseState.Idle)
        {
            Debug.LogError("");
            yield break;
        }
        navMeshAgent.isStopped = true;
        navMeshAgent.velocity = Vector3.zero;

        string animName = baseStateType.ToString();
        animatorOverride.SetCrossFade(baseLayerName, animName, 0.03f, 1f);
        //animatorOverride.CrossFade(animName, 0.03f);
        //yield return new WaitUntil(() => animatorOverride.GetCurrentAnimatorStateInfo(0).IsName(animName) 
        //&& animatorOverride.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        yield return new WaitUntil(() => animatorOverride.GetAnimatorStateNormalizedTime(baseLayerName) >= 1.0f);
    }
    protected override IEnumerator BaseRunStateProcecssCoroutine()
    {
        if (baseStateType != Define.BaseState.Run)
        {
            Debug.LogError("");
            yield break;
        }

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = stat.moveSpeed;

        string animName = baseStateType.ToString();
        animatorOverride.SetCrossFade(baseLayerName, animName, 0.03f, 1f);
        //animatorOverride.CrossFade(animName, 0.03f);
        yield return new WaitUntil(() => animatorOverride.GetAnimatorStateNormalizedTime(baseLayerName) >= 1.0f);
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
        //yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(1).IsName(stateType.ToString()) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
    }
    protected override IEnumerator UpperAttackStateProcecssCoroutine()
    {
        if (upperStateType != Define.UpperState.Attack)
        {
            Debug.LogWarning("Failed : ");
            yield break;
        }

        //
        string animName = upperStateType.ToString();
        animatorOverride.SetCrossFade(upperLayerName, animName, 0f, 1f);
        //animatorOverride.CrossFade(animName, 0f, -1, 0f);
        //animator.CrossFade("animName", 0.1f, -1, 0);
        //yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(1).IsName(stateType.ToString()) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
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
        if (weapon != null)
        {
            weapon.PlaySFX();
        }

        if (target != null)
        {
            float dist = (target.transPosition - transPosition).magnitude;
            if (dist <= 6f)
            {
                target.OnDamage(stat.attack);
            }
        }

        // Nkife
        {
            //Collider[] hitColliders = Physics.OverlapBox(transform.position + (transform.forward / 2f) + transform.up, transform.localScale, Quaternion.identity);
            //foreach (var hitCollider in hitColliders)
            //{
            //    if (hitCollider.tag == "Monster")
            //    {
            //        Debug.Log("몬스터 Hit 감지 : " + hitCollider.name);
            //        Stat_Test targetStat = hitCollider.GetComponent<Stat_Test>();
            //        if (targetStat != null)
            //            targetStat.OnAttacked(stat);
            //    }
            //}
        }

        // Gun
        {
            //RaycastHit hit;
            //Ray ray = new Ray(transform.position + transform.up, transform.forward);

            //if (Physics.Raycast(ray, out hit, 3f))
            //{
            //    if (hit.collider.gameObject.tag == "Monster")
            //    {
            //        Debug.Log("몬스터 Hit 감지 : " + hit.collider.name);
            //        Debug.DrawRay(gameObject.transform.position, gameObject.transform.forward * hit.distance, Color.red);
            //        Stat_Test targetStat = hit.collider.GetComponent<Stat_Test>();
            //        if (targetStat != null)
            //            targetStat.OnAttacked(stat);
            //    }
            //}
        }
    }
    [Obsolete("수정 필요")] public void OnMove(Vector2 pDir)
    {
        SetBaseStateType(Define.BaseState.Run);

        float angle = Mathf.Atan2(pDir.x, pDir.y) * Mathf.Rad2Deg;
        angle = angle < 0 ? 360 + angle : angle;
        Vector3 eulerAngles = new Vector3(0f, Camera.main.transform.eulerAngles.y + angle, 0f);
        transform.eulerAngles = eulerAngles;

        Vector3 pos = transform.forward * Time.deltaTime * navMeshAgent.speed;
        navMeshAgent.Move(pos);
    }
    public void OnStop()
    {
        SetBaseStateType(Define.BaseState.Idle);
    }
    public void OnAttack()
    {
        SetUpperStateType(Define.UpperState.Attack);
    }
    public void OnReady()
    {
        SetUpperStateType(Define.UpperState.Ready);
    }
    public Transform GetTranform()
    {
        return transform;
    }
    [Obsolete("레벨업 수정")] public void IncreaseExp(int pAddExpValue)
    {
        if (pAddExpValue <= 0)
        {
            Debug.LogWarning("Failed : ");
            return;
        }

        int expValue = userExpValue + pAddExpValue;
        if (expValue < stat.maxExp)
        {
            userExpValue = GlobalScene.UserMng.SetUserExp(expValue);
            Debug.Log($"userExpValue 증가 : {userExpValue}");
        }
        else
        {
            SetLevelUp(); //임시
        }
        //int level = 1;
        //while (true)
        //{
        //    //Data.StatData statData;
        //    //if (Managers.Data.StatDict.TryGetValue(level + 1, out statData) == false)
        //    //    break;

        //    int temp_totalExp = 100;
        //    if (statData.exp < temp_totalExp)
        //        break;
        //    level++;
        //}

        //if (level != statData.level)
        //{
        //    Debug.Log("Level Up!");
        //}
    }
    void SetLevelUp()
    {
        if (userExpValue < stat.maxExp)
        {
            Debug.LogWarning($"Failed : 플레이어의 Exp({userExpValue})가 MaxExp({stat.maxExp})보다 작아 LevelUp할 수 없습니다.");
            return;
        }

        GlobalScene.UserMng.SetUserLevelUp();
        userExpValue    = GlobalScene.UserMng.ExpValue;
        userLevelValue  = GlobalScene.UserMng.LevelValue;

        Debug.Log($"Success : Level Up! : {userLevelValue}(exp : {userExpValue})");

        //Dictionary<int, Data.StatData> dict = Managers.Data.StatDict;
        //Data.StatData statData = dict[level];

        //Stat.Hp = statData.maxHp;
        //Stat.MaxHp = statData.maxHp;
        //Stat.Attack = statData.attack;
    }

    //Draw the Box Overlap as a gizmo to show where it currently is testing. Click the Gizmos button to see this
    [Obsolete("테스트")]
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        //if (m_Started)
        //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
        Gizmos.DrawWireSphere(transform.position, 3f);
        Gizmos.DrawWireCube(transform.position + (transform.forward / 2f) + transform.up, transform.localScale);
        //Debug.DrawRay(transform.position + transform.up, transform.forward * 3f, Color.red);
    }
}
