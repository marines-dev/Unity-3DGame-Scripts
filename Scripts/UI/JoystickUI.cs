using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

class JoystickUI : BaseUI, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    enum Control
    {
        /// <sammary>
        /// JoystickUI
        /// </sammary>
        JoystickUI,

        // UIPosition

        // Object
        JoystickUI_Object_GunAttackDeselect, JoystickUI_Object_GunAttackSelect,

        // Button
        JoystickUI_Button_Attack,

        // Image
        JoystickUI_Image_MoveLookArea, JoystickUI_Image_JoystickArea,
        JoystickUI_Image_Background, JoystickUI_Image_Pointer,
        // Text
    }

    float   r;
    float   moveSpeed               = 4f;
    bool    isAttackButtonPressed   = false;
    public Vector2  beginPos { get; private set; }   = Vector3.zero;
    public Vector2  dragPos { get; private set; }    = Vector3.zero;
    Vector2         centerPos                        = Vector3.zero;

    RectTransform backgroundRectTrans   = null;
    RectTransform pointerRectTrans      = null;
    Action<bool> attackButtonAction     = null;

    protected override void BindControls()
    {
        base.BindControls();

        BindControl<Control>();
    }

    protected override void BindEvents()
    {
        base.BindEvents();
        //BindEventControl<Button>(Control.JoystickUI_Button_Attack, OnClick_JoystickUI_Button_Attack);

        EventTrigger eventTrigger = GetControlComponent<Button>(Control.JoystickUI_Button_Attack).gameObject.GetOrAddComponent<EventTrigger>();
        var pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener((e) => OnPointerDown_JoystickUI_Button_Attack());
        eventTrigger.triggers.Add(pointerDown);

        var pointerUp = new EventTrigger.Entry();
        pointerUp.eventID = EventTriggerType.PointerUp;
        pointerUp.callback.AddListener((e) => OnPointerUp_JoystickUI_Button_Attack());
        eventTrigger.triggers.Add(pointerUp);
    }

    protected override void InitUI()
    {
        base.InitUI();
    }

    protected override void OnOpen()
    {
        //
        RectTransform joystickRectTrans = GetControlComponent<RectTransform>(Control.JoystickUI_Image_JoystickArea);
        Vector2 sizeDelta           = new Vector2(Screen.height * 0.5f, Screen.height * 0.5f);
        joystickRectTrans.sizeDelta = sizeDelta;
        //transform.gameObject.GetComponent<RectTransform>().sizeDelta = sizeDelta;

        backgroundRectTrans = GetControlComponent<RectTransform>(Control.JoystickUI_Image_Background);
        pointerRectTrans    = GetControlComponent<RectTransform>(Control.JoystickUI_Image_Pointer);

        backgroundRectTrans.sizeDelta   = new Vector2(Screen.height, Screen.height) * 0.25f;
        pointerRectTrans.sizeDelta      = backgroundRectTrans.sizeDelta * 0.45f;

        centerPos   = pointerRectTrans.position;
        r           = backgroundRectTrans.sizeDelta.x / 2;

        // Attack
        SetActiveControl(Control.JoystickUI_Object_GunAttackDeselect,   isAttackButtonPressed == false);
        SetActiveControl(Control.JoystickUI_Object_GunAttackSelect,     isAttackButtonPressed);
    }

    // Close할 때 실행할 프로세스입니다.
    protected override void OnClose()
    {
        isAttackButtonPressed   = false;
        beginPos                = Vector3.zero;
        dragPos                 = Vector3.zero;
        centerPos               = Vector3.zero;

        backgroundRectTrans = null;
        pointerRectTrans    = null;
        attackButtonAction  = null;
    }

    //void FixedUpdate()
    //{
    //    if (Vector2.Distance(dragPos, beginPos) > 10) // joystick move
    //    {
    //        Vector2 v2 = (dragPos - beginPos).normalized;
    //        float angle = Mathf.Atan2(v2.x, v2.y) * Mathf.Rad2Deg;
    //        angle = angle < 0 ? 360 + angle : angle;
    //        //Vector3 eulerAngles = new Vector3(0, cameraTransform.eulerAngles.y + angle, 0);
    //        //playerCC.transform.eulerAngles = eulerAngles;
    //        //playerCC.Move(player.forward * Time.deltaTime * moveSpeed);

    //        //if (anim)//your ainmation set
    //        //{
    //        //    anim.CrossFade("forward");
    //        //}
    //    }
    //    else // joystick stop
    //    {
    //        //if (anim)//your ainmation set
    //        //{
    //        //    anim.CrossFade("standing");
    //        //}
    //    }
    //    ////Simulated drop
    //    //if (!playerCC.isGrounded)
    //    //{
    //    //    playerCC.Move(new Vector3(0, -10f * Time.deltaTime, 0));
    //    //}

    //    //cameraController.CameraSet();
    //}

    #region Button

    //void OnClick_JoystickUI_Button_Attack()
    //{
    //    Managers.Game.gamePlayer.AnimState = Define.State.Skill;
    //}

    #endregion Button

    #region JoystickUI

    public void SetJoystickController(Action<bool> pAttackBtnAction)
    {
        if(pAttackBtnAction == null)
        {
            Debug.LogWarning("");
            return;
        }

        attackButtonAction = pAttackBtnAction;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        beginPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        dragPos     = eventData.position;
        Vector2 dir = dragPos - beginPos;
        pointerRectTrans.position = Vector2.Distance(dragPos, beginPos) > r ? (centerPos + dir.normalized * r) : (centerPos + dir);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragPos     = Vector2.zero;
        beginPos    = Vector2.zero;
        pointerRectTrans.position = centerPos;
    }

    public void OnPointerDown_JoystickUI_Button_Attack()
    {
        isAttackButtonPressed = true;
        if(attackButtonAction != null)
        {
            attackButtonAction.Invoke(isAttackButtonPressed);
        }
        

        SetActiveControl(Control.JoystickUI_Object_GunAttackDeselect,   isAttackButtonPressed == false);
        SetActiveControl(Control.JoystickUI_Object_GunAttackSelect,     isAttackButtonPressed);
    }

    public void OnPointerUp_JoystickUI_Button_Attack()
    {
        isAttackButtonPressed = false;
        if (attackButtonAction != null)
        {
            attackButtonAction.Invoke(isAttackButtonPressed);
        }

        SetActiveControl(Control.JoystickUI_Object_GunAttackDeselect,   isAttackButtonPressed == false);
        SetActiveControl(Control.JoystickUI_Object_GunAttackSelect,     isAttackButtonPressed);
    }
    #endregion JoystickUI
}
