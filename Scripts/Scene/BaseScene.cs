using System;
using System.Collections.Generic;
using Interface;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene<TScene, TMainUI> : MonoBehaviour, IBaseScene where TScene : BaseScene<TScene, TMainUI> where TMainUI : Component, IMainUI
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

    private static TMainUI mainUI;
    protected TMainUI MainUI
    {
        get
        {
            if (mainUI == null)
            {
                Util.LogError();
                return default;
            }
            return mainUI;
        }
    }
     
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

    private Dictionary<string, IBaseTable> baseTable_dic = new Dictionary<string, IBaseTable>();
    //Dictionary<string, IBaseUI> baseUI_dic = new Dictionary<string, IBaseUI>();

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
                Util.LogWarning($"<{instance.name}>의 Instance가 중복되어 삭제했습니다.");
                ResourceLoader.DestroyGameObject(gameObject);
                instance = null;
            }

            instance = this as TScene;
            Util.LogSuccess($"새로운 <{this}>의 Instance가 등록되었습니다.");
        }

        /// MainUI
        {
            if (mainUI != null)
            {
                Util.LogWarning($"<{mainUI.name}>의 MainUI가 중복되어 삭제합니다.");
                ResourceLoader.DestroyGameObject(gameObject);
                mainUI = default;
            }

            mainUI = UILoader.CreateBaseUI<TMainUI>(MainCanvas);
            mainUI.Close();
        }

        ///
        OnAwake();
    }

    protected virtual void Start()
    {
        mainUI.Open();

        OnStart();
    }

    private void OnDestroy()
    {
        onDestroy();

        if(instance != null)
        {
            ResourceLoader.DestroyGameObject(instance.gameObject);
            instance = null;
        }


        if(mainUI != null)
        {
            ResourceLoader.DestroyGameObject(mainUI.gameObject);
            mainUI = null;
        }

        Util.LogSuccess($"<{this}>이 파괴되고, Instance가 해제되었습니다.");
    }

    protected abstract void OnAwake();
    protected abstract void OnStart();
    protected abstract void onDestroy();

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

    protected TTable CreateOrGetBaseTable<TTable>() where TTable : class, IBaseTable, new()
    {
        TTable baseTable = null;
        string name = typeof(TTable).Name;
        /// Get
        {
            if (baseTable_dic.ContainsKey(name))
            {
                baseTable = baseTable_dic[name] as TTable;
                return baseTable;
            }
        }

        /// Create
        {
            baseTable = TableLoader.CreateBaseTable<TTable>();
            baseTable_dic.Add(name, baseTable);
            return baseTable;
        }
    }

    //protected TBaseUI CreateOrGetBaseUI<TBaseUI>() where TBaseUI : Component, IBaseUI
    //{
    //    /// Get
    //    {
    //        TBaseUI baseUI = null;
    //        baseUI = MainCanvas.transform.GetComponentInChildren<TBaseUI>();
    //        if (baseUI != null) { return baseUI; }
    //    }

    //    /// Create
    //    return UILoader.CreateBaseUI<TBaseUI>(MainCanvas);
    //}

    //public void OpenBaseUIAll()
    //{
    //    foreach (IBaseUI baseUI in baseUI_dic.Values)
    //    {
    //        if (baseUI != null) { baseUI.Open(); }
    //    }
    //}

    //public void CloseBaseUIAll()
    //{
    //    foreach (IBaseUI baseUI in baseUI_dic.Values)
    //    {
    //        if (baseUI != null) { baseUI.Close(); }
    //    }
    //}

    //private void DestroyBaseUIAll()
    //{
    //    if(baseUI_dic == null)
    //    {
    //        Util.LogWarning("");
    //        return;
    //    }    

    //    IBaseUI[] baseTable_arr = baseUI_dic.Values.ToArray();
    //    foreach (IBaseUI baseUI in baseTable_arr)
    //    {
    //        baseUI.DestroySelf();
    //    }

    //    baseUI_dic.Clear();
    //}
}
