using System;
using System.Collections;
using Server;
using UnityEngine;
using static Define;


[Obsolete]
public class TitleScene : BaseScene<TitleScene, TitleUI>
{
    private LogInProcess LogInProcessType = LogInProcess.None;
    public Account AccountType
    {
        get
        {
            string subscriptionName = BackendManager.GetSubscriptionName();
            return GetAccountType(subscriptionName);
        }
    }

    private IEnumerator titleProcessCoroutine = null;


    //private void Update()
    //{
    //    try
    //    {
    //        string log =
    //     $"[LogInProcess] {LogInProcessType}\n" +
    //     $"[Account]      {AccountType}\n" +
    //     $"[UserInDate]   {BackendManager.GetInData()}\n" +
    //     $"[Nickname]  {BackendManager.GetNickname()}";

    //        MainUI.SetDebugLog(log);
    //    }
    //    catch (Exception e) 
    //    {
    //        MainUI.SetDebugLog("");
    //    }
    //}

    protected override void OnAwake() { }

    protected override void OnStart()
    {

        MainUI.SetTitleUI_InitData();
        
        ///
        StartTitleProcess();
    }

    protected override void onDestroy()
    {
        if (titleProcessCoroutine != null)
        {
            StopCoroutine(titleProcessCoroutine);
            titleProcessCoroutine = null;
        }
    }

    /// <summary>
    /// 선택 계정으로 회원가입을 진행합니다.
    /// </summary>
    public void SetSinUp(Account pAccountType)
    {
        if (LogInProcessType != LogInProcess.LogOut)
        {
            Util.LogWarning();
            return;
        }

        switch (pAccountType)
        {
            case Account.Guest:
                {
                    BackendManager.GuestLogIn();
                }
                break;

            case Account.Google:
                {
                    Util.LogMessage("개발 중");
                    return;

                    //if (Application.platform != RuntimePlatform.Android)
                    //{
                    //    Util.LogWarning("사용할 수 없는 기기입니다.");
                    //    return;
                    //}

                    //isDone = BackendManager.CheckFederationAccount();
                    //if (isDone) { isDone = BackendManager.AuthorizeFederation(); }
                }
                break;

            default:
                Util.LogError();
                break;
        }

        // Restart
        StartTitleProcess();
    }

    /// <summary>
    /// 계정을 인증하고 게임을 시작합니다.
    /// </summary>
    public void SetGameStart()
    {
        if (LogInProcessType != LogInProcess.LogIn)
        {
            Util.LogWarning();
            return;
        }

        string subscriptionName = BackendManager.GetSubscriptionName();
        bool isAuth = ! string.IsNullOrEmpty(subscriptionName); //임시
        if (isAuth) { LogInProcessType = LogInProcess.Auth; }
    }

    /// <summary>
    /// 닉네임을 검사하고 생성합니다.
    /// </summary>
    public bool CreateNickName(string pNickName)
    {
        bool isDone = BackendManager.CreateNickname(pNickName);

        /// Restart
        StartTitleProcess();
        return isDone;
    }

    /// <summary>
    /// 계정을 로그아웃합니다.
    /// </summary>
    public void SetLogOut()
    {
        if (LogInProcessType != LogInProcess.LogIn)
        {
            Util.LogWarning();
            return;
        }

        switch (AccountType)
        {
            case Account.Guest:
                {
                    bool isDone = BackendManager.SignOut();
                    if (isDone) { BackendManager.DeleteGuestInfo(); }
                }
                break;

            case Account.Google:
                {
                    BackendManager.LogOut();
                }
                break;
        }

        /// Restart
        StartTitleProcess();
    }

    private void StartTitleProcess()
    {
        if (titleProcessCoroutine != null)
        {
            StopCoroutine(titleProcessCoroutine);
            titleProcessCoroutine = null;
        }

        titleProcessCoroutine = TitleProcessCoroutine();
        StartCoroutine(titleProcessCoroutine);
    }

    [Obsolete]
    IEnumerator TitleProcessCoroutine()
    {
        LogInProcessType = Define.LogInProcess.None;
        yield return null;

        /// InitData
        {
            MainUI.SetTitleUI_InitData();
            yield return null;

            /// Server
            BackendManager.InitBackendSDK();
            //GPGSManager.InitGPGSAuth();
            yield return new WaitUntil(() => MainUI.IsTitleUI_AnimationCompleted);
        }

        /// LogIn
        {
            bool isLogin = false;

            /// CheckTokenLogin
            {
                isLogin = BackendManager.TokenLogin();
                if (isLogin)
                {
                    LogInProcessType = LogInProcess.LogIn;
                    MainUI.SetTitleUI_LogIn(LoginState.Login);
                    yield return null;
                }
                else
                {
                    LogInProcessType = LogInProcess.LogOut;
                    MainUI.SetTitleUI_LogIn(LoginState.LogOut);
                    yield break;
                }
            }

            /// CheckNinme
            {
                if(isLogin)
                {
                    string  nickname          = BackendManager.GetNickname();
                    bool    isNicknameCreated = string.IsNullOrEmpty(nickname);
                    if(isNicknameCreated)
                    {
                        MainUI.OpenNicknamePopup();
                        yield return null;
                    }
                }
            }

            /// AccountAuth
            {
                yield return new WaitUntil(() => LogInProcessType == LogInProcess.Auth);
            }
        }

        /// LoadUserData
        {
            MainUI.SetTitleUI_LoadUserData();
            User.LoadUserData();
            yield return new WaitUntil(() => MainUI.IsTitleUI_AnimationCompleted);
        }

        /// LoadGameScene
        {
            SceneLoader.LoadBaseScene<WorldScene>();
        }
    }

    #region LogIn

    private Account GetAccountType(string pAccountName)
    {
        string subscriptionType = BackendManager.GetSubscriptionName();
        switch (subscriptionType)
        {
            case "customSignUp":
                {
                    return Account.Guest;
                }

            case "google":
                {
                    return Account.Google;
                }

            case "":
                {
                    return Account.None;
                }

            default:
                {
                    Util.LogError("알 수 없는 계정 타입 : " + subscriptionType);
                    return Account.None;
                }
        }
    }

    #endregion LogIn
}
