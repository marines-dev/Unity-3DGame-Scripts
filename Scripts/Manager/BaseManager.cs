using Interface;
using UnityEngine;

public abstract class BaseManager<TMng> : IBaseManager where TMng : class, IBaseManager, new()
{
    private static TMng instance = null;
    public static TMng Instance
    {
        get
        {
            if (instance == null)
            {
                instance = ManagerLoader.CreateManager<TMng>();
            }
            return instance;
        }
    }

    public BaseManager()
    {
        Util.LogSuccess($"<{typeof(TMng).ToString()}> Manager가 생성되었습니다.");

        OnInitialized();
    }

    ~BaseManager()
    {
        Util.LogSuccess($"<{typeof(TMng).ToString()}> Manager가 삭제되었습니다.");
    }

    protected abstract void OnInitialized();
    public abstract void OnRelease();
}
