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

    Camera mainCamera = null;
    Camera MainCamera
    {
        get
        {
            if (mainCamera == null)
            {
                mainCamera = FindOrCreateMainCamera();
            }
            return mainCamera;
        }
    }

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

    //public static void SetCameraManager(Camera pMainCamera)
    //{
    //    mainCamera_ref = pMainCamera;
    //}

    protected override void OnInitialized()
    {
        mainCamera = FindOrCreateMainCamera();
        ResetCameraMode();

        SceneManager.Instance.AddSceneLoadedEvent(AddSceneLoadedEvent_CameraManager);
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
        CamController generalCamCtrl = CamController.CreateCameraController();
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
        QuarterViewCamController quarterViewCamCtrl = QuarterViewCamController.CreateCameraController();
        quarterViewCamCtrl.SetQuarterViewCam(MainCamera, pTarget);

        cameraCtrl = quarterViewCamCtrl;
    }

    public void PlayQuarterViewCam(bool pSwitch)
    {
        if (CameraModeType != Define.CameraMode.QuarterView)
        {
            Debug.LogWarning("");
            return;
        }

        if(pSwitch)
        {
            CameraCtrl.Play();
        }
        else
        {
            CameraCtrl.Stop();
        }
    }

    private void ResetCameraMode()
    {
        CameraModeType = Define.CameraMode.None;

        if (cameraCtrl != null)
        {
            ResourceManager.Instance.DestroyGameObject(cameraCtrl.gameObject);
            cameraCtrl = null;
        }

        if (MainCamera != null)
        {
            MainCamera.backgroundColor = Config.cam_backgroundColor;

            MainCamera.transform.localPosition = Config.cam_initPos;
            MainCamera.transform.localRotation = Quaternion.Euler(Config.cam_initRot);
            MainCamera.transform.localScale = Config.cam_initScale;
        }
    }

    private Camera FindOrCreateMainCamera()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                mainCamera = Util.CreateGameObject<Camera>();
            }

            mainCamera.gameObject.name = $"@{typeof(Camera).Name}";
        }

        /// 중복 검사
        Camera[] camera_arr = Camera.allCameras;
        Debug.Log($"{typeof(Camera).Name} 개수 : {camera_arr.Length}");
        foreach (Camera camera in camera_arr)
        {
            if (camera != null && mainCamera != camera)
            {
                ResourceManager.Instance.DestroyGameObject(camera.gameObject);
            }
        }

        ///
        GameObject.DontDestroyOnLoad(mainCamera);
        return mainCamera;
    }

    private void AddSceneLoadedEvent_CameraManager(UnityEngine.SceneManagement.Scene pScene, UnityEngine.SceneManagement.LoadSceneMode pLoadSceneMode)
    {
        mainCamera = FindOrCreateMainCamera();
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