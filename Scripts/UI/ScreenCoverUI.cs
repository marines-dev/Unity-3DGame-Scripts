using System.Collections;
using UnityEngine;
using UnityEngine.UI;

class ScreenCoverUI : BaseUI<ScreenCoverUI.UI>
{
    public enum UI
    {
        /// <sammary>
        /// ScreenCoverUI
        /// </sammary>

        // UIPosition

        // Object
        SampleUI_Object_TuchBlocking, SampleUI_Object_LoadingData,

        // Button

        // Image

        // Text

    }

    public enum ScreenCoverType
    {
        TuchBlocking,
        LoadingData,
    }


    //private void Update()
    //{
    //}

    protected override void BindEvents()
    {
        //BindEventControl<Button>(Control., OnClick_);
    }

    protected override void OnAwake() { }
    protected override void OnOpen()
    {
        UpdateScreenCoverUI(ScreenCoverType.TuchBlocking);
    }

    protected override void OnClose() { }

    #region Button

    //void OnClick_()
    //{
    //    Debug.LogWarning("개발 진행 중 입니다.");
    //}

    #endregion Button

    #region ScreenCoverUI
    
    public void SetScreeConverUI(ScreenCoverType _screenCoverType)
    {
        UpdateScreenCoverUI(_screenCoverType);
    }

    void UpdateScreenCoverUI(ScreenCoverType _screenCoverType)
    {
        SetActiveUI(UI.SampleUI_Object_TuchBlocking, _screenCoverType == ScreenCoverType.TuchBlocking);
        SetActiveUI(UI.SampleUI_Object_LoadingData, _screenCoverType == ScreenCoverType.LoadingData);
    }

    #endregion ScreenCoverUI

    #region SampleUI_Panel_

    //void Open_SampleUI_Panel_()
    //{
    //    SetActiveControl(Control.SampleUI_Panel_, true);
    //}

    //void Close_SampleUI_Panel_()
    //{
    //    SetActiveControl(Control.SampleUI_Panel_, false);
    //}

    #endregion SampleUI_Panel_
}
