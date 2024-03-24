using System;
using UnityEngine;
using BackEnd;
using static Define;
//using GooglePlayGames;
//using GooglePlayGames.BasicApi;

namespace Server
{
    public static class GPGSManager
    {
        //public static void InitGPGSAuth()
        //{
        //    // GPGS 플러그인 설정
        //    PlayGamesClientConfiguration config = new PlayGamesClientConfiguration
        //        .Builder()
        //        .RequestServerAuthCode(false)
        //        .RequestEmail() // 이메일 권한 설정 유무
        //        .RequestIdToken()
        //        .Build();

        //    //커스텀 된 정보로 GPGS 초기화
        //    PlayGamesPlatform.InitializeInstance(config);
        //    PlayGamesPlatform.DebugLogEnabled = false; // 디버그 로그 유무
        //                                               //GPGS 시작.
        //    PlayGamesPlatform.Activate();
        //}

        //public static bool CheckGoogleAuthenticated()
        //{
        //    bool isAuthenticated = Social.localUser.authenticated;
        //    // GPGS 로그인 검사
        //    if (isAuthenticated)
        //    {
        //        Debug.Log(string.Format("Success: CheckGoogleAuthenticated"));

        //        CheckFederationAccount();
        //    }
        //    else
        //    {
        //        Debug.LogError(string.Format("Failed: CheckGoogleAuthenticated"));

        //        // GPGS 로그인
        //        Social.localUser.Authenticate((bool success) =>
        //        {
        //            if (success)
        //            {
        //                Debug.Log(string.Format("Success: GPGSAuth"));

        //                CheckFederationAccount();
        //            }
        //            else
        //            {
        //                //구글 인증 실패에 대한 예외처리
        //                Debug.LogError(string.Format("Failed: GPGSAuth"));
        //            }
        //        });
        //    }

        //    return isAuthenticated;
        //}
    }

    public static class BackendManager
    {
        private static string gameDataRowInDate = string.Empty;

        private static BackendReturnObject bro = null;

        #region Init

        public static void InitBackendSDK()
        {
            gameDataRowInDate = string.Empty;
            bro = BackEnd.Backend.Initialize(true);

            if (bro.IsSuccess())
            {
                Util.LogSuccess($"InitBackendSDK - {bro}");
            }
            else
            {
                Util.LogError($"InitBackendSDK - {bro}");
            }
        }

        #endregion Init

        #region LogIn

        /// <summary>
        /// LogIn
        /// </summary>
        public static bool TokenLogin()
        {
            //string id = Backend.BMember.GetGuestID();
            //Util.LogSuccess("로컬 기기에 저장된 아이디 :" + id);

            //bro = Backend.BMember.CheckUserInBackend("federationToken", FederationType.Google);
            //Util.LogSuccess("federationToken : " + bro);

            bro = BackEnd.Backend.BMember.LoginWithTheBackendToken();

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

        public static bool GuestLogIn()
        {
            bro = BackEnd.Backend.BMember.GuestLogin();
            bool isDone = bro.IsSuccess();
            if (isDone)
            {
                Util.LogSuccess($"GuestLogIn - {bro}");
            }
            else
            {
                DeleteGuestInfo();
                Util.LogError($"GuestLogIn - {bro}");
            }

            return isDone;
        }

        // 뒤끝 서버에 획득한 구글 토큰으로 회원가입 또는 로그인
        public static bool AuthorizeFederation()
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
        public static bool CheckFederationAccount()
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

        public static bool LogOut()
        {
            bro = BackEnd.Backend.BMember.Logout();
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

        public static bool SignOut()
        {
            bro = BackEnd.Backend.BMember.WithdrawAccount();
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

        public static void DeleteGuestInfo()
        {
            BackEnd.Backend.BMember.DeleteGuestInfo();
        }

        // Nickname
        public static bool CreateNickname(string pNickname)
        {
            Util.LogMessage("Input Nickname : " + pNickname);

            bro = BackEnd.Backend.BMember.CreateNickname(pNickname);
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

        public static string GetInData()
        {
            if (bro == null)
                return string.Empty;

            return BackEnd.Backend.UserInDate;
        }

        public static string GetNickname()
        {
            if (bro == null)
                return string.Empty;

            return BackEnd.Backend.UserNickName;
        }

        public static string GetSubscriptionName()
        {
            if (bro == null)
                return string.Empty;

            bro = BackEnd.Backend.BMember.GetUserInfo();
            return bro.GetReturnValuetoJSON()["row"]["subscriptionType"].ToString();
        }

        #endregion LogIn

        #region Data

        public static LitJson.JsonData LoadBackendData(string pTable)
        {
            Util.LogSuccess("게임 정보 조회 함수를 호출합니다.");
            var bro = BackEnd.Backend.GameData.GetMyData(pTable, new Where()); // _table = "USER_DATA"
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

        public static void SaveBackendData(string pTable, ref Param pParam)
        {
            if (pParam == null)
            {
                Util.LogError();
                return;
            }

            Util.LogMessage("서버 업데이트 목록에 게임 정보를 저장합니다.");

            // 게임정보 데이터 삽입을 요청합니다.
            var bro = BackEnd.Backend.GameData.Insert(pTable, pParam);

            if (bro.IsSuccess())
            {
                Util.LogSuccess("게임 정보 저장에 성공했습니다. : " + bro);

                //삽입한 게임정보의 고유값입니다.  
                gameDataRowInDate = bro.GetInDate();
            }
            else
            {
                Util.LogError("게임 정보 저장에 실패했습니다. : " + bro);
            }
        }

        public static void UpdateBackendData(string pTable, ref Param pParam)
        {
            BackendReturnObject bro = null;

            if (string.IsNullOrEmpty(gameDataRowInDate))
            {
                Util.LogSuccess("내 제일 최신 게임정보 데이터 수정을 요청합니다.");

                bro = BackEnd.Backend.GameData.Update(pTable, new Where(), pParam);
            }
            else
            {
                Util.LogSuccess($"{gameDataRowInDate}의 게임정보 데이터 수정을 요청합니다.");

                bro = BackEnd.Backend.GameData.UpdateV2(pTable, gameDataRowInDate, BackEnd.Backend.UserInDate, pParam);
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
}

