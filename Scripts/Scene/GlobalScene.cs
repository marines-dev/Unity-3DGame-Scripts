using UnityEngine;


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

    #region Manager

    SceneManager sceneMng = null;
    public static SceneManager SceneMng
    {
        get
        {
            if (Instance.sceneMng == null)
            {
                Instance.sceneMng = Instance.CreateGlobalManager<SceneManager>();
            }

            return Instance.sceneMng;
        }
    }

    CameraManager cameraMng = null;
    public static CameraManager CameraMng
    {
        get
        {
            if (Instance.cameraMng == null)
            {
                Instance.cameraMng = Instance.CreateGlobalManager<CameraManager>();
            }

            return Instance.cameraMng;
        }
    }

    GameManagerEX gameMng = null;
    public static GameManagerEX GameMng
    {
        get
        {
            if (Instance.gameMng == null)
            {
                Instance.gameMng = Instance.CreateGlobalManager<GameManagerEX>();
            }

            return Instance.gameMng;
        }
    }

    ResourceManager resourceMng = null;
    public static ResourceManager ResourceMng
    {
        get
        {
            if (Instance.resourceMng == null)
            {
                Instance.resourceMng = Instance.CreateGlobalManager<ResourceManager>();
            }

            return Instance.resourceMng;
        }
    }

    UIManager uiMng = null;
    public static UIManager UIMng
    {
        get
        {
            if (Instance.uiMng == null)
            {
                Instance.uiMng = Instance.CreateGlobalManager<UIManager>();
            }

            return Instance.uiMng;
        }
    }

    BackendManager backendMng = null;
    public static BackendManager BackendMng
    {
        get
        {
            if (Instance.backendMng == null)
            {
                Instance.backendMng = Instance.CreateGlobalManager<BackendManager>();
            }

            return Instance.backendMng;
        }
    }

    GPGSManager gpgsMng = null;
    public static GPGSManager GPGSMng
    {
        get
        {
            if (Instance.gpgsMng == null)
            {
                Instance.gpgsMng = Instance.CreateGlobalManager<GPGSManager>();
            }

            return Instance.gpgsMng;
        }
    }

    LogInManager logInMng = null;
    public static LogInManager LogInMng
    {
        get
        {
            if (Instance.logInMng == null)
            {
                Instance.logInMng = Instance.CreateGlobalManager<LogInManager>();
            }

            return Instance.logInMng;
        }
    }

    TableManager tableMng = null;
    public static TableManager TableMng
    {
        get
        {
            if (Instance.tableMng == null)
            {
                Instance.tableMng = Instance.CreateGlobalManager<TableManager>();
            }

            return Instance.tableMng;
        }
    }

    SpawnManager spawnMng = null;
    public static SpawnManager SpawnMng
    {
        get
        {
            if (Instance.spawnMng == null)
            {
                Instance.spawnMng = Instance.CreateGlobalManager<SpawnManager>();
            }

            return Instance.spawnMng;
        }
    }

    UserManager userMng = null;
    public static UserManager UserMng
    {
        get
        {
            if (Instance.userMng == null)
            {
                Instance.userMng = Instance.CreateGlobalManager<UserManager>();
            }

            return Instance.userMng;
        }
    }

    GUIManager guiMng = null;
    public static GUIManager GUIMng
    {
        get
        {
            if (Instance.guiMng == null)
            {
                Instance.guiMng = Instance.CreateGlobalManager<GUIManager>();
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
        instance = FindObjectOfType<GlobalScene>();
        // Destroy(InitScene에 진입했을 때 GlobalScene을 다시 생성하기 위함)
        if (instance != null && instance.gameObject != null)
        {
            Destroy(instance.gameObject);
        }

        string name = $"@{typeof(GlobalScene).Name}";
        instance = new GameObject(name).AddComponent<GlobalScene>();

        DontDestroyOnLoad(instance);
    }

    protected override void Awake()
    {
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
        sceneMng = CreateGlobalManager<SceneManager>();
    }

    protected override void OnStart() { }
    protected override void OnDestroy_() { }

    T CreateGlobalManager<T>() where T : BaseManager
    {
        T manager = GameObject.FindObjectOfType<T>();
        if (manager != null && manager.gameObject != null)
        {
            Destroy(manager.gameObject);
        }

        string name = $"@{typeof(T).Name}";
        manager = new GameObject(name).AddComponent<T>();

        manager.transform.SetParent(Instance.transform);

        return manager;
    }
}