using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers instance;
    static Managers Instance 
    { 
        get 
        {
            if (instance == null)
            {
                instance = CreateInstance();
            }

            return instance; 
        } 
    }

    #region Manager

    GameManagerEX game = null;
    public static GameManagerEX Game
    { 
        get 
        { 
            if(Instance.game == null)
            {
                Instance.game = CreateManagerInstance<GameManagerEX>();
            }

            return Instance.game; 
        } 
    }

    SceneManager scene = null;
    public static SceneManager Scene
    {
        get
        {
            if (Instance.scene == null)
            {
                Instance.scene = CreateManagerInstance<SceneManager>();
            }

            return Instance.scene;
        }
    }

    UIManager ui = null;
    public static UIManager UI
    {
        get
        {
            if (Instance.ui == null)
            {
                Instance.ui = CreateManagerInstance<UIManager>();
            }

            return Instance.ui;
        }
    }

    ResourceManager resource = null;
    public static ResourceManager Resource
    {
        get
        {
            if (Instance.resource == null)
            {
                Instance.resource = CreateManagerInstance<ResourceManager>();
            }

            return Instance.resource;
        }
    }

    BackendManager backend = null;
    public static BackendManager Backend
    {
        get
        {
            if (Instance.backend == null)
            {
                Instance.backend = CreateManagerInstance<BackendManager>();
            }

            return Instance.backend;
        }
    }

    GPGSManager gpgs = null;
    public static GPGSManager GPGS
    {
        get
        {
            if (Instance.gpgs == null)
            {
                Instance.gpgs = CreateManagerInstance<GPGSManager>();
            }

            return Instance.gpgs;
        }
    }

    LogInManager logIn = null;
    public static LogInManager LogIn
    {
        get
        {
            if (Instance.logIn == null)
            {
                Instance.logIn = CreateManagerInstance<LogInManager>();
            }

            return Instance.logIn;
        }
    }

    TableManager table = null;
    public static TableManager Table
    {
        get
        {
            if (Instance.table == null)
            {
                Instance.table = CreateManagerInstance<TableManager>();
            }

            return Instance.table;
        }
    }

    SpawnManager spawn = null;
    public static SpawnManager Spawn
    {
        get
        {
            if (Instance.spawn == null)
            {
                Instance.spawn = CreateManagerInstance<SpawnManager>();
            }

            return Instance.spawn;
        }
    }

    UserManager user = null;
    public static UserManager User
    {
        get
        {
            if (Instance.user == null)
            {
                Instance.user = CreateManagerInstance<UserManager>();
            }

            return Instance.user;
        }
    }

    CameraManager camera = null;
    public static CameraManager CameraEX
    {
        get
        {
            if (Instance.camera == null)
            {
                Instance.camera = CreateManagerInstance<CameraManager>();
            }

            return Instance.camera;
        }
    }

    GUIManager gui = null;
    public static GUIManager GUI
    {
        get
        {
            if (Instance.gui == null)
            {
                Instance.gui = CreateManagerInstance<GUIManager>();
            }

            return Instance.gui;
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

    static public void InitData()
    {
        instance = CreateInstance();
    }

    /// <summary>
    /// 파괴되지 않는 Managers의 오브젝트를 생성하고 반환합니다.
    static Managers CreateInstance()
    {
        Managers managers_go = GameObject.FindObjectOfType<Managers>();
        if (managers_go == null)
        {
            string managers_name = $"@{typeof(Managers).Name}";
            managers_go = new GameObject { name = managers_name }.AddComponent<Managers>();
        }

        managers_go.gameObject.SetActive(true);
        DontDestroyOnLoad(managers_go);

        return managers_go;
    }
    static void DestroyInstance()
    {
        if (instance != null && instance.gameObject != null)
        {
            Destroy(instance.gameObject);
        }

        instance = null;
    }

    /// <summary>
    /// 파괴되지 않는 BaseManager의 오브젝트를 생성하여 Managers의 자식으로 등록하고 반환합니다.
    /// </summary>
    static T CreateManagerInstance<T>() where T : BaseManager
    {
        T manager_go = GameObject.FindObjectOfType<T>();
        if(manager_go == null)
        {
            string manager_name = $"@{typeof(T).Name}";
            manager_go = new GameObject { name = manager_name }.AddComponent<T>();
        }

        manager_go.gameObject.SetActive(true);
        DontDestroyOnLoad(manager_go);
        manager_go.transform.SetParent(Instance.transform);

        return manager_go;
    }
    static void DestroyManagerInstance<T>() where T : BaseManager
    {
        T manager_go = GameObject.FindObjectOfType<T>();
        if (manager_go != null && manager_go.gameObject != null)
        {
            Destroy(manager_go.gameObject);
        }

        manager_go = null;
    }
}
