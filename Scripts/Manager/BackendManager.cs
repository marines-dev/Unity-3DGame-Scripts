using System;
using UnityEngine;
using BackEnd;


public class BackendManager : BaseManager<BackendManager>
{
    string gameDataRowInDate = string.Empty;

    //
    BackendReturnObject bro = null;

    protected override void OnInitialized() { }
    public override void OnRelease()
    {
        gameDataRowInDate = string.Empty;
    }

    #region Init

    public void InitBackendSDK()
    {
        bro = Backend.Initialize(true);

        if (bro.IsSuccess())
        {
            Util.LogSuccess($"Success: InitBackendSDK - {bro}");
        }
        else
        {
            Util.LogError($"InitBackendSDK - {bro}");
        }
    }

    #endregion Init

    #region LogIn

    // LogIn
    public bool TokenLogin()
    {
        //string id = Backend.BMember.GetGuestID();
        //Util.LogSuccess("���� ��⿡ ����� ���̵� :" + id);

        //bro = Backend.BMember.CheckUserInBackend("federationToken", FederationType.Google);
        //Util.LogSuccess("federationToken : " + bro);

        bro = Backend.BMember.LoginWithTheBackendToken();

        if (bro.IsSuccess())
        {
            Util.LogSuccess($"TokenLogin - {bro}");
            return true;
        }
        else
        {
            Util.LogWarning($"TokenLogin - {bro}");
            return false;
        }
    }

    public bool GuestLogIn()
    {
        bro = Backend.BMember.GuestLogin();
        if (bro.IsSuccess())
        {
            Util.LogSuccess($"GuestLogIn - {bro}");
            return true;
        }
        else
        {
            DeleteGuestInfo();

            Util.LogError($"GuestLogIn - {bro}");
            return false;
        }
    }

    // �ڳ� ������ ȹ���� ���� ��ū���� ȸ������ �Ǵ� �α���
    public bool AuthorizeFederation()
    {
        bool isSuccess = false;

#if UNITY_ANDROID
        bro = Backend.BMember.AuthorizeFederation(GetTokens(), FederationType.Google, "gpgs");
        isSuccess = bro.IsSuccess();
        if (bro.IsSuccess())
        {
            Util.LogSuccess($"AuthorizeFederation - {bro}");
        }
        else
        {
            Util.LogError($"AuthorizeFederation - {bro}");
        }
#endif

        return isSuccess;
    }

    // LogOut
    public bool CheckFederationAccount()
    {
        bool isSuccess = false;

#if UNITY_ANDROID
        bro = Backend.BMember.CheckUserInBackend(GetTokens(), FederationType.Google);
        isSuccess = bro.IsSuccess();
        if (isSuccess)
        {
            Util.LogSuccess($"CheckFederationAccount - {bro}");
        }
        else
        {
            Util.LogError($"CheckFederationAccount - {bro}");

            OpenSignUpPopupObject(selectAccoutType);
        }
#endif

        return isSuccess;
    }

    public bool LogOut()
    {
        bro = Backend.BMember.Logout();
        bool isSuccess = bro.IsSuccess();
        if (bro.IsSuccess())
        {
            Util.LogSuccess($"FederationLogOut - {bro}");
        }
        else
        {
            Util.LogError($"FederationLogOut - {bro}");
        }

        return isSuccess;
    }

    public bool SignOut()
    {
        bro = Backend.BMember.WithdrawAccount();
        bool isSuccess = bro.IsSuccess();
        if (bro.IsSuccess())
        {
            Util.LogSuccess($"SignOut - {bro}");
        }
        else
        {
            Util.LogError($"Failed : SignOut - {bro}");
        }

        return isSuccess;
    }

    public void DeleteGuestInfo()
    {
        Backend.BMember.DeleteGuestInfo();
    }

    // Nickname
    public bool CreateNickname(string _nickname)
    {
        Util.LogMessage("Input Nickname : " + _nickname);

        bro = Backend.BMember.CreateNickname(_nickname);
        bool isSuccess = bro.IsSuccess();
        if (isSuccess)
        {
            Util.LogSuccess($": CreateNickname - {bro}");
        }
        else
        {
            Util.LogError($"CreateNickname - {bro}");
        }

        return isSuccess;
    }

    public string GetInData()
    {
        return Backend.UserInDate;
    }

    public string GetNickname()
    {
        return Backend.UserNickName;
    }

    public string GetSubscriptionType()
    {
        bro = Backend.BMember.GetUserInfo();
        return bro.GetReturnValuetoJSON()["row"]["subscriptionType"].ToString();
    }

    #endregion LogIn

    #region Data

    public LitJson.JsonData LoadBackendData(string _table)
    {
        Util.LogSuccess("���� ���� ��ȸ �Լ��� ȣ���մϴ�.");
        var bro = Backend.GameData.GetMyData(_table, new Where()); // _table = "USER_DATA"
        if (bro.IsSuccess())
        {
            Util.LogSuccess("���� ���� ��ȸ�� �����߽��ϴ�. : " + bro);
            LitJson.JsonData gameDataJson = bro.FlattenRows(); // Json���� ���ϵ� �����͸� �޾ƿɴϴ�.  

            if (gameDataJson.Count <= 0)
            {
                Util.LogWarning("�����Ͱ� �������� �ʽ��ϴ�.");
            }
            else
            {
                gameDataRowInDate = gameDataJson[0]["inDate"].ToString(); //�ҷ��� ���������� �������Դϴ�.  
                return gameDataJson;
            }
        }
        else
        {
            Util.LogError("���� ���� ��ȸ�� �����߽��ϴ�. : " + bro);
        }

        return null;
    }

    public void SaveBackendData(string _table, ref Param _param)
    {
        Util.LogWarning("���� ������Ʈ ��Ͽ� �ش� �����͵��� �߰��մϴ�.");
        // �������� ������ ������ ��û�մϴ�.
        var bro = Backend.GameData.Insert(_table, _param);

        if (bro.IsSuccess())
        {
            Util.LogSuccess("�������� ������ ���Կ� �����߽��ϴ�. : " + bro);

            //������ ���������� �������Դϴ�.  
            gameDataRowInDate = bro.GetInDate();
        }
        else
        {
            Util.LogError("�������� ������ ���Կ� �����߽��ϴ�. : " + bro);
        }
    }

    public void UpdateBackendData(string _table, ref Param _param)
    {
        BackendReturnObject bro = null;

        if (string.IsNullOrEmpty(gameDataRowInDate))
        {
            Util.LogSuccess("�� ���� �ֽ� �������� ������ ������ ��û�մϴ�.");

            bro = Backend.GameData.Update(_table, new Where(), _param);
        }
        else
        {
            Util.LogSuccess($"{gameDataRowInDate}�� �������� ������ ������ ��û�մϴ�.");

            bro = Backend.GameData.UpdateV2(_table, gameDataRowInDate, Backend.UserInDate, _param);
        }

        if (bro.IsSuccess())
        {
            Util.LogSuccess("�������� ������ ������ �����߽��ϴ�. : " + bro);
        }
        else
        {
            Util.LogError("�������� ������ ������ �����߽��ϴ�. : " + bro);
        }
    }

    #endregion Data
}

