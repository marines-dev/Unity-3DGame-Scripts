using System.Collections.Generic;
using System.Linq;
using Interface;
using UnityEngine;


public class Manager
{
    private static SceneManager sceneMng = null;
    public SceneManager SceneMng => sceneMng ?? (sceneMng = CreateManager<SceneManager>());

    private static ResourceManager resourceMng = null;
    public ResourceManager ResourceMng => resourceMng ?? (resourceMng = CreateManager<ResourceManager>());

    private static TableManager tableMng = null;
    public TableManager TableMng => tableMng ?? (tableMng = CreateManager<TableManager>());

    private static SystemManager systemMng = null;
    public SystemManager SystemMng => systemMng ?? (systemMng = CreateManager<SystemManager>());

    private static UIManager uiMng = null;
    public UIManager UIMng => uiMng ?? (uiMng = CreateManager<UIManager>());

    private static GPGSManager gpgsMng = null;
    public GPGSManager GPGSMng => gpgsMng ?? (gpgsMng = CreateManager<GPGSManager>());

    private static BackendManager backendMng = null;
    public BackendManager BackendMng => backendMng ?? (backendMng = CreateManager<BackendManager>());

    private static LogInManager logInMng = null;
    public LogInManager LogInMng => logInMng ?? (logInMng = CreateManager<LogInManager>());

    private static UserManager userMng = null;
    public UserManager UserMng => userMng ?? (userMng = CreateManager<UserManager>());

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
        resourceMng = CreateManager<ResourceManager>();
        tableMng    = CreateManager<TableManager>();
        systemMng   = CreateManager<SystemManager>();
        uiMng       = CreateManager<UIManager>();
        gpgsMng     = CreateManager<GPGSManager>();
        backendMng  = CreateManager<BackendManager>();
        logInMng    = CreateManager<LogInManager>();
        userMng     = CreateManager<UserManager>();
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
            Util.LogSuccess($"<{manager.GetType().ToString()}> Manager�� ���µǾ����ϴ�.");
        }
    }

    private static TMng CreateManager<TMng>() where TMng : Manager, new()
    {
        /// Delete
        {
            TMng removeManager = manager_hashSet.FirstOrDefault(m => m is TMng) as TMng;
            if (removeManager != null)
            {
                Util.LogSuccess($"<{removeManager.GetType()}> Manager�� �����մϴ�.");
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
        Util.LogSuccess($"<{this.GetType().Name}> Manager�� �����Ǿ����ϴ�.");
    }

    ~Manager()
    {
        Util.LogSuccess($"<{this.GetType().Name}> Manager�� �����Ǿ����ϴ�.");
    }
}