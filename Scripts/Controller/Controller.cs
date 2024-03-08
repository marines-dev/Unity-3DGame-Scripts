using System;
using System.Collections;
using UnityEngine;

public interface IHandler
{
    public void StartController();
    public void ExitController();
}

public interface IJoystickHandler : IHandler
{

}

//public interface ICusorHandler : IHandler
//{
//}

public class Controller : MonoBehaviour, IJoystickHandler
{
    //public float moveSpeed = 4f;
    public bool IsAttackButtonPressed { get; private set; } = false;

    JoystickUI joystickUI_ = null;
    JoystickUI joystickUI
    {
        get
        {
            if (joystickUI_ == null)
            {
                GlobalScene.UIMng.GetOrCreateBaseUI<JoystickUI>();
            }
            return joystickUI_;
        }
    }

    IEnumerator fixedUpdateJoystickControllerCoroutine = null;


    void Start()
    {
        joystickUI_.Close();

        //
        joystickUI_.SetJoystickController(OnJoystickAttackButtionAction);
    }

    void OnDestroy()
    {
        ClearFixedUpdateJoystickControllerCoroutine();

        if (joystickUI_ != null)
        {
            joystickUI_.Close();
            joystickUI_ = null;
        }
    }

    #region Joystick

    void OnJoystickAttackButtionAction(bool pIsDown)
    {
        if (pIsDown)
        {
            GlobalScene.GameMng.playerCtrl.OnAttack();
        }
        else
        {
            GlobalScene.GameMng.playerCtrl.OnReady();
        }
    }

    public void StartController()
    {
        joystickUI.Open();
        FixedUpdateJoystickController();
    }

    public void ExitController()
    {
        joystickUI.Close();
        ClearFixedUpdateJoystickControllerCoroutine();
    }

    public void FixedUpdateJoystickController()
    {
        ClearFixedUpdateJoystickControllerCoroutine();

        fixedUpdateJoystickControllerCoroutine = FixedUpdateJoystickControllerCoroutine();
        StartCoroutine(fixedUpdateJoystickControllerCoroutine);
    }

    void ClearFixedUpdateJoystickControllerCoroutine()
    {
        if (fixedUpdateJoystickControllerCoroutine != null)
        {
            StopCoroutine(fixedUpdateJoystickControllerCoroutine);
            fixedUpdateJoystickControllerCoroutine = null;
        }
    }

    [Obsolete("수정 필요")]
    IEnumerator FixedUpdateJoystickControllerCoroutine()
    {
        while (true)
        {
            if (GlobalScene.GameMng.IsGamePlay)
            {
                if (Vector2.Distance(joystickUI.dragPos, joystickUI.beginPos) > 10) // Move
                {
                    //
                    Vector2 dir = (joystickUI.dragPos - joystickUI.beginPos).normalized;
                    //float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
                    //angle = angle < 0 ? 360 + angle : angle;
                    //Vector3 eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + angle, 0);
                    //Managers.Game.playerCtrl.OnMove(eulerAngles);
                    GlobalScene.GameMng.playerCtrl.OnMove(dir);
                }
                else // Stop
                {
                    GlobalScene.GameMng.playerCtrl.OnStop();
                }
            }

            yield return null;
        }

        yield return null;

        //
        Debug.Log("JoystickController 종료");
        joystickUI.Close();
    }

    #endregion Joystick

}
