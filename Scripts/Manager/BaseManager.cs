using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseManager : MonoBehaviour
{
    protected abstract void InitDataProcess();
    protected abstract void ResetDataProcess();

    /// <summary>
    /// InitScene에서 초기화 합니다.
    /// (SceneManager 제외)
    /// </summary>
    public void InitData()
    {
        InitDataProcess();
        Debug.Log($"Success : {this.GetType().Name}의 초기화를 완료했습니다.");
    }

    /// <summary>
    /// LoadScene에서 리셋 합니다.
    /// (SceneManager 제외)
    /// </summary>
    public void ResetData()
    {
        ResetDataProcess();
        Debug.Log($"Success : {this.GetType().Name}의 Reset을 완료했습니다.");
    }
}
