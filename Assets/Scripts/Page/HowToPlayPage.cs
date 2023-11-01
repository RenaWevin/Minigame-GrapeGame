
using UnityEngine;
using UnityEngine.UI;

public class HowToPlayPage : MonoBehaviour {

    #region UI物件

    [SerializeField]
    private CanvasGroupWindow canvasGroupWindow;

    [SerializeField]
    private Button Button_Close;

    #endregion
    #region Awake

    private void Awake() {
        Button_Close.onClick.AddListener(delegate {
            SetShowWindow(false);
        });
    }

    #endregion
    #region UI事件

    /// <summary>
    /// 顯示/關閉視窗
    /// </summary>
    /// <param name="show"></param>
    public void SetShowWindow(bool show) {
        canvasGroupWindow.SetShowWindow(show);
    }

    #endregion

}
