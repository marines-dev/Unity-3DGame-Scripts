using System;
using System.Collections.Generic;
using Interface;
using UnityEngine;
using static Define;


public class WorldScene : BaseScene<WorldScene, WorldUI>
{
    /// <summary>
    /// MainUI
    /// </summary>
    //private WorldUI worldUI = null;

    /// <summary>
    /// Input
    /// </summary>
    //public BaseInput BaseInput { get; }

    #region World

    private Player      player      = null;
    private NullObject  nullPlayer  = new NullObject();
    public IPlayerCtrl  PlayerCtrl
    {
        get
        {
            if (player == null || player.ExistenceStateType == Define.ExistenceState.Despawn || player.SurvivalStateType == SurvivalState.Dead)
            {
                Util.LogWarning($"플레이어를 사용할 수 없으므로 {nullPlayer.GetType()}를 반환합니다.");
                return nullPlayer;
            }

            return player;
        }
    }

    private Dictionary<GameObject, int>     fieldSpawning_dic = new Dictionary<GameObject, int>(); /// <EnemyGO, SpawnerID>
    private Dictionary<int, WorldSpawner>   worldSpawner_dic  = new Dictionary<int, WorldSpawner>(); /// <SpawnerID, WorldSpawner>

    #endregion World


    protected override void OnAwake()
    {
        /// CreateTable
        {
            CreateOrGetBaseTable<SpawnerTable>();
            CreateOrGetBaseTable<EnemyTable>();
            CreateOrGetBaseTable<CharacterTable>();
            CreateOrGetBaseTable<StatTable>();
        }

        /// Spawner
        {
            /// Player
            Manager.UserMng.LoadUserData();

            Vector3 spawnPos = new Vector3(7.5f, 0f, 1f);
            Vector3 spawnRot = Quaternion.identity.eulerAngles; 

            GameObject player_go = WorldSpawner.CreateGameObject(Define.Spawning.Player, 1, spawnPos, spawnRot);//임시
            player = player_go.GetOrAddComponent<Player>();

            /// Enemy
            SpawnerTable.Data[] spawnerData_arr = GlobalScene.Instance.SpawnerTable.GetTableDatasAll();
            foreach (SpawnerTable.Data spawnerData in spawnerData_arr)
            {
                WorldSpawner worldSpawner = WorldSpawner.CreateSpawner(spawnerData.ID);
                
                ///
                if(worldSpawner_dic.ContainsKey(spawnerData.ID))
                {
                    Util.LogWarning();
                    worldSpawner = worldSpawner_dic[spawnerData.ID];
                    ResourceLoader.DestroyGameObject(worldSpawner.gameObject);
                }

                worldSpawner_dic.Add(spawnerData.ID, worldSpawner);
            }
        }
    }

    protected override void OnStart()
    {
        // Spawn
        {
            ///Player
            SetSpawning(player.gameObject, Spawning.Player);
            

            /// Enemy
            SwitchSpawnersPooling(true);
        }

        //MainUI.Open();
        CamCtrl.SwitchQuarterViewMoed = true;
    }

    protected override void onDestroy()
    {
        //if(instance != null && instance.gameObject != null)
        //{
        //    ResourceManager.Instance.DestroyGameObject(instance.gameObject);
        //}

        //instance = null;
    }

    public void MoveTitle()
    {
        //MainUI.Close();
        CamCtrl.SwitchQuarterViewMoed = false;
        SwitchSpawnersPooling(false);

        Manager.SceneMng.LoadBaseScene<TitleScene>();
    }

    #region Spawner

    public void SetSpawnSpawning(GameObject pGO, int SpawnerID)
    {
        SpawnerTable.Data spawnerData = GlobalScene.Instance.SpawnerTable.GetTableData(SpawnerID);
        SetSpawning(pGO, spawnerData.SpawningType, spawnerData.SpawningID);

        if(fieldSpawning_dic.ContainsKey(pGO))
        {
            Util.LogWarning();
            fieldSpawning_dic.Remove(pGO);
        }

        fieldSpawning_dic.Add(pGO, SpawnerID);
    }

    private void SetSpawning(GameObject pGO, Spawning pSpawningType, int pSpawningID = 0)
    {
        switch (pSpawningType)
        {
            case Spawning.Player:
                {
                    Player player = pGO.GetOrAddComponent<Player>();
                    player.Spawn(Actor.Player, OnPlayerDeadEvent);
                }
                break;
            case Spawning.Enemy:
                {
                    Enemy enemy = pGO.GetOrAddComponent<Enemy>();
                    enemy.Spawn(Actor.Enemy, pSpawningID);
                }
                break;
        }
    }

    public void SetDespawnSpawning(GameObject pGO, Actor pActorType)
    {
        switch (pActorType)
        {
            case Actor.Player:
                {
                    /// Respawn
                    Manager.SceneMng.LoadBaseScene<WorldScene>(); // WorldScene을 재로드 합니다.
                }
                break;
            case Actor.Enemy:
                {
                    if(fieldSpawning_dic.ContainsKey(pGO))
                    {
                        int spawnerID = fieldSpawning_dic[pGO];
                        if (worldSpawner_dic.ContainsKey(spawnerID))
                        {
                            WorldSpawner worldSpawner = worldSpawner_dic[spawnerID];
                            worldSpawner.Despawn(pGO);
                            fieldSpawning_dic.Remove(pGO);
                            return;
                        }
                    }

                    Util.LogWarning();
                    ResourceLoader.DestroyGameObject(pGO);
                }
                break;
        }
    }

    private void SwitchSpawnersPooling(bool pSwitch)
    {
        foreach (WorldSpawner spawner in worldSpawner_dic.Values)
        {
            if (spawner != null)
            {
                spawner.SwitchPooling = pSwitch;
            }
        }
    }

    #endregion Spawner

    private void OnPlayerDeadEvent()
    {
        //MainUI.Close();
        CamCtrl.SwitchQuarterViewMoed = false;
        SwitchSpawnersPooling(false);
    }

    public ITarget GetTargetCharacter(GameObject pTarget)
    {
        if (pTarget == null)
        {
            Util.LogWarning();
            return null;
        }

        BaseActor baseActor = pTarget.GetComponent<BaseActor>();
        if (baseActor == null)
        {
            Util.LogWarning();
            return null;
        }

        return baseActor;
    }

    [Obsolete("임시")]
    public TSpaceUI CreateBaseSpaceUI<TSpaceUI>(Transform pParent = null) where TSpaceUI : Component, IBaseUI
    {
        return UILoader.CreateBaseSpaceUI<TSpaceUI>(pParent);
    }


}
