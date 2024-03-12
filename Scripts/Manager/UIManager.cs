using System;
using System.Collections.Generic;
using System.Linq;
using Interface;
using UnityEngine;
// MainUI /  PopupUI 구분 처리 필요
// UIID로 변경 필요
// EventSystem : Canvas 생성 시 자동 생성 확인 필요

public class UIManager : BaseManager<UIManager>
{
    Dictionary<string, IBaseUI> baseUI_dic = new Dictionary<string, IBaseUI>();
    //List<BaseUI> LoadedBaseUI_list = new List<BaseUI>();

    private static Canvas canvas_ref = null;
    private static UnityEngine.EventSystems.EventSystem eventSystem_ref = null;


    protected override void OnInitialized()
    {
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

    public static void SetUIManager(Canvas pCanvas, UnityEngine.EventSystems.EventSystem pEventSystem)
    {
        canvas_ref = pCanvas;
        eventSystem_ref = pEventSystem;
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
            baseUI = ResourceManager.Instance.Instantiate(path, canvas_ref.transform).GetOrAddComponent<TBaseUI>();
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
    public T CreateBaseSpaceUI<T>(Transform pParent = null, string pName = null) where T : Component, IBaseUI
    {
        GameObject go = ResourceManager.Instance.Instantiate($"Prefabs/UI/WorldSpace/{pName}", pParent);

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

    //void LoadCanvas()
    //{
    //    if (canvas != null)
    //        return;

    //    canvas = GameObject.FindObjectOfType<Canvas>();
    //    if (canvas == null)
    //    {
    //        canvas = GlobalScene.ResourceMng.Instantiate("Prefabs/UI/Canvas").GetComponent<Canvas>();

    //    }

    //    string go_name = $"@{typeof(Canvas).Name}";
    //    canvas.gameObject.name = go_name;
    //    canvas.gameObject.SetActive(true);

    //    //
    //    DontDestroyOnLoad(canvas);
    //}

    //[Obsolete("테스트 중")]
    //void LoadEventSystem()
    //{
    //    if (eventSystem_go_ != null)
    //        return;

    //    eventSystem_go_ = FindObjectOfType<UnityEngine.EventSystems.EventSystem>();
    //    if (eventSystem_go_ == null)
    //    {
    //        eventSystem_go_ = GlobalScene.ResourceMng.Instantiate("Prefabs/UI/EventSystem").GetComponent<UnityEngine.EventSystems.EventSystem>();
    //    }

    //    string go_name = $"@{typeof(UnityEngine.EventSystems.EventSystem).Name}";
    //    eventSystem_go_.gameObject.name = go_name;
    //    eventSystem_go_.gameObject.SetActive(true);

    //    //
    //    DontDestroyOnLoad(eventSystem_go_);
    //}

    #endregion Load
}
