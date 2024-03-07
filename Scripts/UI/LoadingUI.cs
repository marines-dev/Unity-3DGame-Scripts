using UnityEngine;

class LoadingUI : BaseUI
{
    enum Control
    {
        /// <sammary>
        /// LoadingUI
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

        BindControl<Control>();
    }

    protected override void BindEvents()
    {
        base.BindEvents();
        //BindEventControl<Button>(Control., OnClick_);
    }

    protected override void InitUI()
    {
        base.InitUI();
    }

    protected override void OpenUIProcess()
    {
        //yield return null;

        //
    }

    protected override void CloseUIProcess()
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

    #region LoadingUI

    #endregion LoadingUI
}
