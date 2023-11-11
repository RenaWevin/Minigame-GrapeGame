
using UnityEngine;
using UnityEngine.UI;

public class KeybindUIObject : MonoBehaviour {

    #region UI物件

    [SerializeField]
    private Text Text_KeyBindingLabel;

    [SerializeField]
    private Button Button_Keybind;

    [SerializeField]
    private Text Text_KeyBindValue;

    #endregion
    #region 參數

    public Button.ButtonClickedEvent onClick => Button_Keybind.onClick;

    #endregion
    #region 外部方法

    /// <summary>
    /// 取得按鍵設定功能名字
    /// </summary>
    /// <returns></returns>
    public string GetNameOfKeyBind() {
        return Text_KeyBindingLabel.text;
    }

    /// <summary>
    /// 設定按鍵設定值名字
    /// </summary>
    /// <param name="textValue"></param>
    public void SetKeybindValueText(string textValue) {
        Text_KeyBindValue.text = textValue;
    }

    #endregion

}
