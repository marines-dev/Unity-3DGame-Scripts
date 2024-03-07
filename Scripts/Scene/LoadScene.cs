using System.Collections;
using UnityEngine;

public class LoadScene : BaseScene
{
    protected override IEnumerator LoadingProcessRoutine()
    {
        // ResetManager
        {
            // Base
            Managers.Game.ResetData();
            Managers.CameraEX.ResetData();
            Managers.Resource.ResetData();
            Managers.Table.ResetData();
            Managers.UI.ResetData();
            Managers.GUI.ResetData();

            // Server
            Managers.Backend.ResetData();
            Managers.GPGS.ResetData();
            Managers.LogIn.ResetData();

            //
            Managers.User.ResetData();
            //Managers.Input.Init();
            Managers.Spawn.ResetData();
        }
        yield return null;

        // Load UI
        {

        }
        yield return null;
    }

    protected override void OpenScene() { }
    protected override void CloseScene() { }
}
