using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public static class ManagerLoader
{
    private static readonly object lockObject = new object();
    private static readonly HashSet<IBaseManager> managerHashSet = new HashSet<IBaseManager>();

    public static void CreateManagers()
    {
        CreateManager<SceneManager>();
        CreateManager<ResourceManager>();
        CreateManager<SystemManager>();
        CreateManager<CameraManager>();
        CreateManager<TableManager>();
        CreateManager<UIManager>();
        CreateManager<BackendManager>();
        CreateManager<GPGSManager>();
        CreateManager<LogInManager>();
        CreateManager<UserManager>();
        //CreateManager<InputManager>();
        //CreateManager<GUIManager>();
    }

    public static TMng CreateManager<TMng>() where TMng : class, IBaseManager, new()
    {
        lock (lockObject)
        {
            /// DeleteManager
            {
                TMng managerToRemove = managerHashSet.FirstOrDefault(m => m is TMng) as TMng;
                if (managerToRemove != null)
                {
                    managerHashSet.Remove(managerToRemove);
                }
            }

            /// CreateManager
            TMng manager = new TMng();
            managerHashSet.Add(manager);
            Debug.Log($"Success : <{typeof(TMng).ToString()}> Manager가 생성되었습니다.");

            return manager;
        }
    }

    public static void ResetManagers()
    {
        lock (lockObject)
        {
            foreach (IBaseManager manager in managerHashSet)
            {
                manager?.OnReset();
                Debug.Log($"Success : <{manager.GetType().ToString()}> Manager가 리셋되었습니다.");
            }
        }
    }
}
