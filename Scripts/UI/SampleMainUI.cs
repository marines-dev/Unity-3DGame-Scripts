using Interface;
using UnityEngine;

/// ※클래스 이름 변경 시 리소스 이름 변경 필수 입니다.
public class SampleMainUI : BaseUI<SampleMainUI.UI>, IMainUI
{
    public enum UI
    {
        /// <sammary>
        /// SampleUI
        /// </sammary>
        SampleUI,

        /// UIPosition

        /// Object

        /// Button

        /// Image

        /// Text

    }


    //private void Update()
    //{
    //}

    protected override void BindEvents()
    {
        //BindEventControl<Button>(Control., OnClick_);
    }
    protected override void OnAwake() { }
    protected override void OnOpen() { }
    protected override void OnClose() { }

    #region Button

    //private void OnClick_()
    //{
    //    Debug.LogWarning("개발 진행 중 입니다.");
    //}

    #endregion Button

    #region SampleMainUI

    #endregion SampleMainUI

    #region SampleMainUI_Panel_

    //private void Open_SampleMainUI_Panel_()
    //{
    //    SetActiveUI(UI.SampleMainUI_Panel_, true);
    //}

    //private void Close_SampleMainUI_Panel_()
    //{
    //    SetActiveUI(UI.SSampleMainUI_Panel_, false);
    //}

    //private void Update_SampleMainUI_Panel_()
    //{
    //}

    #endregion SampleMainUI_Panel_
}
