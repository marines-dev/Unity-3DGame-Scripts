using System.Collections.Generic;
using System.Linq;
using Interface;
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
            Debug.Log($"Success : <{typeof(TMng).ToString()}> Manager�� �����Ǿ����ϴ�.");

            return manager;
        }
    }

    public static void ReleaseManagers()
    {
        lock (lockObject)
        {
            foreach (IBaseManager manager in managerHashSet)
            {
                manager?.OnRelease();
                Debug.Log($"Success : <{manager.GetType().ToString()}> Manager�� ���µǾ����ϴ�.");
            }
        }
    }
}
