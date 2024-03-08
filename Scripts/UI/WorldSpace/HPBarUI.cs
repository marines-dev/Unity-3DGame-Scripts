using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBarUI : BaseUI<HPBarUI.UI>
{
    public enum UI
    {
        /// <sammary>
        /// SampleUI
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

    // Event를 연결합니다.
    protected override void BindEvents()
    {
        //BindEventControl<Button>(Control., OnClick_);
    }

    // UIPanel을 생성할 때 초기화하는 함수입니다.
    protected override void OnAwake()
    {

        //
        //_stat = transform.parent.GetComponent<Stat>();
    }

    // Open할 때 실행할 프로세스입니다.
    protected override void OnOpen()
    {
        //yield return null;
    }

    // Close할 때 실행할 프로세스입니다.
    protected override void OnClose()
    {
        //yield return null;

        //
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
