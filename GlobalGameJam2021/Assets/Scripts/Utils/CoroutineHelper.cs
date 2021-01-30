using UnityEngine;
using System.Collections;
using System;

public class CoroutineHelper : MonoBehaviour {

    private static CoroutineHelper instance;

    private static CoroutineHelper Instance {
        get {
            if (!instance) {
                GameObject go = new GameObject("CoroutineHelper");
                instance = go.AddComponent<CoroutineHelper>();
            }

            return instance;
        }
    }

    public static Coroutine Start(IEnumerator routine) {
        return Instance.StartLocalCoroutine(routine);
    }

    public static void Stop(Coroutine routine) {
        if (routine == null) { return; }
        Instance.StopLocalCoroutine(routine);
    }

    private Coroutine StartLocalCoroutine(IEnumerator routine) {
        return StartCoroutine(routine);
    }

    private void StopLocalCoroutine(Coroutine routine) {
        StopCoroutine(routine);
    }

    public static Coroutine Delay(Action action, float time, bool unscaledTime = false) {
        if (!unscaledTime) {
            return Instance.StartLocalCoroutine(Instance.DelayOverTime(action, time));
        } else {
            return Instance.StartLocalCoroutine(Instance.DelayOverTimeUnScaled(action, time));
        }
    }

    private IEnumerator DelayOverTime(Action action, float time) {
        yield return new WaitForSeconds(time);
        action?.Invoke();
    }

    private IEnumerator DelayOverTimeUnScaled(Action action, float time) {
        yield return new WaitForSecondsRealtime(time);
        action?.Invoke();
    }
}