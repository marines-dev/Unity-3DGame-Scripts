using System;
using System.Collections.Generic;
using System.Linq;
using Interface;
using UnityEngine;
using UnityEngine.EventSystems;
// MainUI /  PopupUI 구분 처리 필요
// UIID로 변경 필요
// EventSystem : Canvas 생성 시 자동 생성 확인 필요

public class UIManager : BaseManager<UIManager>
{
    Dictionary<string, IBaseUI> baseUI_dic = new Dictionary<string, IBaseUI>();
    //List<BaseUI> LoadedBaseUI_list = new List<BaseUI>();

    Canvas mainCanvas = null;
    public Canvas MainCanvas
    {
        get
        {
            if (mainCanvas == null)
            {
                mainCanvas = FindOrCreateMainCanvas();
            }
            return mainCanvas;
        }
    }

    EventSystem mainEventSystem = null;
    public EventSystem MainEventSystem
    {
        get
        {
            if (mainEventSystem == null)
            {
                mainEventSystem = FindOrCreateMainEventSystem();
            }
            return mainEventSystem;
        }
    }


    protected override void OnInitialized()
    {
        mainCanvas = FindOrCreateMainCanvas();
        mainEventSystem = FindOrCreateMainEventSystem();

        SceneManager.Instance.AddSceneLoadedEvent(AddSceneLoadedEvent_UIManager);
    }

    public override void OnReset() 
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

    public TBaseUI CreateOrGetBaseUI<TBaseUI>() where TBaseUI : Component, IBaseUI
    {
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
            baseUI = ResourceManager.Instance.Instantiate(path, MainCanvas.transform).GetOrAddComponent<TBaseUI>();
        }

        ///
        baseUI.Close();
        return baseUI;
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

    [Obsolete("임시")]
    public T CreateBaseSpaceUI<T>(Transform pParent = null) where T : Component, IBaseUI
    {
        string name = typeof(T).Name;
        GameObject go = ResourceManager.Instance.Instantiate($"Prefabs/UI/WorldSpace/{name}", pParent);

        Canvas canvas = go.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;

        T baseUI = go.GetOrAddComponent<T>();
        baseUI.Close();

        return baseUI;
    }

    public void OpenBaseUIAll()
    {
        if (baseUI_dic.Count == 0)
        {
            Debug.Log("열기 위한 UI가 없습니다.");
            return;
        }

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
        if (baseUI_dic.Count == 0)
        {
            Debug.Log("닫기 위한 UI가 없습니다.");
            return;
        }

        foreach (IBaseUI baseUI in baseUI_dic.Values)
        {
            if (baseUI != null)
            {
                baseUI.Close();
            }
        }
    }

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

    private Canvas FindOrCreateMainCanvas()
    {
        Canvas[] canvas_arr = null;
        if (mainCanvas == null)
        {
            canvas_arr = GameObject.FindObjectsOfType<Canvas>();
            Debug.Log($"{typeof(Canvas).Name} 개수 : {canvas_arr.Length}");
            foreach (Canvas canvas in canvas_arr)
            {
                if (canvas != null && canvas.renderMode != RenderMode.WorldSpace)
                {
                    mainCanvas = canvas;
                    break;
                }
            }

            if(mainCanvas == null)
            {
                mainCanvas = Util.CreateGameObject<Canvas>();
            }

            mainCanvas.gameObject.name = $"@{typeof(Canvas).Name}";
        }

        /// 중복 검사
        canvas_arr = GameObject.FindObjectsOfType<Canvas>();
        foreach (Canvas canvas in canvas_arr)
        {
            if (canvas != null && canvas.renderMode != RenderMode.WorldSpace && canvas != mainCanvas)
            {
                ResourceManager.Instance.DestroyGameObject(canvas.gameObject);
            }
        }

        ///
        GameObject.DontDestroyOnLoad(mainCanvas);
        return mainCanvas;
    }

    private EventSystem FindOrCreateMainEventSystem()
    {
        if (mainEventSystem == null)
        {
            mainEventSystem = GameObject.FindObjectOfType<EventSystem>();
            if (mainEventSystem == null)
            {
                mainEventSystem = Util.CreateGameObject<EventSystem>();
            }

            mainEventSystem.gameObject.name = $"@{typeof(EventSystem).Name}";
        }

        /// 중복 검사
        EventSystem[] eventSystem_arr = GameObject.FindObjectsOfType<EventSystem>();
        foreach (EventSystem eventSystem in eventSystem_arr)
        {
            if (eventSystem != null && eventSystem != mainEventSystem)
            {
                ResourceManager.Instance.DestroyGameObject(eventSystem.gameObject);
            }
        }

        ///
        GameObject.DontDestroyOnLoad(mainEventSystem);
        return mainEventSystem;
    }

    private void AddSceneLoadedEvent_UIManager(UnityEngine.SceneManagement.Scene pScene, UnityEngine.SceneManagement.LoadSceneMode pLoadSceneMode)
    {
        mainCanvas = FindOrCreateMainCanvas();
        mainEventSystem = FindOrCreateMainEventSystem();
    }

    //public static void SetUIManager(Canvas pCanvas, EventSystem pEventSystem)
    //{
    //    canvas_ref = pCanvas;
    //    eventSystem_ref = pEventSystem;
    //}

    #endregion Load
}
