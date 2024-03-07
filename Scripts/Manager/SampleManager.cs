using System;
using UnityEngine;

/// <summary>
/// 1. Managers클래스에 해당 매니저의 instance 추가합니다.
/// 2. InitScene클래스에 해당 매니저의 init() 추가합니다.
/// 3. TitleScene클래스에서 해당 매니저의 init() 추가합니다.
/// </summary>

[Obsolete("Managers 전용 : 일반 클래스에서 사용할 수 없습니다. Managers를 이용해 주세요.")]
public class SampleManager : BaseManager
{
    protected override void OnAwake() { }
    protected override void OnInit() { }
}
