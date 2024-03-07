using UnityEngine;

class SampleUI : BaseUI
{
    enum Control
    {
        /// <sammary>
        /// SampleUI
        /// </sammary>

        // UIPosition

        // Object

        // Button

        // Image

        // Text

    }


    protected override void BindControls()
    {
        base.BindControls();

        // Control을 연결합니다.
        BindControl<Control>();
    }

    // Event를 연결합니다.
    protected override void BindEvents()
    {
        base.BindEvents();
        //BindEventControl<Button>(Control., OnClick_);
    }

    // UIPanel을 생성할 때 초기화하는 함수입니다.
    protected override void InitUI()
    {
        base.InitUI();
    }

    // Open할 때 실행할 프로세스입니다.
    protected override void OnOpen()
    {
        //yield return null;

        //
    }

    // Close할 때 실행할 프로세스입니다.
    protected override void OnClose()
    {
        //yield return null;

        //
    }

    //void Update()
    //{
    //}

    #region Button

    //void OnClick_()
    //{
    //    Debug.LogWarning("개발 진행 중 입니다.");
    //}

    #endregion Button

    #region SampleUI

    #endregion SampleUI

    #region SampleUI_Panel_

    //void Open_SampleUI_Panel_()
    //{
    //    SetActiveControl(Control.SampleUI_Panel_, true);
    //}

    //void Close_SampleUI_Panel_()
    //{
    //    SetActiveControl(Control.SampleUI_Panel_, false);
    //}

    //void Update_SampleUI_Panel_()
    //{

    //}

    #endregion SampleUI_Panel_
}
