using System;
using System.Collections.Generic;
using UnityEngine;


public class SystemManager : Manager
{
    bool isPaused = false; //앱의 활성화 상태 저장 유무


    protected override void OnInitialized() { }
    public override void OnRelease() { }

    void OnApplicationPause(bool pause)
    {
        if (pause) // 앱이 비활성화 되었을 때 처리
        {
            isPaused = true;
            //if (SceneMng.IsActiveScene<WorldScene>())
                //UserMng.UpdateUserData();
        }
        else // 앱이 활성화 되었을 때 처리
        {
            if (isPaused)
            {
                isPaused = false;
            }
        }
    }

    [Obsolete("테스트 중")]
    void OnApplicationQuit() // 앱이 종료 될 때 처리
    {
        //if (SceneMng.IsActiveScene<WorldScene>())
            //UserMng.UpdateUserData(); //Managers.User.SaveUserData();
    }
}