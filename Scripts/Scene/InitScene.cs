using System;
using System.Collections;
using UnityEngine;

public class InitScene : BaseScene
{
    protected override IEnumerator LoadingProcessRoutine()
    {
        Managers.InitData();
        Managers.Game.InitData();
        Managers.CameraEX.InitData();
        Managers.Resource.InitData();
        Managers.Table.InitData();
        Managers.UI.InitData();
        Managers.GUI.InitData();

        // ResisteredUI
        Managers.UI.ResisteredBaseUI();

        // LoadData
        Managers.UI.LoadUI<LoadingUI>();
        yield return null;

        CompleteInitSceneLoading();
    }

    protected override void OpenScene()
    {
        Managers.Scene.LoadScene<TitleScene>();
    }

    protected override void CloseScene()
    {
    }
}
