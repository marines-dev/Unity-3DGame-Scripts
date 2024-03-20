using UnityEngine;

public class LoadingUI : BaseUI<LoadingUI.UI>
{
    public enum UI
    {
        /// <sammary>
        /// LoadingUI
        /// </sammary>
        LoadingUI,

        // UIPosition

        // Object
        //LoadingUI_Object_BGAnimation,

        // Button

        // Image

        // Text

    }

    public bool IsLoadingUI_AnimationCompleted
    {
        get
        {
            Animator anim = GetUIComponent<Animator>(UI.LoadingUI);
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

    //public bool IsLoadingUI_AnimationCompleted()
    //{
    //    Animator anim = GetUIComponent<Animator>(UI.LoadingUI);
    //    if (anim != null)
    //    {
    //        return anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f;
    //    }

    //    return true;
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
