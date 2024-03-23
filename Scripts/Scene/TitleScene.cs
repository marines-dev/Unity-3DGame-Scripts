using System;
using System.Collections;
using UnityEngine;

[Obsolete]
public class TitleScene : BaseScene<TitleScene, TitleUI>
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

    //TitleUI titleUI = null;

    IEnumerator titleProcessCoroutine = null;
    IEnumerator titleProcessRoutine   = null;
    [Obsolete("테스트")] IEnumerator testDebugProcessCoroutine = null;


    protected override void OnAwake() { }

    protected override void OnStart()
    {
        MainUI.SetTitleUI(currTitleProcessType);

        TitleProcess();
        TestDebugProcess();
    }

    protected override void onDestroy()
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

    [Obsolete("임시")] public AccountType GetCurrAccountType() { return Manager.LogInMng.currAccountType; }
    [Obsolete("임시")] public LogInProcessType GetCurrLogInProcessType() { return Manager.LogInMng.currLogInProcessType; }
    
    void TitleProcess()
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

        titleProcessCoroutine = TitleProcessCoroutine();
        StartCoroutine(titleProcessCoroutine);
    }

    IEnumerator TitleProcessCoroutine()
    {
        yield return null;

        currTitleProcessType = TitleProcessType.Init;
        titleProcessRoutine  = InitDataProcessCoroutine();
        yield return titleProcessRoutine;

        currTitleProcessType = TitleProcessType.LogIn;
        titleProcessRoutine  = LogInProcessCoroutine();
        yield return titleProcessRoutine;

        currTitleProcessType = TitleProcessType.LoadUserData;
        titleProcessRoutine  = LoadUserDataProcessCoroutine();
        yield return titleProcessRoutine;

        currTitleProcessType = TitleProcessType.LoadGameScene;
        titleProcessRoutine  = LoadGameSceneProcessCoroutine();
        yield return titleProcessRoutine;

        currTitleProcessType = TitleProcessType.Complete;
    }

    IEnumerator InitDataProcessCoroutine()
    {
        yield return null;

        // Server
        Manager.BackendMng.InitBackendSDK();
        Manager.GPGSMng.InitGPGSAuth();
        Manager.LogInMng.InitLogInState();
        yield return new WaitUntil(() => MainUI.IsTitleUI_AnimationCompleted);
    }

    IEnumerator LogInProcessCoroutine()
    {
        yield return null;

        MainUI.SetTitleUI(currTitleProcessType);
        MainUI.SetLogInStateAction(OnLogInState);
        yield return new WaitUntil(() => Manager.LogInMng.currLogInProcessType == LogInProcessType.UserLogIn);
    }

    void OnLogInState()
    {
        switch (currTitleProcessType)
        {
            case TitleProcessType.LogIn:
                {
                    switch (Manager.LogInMng.currLogInProcessType)
                    {
                        case LogInProcessType.UserLogOut:
                            {
                                if (MainUI.selectAccountType == AccountType.None)
                                    return;

                                bool isSignUp = Manager.LogInMng.SetSignUp(MainUI.selectAccountType);
                                if(isSignUp)
                                {
                                    TitleProcess();
                                    return;
                                }
                                
                                if(MainUI.selectAccountType == AccountType.Google) { }
                            }
                            break;

                        case LogInProcessType.AccountAuth:
                            {
                                Manager.LogInMng.SetUserLogIn();
                            }
                            break;

                        case LogInProcessType.UpdateNickname:
                            {
                                if (string.IsNullOrEmpty(MainUI.inputNickname) == false)
                                {
                                    Manager.LogInMng.SetUpdateNickname(MainUI.inputNickname);
                                }

                                TitleProcess();
                            }
                            break;
                    }
                }
                break;

            case TitleProcessType.LoadUserData:
                {
                    Util.LogWarning("Debug 테스트용");
                    Manager.LogInMng.SetUserLogOut();

                    TitleProcess();
                }
                break;
        }
    }

    [Obsolete("테스트 중")]
    IEnumerator LoadUserDataProcessCoroutine()
    {
        MainUI.SetTitleUI(currTitleProcessType);
        yield return new WaitUntil(() => MainUI.IsTitleUI_AnimationCompleted);

        Manager.UserMng.LoadUserData();
    }

    IEnumerator LoadGameSceneProcessCoroutine()
    {
        yield return null;
        Manager.SceneMng.LoadBaseScene<WorldScene>();
        //MainUI.Close();
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
            string inData   = Manager.BackendMng.GetInData();
            string nickname = Manager.BackendMng.GetNickname();
            string format   = string.Format(
                "[TitleProcess] " + currTitleProcessType.ToString() + '\n' +
                "[LoginProcess] " + Manager.LogInMng.currLogInProcessType.ToString() + '\n' +
                "[AccountType] " + Manager.LogInMng.currAccountType.ToString() + '\n' +
                "[InData] " + inData + '\n' +
                "[nickname] " + nickname + '\n');

            MainUI.SetDebugLog(format);
        }
        catch(Exception ex)
        {
            Debug.LogError(ex);
        }
    }
}
