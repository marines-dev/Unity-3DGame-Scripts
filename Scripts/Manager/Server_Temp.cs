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
        //    // GPGS �÷����� ����
        //    PlayGamesClientConfiguration config = new PlayGamesClientConfiguration
        //        .Builder()
        //        .RequestServerAuthCode(false)
        //        .RequestEmail() // �̸��� ���� ���� ����
        //        .RequestIdToken()
        //        .Build();

        //    //Ŀ���� �� ������ GPGS �ʱ�ȭ
        //    PlayGamesPlatform.InitializeInstance(config);
        //    PlayGamesPlatform.DebugLogEnabled = false; // ����� �α� ����
        //                                               //GPGS ����.
        //    PlayGamesPlatform.Activate();
        //}

        //public static bool CheckGoogleAuthenticated()
        //{
        //    bool isAuthenticated = Social.localUser.authenticated;
        //    // GPGS �α��� �˻�
        //    if (isAuthenticated)
        //    {
        //        Debug.Log(string.Format("Success: CheckGoogleAuthenticated"));

        //        CheckFederationAccount();
        //    }
        //    else
        //    {
        //        Debug.LogError(string.Format("Failed: CheckGoogleAuthenticated"));

        //        // GPGS �α���
        //        Social.localUser.Authenticate((bool success) =>
        //        {
        //            if (success)
        //            {
        //                Debug.Log(string.Format("Success: GPGSAuth"));

        //                CheckFederationAccount();
        //            }
        //            else
        //            {
        //                //���� ���� ���п� ���� ����ó��
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
            //Util.LogSuccess("���� ��⿡ ����� ���̵� :" + id);

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

        // �ڳ� ������ ȹ���� ���� ��ū���� ȸ������ �Ǵ� �α���
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
            Util.LogSuccess("���� ���� ��ȸ �Լ��� ȣ���մϴ�.");
            var bro = BackEnd.Backend.GameData.GetMyData(pTable, new Where()); // _table = "USER_DATA"
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

        public static void SaveBackendData(string pTable, ref Param pParam)
        {
            if (pParam == null)
            {
                Util.LogError();
                return;
            }

            Util.LogMessage("���� ������Ʈ ��Ͽ� ���� ������ �����մϴ�.");

            // �������� ������ ������ ��û�մϴ�.
            var bro = BackEnd.Backend.GameData.Insert(pTable, pParam);

            if (bro.IsSuccess())
            {
                Util.LogSuccess("���� ���� ���忡 �����߽��ϴ�. : " + bro);

                //������ ���������� �������Դϴ�.  
                gameDataRowInDate = bro.GetInDate();
            }
            else
            {
                Util.LogError("���� ���� ���忡 �����߽��ϴ�. : " + bro);
            }
        }

        public static void UpdateBackendData(string pTable, ref Param pParam)
        {
            BackendReturnObject bro = null;

            if (string.IsNullOrEmpty(gameDataRowInDate))
            {
                Util.LogSuccess("�� ���� �ֽ� �������� ������ ������ ��û�մϴ�.");

                bro = BackEnd.Backend.GameData.Update(pTable, new Where(), pParam);
            }
            else
            {
                Util.LogSuccess($"{gameDataRowInDate}�� �������� ������ ������ ��û�մϴ�.");

                bro = BackEnd.Backend.GameData.UpdateV2(pTable, gameDataRowInDate, BackEnd.Backend.UserInDate, pParam);
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
}

