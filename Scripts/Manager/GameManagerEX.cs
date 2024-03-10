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
            bool isSpawnPlayer = player != null;
            bool isLivePlayer = player != null && player.BaseAnimType != Define.BaseAnim.Die;

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
    public IControllHndl_Legacy playerCtrl
    {
        get
        {
            if (!GlobalScene.GameMng.IsGamePlay)
            {
                Debug.LogWarning("Failed : ");
                return null;
            }
            return player;
        }
    }

    Player player = null;


    protected override void OnAwake() { }
    public override void OnReset()
    {
        player = null;
    }

    #region Player

    public void SetPalyer(Player pGamePlayer)
    {
        player = pGamePlayer;
    }
    public void SetPlayerRespawn()
    {
        player = null;

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