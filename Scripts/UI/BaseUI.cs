using System;
using System.Collections.Generic;
using Interface;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public abstract class BaseUI<TUI> : MonoBehaviour, IBaseUI where TUI : Enum
{
    //public GameObject GameObject { get { return gameObject; } }
    public bool IsOpen { get { return gameObject.activeSelf; } }

    List<Transform> trans_List  = new List<Transform>();
    Transform[]     uiTrans_arr = null;
    

    private void Awake()
    {
        gameObject.name = GetType().Name; //임시

        BindUI();
        BindEvents();

        ///
        OnAwake();
    }

    protected abstract void BindEvents();
    protected abstract void OnAwake();
    protected abstract void OnOpen();
    protected abstract void OnClose();

    public void Open()
    {
        if (gameObject.activeSelf == false)
        {
            gameObject.SetActive(true);
        }

        OnOpen();
    }

    public void Close()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }

        OnClose();
    }

    public void DestroySelf()
    {
        GlobalScene.Instance.DestroyGameObject(gameObject);
    }

    protected Transform GetUITrans(Define _enum)
    {
        int uiIndex = Convert.ToInt32(_enum);
        Transform trans = uiTrans_arr[uiIndex];

        if (trans == null) { Util.LogWarning($"{trans}는 null입니다."); }

        return trans;
    }

    protected GameObject GetUIObject(Enum _enum)
    {
        int uiIndex = Convert.ToInt32(_enum);
        Transform trans = uiTrans_arr[uiIndex];

        if (trans == null) { Util.LogWarning($"{trans}는 null입니다."); }

        return trans.gameObject;
    }

    protected T GetUIComponent<T>(Enum _enum)
    {
        int uiIndex = Convert.ToInt32(_enum);
        Transform trans = uiTrans_arr[uiIndex];

        if (trans == null) { Util.LogWarning($"{trans}는 null입니다."); }

        T component = trans.GetComponent<T>();
        if(component == null) { Util.LogWarning($"{component.ToString()}의 {typeof(T).Name} 컴포넌트는 존재하지 않습니다."); }

        return component;
    }
    protected void SetActiveUI(Enum _enum, bool _enable)
    {
        GameObject obj = GetUIObject(_enum);
        obj.SetActive(_enable);
    }

    protected void SetImageUI(Enum _enum, string _spritePath, int _spriteIndex)
    {
        Sprite[] arr_sprite = Resources.LoadAll<Sprite>(_spritePath);
        if (arr_sprite == null)
        {
            Util.LogWarning($"{_spritePath} 경로의 리소스를 찾을 수 없습니다.");
            return;
        }

        GameObject  obj     = GetUIObject(_enum);
        Image       image   = obj.GetOrAddComponent<Image>();
        image.sprite        = arr_sprite[_spriteIndex];

        Util.LogWarning($"IndexTest필요");
    }

    protected void SetTextUI(Enum _enum, string _text)
    {
        GameObject  obj  = GetUIObject(_enum);
        TMP_Text    text = obj.GetComponent<TMP_Text>();
        if (text == null) { text = obj.AddComponent<TMP_Text>(); }

        text.text = _text;
    }

    #region Load

    protected void BindEventUI<T>(Enum _enum, UnityAction _action)
    {
        GameObject obj = GetUIObject(_enum);

        // Button
        if (typeof(T) == typeof(Button))
        {
            Button button = obj.GetOrAddComponent<Button>();
            button.onClick.AddListener(_action);
        }
    }

    void BindUI()
    {
        /// GetBaseUITransform
        {
            if (trans_List != null && trans_List.Count != 0)
            {
                trans_List.Clear();
            }

            ReculsiveUI(transform);
        }

        Array   array_control = Enum.GetValues(typeof(TUI));
        int     uiCount       = array_control.Length;
        uiTrans_arr           = new Transform[uiCount];

        foreach (int control in array_control)
        {
            string uiName        = Enum.GetName(typeof(TUI), control);
            Transform childTrans = trans_List.Find(x => x.gameObject.name == uiName);
            int uiIndex = (int)control;

            uiTrans_arr[uiIndex] = childTrans;
        }
    }

    private void ReculsiveUI(Transform _trans)
    {
        int findIndex = trans_List.FindIndex(x => x == _trans);
        if (findIndex == -1) { trans_List.Add(_trans); }
        else { trans_List[findIndex] = _trans; }

        if (_trans.childCount != 0)
        {
            for (int i = 0; i < _trans.childCount; i++)
            {
                Transform childTrans = _trans.GetChild(i);
                ReculsiveUI(childTrans);
            }
        }
    }

    #endregion Load
}
