using System;
using System.Collections;
using UnityEngine;


public class GUIManager : BaseManager<GUIManager>
{
    Controller_Legacy controller_go = null;
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

    protected override void OnInitialized() { }
    public override void OnReset()
    {
        ExitJoystickController();
        DestroyController();
    }

    #endregion Override

    #region WorldScene

    public void SetWorldSceneController()
    {
        if(GameManagerEX.Instance.playerCtrl == null)
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
        if (GameManagerEX.Instance.IsGamePlay == false)
        {
            Debug.Log($"Failed : {typeof(Controller_Legacy).Name}를 실행할 수 없습니다.");
            return;
        }

        IJoystickHandler JoystickHandler = controller_go;
        JoystickHandler.StartController();
    }

    public void ExitJoystickController()
    {
        if(controller_go == null)
        {
            Debug.Log($"Failed : 종료할 {typeof(Controller_Legacy).Name}이 없습니다.");
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
        string name = $"@{typeof(Controller_Legacy).Name}";
        controller_go = ResourceManager.Instance.CreateComponentObject<Controller_Legacy>(name, null);
    }

    void DestroyController()
    {
        if (controller_go == null)
        {
            Debug.Log("");
            return;
        }

        ResourceManager.Instance.DestroyGameObject(controller_go.gameObject);
        controller_go = null;
    }

    #endregion Load
}
