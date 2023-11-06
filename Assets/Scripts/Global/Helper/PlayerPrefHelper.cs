
using System;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerPrefHelper {

    private const string PrefKeyFormat_Setting = "Setting_{0}";

    #region 平台通用設定

    #region  -> 音樂音量★
    #endregion
    #region  -> 音效音量★
    #endregion
    #region  -> 水果圖片種類

    /// <summary>
    /// 取得設定-水果圖片種類
    /// </summary>
    /// <returns></returns>
    public static FruitSpriteType GetSetting_FruitSpriteType() {
        int output = PlayerPrefs.GetInt(
            string.Format(PrefKeyFormat_Setting, nameof(FruitSpriteType)),
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
            string.Format(PrefKeyFormat_Setting, nameof(FruitSpriteType)),
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
