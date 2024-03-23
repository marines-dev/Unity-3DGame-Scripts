using System.Collections.Generic;
using System.Linq;
using Interface;
using UnityEngine;


public class Manager
{
    private static SceneManager sceneMng = null;
    public SceneManager SceneMng => sceneMng ?? (sceneMng = CreateManager<SceneManager>());

    private static SystemManager systemMng = null;
    public SystemManager SystemMng => systemMng ?? (systemMng = CreateManager<SystemManager>());

    private static GPGSManager gpgsMng = null;
    public GPGSManager GPGSMng => gpgsMng ?? (gpgsMng = CreateManager<GPGSManager>());

    private static BackendManager backendMng = null;
    public BackendManager BackendMng => backendMng ?? (backendMng = CreateManager<BackendManager>());

    private static LogInManager logInMng = null;
    public LogInManager LogInMng => logInMng ?? (logInMng = CreateManager<LogInManager>());

    private static UserManager userMng = null;
    public UserManager UserMng => userMng ?? (userMng = CreateManager<UserManager>());

    //private static ResourceManager_Legacy resourceMng = null;
    //public ResourceManager_Legacy ResourceMng => resourceMng ?? (resourceMng = CreateManager<ResourceManager_Legacy>());

    //private static TableManager_Legacy tableMng = null;
    //public TableManager_Legacy TableMng => tableMng ?? (tableMng = CreateManager<TableManager_Legacy>());

    //private static UIManager_Legacy uiMng = null;
    //public UIManager_Legacy UIMng => uiMng ?? (uiMng = CreateManager<UIManager_Legacy>());

    private static HashSet<Manager> manager_hashSet = new HashSet<Manager>();


    protected virtual void OnInitialized() { }
    public virtual void OnRelease() { }

    public static void CreateManagers()
    {
        if (!IsActiveScene<InitScene>())
        {
            Util.LogError();
            return;
        }

        sceneMng    = CreateManager<SceneManager>();
        systemMng   = CreateManager<SystemManager>();
        gpgsMng     = CreateManager<GPGSManager>();
        backendMng  = CreateManager<BackendManager>();
        logInMng    = CreateManager<LogInManager>();
        userMng     = CreateManager<UserManager>();
        //resourceMng = CreateManager<ResourceManager_Legacy>();
        //tableMng    = CreateManager<TableManager_Legacy>();
        //uiMng       = CreateManager<UIManager_Legacy>();
    }

    public static void ReleaseManagers()
    {
        if (! IsActiveScene<LoadScene>())
        {
            Util.LogError();
            return;
        }

        foreach (Manager manager in manager_hashSet)
        {
            manager?.OnRelease();
            Util.LogSuccess($"<{manager.GetType().ToString()}> Manager가 리셋되었습니다.");
        }
    }

    private static TMng CreateManager<TMng>() where TMng : Manager, new()
    {
        /// Delete
        {
            TMng removeManager = manager_hashSet.FirstOrDefault(m => m is TMng) as TMng;
            if (removeManager != null)
            {
                Util.LogSuccess($"<{removeManager.GetType()}> Manager를 삭제합니다.");
                manager_hashSet.Remove(removeManager);
                removeManager = null;
            }
        }

        /// New
        TMng manager = new TMng();
        manager_hashSet.Add(manager);
        return manager;
    }

    private static bool IsActiveScene<TScene>() where TScene : IBaseScene
    {
        string sceneName        = typeof(TScene).Name;
        string activeSceneName  = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        return sceneName == activeSceneName;
    }

    public Manager()
    {
        OnInitialized();
        Util.LogSuccess($"<{this.GetType().Name}> Manager가 생성되었습니다.");
    }

    ~Manager()
    {
        Util.LogSuccess($"<{this.GetType().Name}> Manager가 삭제되었습니다.");
    }
}