using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ManagerLoader
{
    private HashSet<Manager> manager_hashSet = new HashSet<Manager>();

    public TMng CreateManager<TMng>() where TMng : Manager, new()
    {
        /// Delete
        {
            TMng managerToRemove = manager_hashSet.FirstOrDefault(m => m is TMng) as TMng;
            if (managerToRemove != null)
            {
                Util.LogSuccess($"[{this.GetType()}] - <{managerToRemove.GetType()}> Manager를 삭제합니다.");
                manager_hashSet.Remove(managerToRemove);
                managerToRemove = null;
            }
        }

        /// New
        TMng manager = new TMng();
        manager_hashSet.Add(manager);
        return manager;
    }

    public void ReleaseManagers()
    {
        foreach (Manager manager in manager_hashSet)
        {
            manager?.OnRelease();
            Debug.Log($"Success : <{manager.GetType().ToString()}> Manager가 리셋되었습니다.");
        }
    }
}

public class Manager
{
    static ManagerLoader mngLoader = null;
    public ManagerLoader MngLoader
    {
        get
        {
            if (mngLoader == null)
            {
                mngLoader = null;
                mngLoader = new ManagerLoader();
            }

            return mngLoader;
        }
    }

    static SceneManager sceneMng = null;
    public SceneManager SceneMng => sceneMng ?? (sceneMng = MngLoader.CreateManager<SceneManager>());

    static ResourceManager resourceMng = null;
    public ResourceManager ResourceMng => resourceMng ?? (resourceMng = MngLoader.CreateManager<ResourceManager>());

    static TableManager tableMng = null;
    public TableManager TableMng => tableMng ?? (tableMng = MngLoader.CreateManager<TableManager>());

    static CameraManager camMng = null;
    public CameraManager CamMng => camMng ?? (camMng = MngLoader.CreateManager<CameraManager>());

    static SystemManager systemMng = null;
    public SystemManager SystemMng => systemMng ?? (systemMng = MngLoader.CreateManager<SystemManager>());

    static UIManager uiMng = null;
    public UIManager UIMng => uiMng ?? (uiMng = MngLoader.CreateManager<UIManager>());

    static GPGSManager gpgsMng = null;
    public GPGSManager GPGSMng => gpgsMng ?? (gpgsMng = MngLoader.CreateManager<GPGSManager>());

    static BackendManager backendMng = null;
    public BackendManager BackendMng => backendMng ?? (backendMng = MngLoader.CreateManager<BackendManager>());

    static LogInManager logInMng = null;
    public LogInManager LogInMng => logInMng ?? (logInMng = MngLoader.CreateManager<LogInManager>());

    static UserManager userMng = null;
    public UserManager UserMng => userMng ?? (userMng = MngLoader.CreateManager<UserManager>());


    public Manager()
    {
        OnInitialized();
        Util.LogSuccess($"<{this.GetType().Name}> Manager가 생성되었습니다.");
    }

    ~Manager()
    {
        Util.LogSuccess($"<{this.GetType().Name}> Manager가 삭제되었습니다.");
    }

    protected virtual void OnInitialized()
    {
        Util.LogSuccess($"<{this.GetType().Name}> Manager가 초기화되었습니다.");
    }

    public virtual void OnRelease()
    {
        Util.LogSuccess($"<{this.GetType().Name}> Manager가 리셋되었습니다.");
    }

    public void InitializedManagers()
    {
        if (!SceneMng.IsActiveScene<InitScene>())
        {
            Util.LogWarning("");
            return;
        }

        sceneMng = MngLoader.CreateManager<SceneManager>();
        resourceMng = MngLoader.CreateManager<ResourceManager>();
        tableMng = MngLoader.CreateManager<TableManager>();
        camMng = MngLoader.CreateManager<CameraManager>();
        systemMng = MngLoader.CreateManager<SystemManager>();
        uiMng = MngLoader.CreateManager<UIManager>();
        gpgsMng = MngLoader.CreateManager<GPGSManager>();
        backendMng = MngLoader.CreateManager<BackendManager>();
        logInMng = MngLoader.CreateManager<LogInManager>();
        MngLoader.CreateManager<UserManager>();
    }

    public void ReleaseManagers()
    {
        if (!SceneMng.IsActiveScene<LoadScene>())
        {
            Util.LogWarning("");
            return;
        }

        mngLoader.ReleaseManagers();
    }
}