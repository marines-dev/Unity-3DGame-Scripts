using System;
using UnityEngine;


[Obsolete]
public static class SystemManager
{
    private static bool isPaused = false; //���� Ȱ��ȭ ���� ���� ����

    private static void OnApplicationPause(bool pause) { }
    [Obsolete("�׽�Ʈ ��")]
    private static void OnApplicationQuit() { } // ���� ���� �� �� ó��
}

