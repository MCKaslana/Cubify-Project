using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    protected virtual bool IsPersistent => true;
    public static T Instance { get; private set; }

    public virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
            if (IsPersistent)
                DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}