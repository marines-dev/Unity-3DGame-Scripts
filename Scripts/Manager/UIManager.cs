using System;
using System.Collections.Generic;
using System.Linq;
using Interface;
using UnityEngine;
using UnityEngine.EventSystems;


public class UIManager : Manager
{
    //Canvas mainCanvas = null;
    //public Canvas MainCanvas
    //{
    //    get
    //    {
    //        if (mainCanvas == null)
    //        {
    //            mainCanvas = FindOrCreateMainCanvas();
    //        }
    //        return mainCanvas;
    //    }
    //}

    //EventSystem mainEventSystem = null;
    //public EventSystem MainEventSystem
    //{
    //    get
    //    {
    //        if (mainEventSystem == null)
    //        {
    //            mainEventSystem = FindOrCreateMainEventSystem();
    //        }
    //        return mainEventSystem;
    //    }
    //}

    Dictionary<string, IBaseUI> baseUI_dic = new Dictionary<string, IBaseUI>();


    protected override void OnInitialized()
    {
        //mainCanvas = FindOrCreateMainCanvas();
        //mainEventSystem = FindOrCreateMainEventSystem();

        //SceneMng.AddSceneLoadedEvent(AddSceneLoadedEvent_UIManager);
    }

    public override void OnRelease() 
    {
        // DestroyBaseUIAll
        {
            IBaseUI[] baseTable_arr = baseUI_dic.Values.ToArray();
            foreach (IBaseUI baseUI in baseTable_arr)
            {
                baseUI.DestroySelf();
            }

            baseUI_dic.Clear();
        }
    }

    #region BaseUI

    public TBaseUI CreateOrGetBaseUI<TBaseUI>(Canvas pCanvas) where TBaseUI : Component, IBaseUI
    {
        if(pCanvas == null)
        {
            Util.LogError();
            return null;
        }

        TBaseUI baseUI = null;
        string name = typeof(TBaseUI).Name;
        if (baseUI_dic.ContainsKey(name)) /// Get
        {
            baseUI = baseUI_dic[name] as TBaseUI;
        }
        else /// Create
        {
            string uiName = typeof(TBaseUI).Name;
            string path = $"Prefabs/UI/{uiName}";
            baseUI = ResourceMng.Instantiate(path, pCanvas.transform).GetOrAddComponent<TBaseUI>();
            baseUI.gameObject.name = name;
        }

        ///
        baseUI.Close();
        return baseUI;
    }

    public TSpaceUI CreateBaseSpaceUI<TSpaceUI>(Transform pParent = null) where TSpaceUI : Component, IBaseUI
    {
        string name = typeof(TSpaceUI).Name;
        GameObject go = ResourceMng.Instantiate($"Prefabs/UI/WorldSpace/{name}", pParent);

        Canvas canvas = go.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;

        TSpaceUI baseUI = go.GetOrAddComponent<TSpaceUI>();
        baseUI.Close();

        return baseUI;
    }

    public void OpenBaseUIAll()
    {
        foreach (IBaseUI baseUI in baseUI_dic.Values)
        {
            if (baseUI != null)
            {
                baseUI.Open();
            }
        }
    }

    public void CloseBaseUIAll()
    {
        foreach (IBaseUI baseUI in baseUI_dic.Values)
        {
            if (baseUI != null)
            {
                baseUI.Close();
            }
        }
    }

    //public TBaseUI CreateOrGetBaseUI<TBaseUI>() where TBaseUI : Component, IBaseUI
    //{
    //    TBaseUI baseUI = GetBaseUI<TBaseUI>();
    //    if (baseUI != null)
    //    {
    //        return baseUI;
    //    }

    //    string uiName = typeof(TBaseUI).Name;
    //    string path = $"Prefabs/UI/{uiName}";
    //    baseUI = GlobalScene.ResourceMng.Instantiate(path, canvas_ref.transform).GetOrAddComponent<TBaseUI>();
    //    baseUI.Close();

    //    baseUI_dic.Add(uiName, baseUI);
    //    return baseUI;
    //}

    //public IBaseUI CreateOrGetBaseUI(Type pType)
    //{
    //    if (pType == null || pType.BaseType != typeof(IBaseUI))
    //    {
    //        Debug.LogError("");
    //        return null;
    //    }

    //    IBaseUI baseUI = GetBaseUI(pType);
    //    if (baseUI != null)
    //    {
    //        return baseUI;
    //    }

    //    baseUI = GlobalScene.ResourceMng.Instantiate($"Prefabs/UI/{pType}", canvas_ref.transform).GetOrAddComponent(pType) as IBaseUI;
    //    baseUI.Close();

    //    string uiName = pType.Name;
    //    baseUI_dic.Add(uiName, baseUI);
    //    return baseUI;
    //}

    //IBaseUI GetBaseUI(Type pType)
    //{
    //    string name = pType.Name;
    //    if (baseUI_dic == null || !baseUI_dic.ContainsKey(name))
    //    {
    //        Debug.Log("Failed : ");
    //        return null;
    //    }

    //    IBaseUI baseUI = baseUI_dic[name];
    //    return baseUI;
    //}

    #endregion BaseUI

    #region Load

    //string FindBaseUIPath(Define.Resource pResourceType, string pResourceName)
    //{
    //    switch (pResourceType)
    //    {
    //        case Define.Resource.Character:
    //        case Define.Resource.Item:
    //            {
    //                return $"Prefabs/WorldObj/{pResourceType.ToString()}/{pResourceName}";
    //            }
    //        case Define.Resource.UI:
    //            {
    //                return $"Prefabs/UI/{pResourceName}";
    //            }
    //        case Define.Resource.WorldSpace:
    //            {
    //                return $"Prefabs/UI/WorldSpace/{pResourceName}";
    //            }
    //        case Define.Resource.Table:
    //            {
    //                return $"Data/{pResourceType.ToString()}/{pResourceName}";
    //            }

    //    }

    //    Debug.LogWarning($"Failed : ");
    //    return string.Empty;
    //}

    //private Canvas FindOrCreateMainCanvas()
    //{
    //    Canvas[] canvas_arr = null;
    //    if (mainCanvas == null)
    //    {
    //        canvas_arr = GameObject.FindObjectsOfType<Canvas>();
    //        Debug.Log($"{typeof(Canvas).Name} 개수 : {canvas_arr.Length}");
    //        foreach (Canvas canvas in canvas_arr)
    //        {
    //            if (canvas != null && canvas.renderMode != RenderMode.WorldSpace)
    //            {
    //                mainCanvas = canvas;
    //                break;
    //            }
    //        }

    //        if(mainCanvas == null)
    //        {
    //            mainCanvas = Util.CreateGameObject<Canvas>();
    //        }

    //        mainCanvas.gameObject.name = $"@{typeof(Canvas).Name}";
    //    }

    //    /// 중복 검사
    //    canvas_arr = GameObject.FindObjectsOfType<Canvas>();
    //    foreach (Canvas canvas in canvas_arr)
    //    {
    //        if (canvas != null && canvas.renderMode != RenderMode.WorldSpace && canvas != mainCanvas)
    //        {
    //            ResourceMng.DestroyGameObject(canvas.gameObject);
    //        }
    //    }

    //    ///
    //    GameObject.DontDestroyOnLoad(mainCanvas);
    //    return mainCanvas;
    //}

    //private EventSystem FindOrCreateMainEventSystem()
    //{
    //    if (mainEventSystem == null)
    //    {
    //        mainEventSystem = GameObject.FindObjectOfType<EventSystem>();
    //        if (mainEventSystem == null)
    //        {
    //            mainEventSystem = Util.CreateGameObject<EventSystem>();
    //        }

    //        mainEventSystem.gameObject.name = $"@{typeof(EventSystem).Name}";
    //    }

    //    /// 중복 검사
    //    EventSystem[] eventSystem_arr = GameObject.FindObjectsOfType<EventSystem>();
    //    foreach (EventSystem eventSystem in eventSystem_arr)
    //    {
    //        if (eventSystem != null && eventSystem != mainEventSystem)
    //        {
    //            ResourceMng.DestroyGameObject(eventSystem.gameObject);
    //        }
    //    }

    //    ///
    //    GameObject.DontDestroyOnLoad(mainEventSystem);
    //    return mainEventSystem;
    //}

    //private void AddSceneLoadedEvent_UIManager(UnityEngine.SceneManagement.Scene pScene, UnityEngine.SceneManagement.LoadSceneMode pLoadSceneMode)
    //{
    //    mainCanvas = FindOrCreateMainCanvas();
    //    mainEventSystem = FindOrCreateMainEventSystem();
    //}

    //public static void SetUIManager(Canvas pCanvas, EventSystem pEventSystem)
    //{
    //    canvas_ref = pCanvas;
    //    eventSystem_ref = pEventSystem;
    //}

    #endregion Load
}
