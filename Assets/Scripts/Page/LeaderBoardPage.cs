
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoardPage : MonoBehaviour {

    #region UI物件

    [SerializeField]
    private CanvasGroupWindow canvasGroupWindow;

    [SerializeField]
    private Button Button_Close;

    #region  -> 排行榜物件

    [SerializeField] private Text[] Text_Scores;
    [SerializeField] private Text[] Text_Names;

    #endregion

    #endregion
    #region Awake

    private void Awake() {
        Button_Close.onClick.AddListener(delegate {
            Core.Instance.audioComponent.PlaySound(SoundId.Click_Close);
            SetShowWindow(false);
        });
    }

    #endregion
    #region UI事件

    #region  -> 顯示/關閉視窗

    /// <summary>
    /// 顯示/關閉視窗
    /// </summary>
    /// <param name="show"></param>
    public void SetShowWindow(bool show) {
        canvasGroupWindow.SetShowWindow(show);
        if (show) {
            UpdateDisplay_Leaderboard();
        }
    }

    #endregion
    #region  -> 刷新顯示分數

    /// <summary>
    /// 刷新顯示分數
    /// </summary>
    private void UpdateDisplay_Leaderboard() {
        LeaderboardDataComponent leaderboardDataComponent = Core.Instance.leaderboardDataComponent;
        for (int i = LeaderboardDataComponent.FirstRank; i <= LeaderboardDataComponent.LastRank; i++) {
            Text textName = Text_Names[i - 1];
            Text textScore = Text_Scores[i - 1];
            if (leaderboardDataComponent.TryGetLeaderboardData(i, out var data)) {
                textName.fontStyle = FontStyle.Normal;
                textName.text = data.Name;
                textScore.text = data.Score.ToString();
            } else {
                textName.fontStyle = FontStyle.Italic;
                textName.text = "<無紀錄>";
                textScore.text = "------";
            }
        }
    }

    #endregion

    #endregion

}
