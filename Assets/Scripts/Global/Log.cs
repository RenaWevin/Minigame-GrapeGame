
using UnityEngine;

public static class Log {

    private const bool IS_ALLOW_LOG = true;

    public static void Info(object message) {
        if (IS_ALLOW_LOG) {
            Debug.Log(message);
        }
    }
    
    public static void Warning(object message) {
        if (IS_ALLOW_LOG) {
            Debug.LogWarning(message);
        }
    }
    
    public static void Error(object message) {
        if (IS_ALLOW_LOG) {
            Debug.LogError(message);
        }
    }

}
