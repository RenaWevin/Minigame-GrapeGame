
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

    #endregion

    private void Awake() {
        Instance = this;
    }

}
