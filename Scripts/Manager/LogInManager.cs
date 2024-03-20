using UnityEngine;

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

public class LogInManager : Manager
{
    public LogInProcessType currLogInProcessType { get; private set; }  = LogInProcessType.None;
    public AccountType      currAccountType { get; private set; }       = AccountType.None;
   
    public bool isDone { get; private set; } = false;


    protected override void OnInitialized() { }
    public override void OnRelease() { }

    public void InitLogInState()
    {
        currLogInProcessType = LogInProcessType.InitLogIn;
        currAccountType      = AccountType.None;

        isDone = BackendMng.TokenLogin();
        if (isDone)
        {
            string nickname = BackendMng.GetNickname();
            isDone = CheckNickname(nickname);
            if (isDone) { currLogInProcessType = LogInProcessType.AccountAuth; }
            else { currLogInProcessType = LogInProcessType.UpdateNickname; }
        }
        else
        {
            currLogInProcessType = LogInProcessType.UserLogOut;
        }

        isDone = false;
    }

    bool CheckNickname(string _nickname)
    {
        bool nicknameAble = string.IsNullOrEmpty(_nickname) == false; //�г����� ���� ��� true
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
                    isDone = BackendMng.GuestLogIn();
                    currAccountType = GetAccountType();
                }
                break;

            case AccountType.Google:
                {
                    if (Application.platform != RuntimePlatform.Android)
                    {
                        Util.LogWarning("����� �� ���� ����Դϴ�.");
                        return isDone;
                    }

                    isDone = BackendMng.CheckFederationAccount();
                    if (isDone) { isDone = BackendMng.AuthorizeFederation(); }
                }
                break;

            default:
                Util.LogError("���� ó�� �ʿ�");
                break;
        }

        return isDone;
    }

    public bool SetUpdateNickname(string _updateNickname)
    {
        isDone = BackendMng.CreateNickname(_updateNickname);
        return isDone;
    }

    public void SetUserLogIn()
    {
        currLogInProcessType = LogInProcessType.UserLogIn;
        currAccountType      = GetAccountType();

        isDone = true;
    }

    public void SetUserLogOut()
    {
        switch (currAccountType)
        {
            case AccountType.Guest:
                {
                    isDone = BackendMng.SignOut();
                    if (isDone) { BackendMng.DeleteGuestInfo(); }
                }
                break;

            case AccountType.Google:
                {
                    isDone = BackendMng.LogOut();
                }
                break;

            default:
                Util.LogError("���� ó�� �ʿ�");
                break;
        }
    }

    private AccountType GetAccountType()
    {
        string subscriptionType = BackendMng.GetSubscriptionType();
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
                    Util.LogError("�� �� ���� ���� Ÿ�� : " + subscriptionType);
                    return AccountType.None;
                }
        }
    }
}