
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

    [Space]

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

    [Space]

    //音樂設定
    [SerializeField]
    private Toggle Toggle_BGM;
    [SerializeField]
    private Slider Slider_BGM;
    [SerializeField]
    private Text Text_BGMValue;

    //音效設定
    [SerializeField]
    private Toggle Toggle_SoundFX;
    [SerializeField]
    private Slider Slider_SoundFX;
    [SerializeField]
    private Text Text_SoundFXValue;

    //水果圖片種類設定 
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

        #region  -> 初始化音量設定

        Toggle_BGM.onValueChanged.AddListener(OnValueChanged_Toggle_BGM);
        Slider_BGM.onValueChanged.AddListener(OnValueChanged_Slider_BGM);

        Toggle_SoundFX.onValueChanged.AddListener(OnValueChanged_Toggle_Sound);
        Slider_SoundFX.onValueChanged.AddListener(OnValueChanged_Slider_Sound);

        #endregion
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
        //更新音量與啟用
        PlayerPrefHelper.SetSetting_Enable_BGM(Toggle_BGM.isOn);
        PlayerPrefHelper.SetSetting_Volume_BGM((int)Slider_BGM.value);
        PlayerPrefHelper.SetSetting_Enable_SoundFX(Toggle_SoundFX.isOn);
        PlayerPrefHelper.SetSetting_Volume_SoundFX((int)Slider_SoundFX.value);
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
        //更新音量與啟用
        Toggle_BGM.isOn = Slider_BGM.interactable = PlayerPrefHelper.GetSetting_Enable_BGM();
        int volumeBGM = PlayerPrefHelper.GetSetting_Volume_BGM();
        Slider_BGM.value = volumeBGM;
        Text_BGMValue.text = volumeBGM.ToString();
        Toggle_SoundFX.isOn = Slider_SoundFX.interactable = PlayerPrefHelper.GetSetting_Enable_SoundFX();
        int volumeSound = PlayerPrefHelper.GetSetting_Volume_SoundFX();
        Slider_SoundFX.value = volumeSound;
        Text_SoundFXValue.text = volumeSound.ToString();
        //更新水果圖片種類下拉選單index
        FruitSpriteType saved_FruitSpriteType = PlayerPrefHelper.GetSetting_FruitSpriteType();
        int fruitSpriteTypeIndex = fruitSpriteType_DropdownOptions.FindIndex((f) => f == saved_FruitSpriteType);
        Dropdown_FruitSpriteType.value = fruitSpriteTypeIndex;
    }

    #endregion

    #region  --> 切換音樂啟用停用

    private void OnValueChanged_Toggle_BGM(bool isOn) {
        Core.Instance.audioComponent.SetBGMMute(!isOn);
        Slider_BGM.interactable = isOn;
    }

    #endregion
    #region  --> 拖曳音樂音量條

    private void OnValueChanged_Slider_BGM(float value) {
        int valueInt = (int)value;
        Core.Instance.audioComponent.SetBGMVolume(valueInt);
        Text_BGMValue.text = valueInt.ToString();
    }

    #endregion
    #region  --> 切換音效啟用停用

    private void OnValueChanged_Toggle_Sound(bool isOn) {
        Core.Instance.audioComponent.SetSoundMute(!isOn);
        Slider_SoundFX.interactable = isOn;
    }

    #endregion
    #region  --> 拖曳音效音量條

    private void OnValueChanged_Slider_Sound(float value) {
        int valueInt = (int)value;
        Core.Instance.audioComponent.SetSoundFXVolume(valueInt);
        //Core.Instance.audioComponent.PlaySound(); //★
        Text_SoundFXValue.text = valueInt.ToString();
    }

    #endregion

    #endregion

    #endregion

}
