using System;
using Interface;
using UnityEngine;
using static Define;

public class Player : BaseActor, IPlayerCtrl
{
    //int temp_userLevelValue = 0;
    //int temp_userExpValue = 0;

    private ITarget_Temp target = null;

    private Action deadAction = null;


    protected override void Update()
    {
        base.Update();

        if (target != null)
        {
            //
            Vector3 target_dir = target.Position - Position;
            if ((target_dir.magnitude > temp_scanRange) || target.SurvivalStateType == SurvivalState.Dead)
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
                ITarget_Temp tempTarget = WorldScene.Instance.GetTargetCharacter(hitCollider.gameObject);
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

    public void Spawn(Actor pActorType, Action pDeadAction)
    {
        Spawn(pActorType);

        /// Temp
        {
            Temp_Tag = "Player";

            int currentHp = GlobalScene.Instance.UserData.CurrHP;
            SetHP(currentHp);
        }

        deadAction = pDeadAction;
    }

    protected new void Spawn(Actor pActorType, int pActorID = 0)
    {
        base.Spawn(pActorType, pActorID);
    }

    protected override void SetDead()
    {
        base.SetDead();

        GlobalScene.Instance.UpdateUserData_CurrHPValue(Stat.maxHp);
        int currentHp = GlobalScene.Instance.UserData.CurrHP;
        SetHP(currentHp);

        if (deadAction != null)
        {
            deadAction.Invoke();
        }
    }

    protected override void SetAttack()
    {
        temp_isAttacking = true;
        temp_isHit = false;

        PlayUpperAnimation(UpperAnimType, 0.01f, 1f);
        Weap.SetAttackPos();
    }

    public void OnMove(Vector3 pEulerAngles)
    {
        BaseAnimType = Define.BaseAnim.Run;

        //float angle = Mathf.Atan2(pEulerAngles.x, pEulerAngles.y) * Mathf.Rad2Deg;
        //angle = angle < 0 ? 360 + angle : angle;
        //Vector3 eulerAngles = new Vector3(0f, Camera.main.transform.eulerAngles.y + angle, 0f);
        //transform.eulerAngles = eulerAngles;

        transform.eulerAngles = pEulerAngles;
        Vector3 pos = transform.forward * Time.deltaTime * NevAgent.speed;
        NevAgent.Move(pos);
    }

    public void OnStop()
    {
        BaseAnimType = Define.BaseAnim.Idle;
    }

    public void OnReady()
    {
        UpperAnimType = Define.UpperAnim.Ready;
    }

    public void OnAttack()
    {
        UpperAnimType = Define.UpperAnim.Attack;
    }

    protected override void OnHit()
    {
        base.OnHit();

        if (target != null)
        {
            float dist = (target.Position - Position).magnitude;
            if (dist <= 6f)
            {
                target.OnDamage(Stat.attack);
            }
        }
    }

    public void OnIncreaseExp(int pAddExpValue)
    {
        if (pAddExpValue <= 0)
        {
            Util.LogWarning();
            return;
        }

        int userExpValue = GlobalScene.Instance.UserData.ExpValue;
        int expValue = userExpValue + pAddExpValue;
        if (expValue < Config.User_MaxExp_init)
        {
            GlobalScene.Instance.UpdateUserData_ExpValue(expValue);
            userExpValue = GlobalScene.Instance.UserData.ExpValue;
            Util.LogSuccess($"userExpValue 증가 : {userExpValue}");
        }
        else
        {
            SetLevelUp(); //임시
        }
    }

    private void SetLevelUp()
    {
        int userExpValue = GlobalScene.Instance.UserData.ExpValue;
        int maxExp = Config.User_MaxExp_init;
        if (userExpValue < maxExp)
        {
            Debug.LogWarning($"Failed : 플레이어의 Exp({userExpValue})가 MaxExp({maxExp})보다 작아 LevelUp할 수 없습니다.");
            return;
        }

        GlobalScene.Instance.UpdateUserData_LevelUp();
        Util.LogSuccess($"Level Up!(level : {GlobalScene.Instance.UserData.LevelValue}, exp : {GlobalScene.Instance.UserData.ExpValue})");
    }

    //Draw the Box Overlap as a gizmo to show where it currently is testing. Click the Gizmos button to see this
    [Obsolete("테스트")]
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        //if (m_Started)
        //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
        Gizmos.DrawWireSphere(transform.position, temp_scanRange);
        Gizmos.DrawWireCube(transform.position + (transform.forward / 2f) + transform.up, transform.localScale);
        //Debug.DrawRay(transform.position + transform.up, transform.forward * 3f, Color.red);
    }
}
