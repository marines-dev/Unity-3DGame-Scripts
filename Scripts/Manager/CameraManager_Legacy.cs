//using UnityEngine;


//public class CameraManager_Legacy : Manager//BaseManager<CameraManager>
//{
//    public static Define.CameraMode CameraModeType { get; private set; } = Define.CameraMode.Defualt;

//    //private Camera mainCamera = null;
//    //private Camera MainCamera => mainCamera ?? (mainCamera = FindOrCreateMainCamera());
//    private Camera mainCamera_ref = null;

//    //private CamController cameraCtrl = null;
//    //private CamController CameraCtrl
//    //{
//    //    get
//    //    {
//    //        if(cameraCtrl == null)
//    //        {
//    //            CameraModeType = Define.CameraMode.None;
//    //            Util.LogWarning();
//    //            SetDefualtCamMode();
//    //        }
//    //        return cameraCtrl;
//    //    }
//    //}

//    public void SetCameraManager()
//    {

//    }

//    protected override void OnInitialized()
//    {
//        //mainCamera = FindOrCreateMainCamera();
//        ResetCameraMode();

//        //SceneMng.AddSceneLoadedEvent(AddSceneLoadedEvent_CameraManager);
//    }

//    public override void OnRelease()
//    {
//        ResetCameraMode();
//    }

//    //public void SetDefualtCamMode()
//    //{
//    //    /// 카메라 초기화 확인
//    //    if (CameraModeType != Define.CameraMode.None)
//    //    {
//    //        Util.LogWarning();
//    //        return;
//    //    }

//    //    CameraModeType = Define.CameraMode.Defualt;
//    //    CamController generalCamCtrl = CamController.CreateCameraController();
//    //    cameraCtrl = generalCamCtrl;
//    //}

//    public void SetQuarterViewCamMode(Transform pTarget)
//    {
//        /// 카메라 초기화 확인
//        if (CameraModeType != Define.CameraMode.None)
//        {
//            Util.LogWarning();
//            return;
//        }

//        CameraModeType = Define.CameraMode.QuarterView;
//        QuarterViewCamController_Legacy quarterViewCamCtrl = QuarterViewCamController_Legacy.CreateCameraController();
//        quarterViewCamCtrl.SetQuarterViewCam(MainCamera, pTarget);

//        cameraCtrl = quarterViewCamCtrl;
//    }

//    public void PlayQuarterViewCam(bool pSwitch)
//    {
//        if (CameraModeType != Define.CameraMode.QuarterView)
//        {
//            Util.LogWarning();
//            return;
//        }

//        if(pSwitch)
//        {
//            CameraCtrl.Play();
//        }
//        else
//        {
//            CameraCtrl.Stop();
//        }
//    }

//    private void ResetCameraMode()
//    {
//        CameraModeType = Define.CameraMode.None;

//        if (cameraCtrl != null)
//        {
//            ResourceMng.DestroyGameObject(cameraCtrl.gameObject);
//            cameraCtrl = null;
//        }

//        if (MainCamera != null)
//        {
//            MainCamera.backgroundColor = Config.cam_backgroundColor;

//            MainCamera.transform.localPosition = Config.cam_initPos;
//            MainCamera.transform.localRotation = Quaternion.Euler(Config.cam_initRot);
//            MainCamera.transform.localScale = Config.cam_initScale;
//        }
//    }

//    //private Camera FindOrCreateMainCamera()
//    //{
//    //    if (mainCamera == null)
//    //    {
//    //        mainCamera = Camera.main;
//    //        if (mainCamera == null)
//    //        {
//    //            mainCamera = Util.CreateGameObject<Camera>();
//    //        }

//    //        mainCamera.gameObject.name = $"@{typeof(Camera).Name}";
//    //    }

//    //    /// 중복 검사
//    //    Camera[] camera_arr = Camera.allCameras;
//    //    foreach (Camera camera in camera_arr)
//    //    {
//    //        if (camera != null && mainCamera != camera)
//    //        {
//    //            ResourceMng.DestroyGameObject(camera.gameObject);
//    //        }
//    //    }

//    //    ///
//    //    GameObject.DontDestroyOnLoad(mainCamera);
//    //    return mainCamera;
//    //}

//    //private void AddSceneLoadedEvent_CameraManager(UnityEngine.SceneManagement.Scene pScene, UnityEngine.SceneManagement.LoadSceneMode pLoadSceneMode)
//    //{
//    //    mainCamera = FindOrCreateMainCamera();
//    //}
//}