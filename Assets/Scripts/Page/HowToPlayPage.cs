
using UnityEngine;
using UnityEngine.UI;

public class HowToPlayPage : MonoBehaviour {

    #region UI物件

    [SerializeField]
    private CanvasGroupWindow canvasGroupWindow;

    [SerializeField]
    private Button Button_Close;

    #region  -> 控制方法

    [SerializeField]
    private Text Text_Controll_HowToPlay;

    #endregion

    #endregion
    #region Awake

    private void Awake() {
        Button_Close.onClick.AddListener(delegate {
            Core.Instance.audioComponent.PlaySound(SoundId.Click_Close);
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
        if (show) {
            UpdateDisplayPage();
        }
    }

    /// <summary>
    /// 更新頁面
    /// </summary>
    private void UpdateDisplayPage() {
        //操作方法
        OptionsPage optionsPage = Core.Instance.optionsPage;
        string leftkey = Core.Instance.keyCodeNameComponent.GetKeyCodeName(optionsPage.keycode_MoveLeft);
        string rightkey = Core.Instance.keyCodeNameComponent.GetKeyCodeName(optionsPage.keycode_MoveRight);
        string putFruit = Core.Instance.keyCodeNameComponent.GetKeyCodeName(optionsPage.keycode_PutFruit);
        Text_Controll_HowToPlay.text = $"使用 {leftkey} 與 {rightkey} 左右移動\n按 {putFruit} 放下水果";
    }

    #endregion

}
