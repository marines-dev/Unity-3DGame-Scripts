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
        //Util.LogSuccess("로컬 기기에 저장된 아이디 :" + id);

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

    // 뒤끝 서버에 획득한 구글 토큰으로 회원가입 또는 로그인
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
        Util.LogSuccess("게임 정보 조회 함수를 호출합니다.");
        var bro = Backend.GameData.GetMyData(_table, new Where()); // _table = "USER_DATA"
        if (bro.IsSuccess())
        {
            Util.LogSuccess("게임 정보 조회에 성공했습니다. : " + bro);
            LitJson.JsonData gameDataJson = bro.FlattenRows(); // Json으로 리턴된 데이터를 받아옵니다.  

            if (gameDataJson.Count <= 0)
            {
                Util.LogWarning("데이터가 존재하지 않습니다.");
            }
            else
            {
                gameDataRowInDate = gameDataJson[0]["inDate"].ToString(); //불러온 게임정보의 고유값입니다.  
                return gameDataJson;
            }
        }
        else
        {
            Util.LogError("게임 정보 조회에 실패했습니다. : " + bro);
        }

        return null;
    }

    public void SaveBackendData(string _table, ref Param _param)
    {
        Util.LogWarning("서버 업데이트 목록에 해당 데이터들을 추가합니다.");
        // 게임정보 데이터 삽입을 요청합니다.
        var bro = Backend.GameData.Insert(_table, _param);

        if (bro.IsSuccess())
        {
            Util.LogSuccess("게임정보 데이터 삽입에 성공했습니다. : " + bro);

            //삽입한 게임정보의 고유값입니다.  
            gameDataRowInDate = bro.GetInDate();
        }
        else
        {
            Util.LogError("게임정보 데이터 삽입에 실패했습니다. : " + bro);
        }
    }

    public void UpdateBackendData(string _table, ref Param _param)
    {
        BackendReturnObject bro = null;

        if (string.IsNullOrEmpty(gameDataRowInDate))
        {
            Util.LogSuccess("내 제일 최신 게임정보 데이터 수정을 요청합니다.");

            bro = Backend.GameData.Update(_table, new Where(), _param);
        }
        else
        {
            Util.LogSuccess($"{gameDataRowInDate}의 게임정보 데이터 수정을 요청합니다.");

            bro = Backend.GameData.UpdateV2(_table, gameDataRowInDate, Backend.UserInDate, _param);
        }

        if (bro.IsSuccess())
        {
            Util.LogSuccess("게임정보 데이터 수정에 성공했습니다. : " + bro);
        }
        else
        {
            Util.LogError("게임정보 데이터 수정에 실패했습니다. : " + bro);
        }
    }

    #endregion Data
}

