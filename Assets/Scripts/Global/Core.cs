
using UnityEngine;

public class Core : MonoBehaviour {

    #region Instance

    public static Core Instance { get; private set; }

    #endregion
    #region Components

    [SerializeField]
    public TitlePage titlePage;

    [SerializeField]
    public GrapeGameCore grapeGameCore;

    [SerializeField]
    public HowToPlayPage howToPlayPage;

    [SerializeField]
    public LeaderBoardPage leaderboardPage;

    [SerializeField]
    public OptionsPage optionsPage;

    #endregion

    private void Awake() {
        Instance = this;
    }

}
