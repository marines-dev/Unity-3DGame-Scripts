using System;
using UnityEngine;

/// <summary>
/// 1. Managers클래스에 해당 매니저의 instance 추가합니다.
/// 2. InitScene클래스에 해당 매니저의 init() 추가합니다.
/// 3. TitleScene클래스에서 해당 매니저의 init() 추가합니다.
/// </summary>


public class SampleManager : BaseManager
{
    protected override void OnAwake() { }
    public override void OnReset() { }
}
