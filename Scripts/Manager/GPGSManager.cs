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
        //// GPGS �÷����� ����
        //PlayGamesClientConfiguration config = new PlayGamesClientConfiguration
        //    .Builder()
        //    .RequestServerAuthCode(false)
        //    .RequestEmail() // �̸��� ���� ���� ����
        //    .RequestIdToken()
        //    .Build();

        ////Ŀ���� �� ������ GPGS �ʱ�ȭ
        //PlayGamesPlatform.InitializeInstance(config);
        //PlayGamesPlatform.DebugLogEnabled = false; // ����� �α� ����
        //                                           //GPGS ����.
        //PlayGamesPlatform.Activate();
    }

    //public bool CheckGoogleAuthenticated()
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