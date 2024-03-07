using System;
using System.Collections;
using UnityEngine;

[Obsolete("Managers 전용 : 일반 클래스에서 사용할 수 없습니다. Managers를 이용해 주세요.")]
public class LogInManager : BaseManager
{
    public enum LogInProcessType
    {
        None,
        InitLogIn,
        UpdateNickname,
        AccountAuth,
        UserLogIn,
        UserLogOut,
    }

    public enum AccountType
    {
        None,
        Guest,
        Google
    }

    public LogInProcessType currLogInProcessType { get; private set; }  = LogInProcessType.None;
    public AccountType      currAccountType { get; private set; }       = AccountType.None;
    public bool isDone { get; private set; } = false;

    protected override void OnAwake() { }
    protected override void OnInit() { }


    public void InitLogInState()
    {
        currLogInProcessType = LogInProcessType.InitLogIn;
        currAccountType = AccountType.None;

        isDone = GlobalScene.BackendMng.TokenLogin();
        if (isDone)
        {
            string nickname = GlobalScene.BackendMng.GetNickname();
            isDone = CheckNickname(nickname);
            if (isDone)
            {
                currLogInProcessType = LogInProcessType.AccountAuth;
            }
            else
            {
                currLogInProcessType = LogInProcessType.UpdateNickname;
            }
        }
        else
        {
            currLogInProcessType = LogInProcessType.UserLogOut;
        }

        isDone = false;
    }

    bool CheckNickname(string _nickname)
    {
        bool nicknameAble = string.IsNullOrEmpty(_nickname) == false; //닉네임이 있을 경우 true

        return nicknameAble;
    }

    public bool SetSignUp(AccountType pAccountType)
    {
        AccountType selectAccountType = pAccountType;
        isDone = false;

        switch (selectAccountType)
        {
            case AccountType.Guest:
                {
                    isDone = GlobalScene.BackendMng.GuestLogIn();
                    currAccountType = GetAccountType();
                }
                break;

            case AccountType.Google:
                {
                    if (Application.platform != RuntimePlatform.Android)
                    {
                        Debug.LogWarning("사용할 수 없는 기기입니다.");
                        return isDone;
                    }

                    isDone = GlobalScene.BackendMng.CheckFederationAccount();

                    if (isDone)
                    {
                        isDone = GlobalScene.BackendMng.AuthorizeFederation();
                    }
                }
                break;

            default:
                Debug.LogError("예외 처리 필요");
                break;
        }

        return isDone;
    }

    public bool SetUpdateNickname(string _updateNickname)
    {
        isDone = GlobalScene.BackendMng.CreateNickname(_updateNickname);

        return isDone;
    }

    public void SetUserLogIn()
    {
        currLogInProcessType = LogInProcessType.UserLogIn;
        currAccountType = GetAccountType();

        isDone = true;
    }

    public void SetUserLogOut()
    {
        switch (currAccountType)
        {
            case AccountType.Guest:
                {
                    isDone = GlobalScene.BackendMng.SignOut();

                    if(isDone)
                    {
                        GlobalScene.BackendMng.DeleteGuestInfo();
                    }
                }
                break;

            case AccountType.Google:
                {
                    isDone = GlobalScene.BackendMng.LogOut();
                }
                break;

            default:
                Debug.LogError("예외 처리 필요");
                break;
        }
    }

    private AccountType GetAccountType()
    {
        string subscriptionType = GlobalScene.BackendMng.GetSubscriptionType();
        switch (subscriptionType)
        {
            case "customSignUp":
                {
                    return AccountType.Guest;
                }

            case "google":
                {
                    return AccountType.Google;
                }

            case "":
                {
                    return AccountType.None;
                }

            default:
                {
                    Debug.LogError("알 수 없는 계정 타입 : " + subscriptionType);

                    return AccountType.None;
                }
        }
    }
}