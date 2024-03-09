using System;
using System.Collections;
using UnityEngine;


public class CameraManager : BaseManager
{
    Define.CameraMode cameraModeType = Define.CameraMode.None;
    Vector3 deltaPos = Vector3.zero;

    Camera mainCamera_ref = null;
    IEnumerator lateUpdateQuarterViewCamCoroutine = null;

    protected override void OnAwake() { }
    protected override void OnInit()
    {
        ClearLateUpdateQuarterCameraCoroutine();

        cameraModeType = Define.CameraMode.None;
        deltaPos = Vector3.zero;
        //camera = null;
    }

    public void SetCameraManager(Camera pMainCamera)
    {
        mainCamera_ref = pMainCamera;
    }

    public void SetWorldSceneCamera()
    {
        if(GlobalScene.SceneMng.IsActiveScene<WorldScene>())
        {
            Debug.LogWarning("");
            return;
        }

        cameraModeType = Define.CameraMode.QuarterView;
        deltaPos = Config.camera_deltaPos;
        //camera = Camera.main;
        if(GetComponent<Camera>() == null)
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
            if(GlobalScene.GameMng.IsGamePlay) //�ӽ� : ���� ���� ó�� �� ����
            {
                RaycastHit hit;
                if (Physics.Raycast(GlobalScene.GameMng.playerCtrl.transPosition, deltaPos, out hit, deltaPos.magnitude, 1 << (int)Define.Layer.Block))
                {
                    float dist = (hit.point - GlobalScene.GameMng.playerCtrl.transPosition).magnitude * 0.8f;
                    GetComponent<Camera>().transform.position = GlobalScene.GameMng.playerCtrl.transPosition + deltaPos.normalized * dist;
                }
                else
                {
                    GetComponent<Camera>().transform.position = GlobalScene.GameMng.playerCtrl.transPosition + deltaPos;
                    GetComponent<Camera>().transform.LookAt(GlobalScene.GameMng.playerCtrl.transPosition);
                }
            }

            yield return null;
        }

        yield return null;
        deltaPos = Vector3.zero;
        cameraModeType = Define.CameraMode.None;
        //camera = null;
    }
}