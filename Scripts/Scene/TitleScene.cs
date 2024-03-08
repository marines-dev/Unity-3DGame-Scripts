using System;
using System.Collections;
using UnityEngine;

public class TitleScene : BaseScene
{
    public enum TitleProcessType
    {
        Init,
        Patch,
        LogIn,
        LoadUserData,
        LoadGameScene,
        Complete,
    }
    TitleProcessType currTitleProcessType = TitleProcessType.Init;

    TitleUI titleUI = null;
    IEnumerator titleProcessCoroutine = null;
    IEnumerator titleProcessRoutine   = null;
    [Obsolete("테스트")] IEnumerator testDebugProcessCoroutine = null;


    protected override void OnAwake()
    {
        titleUI = GlobalScene.UIMng.GetOrCreateBaseUI<TitleUI>();
    }

    protected override void OnStart()
    {
        TitleProcess();
        TestDebugProcess();
    }

    protected override void OnDestroy_()
    {
        if (titleProcessCoroutine != null)
        {
            StopCoroutine(titleProcessCoroutine);
            titleProcessCoroutine = null;
        }

        if (titleProcessRoutine != null)
        {
            StopCoroutine(titleProcessRoutine);
            titleProcessRoutine = null;
        }

        if (testDebugProcessCoroutine != null)
        {
            StopCoroutine(testDebugProcessCoroutine);
            testDebugProcessCoroutine = null;
        }
    }

    void TitleProcess()
    {
        //SetUI
        {
            // TitleUI
            titleUI.Open();
            titleUI.SetTitleUI(currTitleProcessType);
        }

        if (titleProcessCoroutine != null)
        {
            StopCoroutine(titleProcessCoroutine);
            titleProcessCoroutine = null;
        }

        if (titleProcessRoutine != null)
        {
            StopCoroutine(titleProcessRoutine);
            titleProcessRoutine = null;
        }

        titleProcessCoroutine = TitleProcessCoroutine();
        StartCoroutine(titleProcessCoroutine);
    }

    IEnumerator TitleProcessCoroutine()
    {
        currTitleProcessType = TitleProcessType.Init;
        titleProcessRoutine = InitDataProcessCoroutine();
        yield return titleProcessRoutine;

        currTitleProcessType = TitleProcessType.LogIn;
        titleProcessRoutine = LogInProcessCoroutine();
        yield return titleProcessRoutine;

        currTitleProcessType = TitleProcessType.LoadUserData;
        titleProcessRoutine = LoadUserDataProcessCoroutine();
        yield return titleProcessRoutine;

        currTitleProcessType = TitleProcessType.LoadGameScene;
        titleProcessRoutine = LoadGameSceneProcessCoroutine();
        yield return titleProcessRoutine;

        currTitleProcessType = TitleProcessType.Complete;
    }

    IEnumerator InitDataProcessCoroutine()
    {
        // Server
        GlobalScene.BackendMng.InitBackendSDK();
        GlobalScene.GPGSMng.InitGPGSAuth();
        GlobalScene.LogInMng.InitLogInState();
        yield return null;
    }

    IEnumerator LogInProcessCoroutine()
    {
        titleUI.SetTitleUI(currTitleProcessType);
        titleUI.Set_OnLogInState(OnLogInState);
        yield return new WaitUntil(() => GlobalScene.LogInMng.currLogInProcessType == LogInManager.LogInProcessType.UserLogIn);
    }

    void OnLogInState()
    {
        switch (currTitleProcessType)
        {
            case TitleProcessType.LogIn:
                {
                    switch (GlobalScene.LogInMng.currLogInProcessType)
                    {
                        case LogInManager.LogInProcessType.UserLogOut:
                            {
                                if (titleUI.selectAccountType == LogInManager.AccountType.None)
                                    return;

                                bool isSignUp = GlobalScene.LogInMng.SetSignUp(titleUI.selectAccountType);
                                if(isSignUp)
                                {
                                    TitleProcess();
                                    return;
                                }
                                
                                if(titleUI.selectAccountType == LogInManager.AccountType.Google)
                                {
                                    
                                }
                            }
                            break;

                        case LogInManager.LogInProcessType.AccountAuth:
                            {
                                GlobalScene.LogInMng.SetUserLogIn();
                            }
                            break;

                        case LogInManager.LogInProcessType.UpdateNickname:
                            {
                                if (string.IsNullOrEmpty(titleUI.inputNickname) == false)
                                {
                                    GlobalScene.LogInMng.SetUpdateNickname(titleUI.inputNickname);
                                }

                                TitleProcess();
                            }
                            break;
                    }
                }
                break;

            case TitleProcessType.LoadUserData:
                {
                    Debug.LogWarning("Debug 테스트용");
                    GlobalScene.LogInMng.SetUserLogOut();

                    TitleProcess();
                }
                break;
        }
    }

    [Obsolete("테스트 중")]
    IEnumerator LoadUserDataProcessCoroutine()
    {
        titleUI.SetTitleUI(currTitleProcessType);
        yield return new WaitForSeconds(2f); // 테스트 코드

        if (GlobalScene.UserMng.LoadUserData() == false) // 유저 데이터 로드 실패할 경우
        {
            // 유저 데이터 생성 및 저장
            GlobalScene.UserMng.CreateUserData();
        }
    }

    IEnumerator LoadGameSceneProcessCoroutine()
    {
        GlobalScene.SceneMng.LoadBaseScene<WorldScene>();
        yield return null;

        titleUI.Close();
    }

    [Obsolete("테스트")]
    void TestDebugProcess()
    {
        if(testDebugProcessCoroutine != null)
        {
            StopCoroutine(testDebugProcessCoroutine);
            testDebugProcessCoroutine = null;
        }

        testDebugProcessCoroutine = TestDebugProcessCorouine();
        StartCoroutine(testDebugProcessCoroutine);
    }
    IEnumerator TestDebugProcessCorouine()
    {
        while(true)
        {
            UpdateTitleProcessDebug();

            yield return null;
        }
    }
    void UpdateTitleProcessDebug()
    {
        try
        {
            string inData = GlobalScene.BackendMng.GetInData();
            string nickname = GlobalScene.BackendMng.GetNickname();
            string format = string.Format(
                "[TitleProcess] " + currTitleProcessType.ToString() + '\n' +
                "[LoginProcess] " + GlobalScene.LogInMng.currLogInProcessType.ToString() + '\n' +
                "[AccountType] " + GlobalScene.LogInMng.currAccountType.ToString() + '\n' +
                "[InData] " + inData + '\n' +
                "[nickname] " + nickname + '\n');

            titleUI.SetDebugLog(format);
        }
        catch(Exception ex)
        {
            Debug.LogError(ex);
        }
    }
}
