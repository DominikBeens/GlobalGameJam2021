using UnityEngine;
using System.Linq;

public abstract class ScriptableObjectSingleton<T> : ScriptableObject where T : ScriptableObject {

    private static bool firstGet = true;

    private static T instance;

    public static T Instance {
        get {
            if (!instance) {
                T[] types = GetTypes();
                instance = types.FirstOrDefault();
                if (!instance && firstGet) {
                    Debug.LogError($"No scriptable object instance of type {typeof(T)} has been found! Try calling {typeof(T)}.Exists first.");
                }
            }
            firstGet = false;
            return instance;
        }
    }

    public static T[] GetTypes() {
        Resources.LoadAll<T>("");
        return Resources.FindObjectsOfTypeAll<T>();
    }
}

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour {

    private static bool firstGet = true;

    private static T instance;

    public static T Instance {
        get {
            if (!instance) {
                instance = FindObjectOfType<T>();
                if (!instance && firstGet) {
                    Debug.LogError($"No singleton instance of type {typeof(T)} has been found! Try calling {typeof(T)}.Exists first.");
                }
            }
            firstGet = false;
            return instance;
        }
    }

    public static bool Exists => instance != null;

    protected virtual void Awake() {
        if (Instance != this) {
            Destroy(gameObject);
        }
    }
}
