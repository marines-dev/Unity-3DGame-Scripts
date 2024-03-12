using System;
using UnityEngine;
using UnityEngine.EventSystems;

//public class ManagerLoader_Test : MonoBehaviour
//{
//    static HashSet<BaseManager> manager_hashSet = new HashSet<BaseManager>();

//    public T CreateManager<T>() where T : BaseManager
//    {
//        T manager = Util.CreateGlobalObject<T>(transform);
//        manager_hashSet.Add(manager);

//        return manager;
//    }

//    public void ResetManagers()
//    {
//        foreach (BaseManager manager in manager_hashSet)
//        {
//            if (manager != null)
//            {
//                manager.OnReset();
//            }
//        }
//    }
//}

public static class Global
{
    #region GlobalObject
    static Camera mainCamera = null;
    public static Camera MainCamera
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


    static Canvas mainCanvas = null;
    public static Canvas MainCanvas
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

    static EventSystem mainEventSystem = null;
    public static EventSystem MainEventSystem
    {
        get
        {
            if (mainEventSystem == null)
            {
                mainEventSystem = ResisteredGlobalObject<EventSystem>(mainEventSystem);
            }
            return mainEventSystem;
        }
    }

    static Light mainLight = null;
    [Obsolete("임시")]
    public static Light MainLight
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

    #endregion GlobalObject

    public static void RegisteredGlobalObjects()
    {
        mainCamera = ResisteredGlobalObject<Camera>(mainCamera);
        mainCanvas = ResisteredGlobalObject<Canvas>(mainCanvas);
        mainEventSystem = ResisteredGlobalObject<EventSystem>(mainEventSystem);
        mainLight = ResisteredGlobalObject<Light>(mainLight);

        /// Registered as a Manager.
        CameraManager.SetCameraManager(MainCamera);
        UIManager.SetUIManager(MainCanvas, MainEventSystem);
    }

    static T ResisteredGlobalObject<T>(T pGlobalObj) where T : Component
    {
        if (pGlobalObj == null)
        {
            pGlobalObj = GameObject.FindObjectOfType<T>();
            pGlobalObj.gameObject.name = $"@{typeof(T).Name}";

            ///
            GameObject.DontDestroyOnLoad(pGlobalObj);
        }

        /// 중복 검사
        T[] go_arr = GameObject.FindObjectsOfType<T>();
        Debug.Log($"{typeof(T).Name} 개수 : {go_arr.Length}");
        foreach (T go in go_arr)
        {
            if (go != null && pGlobalObj != go)
            {
                GameObject.Destroy(go.gameObject);
            }
        }

        return pGlobalObj;
    }
}