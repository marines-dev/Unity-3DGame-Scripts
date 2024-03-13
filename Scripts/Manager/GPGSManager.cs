using System;
using UnityEngine;
//using GooglePlayGames;
//using GooglePlayGames.BasicApi;


public class GPGSManager : BaseManager<GPGSManager>
{
    protected override void OnInitialized() { }
    public override void OnRelease() { }

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