using UnityEngine;

/// <summary>
/// Default
/// </summary>
public class CamController : BaseHandler
{
    public static CamController CreateCamera()
    {
        if (CameraManager.Instance.CameraModeType != Define.CameraMode.Defualt)
        {
            Debug.LogWarning("");
            return null;
        }

        return Util.CreateGlobalObject<QuarterViewCamController>();
    }

    //protected override void OnPlay() { }
    //protected override void OnStop() { }

    protected virtual void Awake()
    {
        if (CameraManager.Instance.CameraModeType != Define.CameraMode.Defualt)
        {
            Debug.LogWarning("");
            Destroy(gameObject);
            return;
        }
    }
}

