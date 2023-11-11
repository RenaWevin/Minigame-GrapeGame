
using System;
using System.Collections.Generic;
using UnityEngine;

public class KeyCodeNameComponent : MonoBehaviour {

    #region Inspector-KeyCode與按鍵名對應

    #region  -> KeyCodeStringPair

    [Serializable]
    public struct KeyCodeStringPair {
        public KeyCode keyCode;
        public string name;
    }

    #endregion

    [SerializeField]
    private List<KeyCodeStringPair> keycodeNamesList = new List<KeyCodeStringPair>();

    #endregion
    #region 參數相關

    private readonly Dictionary<KeyCode, string> keycodeNameDict = new Dictionary<KeyCode, string>();

    #endregion
    #region Awake

    private void Awake() {
        keycodeNameDict.Clear();
        for (int i = 0; i < keycodeNamesList.Count; i++) {
            keycodeNameDict.Add(keycodeNamesList[i].keyCode, keycodeNamesList[i].name);
        }
    }

    #endregion
    #region 外部方法-取得按鍵名

    /// <summary>
    /// 外部方法-取得按鍵名
    /// </summary>
    /// <param name="keyCode"></param>
    /// <returns></returns>
    public string GetKeyCodeName(KeyCode keyCode) {
        if (keycodeNameDict.TryGetValue(key: keyCode, out string name)) {
            return name;
        }
        //若字典中沒收錄就直接取enum名稱
        return keyCode.ToString();
    }

    #endregion

}
