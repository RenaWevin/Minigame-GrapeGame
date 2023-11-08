
using UnityEngine;

public static class PlayerPrefHelper {

    #region SettingKeys

    private const string PrefKeyFormat_Setting = "Setting_{0}";

    private const string KeyPart_Enable_BGM = "Enable_BGM";
    private const string KeyPart_Enable_SoundFX = "Enable_SoundFX";
    private const string KeyPart_Volume_BGM = "Volume_BGM";
    private const string KeyPart_Volume_SoundFX = "Volume_SoundFX";
    private const string KeyPart_FruitSpriteType = nameof(FruitSpriteType);

    #endregion
    #region 平台通用設定

    #region  -> 音樂啟用

    /// <summary>
    /// 取得設定-BGM啟用(0/1)
    /// </summary>
    /// <returns></returns>
    public static bool GetSetting_Enable_BGM() {
        int outputInt = PlayerPrefs.GetInt(string.Format(PrefKeyFormat_Setting, KeyPart_Enable_BGM), 1);
        bool output = outputInt == 1;
        return output;
    }

    /// <summary>
    /// 儲存設定-BGM啟用(0/1)
    /// </summary>
    /// <returns></returns>
    public static void SetSetting_Enable_BGM(bool isEnable) {
        int value = isEnable ? 1 : 0;
        PlayerPrefs.SetInt(string.Format(PrefKeyFormat_Setting, KeyPart_Enable_BGM), value);
        PlayerPrefs.Save();
    }

    #endregion
    #region  -> 音樂音量

    /// <summary>
    /// 取得設定-BGM音量(0~100)
    /// </summary>
    /// <returns></returns>
    public static int GetSetting_Volume_BGM() {
        int output = PlayerPrefs.GetInt(string.Format(PrefKeyFormat_Setting, KeyPart_Volume_BGM), 50);
        return output;
    }

    /// <summary>
    /// 儲存設定-BGM音量(0~100)
    /// </summary>
    /// <returns></returns>
    public static void SetSetting_Volume_BGM(int volume) {
        volume.FixValueInRange(0, 100);
        PlayerPrefs.SetInt(string.Format(PrefKeyFormat_Setting, KeyPart_Volume_BGM), volume);
        PlayerPrefs.Save();
    }

    #endregion
    #region  -> 音效啟用

    /// <summary>
    /// 取得設定-音效啟用(0/1)
    /// </summary>
    /// <returns></returns>
    public static bool GetSetting_Enable_SoundFX() {
        int outputInt = PlayerPrefs.GetInt(string.Format(PrefKeyFormat_Setting, KeyPart_Enable_SoundFX), 1);
        bool output = outputInt == 1;
        return output;
    }

    /// <summary>
    /// 儲存設定-音效啟用(0/1)
    /// </summary>
    /// <returns></returns>
    public static void SetSetting_Enable_SoundFX(bool isEnable) {
        int value = isEnable ? 1 : 0;
        PlayerPrefs.SetInt(string.Format(PrefKeyFormat_Setting, KeyPart_Enable_SoundFX), value);
        PlayerPrefs.Save();
    }

    #endregion
    #region  -> 音效音量

    /// <summary>
    /// 取得設定-音效音量(0~100)
    /// </summary>
    /// <returns></returns>
    public static int GetSetting_Volume_SoundFX() {
        int output = PlayerPrefs.GetInt(string.Format(PrefKeyFormat_Setting, KeyPart_Volume_SoundFX), 50);
        return output;
    }

    /// <summary>
    /// 儲存設定-音效音量(0~100)
    /// </summary>
    /// <returns></returns>
    public static void SetSetting_Volume_SoundFX(int volume) {
        volume.FixValueInRange(0, 100);
        PlayerPrefs.SetInt(string.Format(PrefKeyFormat_Setting, KeyPart_Volume_SoundFX), volume);
        PlayerPrefs.Save();
    }

    #endregion
    #region  -> 水果圖片種類

    /// <summary>
    /// 取得設定-水果圖片種類
    /// </summary>
    /// <returns></returns>
    public static FruitSpriteType GetSetting_FruitSpriteType() {
        int output = PlayerPrefs.GetInt(
            string.Format(PrefKeyFormat_Setting, KeyPart_FruitSpriteType),
            (int)FruitSpriteType.TofuSkin
        );
        return (FruitSpriteType)output;
    }

    /// <summary>
    /// 儲存設定-水果圖片種類
    /// </summary>
    /// <param name="fruitSpriteType"></param>
    public static void SetSetting_FruitSpriteType(FruitSpriteType fruitSpriteType) {
        PlayerPrefs.SetInt(
            string.Format(PrefKeyFormat_Setting, KeyPart_FruitSpriteType),
            (int)fruitSpriteType
        );
        PlayerPrefs.Save();
    }

    #endregion

    #endregion
    #region 電腦版專用設定

    #region  -> 按鍵設定★
    #endregion
    #region  -> 解析度★
    #endregion
    #region  -> 全螢幕模式★
    #endregion

    #endregion

}
