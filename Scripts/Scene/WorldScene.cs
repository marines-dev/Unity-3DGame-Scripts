
using System;
using System.Collections;
using UnityEngine;

public class WorldScene : BaseScene
{
    protected override IEnumerator LoadingProcessRoutine()
    {
        // LoadTable
        {
            Managers.Table.LoadTable<Table.Spawning>();
            Managers.Table.LoadTable<Table.Spawner>();
            Managers.Table.LoadTable<Table.Character>();
            Managers.Table.LoadTable<Table.Stat>();
        }

        // SetUI
        {
            // Joystick
            Managers.UI.LoadUI<JoystickUI>();
            JoystickUI joystickUI = Managers.UI.GetBaseUI<JoystickUI>();
            joystickUI.CloseUI();
        }

        // Cursor
        //gameObject.GetOrAddComponent<CursorController>();

        // Player

        //Dictionary<int, Data.StatData> dict = Managers.Data.StatDict;
        Managers.User.LoadUserData();
        Managers.Spawn.SetPlayerSpawner();
        Managers.Spawn.SpawnCharacter(Managers.User.SpawnerID);
        yield return new WaitUntil(() => Managers.Game.IsGamePlay); // 플레이어가 저장되면 다음 구문으로 이동

        // Controller
        Managers.GUI.SetWorldSceneController();
        yield return null;

        // Camera
        Managers.CameraEX.SetWorldSceneCamera();
        yield return null;

        // Monster
        int tempSpawnerID = 1;
        Managers.Spawn.SetMonsterSpawner(tempSpawnerID);
    }

    protected override void OpenScene()
    {
        Managers.GUI.StartJoystickController();
    }

    protected override void CloseScene()
    {
    }

    //IEnumerator SpawnPlayerProcessRoutine()
    //{
    //    //Dictionary<int, Data.StatData> dict = Managers.Data.StatDict;
    //    Managers.Spawn.SetPlayerSpawner();
    //    Managers.Spawn.SpawnCharacter(Managers.User.SpawnerID);
    //    yield return null;
    //}
}
