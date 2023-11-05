
using UnityEngine;
using UnityEngine.UI;

public class OptionsPage : MonoBehaviour {

    #region enum

    /// <summary>
    /// 頁面列舉
    /// </summary>
    private enum OptionsPageType {
        Settings,
        Staff,
    }

    #endregion
    #region UI物件

    [SerializeField]
    private CanvasGroupWindow canvasGroupWindow;

    [SerializeField]
    private Button Button_Close;

    [SerializeField]
    private Text Text_Options;

    #region  -> 切換頁籤與頁籤本體

    [SerializeField]
    private Button ButtonPage_Settings;
    [SerializeField]
    private ScrollRect ScrollRect_Settings;

    [SerializeField]
    private Button ButtonPage_Staff;
    [SerializeField]
    private ScrollRect ScrollRect_Staff;

    #endregion

    #endregion
    #region 參數參考區

    [SerializeField]
    private Color color_PageImageColor_On;
    [SerializeField]
    private Color color_PageImageColor_Off;

    private Color GetPageImageColor(bool on) {
        return on ? color_PageImageColor_On : color_PageImageColor_Off;
    }

    #endregion
    #region Awake

    private void Awake() {
        //關閉視窗事件
        Button_Close.onClick.AddListener(delegate {
            SetShowWindow(false);
        });
        //頁籤
        ButtonPage_Settings.onClick.AddListener(delegate {
            SwitchPanel(OptionsPageType.Settings);
        });
        ButtonPage_Staff.onClick.AddListener(delegate {
            SwitchPanel(OptionsPageType.Staff);
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
            //顯示第一頁
            SwitchPanel(OptionsPageType.Settings);
        }
    }

    /// <summary>
    /// 切換頁面
    /// </summary>
    /// <param name="optionsPageType"></param>
    private void SwitchPanel(OptionsPageType optionsPageType) {
        ScrollRect_Settings.verticalNormalizedPosition = 1f;
        ScrollRect_Staff.verticalNormalizedPosition = 1f;

        ScrollRect_Settings.gameObject.SetActive(optionsPageType == OptionsPageType.Settings);
        ScrollRect_Staff.gameObject.SetActive(optionsPageType == OptionsPageType.Staff);

        ButtonPage_Settings.targetGraphic.color = GetPageImageColor(optionsPageType == OptionsPageType.Settings);
        ButtonPage_Staff.targetGraphic.color = GetPageImageColor(optionsPageType == OptionsPageType.Staff);

        switch (optionsPageType) {
            case OptionsPageType.Settings:
                Text_Options.text = "設定頁面";
                break;
            case OptionsPageType.Staff:
                Text_Options.text = "製作名單";
                break;
        }
    }

    #endregion

}
