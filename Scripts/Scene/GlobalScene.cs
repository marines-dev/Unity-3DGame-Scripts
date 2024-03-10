using System;
using System.Collections.Generic;
using UnityEngine;

public class ManagerLoader : MonoBehaviour
{
    static HashSet<BaseManager> manager_hashSet = new HashSet<BaseManager>();

    public T CreateManager<T>() where T : BaseManager
    {
        T manager = Util.CreateGlobalObject<T>(transform);
        manager_hashSet.Add(manager);

        return manager;
    }

    public void ResetManagers()
    {
        foreach (BaseManager manager in manager_hashSet)
        {
            if (manager != null)
            {
                manager.OnReset();
            }
        }
    }
}

public class GlobalScene : BaseScene
{
    private static object lockObject = new object(); // 스레드 세이프를 위한 락 오브젝트
    private static GlobalScene instance;
    private static GlobalScene Instance
    {
        get
        {
            return instance;
        }
    }

    /// <summary>
    /// GlobalObject
    /// </summary>
    Camera mainCamera = null;
    Camera MainCamera
    {
        get
        {
            if (mainCamera == null)
            {
                mainCamera = ResisteredGlobalObject<Camera>(mainCamera);
            }
            return mainCamera;
        }
    }


    Canvas mainCanvas = null;
    Canvas MainCanvas
    {
        get
        {
            if (mainCanvas == null)
            {
                mainCanvas = ResisteredGlobalObject<Canvas>(mainCanvas);
            }
            return mainCanvas;
        }
    }

    UnityEngine.EventSystems.EventSystem mainEventSystem = null;
    UnityEngine.EventSystems.EventSystem MainEventSystem
    {
        get
        {
            if (mainEventSystem == null)
            {
                mainEventSystem = ResisteredGlobalObject<UnityEngine.EventSystems.EventSystem>(mainEventSystem);
            }
            return mainEventSystem;
        }
    }

    Light mainLight = null;
    [Obsolete("임시")] Light MainLight
    {
        get
        {
            if (mainLight == null)
            {
                mainLight = ResisteredGlobalObject<Light>(mainLight);
            }
            return mainLight;
        }
    }

    /// <summary>
    /// Manager
    /// </summary>
    private ManagerLoader mngLoader = null;
    private ManagerLoader MngLoader
    {
        get
        {
            if (mngLoader == null)
            {
                mngLoader = Util.CreateGlobalObject<ManagerLoader>();
            }

            return mngLoader;
        }
    }

    #region Manager

    private SceneManager sceneMng = null;
    public static SceneManager SceneMng
    {
        get
        {
            if (Instance.sceneMng == null)
            {
                Instance.sceneMng = Instance.mngLoader.CreateManager<SceneManager>();
            }

            return Instance.sceneMng;
        }
    }

    private CameraManager cameraMng = null;
    public static CameraManager CameraMng
    {
        get
        {
            if (Instance.cameraMng == null)
            {
                Instance.cameraMng = Instance.mngLoader.CreateManager<CameraManager>();
            }

            return Instance.cameraMng;
        }
    }

    private GameManagerEX gameMng = null;
    public static GameManagerEX GameMng
    {
        get
        {
            if (Instance.gameMng == null)
            {
                Instance.gameMng = Instance.mngLoader.CreateManager<GameManagerEX>();
            }

            return Instance.gameMng;
        }
    }

    private ResourceManager resourceMng = null;
    public static ResourceManager ResourceMng
    {
        get
        {
            if (Instance.resourceMng == null)
            {
                Instance.resourceMng = Instance.mngLoader.CreateManager<ResourceManager>();
            }

            return Instance.resourceMng;
        }
    }

    private UIManager uiMng = null;
    public static UIManager UIMng
    {
        get
        {
            if (Instance.uiMng == null)
            {
                Instance.uiMng = Instance.mngLoader.CreateManager<UIManager>();
            }

            return Instance.uiMng;
        }
    }

    private BackendManager backendMng = null;
    public static BackendManager BackendMng
    {
        get
        {
            if (Instance.backendMng == null)
            {
                Instance.backendMng = Instance.mngLoader.CreateManager<BackendManager>();
            }

            return Instance.backendMng;
        }
    }

    private GPGSManager gpgsMng = null;
    public static GPGSManager GPGSMng
    {
        get
        {
            if (Instance.gpgsMng == null)
            {
                Instance.gpgsMng = Instance.mngLoader.CreateManager<GPGSManager>();
            }

            return Instance.gpgsMng;
        }
    }

    private LogInManager logInMng = null;
    public static LogInManager LogInMng
    {
        get
        {
            if (Instance.logInMng == null)
            {
                Instance.logInMng = Instance.mngLoader.CreateManager<LogInManager>();
            }

            return Instance.logInMng;
        }
    }

    private TableManager tableMng = null;
    public static TableManager TableMng
    {
        get
        {
            if (Instance.tableMng == null)
            {
                Instance.tableMng = Instance.mngLoader.CreateManager<TableManager>();
            }

            return Instance.tableMng;
        }
    }

    //private SpawnManager_Legacy spawnMng = null;
    //public static SpawnManager_Legacy SpawnMng
    //{
    //    get
    //    {
    //        if (Instance.spawnMng == null)
    //        {
    //            Instance.spawnMng = Instance.mngLoader.CreateManager<SpawnManager_Legacy>();
    //        }

    //        return Instance.spawnMng;
    //    }
    //}

    private UserManager userMng = null;
    public static UserManager UserMng
    {
        get
        {
            if (Instance.userMng == null)
            {
                Instance.userMng = Instance.mngLoader.CreateManager<UserManager>();
            }

            return Instance.userMng;
        }
    }

    private GUIManager guiMng = null;
    public static GUIManager GUIMng
    {
        get
        {
            if (Instance.guiMng == null)
            {
                Instance.guiMng = Instance.mngLoader.CreateManager<GUIManager>();
            }

            return Instance.guiMng;
        }
    }

    //InputManager input = null;
    //public static InputManager Input
    //{
    //    get
    //    {
    //        if (Instance.input == null)
    //        {
    //            Instance.input = CreateManagerInstance<InputManager>();
    //        }

    //        return Instance.input;
    //    }
    //}

    #endregion Manager

    public static void CreateGlobalScene()
    {
        instance = Util.CreateGlobalObject<GlobalScene>();
    }

    protected override void Awake()
    {
        // Instance
        lock (lockObject)
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        OnAwake();
    }

    protected override void OnAwake()
    {
        ///
        ResisteredGlobalObjs();

        /// Registered as a Manager.
        CameraManager.SetCameraManager(MainCamera);
        UIManager.SetUIManager(MainCanvas, MainEventSystem);
        SceneManager.SetSceneManager(UnloadedGlobalSceneEvent, LoadedGlobalSceneEvent);

        /// LoadMngRepository
        mngLoader = Util.CreateGlobalObject<ManagerLoader>();

        /// ResisteredManagers
        sceneMng = mngLoader.CreateManager<SceneManager>();
        gameMng = mngLoader.CreateManager<GameManagerEX>();
        cameraMng = mngLoader.CreateManager<CameraManager>();
        
        resourceMng = mngLoader.CreateManager<ResourceManager>();
        tableMng = mngLoader.CreateManager<TableManager>();
        uiMng = mngLoader.CreateManager<UIManager>();
        guiMng = mngLoader.CreateManager<GUIManager>();
        backendMng = mngLoader.CreateManager<BackendManager>();
        gpgsMng = mngLoader.CreateManager<GPGSManager>();
        logInMng = mngLoader.CreateManager<LogInManager>();
        userMng = mngLoader.CreateManager<UserManager>();
        //inputMng = mngLoader.CreateManager<InputManager>();

        /// SceneMng
        //SceneMng.RemoveSceneLoaderEvent(OnSceneUnloaded, OnSceneLoaded);
        //SceneMng.AddSceneLoaderEvent(OnSceneUnloaded, OnSceneLoaded);
    }

    protected override void OnStart() { }
    protected override void OnDestroy_() { }

    //void OnSceneUnloaded(UnityEngine.SceneManagement.Scene pScene)
    //{
    //    MngLoader.ResetManagers();
    //    Debug.Log("Success : OnSceneUnloaded");
    //}

    //void OnSceneLoaded(UnityEngine.SceneManagement.Scene pScene, UnityEngine.SceneManagement.LoadSceneMode pLoadSceneMode)
    //{
    //    ResisteredGlobalObjs();
    //    Debug.Log("Success : OnSceneLoaded");
    //}

    void UnloadedGlobalSceneEvent()
    {
        MngLoader.ResetManagers();
    }

    void LoadedGlobalSceneEvent()
    {
        ResisteredGlobalObjs();
    }

    void ResisteredGlobalObjs()
    {
        mainCamera = ResisteredGlobalObject<Camera>(mainCamera);
        mainCanvas = ResisteredGlobalObject<Canvas>(mainCanvas);
        mainEventSystem = ResisteredGlobalObject<UnityEngine.EventSystems.EventSystem>(mainEventSystem);
        mainLight = ResisteredGlobalObject<Light>(mainLight);
    }

    T ResisteredGlobalObject<T>(T pGlobalObj) where T : Component
    {
        if (pGlobalObj == null)
        {
            pGlobalObj = FindObjectOfType<T>();
            pGlobalObj.gameObject.name = $"@{typeof(T).Name}";

            ///
            DontDestroyOnLoad(pGlobalObj);
        }

        /// 중복 검사
        T[] go_arr = FindObjectsOfType<T>();
        Debug.Log($"{typeof(T).Name} 개수 : {go_arr.Length}");
        foreach (T go in go_arr)
        {
            if (go != null && pGlobalObj != go)
            {
                Destroy(go.gameObject);
            }
        }

        return pGlobalObj;
    }
}