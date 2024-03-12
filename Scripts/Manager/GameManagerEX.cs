using System;
using System.Collections.Generic;
using UnityEngine;


public class GameManagerEX : BaseManager<GameManagerEX>
{
    bool isPaused = false; //���� Ȱ��ȭ ���� ���� ����

    //
    public bool IsGamePlay
    {
        get
        {
            bool isWorldScene = SceneManager.Instance.IsActiveScene<WorldScene>();
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
            if (!GameManagerEX.Instance.IsGamePlay)
            {
                Debug.LogWarning("Failed : ");
                return null;
            }
            return player;
        }
    }

    Player player = null;


    protected override void OnInitialized() { }
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
        UserManager.Instance.UpdateUserData();
        SceneManager.Instance.LoadBaseScene<WorldScene>(); // WorldScene�� ��ε� �մϴ�.
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
            if (SceneManager.Instance.IsActiveScene<WorldScene>())
                UserManager.Instance.UpdateUserData();
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
        if (SceneManager.Instance.IsActiveScene<WorldScene>())
            UserManager.Instance.UpdateUserData(); //Managers.User.SaveUserData();
    }
}