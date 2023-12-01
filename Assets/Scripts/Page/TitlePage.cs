
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
        Button_HowToPlay.onClick.AddListener(OnClick_HowToPlay);
        Button_Leaderboard.onClick.AddListener(OnClick_Leaderboard);
        Button_Options.onClick.AddListener(OnClick_Options);
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
    public async void OnClick_Start() {
        Core.Instance.audioComponent.PlaySound(SoundId.Click_StartGame);
        if (TransitionAnimationController.Instance != null) {
            await TransitionAnimationController.Instance.Play_Show();
        }
        SetEnableTitlePage(false);
        var gameCore = Core.Instance.grapeGameCore;
        gameCore.SetEnableGamePage(true);
        gameCore.ResetGame();
        gameCore.PrepareStart();
        if (TransitionAnimationController.Instance != null) {
            await TransitionAnimationController.Instance.Play_Hide();
        }
        gameCore.StartGame();
    }

    /// <summary>
    /// 按下遊玩方法
    /// </summary>
    private void OnClick_HowToPlay() {
        Core.Instance.audioComponent.PlaySound(SoundId.Click_Normal);
        Core.Instance.howToPlayPage.SetShowWindow(true);
    }

    /// <summary>
    /// 按下高分紀錄
    /// </summary>
    private void OnClick_Leaderboard() {
        Core.Instance.audioComponent.PlaySound(SoundId.Click_Normal);
        Core.Instance.leaderboardPage.SetShowWindow(true);
    }

    /// <summary>
    /// 按下設定畫面
    /// </summary>
    private void OnClick_Options() {
        Core.Instance.audioComponent.PlaySound(SoundId.Click_Normal);
        Core.Instance.optionsPage.SetShowWindow(true);
    }

    /// <summary>
    /// 按下離開遊戲
    /// </summary>
    public void OnClick_Exit() {
        Core.Instance.audioComponent.PlaySound(SoundId.Click_Normal);
        Application.Quit();
    }

#endregion

}
