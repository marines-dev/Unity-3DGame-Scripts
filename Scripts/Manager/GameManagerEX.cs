using System;
using System.Collections.Generic;
using UnityEngine;


public class GameManagerEX : BaseManager
{
    bool isPaused = false; //앱의 활성화 상태 저장 유무

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
                Debug.Log("Failed : 게임을 플레이할 수 없습니다.");
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
        GlobalScene.SceneMng.LoadBaseScene<WorldScene>(); // WorldScene을 재로드 합니다.
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
        if (pause) // 앱이 비활성화 되었을 때 처리
        {
            isPaused = true;
            if (GlobalScene.SceneMng.IsActiveScene<WorldScene>())
                GlobalScene.UserMng.UpdateUserData();
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
        if (GlobalScene.SceneMng.IsActiveScene<WorldScene>())
            GlobalScene.UserMng.UpdateUserData(); //Managers.User.SaveUserData();
    }
}