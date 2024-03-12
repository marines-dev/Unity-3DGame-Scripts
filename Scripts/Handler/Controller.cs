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

public class Controller_Legacy : MonoBehaviour, IJoystickHandler
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
                UIManager.Instance.CreateOrGetBaseUI<JoystickUI>();
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
            GameManagerEX.Instance.playerCtrl.OnAttack();
        }
        else
        {
            GameManagerEX.Instance.playerCtrl.OnReady();
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
            if (GameManagerEX.Instance.IsGamePlay)
            {
                if (Vector2.Distance(joystickUI.dragPos, joystickUI.beginPos) > 10) // Move
                {
                    //
                    Vector2 dir = (joystickUI.dragPos - joystickUI.beginPos).normalized;
                    //float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
                    //angle = angle < 0 ? 360 + angle : angle;
                    //Vector3 eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + angle, 0);
                    //Managers.Game.playerCtrl.OnMove(eulerAngles);
                    GameManagerEX.Instance.playerCtrl.OnMove(dir);
                }
                else // Stop
                {
                    GameManagerEX.Instance.playerCtrl.OnStop();
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
