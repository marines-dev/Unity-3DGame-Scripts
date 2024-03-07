using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseManager : MonoBehaviour
{
    protected abstract void InitDataProcess();
    protected abstract void ResetDataProcess();

    /// <summary>
    /// InitScene���� �ʱ�ȭ �մϴ�.
    /// (SceneManager ����)
    /// </summary>
    public void InitData()
    {
        InitDataProcess();
        Debug.Log($"Success : {this.GetType().Name}�� �ʱ�ȭ�� �Ϸ��߽��ϴ�.");
    }

    /// <summary>
    /// LoadScene���� ���� �մϴ�.
    /// (SceneManager ����)
    /// </summary>
    public void ResetData()
    {
        ResetDataProcess();
        Debug.Log($"Success : {this.GetType().Name}�� Reset�� �Ϸ��߽��ϴ�.");
    }
}
