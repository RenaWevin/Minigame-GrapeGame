
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnykeyReceiver : MonoBehaviour {

    private readonly HashSet<KeyCode> supportKeys = new HashSet<KeyCode>();

    public readonly UnityEvent<KeyCode> onKeyDown = new UnityEvent<KeyCode>();

    void Awake() {
        supportKeys.Clear();
        //A~Z
        for (int i = (int)KeyCode.A; i <= (int)KeyCode.Z; i++) {
            supportKeys.Add((KeyCode)i);
        }
        //方向鍵
        for (int i = (int)KeyCode.UpArrow; i <= (int)KeyCode.LeftArrow; i++) {
            supportKeys.Add((KeyCode)i);
        }
        //上排數字鍵
        for (int i = (int)KeyCode.Alpha0; i <= (int)KeyCode.Alpha9; i++) {
            supportKeys.Add((KeyCode)i);
        }
        //九宮格數字鍵所有按鍵(=鍵為特例不包含在內)
        for (int i = (int)KeyCode.Keypad0; i <= (int)KeyCode.KeypadEnter; i++) {
            supportKeys.Add((KeyCode)i);
        }
        //空白鍵
        supportKeys.Add(KeyCode.Space);
        //Enter
        supportKeys.Add(KeyCode.Return);
        //Esc
        supportKeys.Add(KeyCode.Escape);
    }

    void Update() {
        if (Input.anyKeyDown) {
            foreach (KeyCode keyCode in supportKeys) {
                if (Input.GetKeyDown(keyCode)) {
                    onKeyDown.Invoke(keyCode);
                    break;
                }
            }
        }
    }
}
