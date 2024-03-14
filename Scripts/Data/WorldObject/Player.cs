using System;
using UnityEngine;

public interface IPlayerCtrl
{
    public Vector3 Position { get; }
    public Vector3 Rotation { get; }

    public void OnMove(Vector3 pEulerAngles);
    public void OnStop();
    public void OnAttack();
    public void OnReady();
    //public void IncreaseExp(int pAddExpValue);
}

public class NullPlayer : IPlayerCtrl
{
    public Vector3 Position { get { return Vector3.zero; } }
    public Vector3 Rotation { get { return Vector3.zero; } }

    public void OnMove(Vector3 pEulerAngles) { }
    public void OnStop() { }
    public void OnAttack() { }
    public void OnReady() { }

}

public class Player : Character, IPlayerCtrl
{
    int temp_userLevelValue = 0;
    int temp_userExpValue = 0;

    ITargetHandler_Temp target = null;

    Action deadAction = null;

    protected override void Update()
    {
        base.Update();

        if (target != null)
        {
            //
            Vector3 target_dir = target.Position - Position;
            if (target_dir.magnitude > temp_scanRange)
            {
                target.OnDisableTargetOutline();
                target = null;
            }
            else if (target.SurvivalStateType == SurvivalState.Dead)
            {
                target.OnDisableTargetOutline();
                target = null;
            }
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, temp_scanRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag == "Monster")
            {
                ITargetHandler_Temp tempTarget = WorldScene.Instance.GetTargetCharacter(hitCollider.gameObject);
                if (tempTarget != null && tempTarget.SurvivalStateType != SurvivalState.Dead)
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

    public override void Spawn(Vector3 pPos, Vector3 pRot)
    {
        base.Spawn(pPos, pRot);

        /// InitSpawnCharacter(Temp)
        {
            Temp_Tag = "Player";

            temp_userExpValue = WorldScene.Instance.GetUserExpValue();
            temp_userLevelValue = WorldScene.Instance.GetUserLevelValue();
            int currentHp = WorldScene.Instance.GetUserHpValue();

            //SetStat(temp_userLevelValue);
            SetHP(currentHp);
        }
    }

    public void SetPlayerDeadEvent(Action pDeadAction)
    {
        deadAction = pDeadAction;
    }

    protected override void SetDead()
    {
        base.SetDead();

        int currentHp = WorldScene.Instance.SetGetUserHP(Stat.maxHp);
        SetHP(currentHp);

        if (deadAction != null)
        {
            deadAction.Invoke();
        }
    }

    protected override void OnHitEvent()
    {
        if (weapon != null)
        {
            weapon.PlaySFX();
        }

        if (target != null)
        {
            float dist = (target.Position - Position).magnitude;
            if (dist <= 6f)
            {
                target.OnDamage(Stat.attack);
            }
        }
    }

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

    //public void IncreaseExp(int pAddExpValue)
    //{
    //    if (pAddExpValue <= 0)
    //    {
    //        Debug.LogWarning("Failed : ");
    //        return;
    //    }

    //    int expValue = userExpValue + pAddExpValue;
    //    if (expValue < stat.maxExp)
    //    {
    //        userExpValue = Manager.User.SetUserExp(expValue);
    //        Debug.Log($"userExpValue 증가 : {userExpValue}");
    //    }
    //    else
    //    {
    //        SetLevelUp(); //임시
    //    }
    //    //int level = 1;
    //    //while (true)
    //    //{
    //    //    //Data.StatData statData;
    //    //    //if (Managers.Data.StatDict.TryGetValue(level + 1, out statData) == false)
    //    //    //    break;

    //    //    int temp_totalExp = 100;
    //    //    if (statData.exp < temp_totalExp)
    //    //        break;
    //    //    level++;
    //    //}

    //    //if (level != statData.level)
    //    //{
    //    //    Debug.Log("Level Up!");
    //    //}
    //}

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
