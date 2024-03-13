using UnityEngine;

/// <summary>
/// Default
/// </summary>
public class CamController : BaseHandler
{
    public static CamController CreateCameraController()
    {
        if (CameraManager.Instance.CameraModeType != Define.CameraMode.QuarterView)
        {
            Debug.LogWarning("");
            return null;
        }

        return Util.CreateGlobalObject<CamController>();
    }

    protected virtual void Awake()
    {
        if (CameraManager.Instance.CameraModeType != Define.CameraMode.Defualt)
        {
            Debug.LogWarning("");
            Destroy(gameObject);
            return;
        }
    }

    //protected override void OnPlay() { }
    //protected override void OnStop() { }
}

