
using System;
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
    [Header("切換頁籤與頁籤本體")]

    [SerializeField]
    private Button ButtonPage_Settings;
    [SerializeField]
    private ScrollRect ScrollRect_Settings;

    [SerializeField]
    private Button ButtonPage_Staff;
    [SerializeField]
    private ScrollRect ScrollRect_Staff;

    [SerializeField]
    private Color color_PageImageColor_On;
    [SerializeField]
    private Color color_PageImageColor_Off;

    //取得頁籤顏色
    private Color GetPageImageColor(bool on) {
        return on ? color_PageImageColor_On : color_PageImageColor_Off;
    }

    #endregion
    #region  -> 設定頁面

    [Space]
    [Header("設定頁面")]

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

    //電腦版專屬設定外層物件
    [SerializeField]
    private GameObject Obj_SettingGroup_PC;

    //按鍵設定
    [SerializeField]
    private KeybindUIObject KeybindUIObject_MoveLeft;
    [SerializeField]
    private KeybindUIObject KeybindUIObject_MoveRight;
    [SerializeField]
    private KeybindUIObject KeybindUIObject_PutFruit;

    //視窗設定
    [SerializeField]
    private Dropdown Dropdown_WindowType;
    //解析度
    [SerializeField]
    private Dropdown Dropdown_Resolution;

    #endregion
    #region  -> 按鍵設定頁面

    [Space]
    [Header("按鍵設定頁面")]

    [SerializeField]
    private CanvasGroup CanvasGroup_KeyReceiveWindow;

    [SerializeField]
    private Text Text_KeyReceiveWindowTitle;

    [SerializeField]
    private string string_KeyReceiveWindowTitleTextFormat;

    [SerializeField]
    private AnykeyReceiver anykeyReceiver;

    #endregion

    #endregion
    #region 參數參考區

    //水果圖片種類下拉選單選項對應
    private readonly List<FruitSpriteType> fruitSpriteType_DropdownOptions = new List<FruitSpriteType>();

    private float soundSliderDelayTime = 0f;

    #region  -> 電腦版-按鍵設定參數

    public KeyCode keycode_MoveLeft { private set; get; } = KeyCode.LeftArrow;
    public KeyCode keycode_MoveRight { private set; get; } = KeyCode.RightArrow;
    public KeyCode keycode_PutFruit { private set; get; } = KeyCode.Space;

    #endregion
    #region  -> 電腦版-全螢幕設定

    private readonly List<FullScreenMode> fullScreenMode_DropdownOptions = new List<FullScreenMode>();

    #endregion
    #region  -> 電腦版-解析度參數

    private struct WidthAndHeight {
        public WidthAndHeight(int w, int h) {
            width = w;
            height = h;
        }
        public int width;
        public int height;
    }

    private readonly List<Resolution> resolution_DropdownOptions = new List<Resolution>();

    #endregion

    #endregion
    #region Awake

    private void Awake() {
        //關閉視窗事件
        Button_Close.onClick.AddListener(delegate {
            Core.Instance.audioComponent.PlaySound(SoundId.Click_Close);
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
        #region  -> 顯示/隱藏電腦版設定

        bool showPCSetting;
#if UNITY_STANDALONE
        showPCSetting = true;
#else
            showPCSetting = false;
#endif
        Obj_SettingGroup_PC.SetActive(showPCSetting);

        #endregion
        #region  -> 初始化按鍵設定列表-電腦版專屬

#if UNITY_STANDALONE

        KeybindUIObject_MoveLeft.onClick.RemoveAllListeners();
        KeybindUIObject_MoveLeft.onClick.AddListener(delegate {
            OnClick_KeybindUIObject_OpenAnyKeyReceiverWindow(
                KeybindUIObject_MoveLeft.GetNameOfKeyBind(),
                (key) => {
                    keycode_MoveLeft = key;
                }
            );
        });
        KeybindUIObject_MoveRight.onClick.RemoveAllListeners();
        KeybindUIObject_MoveRight.onClick.AddListener(delegate {
            OnClick_KeybindUIObject_OpenAnyKeyReceiverWindow(
                KeybindUIObject_MoveRight.GetNameOfKeyBind(),
                (key) => {
                    keycode_MoveRight = key;
                }
            );
        });
        KeybindUIObject_PutFruit.onClick.RemoveAllListeners();
        KeybindUIObject_PutFruit.onClick.AddListener(delegate {
            OnClick_KeybindUIObject_OpenAnyKeyReceiverWindow(
                KeybindUIObject_PutFruit.GetNameOfKeyBind(),
                (key) => {
                    keycode_PutFruit = key;
                }
            );
        });

        //電腦版-按鍵設定
        keycode_MoveLeft = PlayerPrefHelper.GetSetting_Keybind_MoveLeft();
        keycode_MoveRight = PlayerPrefHelper.GetSetting_Keybind_MoveRight();
        keycode_PutFruit = PlayerPrefHelper.GetSetting_Keybind_PutFruit();

#endif

        #endregion
        #region  -> 初始化視窗設定與解析度列表-電腦版專屬

#if UNITY_STANDALONE

        Dropdown_WindowType.ClearOptions();
        List<string> windowTypeStrings = new List<string>();
        fullScreenMode_DropdownOptions.Clear();
        #region  --> 處理選項
        windowTypeStrings.Add("視窗化");
        fullScreenMode_DropdownOptions.Add(FullScreenMode.Windowed);
        windowTypeStrings.Add("無邊框全螢幕視窗");
        fullScreenMode_DropdownOptions.Add(FullScreenMode.FullScreenWindow);
#if UNITY_STANDALONE_WIN
        windowTypeStrings.Add("全螢幕");
        fullScreenMode_DropdownOptions.Add(FullScreenMode.ExclusiveFullScreen);
#endif
        #endregion
        Dropdown_WindowType.AddOptions(windowTypeStrings);
        Dropdown_WindowType.onValueChanged.AddListener(OnValueChanged_Dropdown_WindowType);

        Dropdown_Resolution.ClearOptions();
        List<string> resolutionStrings = new List<string>();
        resolution_DropdownOptions.Clear();
        int screenHeight = Screen.currentResolution.height;
        #region  --> 處理選項
        var supportResolutions = Screen.resolutions;
        for (int i = supportResolutions.Length - 1; i >= 0; i--) {
            var newResolution = supportResolutions[i];
            int ratioRemainder = newResolution.width % 16;
            if (ratioRemainder != 0) {
                //非整除時屏除
                continue;
            }
            int checkRatioMultipier = newResolution.width / 16;
            int heightRatio = newResolution.height / checkRatioMultipier;
            int heightRemainder = newResolution.height % checkRatioMultipier;
            if (heightRatio == 9 && heightRemainder == 0) {
                //只有完全16:9的視窗會被採用
                if (resolution_DropdownOptions.Exists((r) => r.width == newResolution.width && r.height == newResolution.height)) {
                    //已經重複的解析度要篩選掉，這邊不管FPS
                    continue;
                }
                resolutionStrings.Add($"{newResolution.width} x {newResolution.height}");
                resolution_DropdownOptions.Add(newResolution);
            }
        }
        if (resolution_DropdownOptions.Count <= 0) {
            var newResolution = new Resolution() {
                width = 1280,
                height = 720,
                refreshRate = 60
            };
            resolutionStrings.Add($"{newResolution.width} x {newResolution.height}");
            resolution_DropdownOptions.Add(newResolution);
        } else {
            var lastResolution = resolution_DropdownOptions[resolution_DropdownOptions.Count - 1];
            int lastMultipier = lastResolution.height / 9;
            int startMultipier = ((lastMultipier / 10) - 1) * 10;
            for (int i = startMultipier; i >= 50; i -= 10) {
                var newResolution = new Resolution() {
                    width = i * 16,
                    height = i * 9,
                    refreshRate = 60
                };
                resolutionStrings.Add($"{newResolution.width} x {newResolution.height}");
                resolution_DropdownOptions.Add(newResolution);
            }
        }
        resolutionStrings.Add("規格外");
        resolution_DropdownOptions.Add(new Resolution() {
            width = -1,
            height = -1,
            refreshRate = -1
        });
        #endregion
        Dropdown_Resolution.AddOptions(resolutionStrings);
        Dropdown_Resolution.onValueChanged.AddListener(OnValueChanged_Dropdown_Resolution);

#endif

        #endregion
        #region  -> 按鈕設定頁面

        CanvasGroup_KeyReceiveWindow.SetEnable(false);
        anykeyReceiver.onKeyDown.RemoveAllListeners();
        anykeyReceiver.enabled = false;

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
        //儲存按鍵設定
        PlayerPrefHelper.SetSetting_Keybind_MoveLeft(keycode_MoveLeft);
        PlayerPrefHelper.SetSetting_Keybind_MoveRight(keycode_MoveRight);
        PlayerPrefHelper.SetSetting_Keybind_PutFruit(keycode_PutFruit);
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
        Slider_BGM.SetValueWithoutNotify(volumeBGM);
        Text_BGMValue.text = volumeBGM.ToString();
        Toggle_SoundFX.isOn = Slider_SoundFX.interactable = PlayerPrefHelper.GetSetting_Enable_SoundFX();
        int volumeSound = PlayerPrefHelper.GetSetting_Volume_SoundFX();
        Slider_SoundFX.SetValueWithoutNotify(volumeSound);
        Text_SoundFXValue.text = volumeSound.ToString();
        //更新水果圖片種類下拉選單index
        FruitSpriteType saved_FruitSpriteType = PlayerPrefHelper.GetSetting_FruitSpriteType();
        int fruitSpriteTypeIndex = fruitSpriteType_DropdownOptions.FindIndex((f) => f == saved_FruitSpriteType);
        Dropdown_FruitSpriteType.value = fruitSpriteTypeIndex;
        //電腦版-按鍵設定
        keycode_MoveLeft = PlayerPrefHelper.GetSetting_Keybind_MoveLeft();
        keycode_MoveRight = PlayerPrefHelper.GetSetting_Keybind_MoveRight();
        keycode_PutFruit = PlayerPrefHelper.GetSetting_Keybind_PutFruit();
        UpdateDisplay_KeybindingList();
        //電腦版-解析度與全螢幕下拉選單
        UpdateDisplay_ResolutionAndFullScreenSetting();
    }

    #endregion
    #region  --> 根據目前刷新解析度與全螢幕下拉選單

    /// <summary>
    /// 根據目前刷新解析度與全螢幕下拉選單
    /// </summary>
    private void UpdateDisplay_ResolutionAndFullScreenSetting() {
        int indexWindowType = fullScreenMode_DropdownOptions.FindIndex((f) => f == Screen.fullScreenMode);
        if (indexWindowType < 0) {
            indexWindowType = 0;
        }
        Dropdown_WindowType.SetValueWithoutNotify(indexWindowType);
        Dropdown_WindowType.captionText.text = Dropdown_WindowType.options[indexWindowType].text;

        int indexResolution = resolution_DropdownOptions.FindIndex((r) => r.width == Screen.width && r.height == Screen.height);
        if (indexResolution < 0) {
            indexResolution = resolution_DropdownOptions.Count - 1;
        }
        Dropdown_Resolution.SetValueWithoutNotify(indexResolution);
        Dropdown_Resolution.captionText.text = $"{Screen.width} x {Screen.height}";
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
        if (Time.timeSinceLevelLoad - soundSliderDelayTime >= 0.05f) {
            soundSliderDelayTime = Time.timeSinceLevelLoad;
            Core.Instance.audioComponent.PlaySound(SoundId.Fruit_Put);
        }
        Text_SoundFXValue.text = valueInt.ToString();
    }

    #endregion
    #region  --> 按鍵設定

    /// <summary>
    /// 按下按鍵設定列表按紐
    /// </summary>
    private void OnClick_KeybindUIObject_OpenAnyKeyReceiverWindow(string nameOfKeyBind, Action<KeyCode> act_KeyCode) {
        Core.Instance.audioComponent.PlaySound(SoundId.Click_Normal);
        Text_KeyReceiveWindowTitle.text = string.Format(string_KeyReceiveWindowTitleTextFormat, nameOfKeyBind);
        SetEnableAnyKeyReceiverWindow(true);
        anykeyReceiver.onKeyDown.RemoveAllListeners();
        anykeyReceiver.onKeyDown.AddListener(OnKeyReceiverKeyDown);
        void OnKeyReceiverKeyDown(KeyCode key) {
            SetEnableAnyKeyReceiverWindow(false);
            if (key != KeyCode.Escape) {
                act_KeyCode?.Invoke(key);
                Core.Instance.audioComponent.PlaySound(SoundId.Click_Normal);
            } else {
                Core.Instance.audioComponent.PlaySound(SoundId.Click_Close);
            }
            UpdateDisplay_KeybindingList();
        }
    }

    /// <summary>
    /// 顯示/隱藏按鍵設定頁面
    /// </summary>
    /// <param name="isShow"></param>
    private void SetEnableAnyKeyReceiverWindow(bool isShow) {
        CanvasGroup_KeyReceiveWindow.SetEnable(isShow);
        anykeyReceiver.enabled = isShow;
        if (!isShow) {
            anykeyReceiver.onKeyDown.RemoveAllListeners();
        }
    }

    /// <summary>
    /// 更新按鍵設定列表的按鍵
    /// </summary>
    private void UpdateDisplay_KeybindingList() {
        var keyCodeNameComponent = Core.Instance.keyCodeNameComponent;
        KeybindUIObject_MoveLeft.SetKeybindValueText(keyCodeNameComponent.GetKeyCodeName(keycode_MoveLeft));
        KeybindUIObject_MoveRight.SetKeybindValueText(keyCodeNameComponent.GetKeyCodeName(keycode_MoveRight));
        KeybindUIObject_PutFruit.SetKeybindValueText(keyCodeNameComponent.GetKeyCodeName(keycode_PutFruit));
    }

    #endregion
    #region  --> 更改視窗模式

    /// <summary>
    /// 更改視窗模式
    /// </summary>
    /// <param name="value"></param>
    private void OnValueChanged_Dropdown_WindowType(int value) {
        FullScreenMode fullScreenMode = fullScreenMode_DropdownOptions[value];
        Screen.fullScreenMode = fullScreenMode;
    }

    #endregion
    #region  --> 更改解析度

    /// <summary>
    /// 更改解析度(會同時吃到全螢幕模式)
    /// </summary>
    /// <param name="value"></param>
    private void OnValueChanged_Dropdown_Resolution(int value) {
        //Log.Warning(resolution_DropdownOptions[value].ToString());
        Resolution resolution = resolution_DropdownOptions[value];
        FullScreenMode fullScreenMode = fullScreenMode_DropdownOptions[Dropdown_WindowType.value];
        if (resolution.width > 0) {
            Screen.SetResolution(resolution.width, resolution.height, fullScreenMode, 60);
        }
    }

    #endregion

    #endregion

    #endregion

}
