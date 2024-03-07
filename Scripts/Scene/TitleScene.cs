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

    TitleUI titleUI_ = null;
    TitleUI titleUI
    {
        get
        {
            if (titleUI_ == null)
            {
                Managers.UI.LoadUI<TitleUI>();
                titleUI_ = Managers.UI.GetBaseUI<TitleUI>();
            }
            return titleUI_;
        }
    }
    IEnumerator titleProcessCoroutine = null;
    IEnumerator titleProcessRoutine   = null;
    [Obsolete("테스트")] IEnumerator testDebugProcessCoroutine = null;


    protected override IEnumerator LoadingProcessRoutine()
    {
        yield return null;
    }

    protected override void OpenScene()
    {
        TitleProcess();
        TestDebugProcess();
    }

    protected override void CloseScene()
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
            titleUI.OpenUI();
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
        Managers.Backend.InitBackendSDK();
        Managers.GPGS.InitGPGSAuth();
        Managers.LogIn.InitLogInState();
        yield return null;
    }

    IEnumerator LogInProcessCoroutine()
    {
        titleUI.SetTitleUI(currTitleProcessType);
        titleUI.Set_OnLogInState(OnLogInState);
        yield return new WaitUntil(() => Managers.LogIn.currLogInProcessType == LogInManager.LogInProcessType.UserLogIn);
    }

    void OnLogInState()
    {
        switch (currTitleProcessType)
        {
            case TitleProcessType.LogIn:
                {
                    switch (Managers.LogIn.currLogInProcessType)
                    {
                        case LogInManager.LogInProcessType.UserLogOut:
                            {
                                if (titleUI.selectAccountType == LogInManager.AccountType.None)
                                    return;

                                bool isSignUp = Managers.LogIn.SetSignUp(titleUI.selectAccountType);
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
                                Managers.LogIn.SetUserLogIn();
                            }
                            break;

                        case LogInManager.LogInProcessType.UpdateNickname:
                            {
                                if (string.IsNullOrEmpty(titleUI.inputNickname) == false)
                                {
                                    Managers.LogIn.SetUpdateNickname(titleUI.inputNickname);
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
                    Managers.LogIn.SetUserLogOut();

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

        if (Managers.User.LoadUserData() == false) // 유저 데이터 로드 실패할 경우
        {
            // 유저 데이터 생성 및 저장
            Managers.User.CreateUserData();
        }
    }

    IEnumerator LoadGameSceneProcessCoroutine()
    {
        Managers.Scene.LoadScene<WorldScene>();
        yield return null;

        titleUI.CloseUI();
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
            string inData = Managers.Backend.GetInData();
            string nickname = Managers.Backend.GetNickname();
            string format = string.Format(
                "[TitleProcess] " + currTitleProcessType.ToString() + '\n' +
                "[LoginProcess] " + Managers.LogIn.currLogInProcessType.ToString() + '\n' +
                "[AccountType] " + Managers.LogIn.currAccountType.ToString() + '\n' +
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
