using System;
using System.Collections;
using UnityEngine;

public class TitleScene : BaseScene<TitleScene>
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
        titleUI = Manager.UIMng.CreateOrGetBaseUI<TitleUI>(MainCanvas);
        titleUI.Close();
    }

    protected override void OnStart()
    {
        // TitleUI
        titleUI.Open();
        titleUI.SetTitleUI(currTitleProcessType);

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
        yield return null;

        //titleUI.PlayInitDataText_Anim();
        //yield return null;

        // Server
        Manager.BackendMng.InitBackendSDK();
        Manager.GPGSMng.InitGPGSAuth();
        Manager.LogInMng.InitLogInState();

        yield return new WaitUntil(() => titleUI.IsTitleUI_AnimationCompleted);
    }

    IEnumerator LogInProcessCoroutine()
    {
        yield return null;

        titleUI.SetTitleUI(currTitleProcessType);
        titleUI.Set_OnLogInState(OnLogInState);
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
                                if (titleUI.selectAccountType == AccountType.None)
                                    return;

                                bool isSignUp = Manager.LogInMng.SetSignUp(titleUI.selectAccountType);
                                if(isSignUp)
                                {
                                    TitleProcess();
                                    return;
                                }
                                
                                if(titleUI.selectAccountType == AccountType.Google)
                                {
                                    
                                }
                            }
                            break;

                        case LogInProcessType.AccountAuth:
                            {
                                Manager.LogInMng.SetUserLogIn();
                            }
                            break;

                        case LogInProcessType.UpdateNickname:
                            {
                                if (string.IsNullOrEmpty(titleUI.inputNickname) == false)
                                {
                                    Manager.LogInMng.SetUpdateNickname(titleUI.inputNickname);
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
                    Manager.LogInMng.SetUserLogOut();

                    TitleProcess();
                }
                break;
        }
    }

    [Obsolete("테스트 중")]
    IEnumerator LoadUserDataProcessCoroutine()
    {
        titleUI.SetTitleUI(currTitleProcessType);
        yield return new WaitUntil(() => titleUI.IsTitleUI_AnimationCompleted);

        Manager.UserMng.LoadUserData();
    }

    IEnumerator LoadGameSceneProcessCoroutine()
    {
        Manager.SceneMng.LoadBaseScene<WorldScene>();
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
            string inData = Manager.BackendMng.GetInData();
            string nickname = Manager.BackendMng.GetNickname();
            string format = string.Format(
                "[TitleProcess] " + currTitleProcessType.ToString() + '\n' +
                "[LoginProcess] " + Manager.LogInMng.currLogInProcessType.ToString() + '\n' +
                "[AccountType] " + Manager.LogInMng.currAccountType.ToString() + '\n' +
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
