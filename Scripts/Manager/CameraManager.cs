using UnityEngine;


public class CameraManager : BaseManager<CameraManager>
{
    public Define.CameraMode CameraModeType { get; private set; } = Define.CameraMode.Defualt;

    private Camera mainCamera = null;
    private Camera MainCamera => mainCamera ?? (mainCamera = FindOrCreateMainCamera());
    private CamController cameraCtrl = null;
    private CamController CameraCtrl
    {
        get
        {
            if(cameraCtrl == null)
            {
                CameraModeType = Define.CameraMode.None;
                Util.LogWarning();
                SetDefualtCamMode();
            }
            return cameraCtrl;
        }
    }

    protected override void OnInitialized()
    {
        mainCamera = FindOrCreateMainCamera();
        ResetCameraMode();

        SceneManager.Instance.AddSceneLoadedEvent(AddSceneLoadedEvent_CameraManager);
    }

    public override void OnRelease()
    {
        ResetCameraMode();
    }

    public void SetDefualtCamMode()
    {
        /// 카메라 초기화 확인
        if (CameraModeType != Define.CameraMode.None)
        {
            Util.LogWarning();
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
            Util.LogWarning();
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
            Util.LogWarning();
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
}