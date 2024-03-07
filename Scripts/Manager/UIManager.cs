using System;
using System.Collections.Generic;
using UnityEngine;
// MainUI /  PopupUI 구분 처리 필요
// UIID로 변경 필요
// EventSystem : Canvas 생성 시 자동 생성 확인 필요

[Obsolete("Managers 전용 : 일반 클래스에서 사용할 수 없습니다. Managers를 이용해 주세요.")]
public class UIManager : BaseManager
{
    List<BaseUI> list_BaseUI = new List<BaseUI>();

    Canvas canvas_go_ = null;
    public Canvas canvas_go
    {
        get
        {
            if (canvas_go_ == null)
            {
                LoadCanvas();
            }
            return canvas_go_;
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
    GameObject uiIStorage_go
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

    // StaticBaseUI
    public void ResisteredBaseUI()
    {
        if (uiIStorage_go.transform.childCount != 0)
        {
            for (int i = 0; i < uiIStorage_go.transform.childCount; ++i)
            {
                Transform childTrans = uiIStorage_go.transform.GetChild(i);

                SetBaseUI(childTrans.gameObject);
            }
        }
    }

    public void LoadUI<TBaseUI>() where TBaseUI : BaseUI
    {
        BaseUI baseUI = FindBaseUI<TBaseUI>();
        string baseUI_name = typeof(TBaseUI).Name;
        if (baseUI != null)
        {
            Debug.LogWarning($"{baseUI_name} 타입의 BaseUI는 이미 존재합니다.");
            return;
        }

        string path = $"Prefabs/UI/{baseUI_name}";
        GameObject resource = GlobalScene.ResourceMng.InstantiateResource(path, uiIStorage_go.transform);
        SetBaseUI(resource.gameObject);
    }

    public T CreateWorldSpaceUI<T>(Transform parent = null, string name = null) where T : BaseUI
    {
        Debug.Log("HPBarUI가 생성되었습니다.");
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = GlobalScene.ResourceMng.InstantiateResource($"Prefabs/UI/WorldSpace/{name}");
        if (parent != null)
            go.transform.SetParent(parent);

        Canvas canvas = go.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;

        BaseUI uiBase = go.GetOrAddComponent<T>();

        uiBase.Initialized();
        uiBase.Close();

        return Util.GetOrAddComponent<T>(go);
    }

    public void SetBaseUI<T>(T _ui_obj) where T : BaseUI
    {
        if (_ui_obj.transform.parent != uiIStorage_go.transform)
        {
            _ui_obj.transform.SetParent(uiIStorage_go.transform);
        }

        Type type = typeof(T);
        if (_ui_obj.name != type.Name)
        {
            _ui_obj.name = type.Name;
        }

        T ui = _ui_obj.gameObject.GetComponent<T>();
        if (ui == null)
        {
            ui = _ui_obj.gameObject.AddComponent<T>();
        }

        BaseUI uiBase = null;
        int uiBaseIndex = list_BaseUI.FindIndex(x => x.name == ui.name);
        if (uiBaseIndex == -1)
        {
            uiBase = ui;
            list_BaseUI.Add(uiBase);
        }
        else
        {
            uiBase = ui;
            list_BaseUI[uiBaseIndex] = uiBase;
        }

        uiBase.Initialized();

        CloseBaseUI<T>();
    }

    public void SetBaseUI(GameObject _ui_obj)
    {
        if (_ui_obj.transform.parent != uiIStorage_go.transform)
        {
            _ui_obj.transform.SetParent(uiIStorage_go.transform);
        }

        Type type = Type.GetType(_ui_obj.name);
        if (type == null)
        {
            Debug.LogError($"{_ui_obj.name}는 존재하지 않는 UI 이름입니다.");
            return;
        }

        BaseUI uiBase = _ui_obj.GetComponent(type) as BaseUI;
        if (uiBase == null)
        {
            uiBase = _ui_obj.AddComponent(type) as BaseUI;
        }

        int uiBaseIndex = list_BaseUI.FindIndex(x => x.name == uiBase.name);
        if (uiBaseIndex == -1)
        {
            list_BaseUI.Add(uiBase);
        }
        else
        {
            list_BaseUI[uiBaseIndex] = uiBase;
        }

        uiBase.Initialized();

        CloseBaseUI(uiBase.name);
    }

    public T GetBaseUI<T>() where T : BaseUI
    {
        T baseUI = FindBaseUI<T>().GetComponent<T>();

        if (baseUI == null)
        {
            Debug.LogWarning($"Failed : {typeof(T).Name}의 BaseUI는 등록되어 있지 않습니다. 먼저 {typeof(T).Name}를 로드해 주세요.");
            return null;
        }

        return baseUI;
    }

    public BaseUI GetBaseUI(string _uiName)
    {
        Type type = Type.GetType(_uiName);
        BaseUI baseUI = FindBaseUI(_uiName).GetComponent(type) as BaseUI;

        if (baseUI == null)
        {
            Debug.LogWarning("Failed : 얻기 실패 : UIPanel을 찾을 수 없습니다.");
            return null;
        }

        return baseUI;
    }

    BaseUI FindBaseUI<T>() where T : BaseUI
    {
        Type type = typeof(T);
        BaseUI uiBase = list_BaseUI.Find(x => x.name == type.Name);

        return uiBase;
    }

    BaseUI FindBaseUI(string _uiName)
    {
        BaseUI uiBase = list_BaseUI.Find(x => x.name == _uiName);

        return uiBase;
    }

    public void OpenBaseUI<T>() where T : BaseUI
    {
        OpenBaseUI(typeof(T).Name);
    }

    public void OpenBaseUI(string _uiName)
    {
        BaseUI uiBase = FindBaseUI(_uiName);

        if (uiBase == null)
        {
            Debug.LogWarning("Failed : 열기 위한 {0}의 UI를 찾을 수 없습니다.");
            return;
        }

        uiBase.Open();
    }

    public void CloseBaseUI<T>() where T : BaseUI
    {
        CloseBaseUI(typeof(T).Name);
    }

    public void CloseBaseUI(string _uiName)
    {
        BaseUI uiBase = FindBaseUI(_uiName);

        if (uiBase == null)
        {
            Debug.Log("닫을 UI를 찾을 수 없습니다.");
            return;
        }

        uiBase.Close();
    }

    public void OpenBaseUIAll()
    {
        if (list_BaseUI.Count == 0)
        {
            Debug.Log("열기 위한 UI가 없습니다.");
            return;
        }

        foreach (var uiBase in list_BaseUI)
        {
            if (uiBase.gameObject != null)
            {
                uiBase.Open();
            }
        }
    }

    public void CloseBaseUIAll()
    {
        if (list_BaseUI.Count == 0)
        {
            Debug.Log("닫기 위한 UI가 없습니다.");
            return;
        }

        foreach (var uiBase in list_BaseUI)
        {
            if (uiBase != null && uiBase.gameObject != null)
            {
                uiBase.Close();
            }
        }
    }

    #endregion BaseUI

    #region Load

    void LoadCanvas()
    {
        if (canvas_go_ != null)
            return;

        canvas_go_ = GameObject.FindObjectOfType<Canvas>();
        if (canvas_go_ == null)
        {
            canvas_go_ = GlobalScene.ResourceMng.InstantiateResource("Prefabs/UI/Canvas").GetComponent<Canvas>();

        }

        string go_name = $"@{typeof(Canvas).Name}";
        canvas_go_.gameObject.name = go_name;
        canvas_go_.gameObject.SetActive(true);

        //
        DontDestroyOnLoad(canvas_go_);
    }

    [Obsolete("테스트 중")]
    void LoadEventSystem()
    {
        if (eventSystem_go_ != null)
            return;

        eventSystem_go_ = FindObjectOfType<UnityEngine.EventSystems.EventSystem>();
        if (eventSystem_go_ == null)
        {
            eventSystem_go_ = GlobalScene.ResourceMng.InstantiateResource("Prefabs/UI/EventSystem").GetComponent<UnityEngine.EventSystems.EventSystem>();
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

        uiStorage_go_ = canvas_go.transform.Find(Config.ui_uiStorageName).gameObject;
        if (uiStorage_go_ == null)
        {
            uiStorage_go_ = GlobalScene.ResourceMng.CreateGameObject(Config.ui_uiStorageName, canvas_go.transform);
        }

        //
        DontDestroyOnLoad(uiStorage_go_);
    }

    #endregion Load
}
