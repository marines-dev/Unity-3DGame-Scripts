using UnityEngine;

public abstract class BaseHandler : MonoBehaviour
{
    public bool IsPlaying { get; private set; } = false;

    public void Play()
    {
        IsPlaying = true;
        gameObject.SetActive(true);
        OnPlay();
        Debug.Log($"Play : {this.name}�� �����մϴ�.");
    }

    public void Stop()
    {
        IsPlaying = false;
        gameObject.SetActive(false);
        OnStop();

        Debug.Log($"Stop : {this.name}�� �����մϴ�.");
    }

    protected virtual void OnPlay() { }
    protected virtual void OnStop() { }
}
