using System;
using Interface;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene<TScene> : MonoBehaviour, IBaseScene where TScene : BaseScene<TScene>
{
    private static TScene instance;
    public static TScene Instance
    {
        get
        {
            if (instance == null)
            {
                Util.LogError();
                return null;
            }
            return instance;
        }
    }

    //private static TMainUI mainUI;
    //public TMainUI MainUI
    //{
    //    get
    //    {
    //        if (mainUI == null)
    //        {
    //            Util.LogError();
    //            return null;
    //        }
    //        return mainUI;
    //    }
    //}

    #region GlobalObject

    private static Camera mainCamera = null;
    private Camera MainCamera => mainCamera ?? (mainCamera = FindOrCreateGlobalObject<Camera>(mainCamera));

    private static Canvas mainCanvas = null;
    protected static Canvas MainCanvas => mainCanvas ?? (mainCanvas = FindOrCreateGlobalObject<Canvas>(mainCanvas));

    private static EventSystem mainEventSystem = null;
    private static EventSystem MainEventSystem => mainEventSystem ?? (mainEventSystem = FindOrCreateGlobalObject<EventSystem>(mainEventSystem));

    private static Light mainLight = null;
    [Obsolete("임시")] private Light MainLight => mainLight ?? (mainLight = FindOrCreateGlobalObject<Light>(mainLight));

    private static CamController camCtrl = null;
    protected CamController CamCtrl => camCtrl ?? (camCtrl = CamController.CreateCameraController());

    #endregion GlobalObject

    private static Manager manager;
    protected Manager Manager => manager ?? (manager = new Manager());


    protected virtual void Awake()
    {
        if (!InitScene.IsInitSceneLoaded)
        {
            if (! Manager.SceneMng.IsActiveScene<InitScene>())
            {
                Manager.SceneMng.LoadInitScene();
                return;
            }
        }

        ///
        RegisteredGlobalObjects();

        /// Instance : 씬이 생성될 때마다 새로운 인스턴스를 등록합니다.
        {
            if (instance != null)
            {
                Manager.ResourceMng.DestroyGameObject(gameObject);
                instance = null;
                Util.LogSuccess($"<{this}>이 중복되어 삭제하였습니다.");
            }

            instance = this as TScene;
            Util.LogSuccess($"<{this}>을(를) 생성하고, Instance가 등록되었습니다.");
        }

        /////
        //if (mainUI != null)
        //{
        //    Manager.ResourceMng.DestroyGameObject(mainUI.gameObject);
        //    mainUI = null;

        //    Util.LogSuccess($"<{mainUI.GetType().Name}>이 중복되어 삭제하였습니다.");
        //}

        //mainUI = Manager.UIMng.CreateOrGetBaseUI<TMainUI>(MainCanvas);
        //mainUI.Close();

        ///
        OnAwake();
    }

    protected virtual void Start()
    {
        //mainUI.Open();

        OnStart();
    }

    private void OnDestroy()
    {
        OnDestroy_();

        instance = null;
        //mainUI = null;

        Util.LogSuccess($"<{this}>이 파괴되고, Instance가 해제되었습니다.");
    }

    protected abstract void OnAwake();
    protected abstract void OnStart();
    protected abstract void OnDestroy_();

    /// <summary>
    /// 파괴되지 않는 Managers의 오브젝트를 생성하고 반환합니다.
    private Manager CreateManager()
    {
        /// Delete
        if (manager != null)
        {
            manager = null;
        }

        /// New
        manager = new Manager();
        return manager;
    }

    public static void RegisteredGlobalObjects()
    {
        mainCamera      = FindOrCreateGlobalObject<Camera>(mainCamera);
        mainCanvas      = FindOrCreateGlobalObject<Canvas>(mainCanvas);
        mainEventSystem = FindOrCreateGlobalObject<EventSystem>(mainEventSystem);
        mainLight       = FindOrCreateGlobalObject<Light>(mainLight);
        
        if (camCtrl == null)
        {
            camCtrl = FindObjectOfType<CamController>();
            camCtrl = camCtrl ?? (CamController.CreateCameraController());
            camCtrl.SetCamController(mainCamera);
        }

        /// Registered as a Manager.
        //CameraManager.SetCameraManager(MainCamera);
        //UIManager.SetUIManager(MainCanvas, MainEventSystem);
    }

    private static TGlobalObj FindOrCreateGlobalObject<TGlobalObj>(TGlobalObj pGlobalObj) where TGlobalObj : Component
    {
        if (pGlobalObj == null)
        {
            pGlobalObj = GameObject.FindObjectOfType<TGlobalObj>();
            if (pGlobalObj == null)
            {
                Util.LogWarning($"{typeof(TGlobalObj).Name}을(를) 찾을 수 없으므로 새로운 {typeof(TGlobalObj).Name}을 생성합니다.");
                pGlobalObj = Util.CreateGameObject<TGlobalObj>();
            }

            pGlobalObj.gameObject.name = $"@{typeof(TGlobalObj).Name}";

            ///
            GameObject.DontDestroyOnLoad(pGlobalObj);
        }

        /// 중복 검사
        TGlobalObj[] go_arr = GameObject.FindObjectsOfType<TGlobalObj>();
        foreach (TGlobalObj go in go_arr)
        {
            if (go != null && pGlobalObj != go)
            {
                GameObject.Destroy(go.gameObject);
            }
        }

        return pGlobalObj;
    }
}
