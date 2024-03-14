using System;
using System.Collections.Generic;
using UnityEngine;


public class SystemManager : Manager//BaseManager<SystemManager>
{
    bool isPaused = false; //앱의 활성화 상태 저장 유무

    [Obsolete("임시")]
    Light mainLight = null;
    public Light MainLight
    {
        get
        {
            if (mainLight == null)
            {
                mainLight = FindOrCreateMainLight();
            }
            return mainLight;
        }
    }

    //
    //public bool IsGamePlay
    //{
    //    get
    //    {
    //        bool isWorldScene = SceneManager.Instance.IsActiveScene<WorldScene>();
    //        bool isSpawnPlayer = player != null;
    //        bool isLivePlayer = player != null && player.BaseAnimType != Define.BaseAnim.Die;

    //        //
    //        bool isGamePlay = isWorldScene && isSpawnPlayer && isLivePlayer;
    //        if (isGamePlay)
    //        {
    //            return true;
    //        }
    //        else
    //        {
    //            Debug.Log("Failed : 게임을 플레이할 수 없습니다.");
    //            return false;
    //        }

    //    }
    //}

    //public IControllHndl_Temp playerCtrl
    //{
    //    get
    //    {
    //        if (!GameManagerEX.Instance.IsGamePlay)
    //        {
    //            Debug.LogWarning("Failed : ");
    //            return null;
    //        }
    //        return player;
    //    }
    //}

    //Player player = null;


    protected override void OnInitialized() 
    {
        mainLight = FindOrCreateMainLight();

        SceneMng.AddSceneLoadedEvent(AddSceneLoadedEvent_SystemManager);
    }

    public override void OnRelease()
    {
        //player = null;
    }

    #region Player

    //public void SetPalyer(Player pGamePlayer)
    //{
    //    player = pGamePlayer;
    //}

    //public void SetPlayerRespawn()
    //{
    //    //player = null;

    //    // Respawn
    //    //Managers.Spawn.SpawnCharacter(Managers.User.SpawnerID);
    //    UserManager.Instance.UpdateUserData();
    //    SceneManager.Instance.LoadBaseScene<WorldScene>(); // WorldScene을 재로드 합니다.
    //}

    #endregion Player

    //public ITargetHandler_Temp GetTargetCharacter(GameObject pTarget)
    //{
    //    if (pTarget == null)
    //    {
    //        Debug.LogWarning("Falied : ");
    //        return null;
    //    }

    //    Character baseCharacter = pTarget.GetComponent<Character>();
    //    if (baseCharacter == null)
    //    {
    //        Debug.LogWarning("Falied : ");
    //        return null;
    //    }

    //    return baseCharacter;
    //}

    void OnApplicationPause(bool pause)
    {
        if (pause) // 앱이 비활성화 되었을 때 처리
        {
            isPaused = true;
            if (SceneMng.IsActiveScene<WorldScene>())
                UserMng.UpdateUserData();
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
        if (SceneMng.IsActiveScene<WorldScene>())
            UserMng.UpdateUserData(); //Managers.User.SaveUserData();
    }

    private Light FindOrCreateMainLight()
    {
        if (mainLight == null)
        {
            mainLight = GameObject.FindObjectOfType<Light>();
            if (mainLight == null)
            {
                mainLight = Util.CreateGameObject<Light>();
            }

            mainLight.gameObject.name = $"@{typeof(Light).Name}";
        }

        /// 중복 검사
        Light[] eventSystem_arr = GameObject.FindObjectsOfType<Light>();
        foreach (Light eventSystem in eventSystem_arr)
        {
            if (eventSystem != null && eventSystem != mainLight)
            {
                ResourceMng.DestroyGameObject(eventSystem.gameObject);
            }
        }

        ///
        GameObject.DontDestroyOnLoad(mainLight);
        return mainLight;
    }

    private void AddSceneLoadedEvent_SystemManager(UnityEngine.SceneManagement.Scene pScene, UnityEngine.SceneManagement.LoadSceneMode pLoadSceneMode)
    {
        mainLight = FindOrCreateMainLight();
    }
}