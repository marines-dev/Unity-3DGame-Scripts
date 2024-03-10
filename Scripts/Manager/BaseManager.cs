using UnityEngine;

public abstract class BaseManager : MonoBehaviour
{
    private void Awake()
    {
        OnAwake();
        OnReset();
    }

    private void OnDestroy()
    {
        OnReset();
    }

    protected abstract void OnAwake();

    /// <summary>
    /// 활성 씬이 언로드될 때 호출되는 함수 입니다.
    /// </summary>
    public abstract void OnReset();
}
