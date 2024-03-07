using System;
using System.Collections;
using UnityEngine;

[Obsolete("Managers 전용 : 일반 클래스에서 사용할 수 없습니다. Managers를 이용해 주세요.")]
public class GUIManager : BaseManager
{
    Controller controller_go = null;
    //Controller controller_go
    //{
    //    get
    //    {
    //        if (controller_go_pro == null)
    //        {
    //            CreateController();
    //        }
    //        return controller_go_pro;
    //    }
    //}

    #region Override

    protected override void InitDataProcess() { }
    protected override void ResetDataProcess()
    {
        ExitJoystickController();
        DestroyController();
    }

    #endregion Override

    #region WorldScene

    public void SetWorldSceneController()
    {
        if(Managers.Game.playerCtrl == null)
        {
            Debug.LogWarning("Failed : ");
            return;
        }   
        
        CreateController();
        //SetJoystickHandler();
    }

    [Obsolete("플랫폼별 분류")]
    //public void SetJoystickHandler()
    //{
    //    //
    //    IJoystickHandler JoystickHandler = controller_go;
    //}

    public void StartJoystickController()
    {
        if (Managers.Game.IsGamePlay == false)
        {
            Debug.Log($"Failed : {typeof(Controller).Name}를 실행할 수 없습니다.");
            return;
        }

        IJoystickHandler JoystickHandler = controller_go;
        JoystickHandler.StartController();
    }

    public void ExitJoystickController()
    {
        if(controller_go == null)
        {
            Debug.Log($"Failed : 종료할 {typeof(Controller).Name}이 없습니다.");
            return;
        }

        IJoystickHandler JoystickHandler = controller_go;
        JoystickHandler.ExitController();
    }

    #endregion WorldScene

    #region Load

    void CreateController()
    {
        if (controller_go != null)
        {
            Debug.Log("");
            return;
        }

        //DestroyController();
        string name = $"@{typeof(Controller).Name}";
        controller_go = Managers.Resource.CreateComponentObject<Controller>(name, null);
    }

    void DestroyController()
    {
        if (controller_go == null)
        {
            Debug.Log("");
            return;
        }

        Managers.Resource.DestroyGameObject(controller_go.gameObject);
        controller_go = null;
    }

    #endregion Load
}
