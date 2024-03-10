using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Default
/// </summary>
public class CamController : BaseController
{
    public static CamController CreateCamera()
    {
        if (GlobalScene.CameraMng.CameraModeType != Define.CameraMode.Defualt)
        {
            Debug.LogWarning("");
            return null;
        }

        return Util.CreateGlobalObject<QuarterView_CamController>();
    }

    //protected override void OnPlay() { }
    //protected override void OnStop() { }

    protected virtual void Awake()
    {
        if (GlobalScene.CameraMng.CameraModeType != Define.CameraMode.Defualt)
        {
            Debug.LogWarning("");
            Destroy(gameObject);
            return;
        }
    }
}

public class QuarterView_CamController : CamController
{
    private readonly Vector3 deltaPos = Config.followCam_deltaPos;

    protected Camera followCam_ref = null;
    private Transform temp_target = null;


    public new static QuarterView_CamController CreateCamera()
    {
        if (GlobalScene.CameraMng.CameraModeType != Define.CameraMode.QuarterView)
        {
            Debug.LogWarning("");
            return null;
        }

        return Util.CreateGlobalObject<QuarterView_CamController>();
    }

    protected override void Awake()
    {
        if (GlobalScene.CameraMng.CameraModeType != Define.CameraMode.QuarterView)
        {
            Debug.LogWarning("");
            Destroy(gameObject);
            return;
        }
    }

    private void LateUpdate()
    {
        if (! SwitchPlay)
             return;

        if(temp_target != null)
        {
            RaycastHit hit;
            if (Physics.Raycast(temp_target.position, deltaPos, out hit, deltaPos.magnitude, 1 << (int)Define.Layer.Block))
            {
                float dist = (hit.point - temp_target.position).magnitude * 0.8f;
                followCam_ref.transform.position = temp_target.position + deltaPos.normalized * dist;
            }
            else
            {
                followCam_ref.transform.position = temp_target.position + deltaPos;
                followCam_ref.transform.LookAt(temp_target.position);
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
            Debug.LogWarning("");
            return;
        }

        followCam_ref = pFollowCam;
        temp_target = pTarget;
        SwitchPlay = false;
    }
}