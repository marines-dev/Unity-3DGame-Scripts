using System;
using System.Collections.Generic;
using UnityEngine;


public class GameManagerEX : BaseManager
{
    bool isPaused = false; //���� Ȱ��ȭ ���� ���� ����

    //
    public bool IsGamePlay
    {
        get
        {
            bool isWorldScene = GlobalScene.SceneMng.IsActiveScene<WorldScene>();
            bool isSpawnPlayer = gamePlayer != null;
            bool isLivePlayer = gamePlayer != null && gamePlayer.baseStateType != Define.BaseState.Die;

            //
            bool isGamePlay = isWorldScene && isSpawnPlayer && isLivePlayer;
            if (isGamePlay)
            {
                return true;
            }
            else
            {
                Debug.Log("Failed : ������ �÷����� �� �����ϴ�.");
                return false;
            }

        }
    }
    public IControllHandler playerCtrl
    {
        get
        {
            if (!GlobalScene.GameMng.IsGamePlay)
            {
                Debug.LogWarning("Failed : ");
                return null;
            }
            return gamePlayer;
        }
    }

    Player gamePlayer = null;


    protected override void OnAwake() { }
    protected override void OnInit()
    {
        gamePlayer = null;
    }

    #region Player

    public void SetPalyer(Player pGamePlayer)
    {
        gamePlayer = pGamePlayer;
    }
    public void SetPlayerRespawn()
    {
        gamePlayer = null;

        // Respawn
        //Managers.Spawn.SpawnCharacter(Managers.User.SpawnerID);
        GlobalScene.UserMng.UpdateUserData();
        GlobalScene.SceneMng.LoadBaseScene<WorldScene>(); // WorldScene�� ��ε� �մϴ�.
    }

    #endregion Player

    public ITargetHandler GetTargetCharacter(GameObject pTarget)
    {
        if (pTarget == null)
        {
            Debug.LogWarning("Falied : ");
            return null;
        }

        Character baseCharacter = pTarget.GetComponent<Character>();
        if (baseCharacter == null)
        {
            Debug.LogWarning("Falied : ");
            return null;
        }

        return baseCharacter;
    }

    void OnApplicationPause(bool pause)
    {
        if (pause) // ���� ��Ȱ��ȭ �Ǿ��� �� ó��
        {
            isPaused = true;
            if (GlobalScene.SceneMng.IsActiveScene<WorldScene>())
                GlobalScene.UserMng.UpdateUserData();
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
        if (GlobalScene.SceneMng.IsActiveScene<WorldScene>())
            GlobalScene.UserMng.UpdateUserData(); //Managers.User.SaveUserData();
    }
}