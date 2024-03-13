using UnityEngine;


public class LogInManager : BaseManager<LogInManager>
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

    protected override void OnInitialized() { }
    public override void OnRelease() { }


    public void InitLogInState()
    {
        currLogInProcessType = LogInProcessType.InitLogIn;
        currAccountType = AccountType.None;

        isDone = BackendManager.Instance.TokenLogin();
        if (isDone)
        {
            string nickname = BackendManager.Instance.GetNickname();
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
                    isDone = BackendManager.Instance.GuestLogIn();
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

                    isDone = BackendManager.Instance.CheckFederationAccount();

                    if (isDone)
                    {
                        isDone = BackendManager.Instance.AuthorizeFederation();
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
        isDone = BackendManager.Instance.CreateNickname(_updateNickname);

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
                    isDone = BackendManager.Instance.SignOut();

                    if(isDone)
                    {
                        BackendManager.Instance.DeleteGuestInfo();
                    }
                }
                break;

            case AccountType.Google:
                {
                    isDone = BackendManager.Instance.LogOut();
                }
                break;

            default:
                Debug.LogError("예외 처리 필요");
                break;
        }
    }

    private AccountType GetAccountType()
    {
        string subscriptionType = BackendManager.Instance.GetSubscriptionType();
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