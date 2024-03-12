using System;
using System.Collections;
using Unity.Android.Types;
using UnityEngine;


public class CameraManager : BaseManager<CameraManager>
{
    //public enum BGColor
    //{
    //    Black,
    //    Blue,
    //    White,
    //}

    public Define.CameraMode CameraModeType { get; private set; } = Define.CameraMode.Defualt;

    static Camera mainCamera_ref = null;
    private CamController cameraCtrl = null;
    private CamController CameraCtrl
    {
        get
        {
            if(cameraCtrl == null)
            {
                CameraModeType = Define.CameraMode.None;
                Debug.LogWarning("");
                SetDefualtCamMode();
            }

            return cameraCtrl;
        }
    }

    public static void SetCameraManager(Camera pMainCamera)
    {
        mainCamera_ref = pMainCamera;
    }

    protected override void OnInitialized()
    {
        ResetCameraMode();
    }

    public override void OnReset()
    {
        ResetCameraMode();
    }

    public void SetDefualtCamMode()
    {
        /// 카메라 초기화 확인
        if (CameraModeType != Define.CameraMode.None)
        {
            Debug.LogWarning("");
            return;
        }

        CameraModeType = Define.CameraMode.Defualt;
        CamController generalCamCtrl = CamController.CreateCamera();

        cameraCtrl = generalCamCtrl;
    }

    public void SetQuarterViewCamMode(Transform pTarget)
    {
        /// 카메라 초기화 확인
        if (CameraModeType != Define.CameraMode.None)
        {
            Debug.LogWarning("");
            return;
        }

        CameraModeType = Define.CameraMode.QuarterView;
        QuarterView_CamController quarterViewCamCtrl = QuarterView_CamController.CreateCamera();
        quarterViewCamCtrl.SetQuarterViewCam(mainCamera_ref, pTarget);

        cameraCtrl = quarterViewCamCtrl;
    }

    public void SwitchQuarterViewCamPlay(bool pSwitch)
    {
        if (CameraModeType != Define.CameraMode.QuarterView)
        {
            Debug.LogWarning("");
            return;
        }

        cameraCtrl.SwitchPlay = pSwitch;
    }

    private void ResetCameraMode()
    {
        CameraModeType = Define.CameraMode.None;

        if (cameraCtrl != null)
        {
            ResourceManager.Instance.DestroyGameObject(cameraCtrl.gameObject);
            cameraCtrl = null;
        }

        if (mainCamera_ref != null)
        {
            mainCamera_ref.backgroundColor = Config.cam_backgroundColor;

            mainCamera_ref.transform.localPosition = Config.cam_initPos;
            mainCamera_ref.transform.localRotation = Quaternion.Euler(Config.cam_initRot);
            mainCamera_ref.transform.localScale = Config.cam_initScale;
        }
    }

    //private void SetPosition(Vector3 pPos)
    //{
    //    mainCamera_ref.transform.localPosition = pPos;
    //}

    //private void SetLookAt(Vector3 targetPos)
    //{
    //    mainCamera_ref.transform.LookAt(targetPos);
    //}

    //private void SetBackgroundColor(BGColor pColor = BGColor.Black)
    //{
    //    Color color = Color.black;
    //    switch (pColor)
    //    {
    //        case BGColor.Black:
    //            {
    //                color = Color.black;
    //            }
    //            break;
    //        case BGColor.Blue:
    //            {
    //                color = Color.blue;
    //            }
    //            break;
    //        case BGColor.White:
    //            {
    //                color = Color.white;
    //            }
    //            break;
    //    }

    //    mainCamera_ref.backgroundColor = color;
    //}
}