using Interface;
using UnityEngine;

public class GlobalUI : BaseUI<SampleMainUI.UI>, IMainUI
{
    public enum UI
    {
        /// <sammary>
        /// GlobalUI
        /// </sammary>
        GlobalUI,

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

    #region GlobalUI

    #endregion GlobalUI

    #region GlobalUI_Panel_

    //private void Open_GlobalUI_Panel_()
    //{
    //    SetActiveUI(UI.GlobalUI_Panel_, true);
    //}

    //private void Close_GlobalUI_Panel_()
    //{
    //    SetActiveUI(UI.GlobalUI_Panel_, false);
    //}

    //private void Update_GlobalUI_Panel_()
    //{
    //}

    #endregion InitUI_Panel_
}
