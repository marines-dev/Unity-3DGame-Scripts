using System;
using System.Collections;
using UnityEngine;

[Obsolete("Managers ���� : �Ϲ� Ŭ�������� ����� �� �����ϴ�. Managers�� �̿��� �ּ���.")]
public class CameraManager : BaseManager
{
    Define.CameraMode cameraModeType = Define.CameraMode.None;
    Vector3 deltaPos = Vector3.zero;

    //
    Camera  camera = null;
    IEnumerator lateUpdateQuarterViewCamCoroutine = null;

    protected override void InitDataProcess() { }
    protected override void ResetDataProcess()
    {
        ClearLateUpdateQuarterCameraCoroutine();

        cameraModeType = Define.CameraMode.None;
        deltaPos = Vector3.zero;
        camera = null;
    }

    public void SetWorldSceneCamera()
    {
        if(Managers.Scene.currentSceneType != Define.Scene.WorldScene)
        {
            Debug.LogWarning("");
            return;
        }

        cameraModeType = Define.CameraMode.QuarterView;
        deltaPos = Config.camera_deltaPos;
        camera = Camera.main;
        if(camera == null)
        {
            Debug.LogWarning("");
            return;
        }

        LateUpdateQuarterViewCam();
    }

    void LateUpdateQuarterViewCam()
    {
        //
        ClearLateUpdateQuarterCameraCoroutine();
        lateUpdateQuarterViewCamCoroutine = LateUpdateQuarterViewCamCoroutine();
        StartCoroutine(lateUpdateQuarterViewCamCoroutine);
    }

    void ClearLateUpdateQuarterCameraCoroutine()
    {
        if (lateUpdateQuarterViewCamCoroutine != null)
        {
            StopCoroutine(lateUpdateQuarterViewCamCoroutine);
            lateUpdateQuarterViewCamCoroutine = null;
        }
    }

    [Obsolete("�ӽ�")]
    IEnumerator LateUpdateQuarterViewCamCoroutine()
    {
        while (true)
        {
            if(Managers.Game.IsGamePlay) //�ӽ� : ���� ���� ó�� �� ����
            {
                RaycastHit hit;
                if (Physics.Raycast(Managers.Game.playerCtrl.transPosition, deltaPos, out hit, deltaPos.magnitude, 1 << (int)Define.Layer.Block))
                {
                    float dist = (hit.point - Managers.Game.playerCtrl.transPosition).magnitude * 0.8f;
                    camera.transform.position = Managers.Game.playerCtrl.transPosition + deltaPos.normalized * dist;
                }
                else
                {
                    camera.transform.position = Managers.Game.playerCtrl.transPosition + deltaPos;
                    camera.transform.LookAt(Managers.Game.playerCtrl.transPosition);
                }
            }

            yield return null;
        }

        yield return null;
        deltaPos = Vector3.zero;
        cameraModeType = Define.CameraMode.None;
        camera = null;
    }
}