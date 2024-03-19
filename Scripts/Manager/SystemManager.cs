using System;
using System.Collections.Generic;
using UnityEngine;


public class SystemManager : Manager
{
    bool isPaused = false; //���� Ȱ��ȭ ���� ���� ����


    protected override void OnInitialized() { }
    public override void OnRelease() { }

    void OnApplicationPause(bool pause)
    {
        if (pause) // ���� ��Ȱ��ȭ �Ǿ��� �� ó��
        {
            isPaused = true;
            //if (SceneMng.IsActiveScene<WorldScene>())
                //UserMng.UpdateUserData();
        }
        else // ���� Ȱ��ȭ �Ǿ��� �� ó��
        {
            if (isPaused)
            {
                isPaused = false;
            }
        }
    }

    [Obsolete("�׽�Ʈ ��")]
    void OnApplicationQuit() // ���� ���� �� �� ó��
    {
        //if (SceneMng.IsActiveScene<WorldScene>())
            //UserMng.UpdateUserData(); //Managers.User.SaveUserData();
    }
}