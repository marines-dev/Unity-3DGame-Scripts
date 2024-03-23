using Interface;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WorldUI : BaseUI<WorldUI.UI>, IMainUI, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public enum UI
    {
        /// <sammary>
        /// WorldUI
        /// </sammary>
        WorldUI,

        // UIPosition

        // Object
        WorldUI_Object_GunAttackDeselect, WorldUI_Object_GunAttackSelect,

        // Button
        WorldUI_Button_Attack, WorldUI_Button_MoveTitle,

        // Image
        WorldUI_Image_MoveLookArea, WorldUI_Image_JoystickArea,
        WorldUI_Image_Background, WorldUI_Image_Pointer,
        // Text
    }

    private float r = 0f;
    //private float moveSpeed               = 4f;
    private bool isAttackButtonPressed   = false;
    private Vector2 beginPos    = Vector3.zero;
    private Vector2 dragPos     = Vector3.zero;
    private Vector2 centerPos   = Vector3.zero;

    private RectTransform backgroundRectTrans = null;
    private RectTransform pointerRectTrans    = null;


    private void FixedUpdate()
    {
        //if (IsOpen && WorldScene.Instance.IsGamePlay)
        {
            if (Vector2.Distance(dragPos, beginPos) > 10) // Move
            {
                //
                Vector2 dir     = (dragPos - beginPos).normalized;
                float   angle   = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
                angle = angle < 0 ? 360 + angle : angle;
                Vector3 eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + angle, 0);

                ///
                WorldScene.Instance.PlayerCtrl.OnMove(eulerAngles);
            }
            else // Stop
            {
                ///
                WorldScene.Instance.PlayerCtrl.OnStop();
            }
        }
    }

    protected override void BindEvents()
    {
        BindEventUI<Button>(UI.WorldUI_Button_MoveTitle, OnClick_WorldUI_Button_MoveTitle);

        EventTrigger eventTrigger = GetUIComponent<Button>(UI.WorldUI_Button_Attack).gameObject.GetOrAddComponent<EventTrigger>();
        var pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener((e) => OnPointerDown_JoystickUI_Button_Attack());
        eventTrigger.triggers.Add(pointerDown);

        var pointerUp = new EventTrigger.Entry();
        pointerUp.eventID = EventTriggerType.PointerUp;
        pointerUp.callback.AddListener((e) => OnPointerUp_JoystickUI_Button_Attack());
        eventTrigger.triggers.Add(pointerUp);
    }

    protected override void OnAwake()
    {
    }

    protected override void OnOpen()
    {
        //
        RectTransform joystickRectTrans = GetUIComponent<RectTransform>(UI.WorldUI_Image_JoystickArea);
        Vector2 sizeDelta               = new Vector2(Screen.height * 0.5f, Screen.height * 0.5f);
        joystickRectTrans.sizeDelta     = sizeDelta;
        //transform.gameObject.GetComponent<RectTransform>().sizeDelta = sizeDelta;

        backgroundRectTrans = GetUIComponent<RectTransform>(UI.WorldUI_Image_Background);
        pointerRectTrans    = GetUIComponent<RectTransform>(UI.WorldUI_Image_Pointer);

        backgroundRectTrans.sizeDelta   = new Vector2(Screen.height, Screen.height) * 0.25f;
        pointerRectTrans.sizeDelta      = backgroundRectTrans.sizeDelta * 0.45f;

        centerPos   = pointerRectTrans.position;
        r           = backgroundRectTrans.sizeDelta.x / 2;

        // Attack
        SetActiveUI(UI.WorldUI_Object_GunAttackDeselect,   isAttackButtonPressed == false);
        SetActiveUI(UI.WorldUI_Object_GunAttackSelect,     isAttackButtonPressed);
    }

    protected override void OnClose()
    {
        isAttackButtonPressed   = false;
        beginPos                = Vector3.zero;
        dragPos                 = Vector3.zero;
        centerPos               = Vector3.zero;

        backgroundRectTrans = null;
        pointerRectTrans    = null;
    }

    #region Button

    void OnClick_WorldUI_Button_MoveTitle()
    {
        WorldScene.Instance.MoveTitle();
    }

    #endregion Button

    #region JoystickUI

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
        WorldScene.Instance.PlayerCtrl.OnAttack();


        SetActiveUI(UI.WorldUI_Object_GunAttackDeselect,   isAttackButtonPressed == false);
        SetActiveUI(UI.WorldUI_Object_GunAttackSelect,     isAttackButtonPressed);
    }

    public void OnPointerUp_JoystickUI_Button_Attack()
    {
        isAttackButtonPressed = false;
        WorldScene.Instance.PlayerCtrl.OnReady();

        SetActiveUI(UI.WorldUI_Object_GunAttackDeselect,   isAttackButtonPressed == false);
        SetActiveUI(UI.WorldUI_Object_GunAttackSelect,     isAttackButtonPressed);
    }

    //public void SetJoystickController(Action<bool> pAttackBtnAction)
    //{
    //    if(pAttackBtnAction == null)
    //    {
    //        Debug.LogWarning("");
    //        return;
    //    }

    //    attackButtonAction = pAttackBtnAction;
    //}

    #endregion JoystickUI
}
