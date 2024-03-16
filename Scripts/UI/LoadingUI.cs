using UnityEngine;

public class LoadingUI : BaseUI<LoadingUI.UI>
{
    public enum UI
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


    protected override void BindEvents()
    {
        //BindEventControl<Button>(Control., OnClick_);
    }

    protected override void OnAwake()
    {
    }

    protected override void OnOpen()
    {
        //yield return null;

        //
    }

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

    #region LoadingUI

    #endregion LoadingUI
}
