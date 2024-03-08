using System;
using System.Collections.Generic;
using Interface;
using UnityEngine;
// MainUI /  PopupUI 구분 처리 필요
// UIID로 변경 필요
// EventSystem : Canvas 생성 시 자동 생성 확인 필요

public class UIManager : BaseManager
{
    Dictionary<string, IBaseUI> baseUI_dic = new Dictionary<string, IBaseUI>();
    //List<BaseUI> LoadedBaseUI_list = new List<BaseUI>();

    Canvas canvas = null;
    public Canvas Canvas
    {
        get
        {
            if (canvas == null)
            {
                LoadCanvas();
            }
            return canvas;
        }
    }

    UnityEngine.EventSystems.EventSystem eventSystem_go_ = null;
    public UnityEngine.EventSystems.EventSystem eventSystem_go
    {
        get
        {
            if (eventSystem_go_ == null)
            {
                LoadEventSystem();
            }

            return eventSystem_go_;
        }
    }

    GameObject uiStorage_go_;
    GameObject UIStorage
    {
        get
        {
            if (uiStorage_go_ == null)
            {
                LoadUIStorage();
            }

            return uiStorage_go_;
        }
    }


    protected override void OnAwake()
    {
        LoadCanvas();
        LoadEventSystem();
        LoadUIStorage();
    }

    protected override void OnInit() { }

    #region BaseUI

    public T GetOrCreateBaseUI<T>() where T : Component, IBaseUI
    {
        T baseUI = GetBaseUI(typeof(T)) as T;
        if (baseUI != null)
        {
            return baseUI;
        }

        string uiName = typeof(T).Name;
        string path = $"Prefabs/UI/{uiName}";
        baseUI = GlobalScene.ResourceMng.Instantiate(path, canvas.transform).GetOrAddComponent<T>();
        baseUI.Close();

        baseUI_dic.Add(uiName, baseUI);
        return baseUI;
    }

    public IBaseUI GetOrCreateBaseUI(Type pType)
    {
        if (pType == null || pType.BaseType != typeof(IBaseUI))
        {
            Debug.LogError("");
            return null;
        }

        IBaseUI baseUI = GetBaseUI(pType);
        if (baseUI != null)
        {
            return baseUI;
        }

        baseUI = GlobalScene.ResourceMng.Instantiate($"Prefabs/UI/{pType}", canvas.transform).GetOrAddComponent(pType) as IBaseUI;
        baseUI.Close();

        string uiName = pType.Name;
        baseUI_dic.Add(uiName, baseUI);
        return baseUI;
    }

    //T GetBaseUI<T>() where T : BaseUI
    //{
    //    if (ContainsBaseUI<T>() == false)
    //    {
    //        Debug.Log("Failed : ");
    //        LoadBaseUI<T>();
    //    }

    //    string typeName = typeof(T).Name;
    //    T baseUI = baseUI_dic[typeName] as T;
    //    return baseUI;
    //}

    IBaseUI GetBaseUI(Type pType)
    {
        string name = pType.Name;
        if (baseUI_dic == null || ! baseUI_dic.ContainsKey(name))
        {
            Debug.Log("Failed : ");
            return null;
        }

        IBaseUI baseUI = baseUI_dic[pType.Name];
        return baseUI;
    }

    [Obsolete("임시")]
    public T CreateBaseSpaceUI<T>(Transform pParent = null, string pName = null) where T : Component, IBaseUI
    {
        GameObject go = GlobalScene.ResourceMng.Instantiate($"Prefabs/UI/WorldSpace/{pName}", pParent);

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

    void LoadCanvas()
    {
        if (canvas != null)
            return;

        canvas = GameObject.FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            canvas = GlobalScene.ResourceMng.Instantiate("Prefabs/UI/Canvas").GetComponent<Canvas>();

        }

        string go_name = $"@{typeof(Canvas).Name}";
        canvas.gameObject.name = go_name;
        canvas.gameObject.SetActive(true);

        //
        DontDestroyOnLoad(canvas);
    }

    [Obsolete("테스트 중")]
    void LoadEventSystem()
    {
        if (eventSystem_go_ != null)
            return;

        eventSystem_go_ = FindObjectOfType<UnityEngine.EventSystems.EventSystem>();
        if (eventSystem_go_ == null)
        {
            eventSystem_go_ = GlobalScene.ResourceMng.Instantiate("Prefabs/UI/EventSystem").GetComponent<UnityEngine.EventSystems.EventSystem>();
        }

        string go_name = $"@{typeof(UnityEngine.EventSystems.EventSystem).Name}";
        eventSystem_go_.gameObject.name = go_name;
        eventSystem_go_.gameObject.SetActive(true);

        //
        DontDestroyOnLoad(eventSystem_go_);
    }

    void LoadUIStorage()
    {
        if (uiStorage_go_ != null)
            return;

        uiStorage_go_ = Canvas.transform.Find(Config.ui_uiStorageName).gameObject;
        if (uiStorage_go_ == null)
        {
            uiStorage_go_ = GlobalScene.ResourceMng.CreateGameObject(Config.ui_uiStorageName, Canvas.transform);
        }

        //
        DontDestroyOnLoad(uiStorage_go_);
    }

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

    #endregion Load
}
