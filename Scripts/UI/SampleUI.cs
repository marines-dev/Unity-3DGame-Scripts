using UnityEngine;

public class SampleUI : BaseUI<SampleUI.UI>
{
    public enum UI
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



    // Event�� �����մϴ�.
    protected override void BindEvents()
    {
        //BindEventControl<Button>(Control., OnClick_);
    }

    // UIPanel�� ������ �� �ʱ�ȭ�ϴ� �Լ��Դϴ�.
    protected override void OnAwake()
    {
    }

    // Open�� �� ������ ���μ����Դϴ�.
    protected override void OnOpen()
    {
        //yield return null;

        //
    }

    // Close�� �� ������ ���μ����Դϴ�.
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
    //    Debug.LogWarning("���� ���� �� �Դϴ�.");
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
