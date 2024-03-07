using System;
using UnityEngine;
//using GooglePlayGames;
//using GooglePlayGames.BasicApi;

[Obsolete("Managers 전용 : 일반 클래스에서 사용할 수 없습니다. Managers를 이용해 주세요.")]
public class GPGSManager : BaseManager
{
    protected override void InitDataProcess() { }
    protected override void ResetDataProcess() { }

    public void InitGPGSAuth()
    {
        //// GPGS 플러그인 설정
        //PlayGamesClientConfiguration config = new PlayGamesClientConfiguration
        //    .Builder()
        //    .RequestServerAuthCode(false)
        //    .RequestEmail() // 이메일 권한 설정 유무
        //    .RequestIdToken()
        //    .Build();

        ////커스텀 된 정보로 GPGS 초기화
        //PlayGamesPlatform.InitializeInstance(config);
        //PlayGamesPlatform.DebugLogEnabled = false; // 디버그 로그 유무
        //                                           //GPGS 시작.
        //PlayGamesPlatform.Activate();
    }

    //public bool CheckGoogleAuthenticated()
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