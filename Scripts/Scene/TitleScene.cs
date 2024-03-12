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
        titleUI = UIManager.Instance.CreateOrGetBaseUI<TitleUI>();
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
        // Server
        BackendManager.Instance.InitBackendSDK();
        GPGSManager.Instance.InitGPGSAuth();
        LogInManager.Instance.InitLogInState();
        yield return null;
    }

    IEnumerator LogInProcessCoroutine()
    {
        titleUI.SetTitleUI(currTitleProcessType);
        titleUI.Set_OnLogInState(OnLogInState);
        yield return new WaitUntil(() => LogInManager.Instance.currLogInProcessType == LogInManager.LogInProcessType.UserLogIn);
    }

    void OnLogInState()
    {
        switch (currTitleProcessType)
        {
            case TitleProcessType.LogIn:
                {
                    switch (LogInManager.Instance.currLogInProcessType)
                    {
                        case LogInManager.LogInProcessType.UserLogOut:
                            {
                                if (titleUI.selectAccountType == LogInManager.AccountType.None)
                                    return;

                                bool isSignUp = LogInManager.Instance.SetSignUp(titleUI.selectAccountType);
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
                                LogInManager.Instance.SetUserLogIn();
                            }
                            break;

                        case LogInManager.LogInProcessType.UpdateNickname:
                            {
                                if (string.IsNullOrEmpty(titleUI.inputNickname) == false)
                                {
                                    LogInManager.Instance.SetUpdateNickname(titleUI.inputNickname);
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
                    LogInManager.Instance.SetUserLogOut();

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

        if (UserManager.Instance.LoadUserData() == false) // 유저 데이터 로드 실패할 경우
        {
            // 유저 데이터 생성 및 저장
            UserManager.Instance.CreateUserData();
        }
    }

    IEnumerator LoadGameSceneProcessCoroutine()
    {
        SceneManager.Instance.LoadBaseScene<WorldScene>();
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
            string inData = BackendManager.Instance.GetInData();
            string nickname = BackendManager.Instance.GetNickname();
            string format = string.Format(
                "[TitleProcess] " + currTitleProcessType.ToString() + '\n' +
                "[LoginProcess] " + LogInManager.Instance.currLogInProcessType.ToString() + '\n' +
                "[AccountType] " + LogInManager.Instance.currAccountType.ToString() + '\n' +
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
