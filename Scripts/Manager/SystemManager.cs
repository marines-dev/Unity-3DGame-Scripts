using System;
using UnityEngine;


[Obsolete]
public static class SystemManager
{
    private static bool isPaused = false; //앱의 활성화 상태 저장 유무

    private static void OnApplicationPause(bool pause) { }
    [Obsolete("테스트 중")]
    private static void OnApplicationQuit() { } // 앱이 종료 될 때 처리
}

