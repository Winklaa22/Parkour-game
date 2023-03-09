using UnityEngine;

public abstract class SceneSingleton : MonoBehaviour
{
    protected virtual void Awake() { }

    protected virtual void Start() { }
    protected virtual void OnDestroy() { }
}

public abstract class SceneSingleton<T> : SceneSingleton where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    protected sealed override void Awake()
    {
        Instance = this as T;
        OnAwake();
    }

    protected sealed override void Start()
    {
        OnStart();
    }

    protected sealed override void OnDestroy()
    {
        OnFinalize();
    }

    protected virtual void OnAwake() { }

    protected virtual void OnStart() { }

    protected virtual void OnFinalize() { }
}
