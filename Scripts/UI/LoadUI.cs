using Interface;
using UnityEngine;

public class LoadUI : BaseUI<LoadUI.UI>, IMainUI
{
    public enum UI
    {
        /// <sammary>
        /// LoadUI
        /// </sammary>
        LoadUI,

        /// UIPosition

        /// Object

        /// Button

        /// Image

        /// Text

    }

    public bool IsLoadingUI_AnimationCompleted
    {
        get
        {
            Animator anim = GetUIComponent<Animator>(UI.LoadUI);
            if (anim != null)
            {
                bool isCompleted = anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f;
                if(isCompleted) { anim.speed = 0f; }
                
                return isCompleted;
            }
            return true;
        }
    }


    protected override void BindEvents()
    {
        //BindEventControl<Button>(Control., OnClick_);
    }

    protected override void OnAwake() { }
    protected override void OnOpen() { }
    protected override void OnClose() { }

    #region Button
    #endregion Button

    #region LoadingUI
    #endregion LoadingUI
}
