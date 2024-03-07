using System.Collections;
using UnityEngine;

public class SampleScene : BaseScene
{
    protected override IEnumerator LoadingProcessRoutine()
    {
        yield return null;
    }

    protected override void OpenScene() { }
    protected override void CloseScene() { }
}
