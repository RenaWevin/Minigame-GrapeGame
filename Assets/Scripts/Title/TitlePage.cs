
using UnityEngine;
using UnityEngine.UI;

public class TitlePage : MonoBehaviour {

    #region UI物件區

    [SerializeField]
    private Text Text_Version;

    #endregion
    #region Awake

    private void Awake() {
        Text_Version.text = $"v{Application.version}";
    }

    #endregion

}
