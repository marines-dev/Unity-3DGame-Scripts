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
    /// Ȱ�� ���� ��ε�� �� ȣ��Ǵ� �Լ� �Դϴ�.
    /// </summary>
    public abstract void OnReset();
}
