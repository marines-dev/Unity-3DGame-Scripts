using System;
using System.Collections.Generic;
using UnityEngine;

[Obsolete("Managers 전용 : 일반 클래스에서 사용할 수 없습니다. Managers를 이용해 주세요.")]
public class GameManagerEX : BaseManager
{
    bool isPaused = false; //앱의 활성화 상태 저장 유무

    //
    public bool IsGamePlay
    {
        get
        {
            bool isWorldScene = Managers.Scene.currentSceneType == Define.Scene.WorldScene;
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
            if (!Managers.Game.IsGamePlay)
            {
                Debug.LogWarning("Failed : ");
                return null;
            }
            return gamePlayer;
        }
    }

    GamePlayer gamePlayer = null;


    protected override void InitDataProcess() { }
    protected override void ResetDataProcess()
    {
        gamePlayer = null;
    }

    #region Player

    public void SetPalyer(GamePlayer pGamePlayer)
    {
        gamePlayer = pGamePlayer;
    }
    public void SetPlayerRespawn()
    {
        gamePlayer = null;

        // Respawn
        //Managers.Spawn.SpawnCharacter(Managers.User.SpawnerID);
        Managers.User.UpdateUserData();
        Managers.Scene.LoadScene<WorldScene>(); // WorldScene을 재로드 합니다.
    }

    #endregion Player

    public ITargetHandler GetTargetCharacter(GameObject pTarget)
    {
        if(pTarget == null)
        {
            Debug.LogWarning("Falied : ");
            return null;
        }

        BaseCharacter baseCharacter = pTarget.GetComponent<BaseCharacter>();
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
            if (Managers.Scene.currentSceneType == Define.Scene.None)
                Managers.User.UpdateUserData();
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
        if (Managers.Scene.currentSceneType == Define.Scene.WorldScene)
            Managers.User.UpdateUserData(); //Managers.User.SaveUserData();
    }
}