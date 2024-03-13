using System;
using UnityEngine;


public class QuarterViewCamController : CamController
{
    private readonly Vector3 deltaPos = Config.followCam_deltaPos;

    protected Camera followCam_ref = null;
    private Transform target = null;


    public new static QuarterViewCamController CreateCameraController()
    {
        if (CameraManager.Instance.CameraModeType != Define.CameraMode.QuarterView)
        {
            Util.LogWarning();
            return null;
        }

        return Util.CreateGlobalObject<QuarterViewCamController>();
    }

    protected override void Awake()
    {
        if (CameraManager.Instance.CameraModeType != Define.CameraMode.QuarterView)
        {
            Util.LogWarning();
            Destroy(gameObject);
            return;
        }
    }

    private void LateUpdate()
    {
        if (!IsPlaying)
            return;

        if (target != null)
        {
            RaycastHit hit;
            if (Physics.Raycast(target.position, deltaPos, out hit, deltaPos.magnitude, 1 << (int)Define.Layer.Block))
            {
                float dist = (hit.point - target.position).magnitude * 0.8f;
                followCam_ref.transform.position = target.position + deltaPos.normalized * dist;
            }
            else
            {
                followCam_ref.transform.position = target.position + deltaPos;
                followCam_ref.transform.LookAt(target.position);
            }
        }
    }

    //protected override void OnPlay() { }
    //protected override void OnStop() { }

    [Obsolete("인터페이스 연결")]
    public void SetQuarterViewCam(Camera pFollowCam, Transform pTarget)
    {
        if (pFollowCam == null || pTarget == null)
        {
            Util.LogWarning();
            return;
        }

        followCam_ref = pFollowCam;
        target = pTarget;
        Stop();
    }
}