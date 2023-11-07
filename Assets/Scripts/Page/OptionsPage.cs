
using System.Collections.Generic;
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
    #region  -> 設定頁面

    [SerializeField]
    private Dropdown Dropdown_FruitSpriteType;

    #endregion

    #endregion
    #region 參數參考區

    [SerializeField]
    private Color color_PageImageColor_On;
    [SerializeField]
    private Color color_PageImageColor_Off;
    
    //取得頁籤顏色
    private Color GetPageImageColor(bool on) {
        return on ? color_PageImageColor_On : color_PageImageColor_Off;
    }

    //水果圖片種類下拉選單選項對應
    private readonly List<FruitSpriteType> fruitSpriteType_DropdownOptions = new List<FruitSpriteType>();

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

        #region  -> 初始化水果圖片種類下拉選單

        //將選項清除並重新填入
        Dropdown_FruitSpriteType.ClearOptions();
        fruitSpriteType_DropdownOptions.Clear();
        List<string> fruitSpriteTypeStrings = new List<string>();
        fruitSpriteType_DropdownOptions.Add(FruitSpriteType.Normal);
        fruitSpriteTypeStrings.Add("一般水果");
        fruitSpriteType_DropdownOptions.Add(FruitSpriteType.TofuSkin);
        fruitSpriteTypeStrings.Add("豆皮版水果");
        Dropdown_FruitSpriteType.AddOptions(fruitSpriteTypeStrings);

        #endregion

    }

    #endregion
    #region 設定相關事件

    #region  -> 儲存全部目前選擇的設定

    /// <summary>
    /// 儲存全部目前選擇的設定
    /// </summary>
    private void SaveAllSettings() {
        //水果圖片種類下拉選單
        FruitSpriteType fruitSpriteType = fruitSpriteType_DropdownOptions[Dropdown_FruitSpriteType.value];
        PlayerPrefHelper.SetSetting_FruitSpriteType(fruitSpriteType);
    }

    #endregion

    #endregion
    #region UI事件

    #region  -> 顯示/關閉視窗

    /// <summary>
    /// 顯示/關閉視窗
    /// </summary>
    /// <param name="show"></param>
    public void SetShowWindow(bool show) {
        if (!show) {
            //儲存全部目前選擇的設定
            SaveAllSettings();
        }
        canvasGroupWindow.SetShowWindow(show);
        if (show) {
            //刷新設定畫面設定數值
            UpdateDisplay_SettingPage();
            //顯示第一頁
            SwitchPanel(OptionsPageType.Settings);
        }
    }

    #endregion
    #region  -> 切換頁面

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
    #region  -> 設定頁面相關

    #region  --> 刷新設定畫面設定數值

    /// <summary>
    /// 刷新設定畫面設定數值
    /// </summary>
    private void UpdateDisplay_SettingPage() {
        //更新水果圖片種類下拉選單index
        FruitSpriteType saved_FruitSpriteType = PlayerPrefHelper.GetSetting_FruitSpriteType();
        int fruitSpriteTypeIndex = fruitSpriteType_DropdownOptions.FindIndex((f) => f == saved_FruitSpriteType);
        Dropdown_FruitSpriteType.value = fruitSpriteTypeIndex;
    }

    #endregion

    #endregion

    #endregion

}
