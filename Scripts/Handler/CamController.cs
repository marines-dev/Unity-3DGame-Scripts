using UnityEngine;

/// <summary>
/// Default
/// </summary>
public class CamController : MonoBehaviour
{
    public bool SwitchQuarterViewMoed{ private get; set; }

    private Define.CameraMode CameraModeType = Define.CameraMode.Defualt;
    private readonly Vector3 deltaPos = Config.followCam_deltaPos;

    private Camera mainCamera_ref = null;


    public static CamController CreateCameraController()
    {
        return Util.CreateGlobalObject<CamController>();
    }

    private void LateUpdate()
    {
        if (!SwitchQuarterViewMoed)
            return;

        RaycastHit hit;
        if (Physics.Raycast(WorldScene.Instance.PlayerCtrl.Position, deltaPos, out hit, deltaPos.magnitude, 1 << (int)Define.Layer.Block))
        {
            float dist = (hit.point - WorldScene.Instance.PlayerCtrl.Position).magnitude * 0.8f;
            mainCamera_ref.transform.position = WorldScene.Instance.PlayerCtrl.Position + deltaPos.normalized * dist;
        }
        else
        {
            mainCamera_ref.transform.position = WorldScene.Instance.PlayerCtrl.Position + deltaPos;
            mainCamera_ref.transform.LookAt(WorldScene.Instance.PlayerCtrl.Position);
        }
    }

    public void SetCamController(Camera pMainCamera)
    {
        if(pMainCamera == null)
        {
            Util.LogError();
            return;
        }

        mainCamera_ref          = pMainCamera;
        SwitchQuarterViewMoed   = false;

        ResetCameraMode();
    }

    private void ResetCameraMode()
    {
        CameraModeType = Define.CameraMode.Defualt;

        if (mainCamera_ref != null)
        {
            mainCamera_ref.backgroundColor = Config.cam_backgroundColor;

            mainCamera_ref.transform.localPosition  = Config.cam_initPos;
            mainCamera_ref.transform.localRotation  = Quaternion.Euler(Config.cam_initRot);
            mainCamera_ref.transform.localScale     = Config.cam_initScale;
        }
    }

    //protected override void OnPlay() { }
    //protected override void OnStop() { }
}

