
using UnityEngine;

public static class CanvasGroupExtension {

    public static void SetEnable(this CanvasGroup canvasGroup, bool enabled) {
        canvasGroup.alpha = enabled ? 1 : 0;
        canvasGroup.interactable = enabled;
        canvasGroup.blocksRaycasts = enabled;
    }

}
