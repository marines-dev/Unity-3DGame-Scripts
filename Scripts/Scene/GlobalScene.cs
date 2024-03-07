using System.Collections.Generic;
using UnityEngine;

public class ManagerLoader : MonoBehaviour
{
    static HashSet<BaseManager> manager_hashSet = new HashSet<BaseManager>();

    public T CreateManager<T>() where T : BaseManager
    {
        T manager = GlobalScene.CreateGlobalHandler<T>(transform);
        manager_hashSet.Add(manager);

        return manager;
    }

    public void InitManagers()
    {
        foreach (BaseManager manager in manager_hashSet)
        {
            if (manager != null)
            {
                manager.Initialize();
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

    private ManagerLoader mngLoader = null;
    public static ManagerLoader MngLoader
    {
        get
        {
            if (Instance.mngLoader == null)
            {
                Instance.mngLoader = CreateGlobalHandler<ManagerLoader>();
            }

            return Instance.mngLoader;
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

    private SpawnManager spawnMng = null;
    public static SpawnManager SpawnMng
    {
        get
        {
            if (Instance.spawnMng == null)
            {
                Instance.spawnMng = Instance.mngLoader.CreateManager<SpawnManager>();
            }

            return Instance.spawnMng;
        }
    }

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
        instance = CreateGlobalHandler<GlobalScene>();
    }

    public static T CreateGlobalHandler<T>(Transform pTran = null) where T : Component
    {
        T handler = FindObjectOfType<T>();
        if (handler != null && handler.gameObject != null)
        {
            Destroy(handler.gameObject);
        }

        string handlerName = $"@{typeof(T).Name}";
        handler = new GameObject(handlerName).AddComponent<T>();
        // Global
        DontDestroyOnLoad(handler);

        handler.transform.localPosition = Vector3.zero;
        handler.transform.localRotation = Quaternion.identity;
        handler.transform.SetParent(pTran);

        handler.gameObject.SetActive(true);

        return handler;
    }

    //public static Component CreateHandler(Type pType, Transform pTran = null)
    //{
    //    string handlerName = $"@{pType}";
    //    GameObject go = new GameObject(handlerName);
    //    go.transform.SetParent(pTran);
    //    go.transform.localPosition = Vector3.zero;
    //    go.transform.localRotation = Quaternion.identity;
    //    go.SetActive(true);

    //    return go.AddComponent(pType);
    //}

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
        // LoadMngRepository
        mngLoader = CreateGlobalHandler<ManagerLoader>();

        // ResisteredManagers
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
        spawnMng = mngLoader.CreateManager<SpawnManager>();

    }

    protected override void OnStart() { }
    protected override void OnDestroy_() { }
}