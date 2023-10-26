
using UnityEngine;
using UnityEngine.UI;

public class TitlePage : MonoBehaviour {

    #region UI物件區

    [SerializeField]
    private CanvasGroup canvasGroup_Page;

    [SerializeField]
    private Text Text_Version;

    [Header("按鈕")]
    [SerializeField]
    private Button Button_Start;
    [SerializeField]
    private Button Button_HowToPlay = null;
    [SerializeField]
    private Button Button_Leaderboard = null;
    [SerializeField]
    private Button Button_Options = null;
    [SerializeField]
    private Button Button_Exit = null;

    #endregion
    #region Awake

    private void Awake() {
        Text_Version.text = $"v{Application.version}";

        Button_Start.onClick.AddListener(OnClick_Start);
        Button_Exit.onClick.AddListener(OnClick_Exit);
    }

    #endregion
    #region 畫面開關

    /// <summary>
    /// 畫面開關
    /// </summary>
    public void SetEnableTitlePage(bool value) {
        canvasGroup_Page.SetEnable(value);
    }

    #endregion
    #region UI按鈕事件

    /// <summary>
    /// 按下開始遊戲
    /// </summary>
    public void OnClick_Start() {
        SetEnableTitlePage(false);
        var gameCore = Core.Instance.grapeGameCore;
        gameCore.SetEnableGamePage(true);
        gameCore.ResetGame();
        gameCore.StartGame();
    }

    /// <summary>
    /// 按下離開遊戲
    /// </summary>
    public void OnClick_Exit() {
        Application.Quit();
    }

#endregion

}
