using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interface;

public interface IControllHndl_Legacy
{
    /// <summary>
    /// PlayerDat(임시)
    /// </summary>
    //public int maxExp;
    //public int exp = 0;

    public Vector3 Position { get; }
    public Vector3 Rotation { get; }
    //public Define.BaseState baseStateType { get; }

    public void OnMove(Vector3 pEulerAngles);
    public void OnStop();
    public void OnAttack();
    public void OnReady();
    //public Transform GetTranform(); //임시
    public void IncreaseExp(int pAddExpValue);
}

//public interface IPlayerHandler
//{
//    public Vector3 transform { get; set; }/삭제
//    //public Define.BaseState baseStateType { get; }

//    public void OnMove(Vector2 pRot);
//    public void OnStop();
//    public void OnAttack();
//    public void OnReady();
//    public Transform GetTranform(); //임시
//    public void IncreaseExp(int pAddExpValue);
//}

//public interface IPlayerActor
//{
//    public Define.SpawnState SpawnState { get; set; }
//    public Define.BaseState BaseStateType { get; } //임시

//    public Vector3 LocalPos { get; }
//    public Vector3 LocalRot { get; }
//    public Vector3 WorldPos { get; }
//    public Vector3 WorldRot { get; }

//    public void OnMove(Vector3 pEulerAngles);
//    public void OnStop();
//    public void OnAttack();
//    public void OnReady();
//}

public class Player : Character, IControllHndl_Legacy
{
    //Contents.Level levelData;
    //public Contents.Level LevelData { get { return levelData; } }

    //Contents.Money moneyData;
    //public Contents.Money MoneyData { get { return moneyData; } }

    //int userLevelValue = 0;
    //int userExpValue = 0;
    float scanRange = 6f; //임시

    ITargetHandler target = null;


    protected override void Update()
    {
        base.Update();

        if (target != null)
        {
            //
            Vector3 target_dir = target.Position - Position;
            if (target_dir.magnitude > scanRange)
            {
                target.OnDisableTargetOutline();
                target = null;
            }
            else if (target.BaseAnimType == Define.BaseAnim.Die)
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
                ITargetHandler tempTarget = GameManagerEX.Instance.GetTargetCharacter(hitCollider.gameObject);
                if (tempTarget != null && tempTarget.BaseAnimType != Define.BaseAnim.Die)
                {
                    if (target != null) //가까운 타겟팅
                    {
                        float targetDist = (target.Position - Position).magnitude;
                        float tempTargetDist = (tempTarget.Position - Position).magnitude;
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

        if (UpperAnimType == Define.UpperAnim.Attack && target != null)
        {
            Vector3 dir = (target.Position - Position).normalized;
            Rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 30 * Time.deltaTime).eulerAngles;
        }
    }

    //protected override void SetDespawn()
    //{
    //    base.SetDespawn();

    //    Manager.World.SetPlayerRespawn();
    //}

    //[Obsolete("수정 필요")]
    //protected override void InitSpawnCharacter()
    //{
    //    hitTime = 0.4f; //임시

    //    userExpValue = Manager.User.ExpValue;
    //    userLevelValue = Manager.User.LevelValue;
    //    int currentHp = Manager.User.HpValue;

    //    SetStat(userLevelValue);
    //    SetHP(currentHp);

    //    //handler.StartController();
    //}
    //protected override void SetDie()
    //{
    //    int currentHp = Manager.User.SetUserHP(stat.maxHp);
    //    SetHP(currentHp);

    //    base.SetDie();
    //}

    //protected override void OnHitEvent()
    //{
    //    if (weapon != null)
    //    {
    //        weapon.PlaySFX();
    //    }

    //    if (target != null)
    //    {
    //        float dist = (target.transPosition - LocalPos).magnitude;
    //        if (dist <= 6f)
    //        {
    //            target.OnDamage(stat.attack);
    //        }
    //    }

    //    // Nkife
    //    {
    //        //Collider[] hitColliders = Physics.OverlapBox(transform.position + (transform.forward / 2f) + transform.up, transform.localScale, Quaternion.identity);
    //        //foreach (var hitCollider in hitColliders)
    //        //{
    //        //    if (hitCollider.tag == "Monster")
    //        //    {
    //        //        Debug.Log("몬스터 Hit 감지 : " + hitCollider.name);
    //        //        Stat_Test targetStat = hitCollider.GetComponent<Stat_Test>();
    //        //        if (targetStat != null)
    //        //            targetStat.OnAttacked(stat);
    //        //    }
    //        //}
    //    }

    //    // Gun
    //    {
    //        //RaycastHit hit;
    //        //Ray ray = new Ray(transform.position + transform.up, transform.forward);

    //        //if (Physics.Raycast(ray, out hit, 3f))
    //        //{
    //        //    if (hit.collider.gameObject.tag == "Monster")
    //        //    {
    //        //        Debug.Log("몬스터 Hit 감지 : " + hit.collider.name);
    //        //        Debug.DrawRay(gameObject.transform.position, gameObject.transform.forward * hit.distance, Color.red);
    //        //        Stat_Test targetStat = hit.collider.GetComponent<Stat_Test>();
    //        //        if (targetStat != null)
    //        //            targetStat.OnAttacked(stat);
    //        //    }
    //        //}
    //    }
    //}

    [Obsolete("수정 필요")]
    public void OnMove(Vector3 pEulerAngles)
    {
        BaseAnimType = Define.BaseAnim.Run;

        //float angle = Mathf.Atan2(pEulerAngles.x, pEulerAngles.y) * Mathf.Rad2Deg;
        //angle = angle < 0 ? 360 + angle : angle;
        //Vector3 eulerAngles = new Vector3(0f, Camera.main.transform.eulerAngles.y + angle, 0f);
        //transform.eulerAngles = eulerAngles;

        transform.eulerAngles = pEulerAngles;
        Vector3 pos = transform.forward * Time.deltaTime * navMeshAgent.speed;
        navMeshAgent.Move(pos);
    }
    public void OnStop()
    {
        BaseAnimType = Define.BaseAnim.Idle;
    }
    public void OnAttack()
    {
        UpperAnimType = Define.UpperAnim.Attack;
    }
    public void OnReady()
    {
        UpperAnimType = Define.UpperAnim.Ready;
    }

    [Obsolete("레벨업 수정")]
    public void IncreaseExp(int pAddExpValue)
    {
        //if (pAddExpValue <= 0)
        //{
        //    Debug.LogWarning("Failed : ");
        //    return;
        //}

        //int expValue = userExpValue + pAddExpValue;
        //if (expValue < stat.maxExp)
        //{
        //    userExpValue = Manager.User.SetUserExp(expValue);
        //    Debug.Log($"userExpValue 증가 : {userExpValue}");
        //}
        //else
        //{
        //    SetLevelUp(); //임시
        //}
        ////int level = 1;
        ////while (true)
        ////{
        ////    //Data.StatData statData;
        ////    //if (Managers.Data.StatDict.TryGetValue(level + 1, out statData) == false)
        ////    //    break;

        ////    int temp_totalExp = 100;
        ////    if (statData.exp < temp_totalExp)
        ////        break;
        ////    level++;
        ////}

        ////if (level != statData.level)
        ////{
        ////    Debug.Log("Level Up!");
        ////}
    }

    //void SetLevelUp()
    //{
    //    if (userExpValue < stat.maxExp)
    //    {
    //        Debug.LogWarning($"Failed : 플레이어의 Exp({userExpValue})가 MaxExp({stat.maxExp})보다 작아 LevelUp할 수 없습니다.");
    //        return;
    //    }

    //    Manager.User.SetUserLevelUp();
    //    userExpValue = Manager.User.ExpValue;
    //    userLevelValue = Manager.User.LevelValue;

    //    Debug.Log($"Success : Level Up! : {userLevelValue}(exp : {userExpValue})");

    //    //Dictionary<int, Data.StatData> dict = Managers.Data.StatDict;
    //    //Data.StatData statData = dict[level];

    //    //Stat.Hp = statData.maxHp;
    //    //Stat.MaxHp = statData.maxHp;
    //    //Stat.Attack = statData.attack;
    //}

    ////Draw the Box Overlap as a gizmo to show where it currently is testing. Click the Gizmos button to see this
    //[Obsolete("테스트")]
    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;
    //    //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
    //    //if (m_Started)
    //    //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
    //    Gizmos.DrawWireSphere(transform.position, 3f);
    //    Gizmos.DrawWireCube(transform.position + (transform.forward / 2f) + transform.up, transform.localScale);
    //    //Debug.DrawRay(transform.position + transform.up, transform.forward * 3f, Color.red);
    //}
}
