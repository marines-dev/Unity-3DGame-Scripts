using System;
using System.Collections;
using UnityEngine;

[Obsolete("Managers 전용 : 일반 클래스에서 사용할 수 없습니다. Managers를 이용해 주세요.")]
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

    [Obsolete("임시")]
    IEnumerator LateUpdateQuarterViewCamCoroutine()
    {
        while (true)
        {
            if(GlobalScene.GameMng.IsGamePlay) //임시 : 게임 종료 처리 시 수정
            {
                RaycastHit hit;
                if (Physics.Raycast(GlobalScene.GameMng.playerCtrl.transPosition, deltaPos, out hit, deltaPos.magnitude, 1 << (int)Define.Layer.Block))
                {
                    float dist = (hit.point - GlobalScene.GameMng.playerCtrl.transPosition).magnitude * 0.8f;
                    camera.transform.position = GlobalScene.GameMng.playerCtrl.transPosition + deltaPos.normalized * dist;
                }
                else
                {
                    camera.transform.position = GlobalScene.GameMng.playerCtrl.transPosition + deltaPos;
                    camera.transform.LookAt(GlobalScene.GameMng.playerCtrl.transPosition);
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