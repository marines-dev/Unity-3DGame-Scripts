using UnityEngine;

/// <summary>
/// Default
/// </summary>
public class CamController : BaseHandler
{
    public static CamController CreateCameraController()
    {
        if (CameraManager.CameraModeType != Define.CameraMode.QuarterView)
        {
            Util.LogWarning("");
            return null;
        }

        return Util.CreateGlobalObject<CamController>();
    }

    protected virtual void Awake()
    {
        if (CameraManager.CameraModeType != Define.CameraMode.Defualt)
        {
            Util.LogWarning("");
            GlobalScene.Instance.DestroyGameObject(gameObject);
            return;
        }
    }

    //protected override void OnPlay() { }
    //protected override void OnStop() { }
}

