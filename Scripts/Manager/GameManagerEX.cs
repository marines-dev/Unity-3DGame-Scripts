using System;
using System.Collections.Generic;
using UnityEngine;


public class GameManagerEX : BaseManager<GameManagerEX>
{
    bool isPaused = false; //앱의 활성화 상태 저장 유무

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
                Debug.Log("Failed : 게임을 플레이할 수 없습니다.");
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
        SceneManager.Instance.LoadBaseScene<WorldScene>(); // WorldScene을 재로드 합니다.
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
            if (SceneManager.Instance.IsActiveScene<WorldScene>())
                UserManager.Instance.UpdateUserData();
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
        if (SceneManager.Instance.IsActiveScene<WorldScene>())
            UserManager.Instance.UpdateUserData(); //Managers.User.SaveUserData();
    }
}