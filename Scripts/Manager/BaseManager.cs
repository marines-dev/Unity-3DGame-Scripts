using UnityEngine;

public abstract class BaseManager : MonoBehaviour
{
    private void Awake()
    {
        OnAwake();
        Initialize();
    }

    protected abstract void OnAwake();
    protected abstract void OnInit();

    public void Initialize()
    {
        OnInit();
    }
}
