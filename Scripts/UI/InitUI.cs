using Interface;
using UnityEngine;

public class InitUI : BaseUI<SampleMainUI.UI>, IMainUI
{
    public enum UI
    {
        /// <sammary>
        /// InitUI
        /// </sammary>
        InitUI,

        /// UIPosition

        /// Object

        /// Button

        /// Image

        /// Text

    }

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

    #region InitUI

    #endregion InitUI

    #region InitUI_Panel_

    //private void Open_InitUI_Panel_()
    //{
    //    SetActiveUI(UI.InitUI_Panel_, true);
    //}

    //private void Close_GlobalUI_Panel_()
    //{
    //    SetActiveUI(UI.InitUI_Panel_, false);
    //}

    //private void Update_GlobalUI_Panel_()
    //{
    //}

    #endregion InitUI_Panel_
}
