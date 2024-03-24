using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Interface;
using static Define;

public enum TitleUIState
{
    InitData,
    Login,
    LoadData,
}

public enum LoginState
{
    None,
    Login,
    LogOut,
}


public class TitleUI : BaseUI<TitleUI.UI>, IMainUI
{
    public enum UI
    {
        /// <sammary>
        /// TitleUI
        /// </sammary>
        TitleUI,

        /// UIPosition

        /// Object
        TitleUI_Object_MainBG, TitleUI_Object_StartBG, TitleUI_Object_LoadDataBG,
        TitleUI_Object_InitDataProcess, TitleUI_Object_LoginProcess, TitleUI_Object_LoadDataProcess,
        TitleUI_Object_Logout, TitleUI_Object_Login,

        /// Button
        TitleUI_Button_Guest, TitleUI_Button_Google, TitleUI_Button_GameStart, TitleUI_Button_TestLogout,

        /// Slider
        TitleUI_Slider_LoadData,

        /// Image

        /// Text

        /// <sammary>
        /// TitleUI_Panel_PatchPopup
        /// </sammary>
        TitleUI_Panel_PatchPopup,

        /// UIPosition

        /// Object

        /// Button
        PatchPopup_Button_Confirm, PatchPopup_Button_Cancel,

        /// Image

        /// Text


        /// <sammary>
        /// TitleUI_Panel_SignUpPopup
        /// </sammary>
        TitleUI_Panel_SignUpPopup,

        /// UIPosition

        /// Object
        SignUpPopup_Object_Guest, SignUpPopup_Object_Google,

        /// Button
        SignUpPopup_Button_GuestConfirm, SignUpPopup_Button_GuestCancel, SignUpPopup_Button_GoogleConfirm, SignUpPopup_Button_GoogleCancel,

        /// Image

        /// Text

        /// <sammary>
        /// TitleUI_Panel_NicknamePopup
        /// </sammary>
        TitleUI_Panel_NicknamePopup,

        /// UIPosition

        /// Object

        /// Button
        NicknamePopup_Button_Confirm,

        /// InputField
        NicknamePopup_InputField_Nickname,

        /// Image

        /// Text

        /// <sammary>
        /// TitleUI_Panel_GuestLogoutPopup
        /// </sammary>
        TitleUI_Panel_GuestLogoutPopup,

        /// UIPosition

        /// Object

        /// Button
        GuestLogoutPopup_Button_Confirm, GuestLogoutPopup_Button_Cancel,

        /// Image

        /// Text

        ///// <sammary>
        ///// TitleUI_Panel_DebugTestPopup
        ///// </sammary>
        //TitleUI_Panel_DebugTestPopup,

        ///// UIPosition

        ///// Object
        //DebugTestPopup_Object_SelectAble, DebugTestPopup_Object_SelectDesable,
        //DebugTestPopup_Object_DestPopup, DebugTestPopup_Object_TestLogout,

        ///// Button
        //DebugTestPopup_Button_Select, DebugTestPopup_Button_TestLogout,

        ///// Image

        ///// Text
        //DebugTestPopup_Text_Desc,
    }

    //public AccountType  selectAccountType   { get; private set; } = AccountType.None;
    //public string       inputNickname       { get; private set; } = string.Empty;
    public bool         IsTitleUI_AnimationCompleted
    {
        get
        {
            Animator anim = GetUIComponent<Animator>(UI.TitleUI);
            if (anim != null)
            {
                bool isCompleted = anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f;
                return isCompleted;
            }
            return true;
        }
    }

    //bool debugSelectAble = true;


    protected override void BindEvents()
    {
        BindEventUI<Button>(UI.TitleUI_Button_Guest,             OnClick_TitleUI_Button_Guest);
        BindEventUI<Button>(UI.TitleUI_Button_Google,            OnClick_TitleUI_Button_Google);
        BindEventUI<Button>(UI.TitleUI_Button_GameStart,         OnClick_TitleUI_Button_GameStart);
        BindEventUI<Button>(UI.TitleUI_Button_TestLogout,        OnClick_TitleUI_Button_TestLogout);
        BindEventUI<Button>(UI.PatchPopup_Button_Confirm,        OnClick_PatchPopup_Button_Confirm);
        BindEventUI<Button>(UI.PatchPopup_Button_Cancel,         OnClick_PatchPopup_Button_Cancel);
        BindEventUI<Button>(UI.SignUpPopup_Button_GuestConfirm,  OnClick_SignUpPopup_Button_GuestConfirm);
        BindEventUI<Button>(UI.SignUpPopup_Button_GuestCancel,   OnClick_SignUpPopup_Button_GuestCancel);
        BindEventUI<Button>(UI.SignUpPopup_Button_GoogleConfirm, OnClick_SignUpPopup_Button_GoogleConfirm);
        BindEventUI<Button>(UI.SignUpPopup_Button_GoogleCancel,  OnClick_SignUpPopup_Button_GoogleCancel);
        BindEventUI<Button>(UI.NicknamePopup_Button_Confirm,     OnClick_NicknamePopup_Button_Confirm);
        BindEventUI<Button>(UI.GuestLogoutPopup_Button_Confirm,  OnClick_GuestLogoutPopup_Button_Confirm);
        BindEventUI<Button>(UI.GuestLogoutPopup_Button_Cancel,   OnClick_GuestLogoutPopup_Button_Cancel);
        //BindEventUI<Button>(UI.DebugTestPopup_Button_Select,     OnClick_DebugTestPopup_Button_Select);
    }

    protected override void OnAwake() {}

    protected override void OnOpen()
    {
        //debugSelectAble     = true;

        // Close
        CloseTitleUI_Panel_SignUpPopup();
        CloseTitleUI_Panel_NicknamePopup();
        CloseTitleUI_Panel_GuestLogoutPopup();
        //CloseTitleUI_Panel_DebugTestPopup();

        //UpdateTitleUI(TitleUIState.InitData);
        //OpenTitleUI_Panel_DebugTestPopup();
        //OnClick_DebugTestPopup_Button_Select(); //임시
    }

    protected override void OnClose() { }

    #region Button

    void OnClick_TitleUI_Button_Guest()
    {
        OpenTitleUI_Panel_SignUpPopup(Account.Guest);
    }

    void OnClick_TitleUI_Button_Google()
    {
        TitleScene.Instance.SetSinUp(Account.Google);
    }

    void OnClick_TitleUI_Button_GameStart()
    {
        TitleScene.Instance.SetGameStart();
    }

    void OnClick_TitleUI_Button_TestLogout()
    {
        switch (TitleScene.Instance.AccountType)
        {
            case Account.Guest:
                {
                    OpenTitleUI_Panel_GuestLogoutPopup();
                }
                break;

            case Account.Google:
                {
                    Util.LogWarning("개발 진행 중 입니다.");
                    //LoginManager.Instance.SetLogOut(LoginManager.AccountType.Google);
                }
                break;

            default:
                Util.LogError();
                break;
        }
    }

    void OnClick_PatchPopup_Button_Confirm()
    {
        Util.LogWarning("개발 진행 중 입니다.");
    }

    void OnClick_PatchPopup_Button_Cancel()
    {
        Util.LogWarning("개발 진행 중 입니다.");
    }

    void OnClick_SignUpPopup_Button_GuestConfirm()
    {
        TitleScene.Instance.SetSinUp(Account.Guest);
        CloseTitleUI_Panel_SignUpPopup();
    }

    void OnClick_SignUpPopup_Button_GuestCancel()
    {
        CloseTitleUI_Panel_SignUpPopup();
    }

    void OnClick_SignUpPopup_Button_GoogleConfirm()
    {
        CloseTitleUI_Panel_SignUpPopup();
    }

    void OnClick_SignUpPopup_Button_GoogleCancel()
    {
        CloseTitleUI_Panel_SignUpPopup();
    }

    void OnClick_NicknamePopup_Button_Confirm()
    {
        TMP_InputField inputField = GetUIObject(UI.NicknamePopup_InputField_Nickname).GetComponent<TMP_InputField>();
        if (inputField == null)
            return;

        string nickname = inputField.text;
        if (!TitleScene.Instance.CreateNickName(nickname))
        {
            Util.LogWarning(); //임시
            return;
        }

        CloseTitleUI_Panel_NicknamePopup();
    }

    void OnClick_GuestLogoutPopup_Button_Confirm()
    {
        TitleScene.Instance.SetLogOut();
        CloseTitleUI_Panel_GuestLogoutPopup();
    }

    void OnClick_GuestLogoutPopup_Button_Cancel()
    {
        CloseTitleUI_Panel_GuestLogoutPopup();
    }

    //void OnClick_DebugTestPopup_Button_Select()
    //{
    //    debugSelectAble = !debugSelectAble;
    //    UpdateTitleUI_Panel_DebugTestPopup(debugSelectAble);
    //}

    #endregion Button

    #region TitleUI

    public void SetTitleUI_InitData()
    {
        UpdateTitleUI(TitleUIState.InitData);

        /// ReplayAnimation
        {
            Animator anim = GetUIComponent<Animator>(UI.TitleUI);
            anim?.Rebind();
        }
    }

    public void SetTitleUI_LogIn(LoginState pLoginState)
    {
        UpdateTitleUI(TitleUIState.Login, pLoginState);

        /// ReplayAnimation
        {
            Animator anim = GetUIComponent<Animator>(UI.TitleUI);
            anim?.Rebind();
        }
    }

    public void SetTitleUI_LoadUserData()
    {
        UpdateTitleUI(TitleUIState.LoadData);

        /// ReplayAnimation
        {
            Animator anim = GetUIComponent<Animator>(UI.TitleUI);
            anim?.Rebind();
        }
    }

    public void OpenNicknamePopup()
    {
        OpenTitleUI_Panel_NicknamePopup();
    }

    private void UpdateTitleUI(TitleUIState pTitleUIState, LoginState pLoginState = LoginState.None)
    {
        bool isMainBG     = (pTitleUIState == TitleUIState.InitData) || (pLoginState == LoginState.LogOut);
        bool isStartBG    = (pLoginState == LoginState.Login);
        bool isLoadDataBG = pTitleUIState == TitleUIState.LoadData;
        SetActiveUI(UI.TitleUI_Object_MainBG,       isMainBG); //임시
        SetActiveUI(UI.TitleUI_Object_StartBG,      isStartBG); //임시
        SetActiveUI(UI.TitleUI_Object_LoadDataBG,   isLoadDataBG);

        ///
        SetActiveUI(UI.TitleUI_Object_InitDataProcess,  pTitleUIState == TitleUIState.InitData);
        SetActiveUI(UI.TitleUI_Object_LoginProcess,     pTitleUIState == TitleUIState.Login);
        SetActiveUI(UI.TitleUI_Object_LoadDataProcess,  pTitleUIState == TitleUIState.LoadData); //임시

        ///
        SetActiveUI(UI.TitleUI_Object_Login,    pLoginState == LoginState.Login);
        SetActiveUI(UI.TitleUI_Object_Logout,   pLoginState == LoginState.LogOut);
    }

    #endregion TitleUI

    #region TitleUI_Panel_SignUpPopup

    void OpenTitleUI_Panel_SignUpPopup(Account pAccountType)
    {
        SetActiveUI(UI.TitleUI_Panel_SignUpPopup, true);
        UpdateTitleUI_Panel_SignUpPopup(pAccountType);
    }

    void CloseTitleUI_Panel_SignUpPopup()
    {
        SetActiveUI(UI.TitleUI_Panel_SignUpPopup, false);
    }

    void UpdateTitleUI_Panel_SignUpPopup(Account pAccountType)
    {
        SetActiveUI(UI.SignUpPopup_Object_Guest,     pAccountType == Account.Guest);
        SetActiveUI(UI.SignUpPopup_Object_Google,    pAccountType == Account.Google);
    }

    #endregion TitleUI_Panel_SignUpPopup

    #region TitleUI_Panel_NicknamePopup

    void OpenTitleUI_Panel_NicknamePopup()
    {
        SetActiveUI(UI.TitleUI_Panel_NicknamePopup, true);

        UpdateTitleUI_Panel_NicknamePopup();
    }

    void CloseTitleUI_Panel_NicknamePopup()
    {
        SetActiveUI(UI.TitleUI_Panel_NicknamePopup, false);
    }

    void UpdateTitleUI_Panel_NicknamePopup()
    {
        TMP_InputField inputField = GetUIComponent<TMP_InputField>(UI.NicknamePopup_InputField_Nickname);
        if (inputField == null)
            return;

        inputField.text = string.Empty;
    }

    #endregion TitleUI_Panel_NicknamePopup

    #region TitleUI_Panel_GuestLogoutPopup

    void OpenTitleUI_Panel_GuestLogoutPopup()
    {
        SetActiveUI(UI.TitleUI_Panel_GuestLogoutPopup, true);

        UpdateTitleUI_Panel_GuestLogoutPopup();
    }

    void CloseTitleUI_Panel_GuestLogoutPopup()
    {
        SetActiveUI(UI.TitleUI_Panel_GuestLogoutPopup, false);
    }

    void UpdateTitleUI_Panel_GuestLogoutPopup()
    {
    }

    #endregion TitleUI_Panel_GuestLogoutPopup

    //#region TitleUI_Panel_DebugTestPopup

    //void OpenTitleUI_Panel_DebugTestPopup()
    //{
    //    SetActiveUI(UI.TitleUI_Panel_DebugTestPopup, true);

    //    debugSelectAble = true;
    //    UpdateTitleUI_Panel_DebugTestPopup(debugSelectAble);
    //}

    //void CloseTitleUI_Panel_DebugTestPopup()
    //{
    //    SetActiveUI(UI.TitleUI_Panel_DebugTestPopup, false);
    //}

    //void UpdateTitleUI_Panel_DebugTestPopup(bool pDebugSelectAble)
    //{
    //    // Select
    //    SetActiveUI(UI.DebugTestPopup_Object_SelectAble, pDebugSelectAble == false);
    //    SetActiveUI(UI.DebugTestPopup_Object_SelectDesable, pDebugSelectAble);
    //    SetActiveUI(UI.DebugTestPopup_Object_DestPopup, pDebugSelectAble);
    //    SetDebugLog();
    //}

    //[Obsolete("테스트")]
    //public void SetDebugLog(string _logMessage = "")
    //{
    //    if (debugSelectAble == false)
    //        return;

    //    SetTextUI(UI.DebugTestPopup_Text_Desc, _logMessage);
    //    //SetActiveUI(UI.DebugTestPopup_Object_TestLogout, TitleScene.Instance.LogInProcessType == LogInProcess.LogIn);

    //    Debug.Log($"[DebugLog]\n{_logMessage}");
    //}

    //#endregion TitleUI_Panel_DebugTestPopup
}
