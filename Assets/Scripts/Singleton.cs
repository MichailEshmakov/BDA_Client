using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    virtual protected void Awake()
    {
        if (Instance == null)
        {
            Instance = FindObjectOfType<T>();
        }
        else if (Instance != FindObjectOfType<T>())
        {
            Destroy(FindObjectOfType<T>());
        }
    }
}

