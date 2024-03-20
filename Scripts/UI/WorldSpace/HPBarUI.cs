using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBarUI : BaseUI<HPBarUI.UI>
{
    public enum UI
    {
        /// <sammary>
        /// HPBarUI
        /// </sammary>

        // UIPosition

        // Object
        HPBarUI_Slider_HPBar

        // Button

        // Image

        // Text

    }

    //Transform parent;
    //Stat stat;

    protected override void BindEvents()
    {
        //BindEventControl<Button>(Control., OnClick_);
    }

    protected override void OnAwake()
    {
        //_stat = transform.parent.GetComponent<Stat>();
    }

    protected override void OnOpen()
    {
    }

    protected override void OnClose()
    {
    }

    public void SetHPBar(Transform pTarget, int stat_hp = 0, int stat_maxHP = 0)
    {
        //parent = _parent;
        //stat = _stat;

        if(pTarget != null)
        {
            transform.position = pTarget.position + Vector3.up * (pTarget.GetComponent<Collider>().bounds.size.y);
            transform.rotation = Camera.main.transform.rotation;
        }

        float ratio = stat_hp / (float)stat_maxHP;
        SetHpRatio(ratio);
    }

    void SetHpRatio(float ratio)
    {
        GetUIComponent<Slider>(UI.HPBarUI_Slider_HPBar).value = ratio;
    }
}
