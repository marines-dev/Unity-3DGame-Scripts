using UnityEngine;


public class CamController : MonoBehaviour
{
    public bool SwitchQuarterViewMoed{ private get; set; }
    private readonly Vector3 deltaPos = Define.followCam_deltaPos;

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
        if (mainCamera_ref != null)
        {
            mainCamera_ref.backgroundColor = Define.cam_backgroundColor;

            mainCamera_ref.transform.localPosition  = Define.cam_initPos;
            mainCamera_ref.transform.localRotation  = Quaternion.Euler(Define.cam_initRot);
            mainCamera_ref.transform.localScale     = Define.cam_initScale;
        }
    }
}

