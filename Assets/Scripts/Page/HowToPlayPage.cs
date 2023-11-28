
using UnityEngine;
using UnityEngine.UI;

public class HowToPlayPage : MonoBehaviour {

    #region UI物件

    [SerializeField]
    private CanvasGroupWindow canvasGroupWindow;

    [SerializeField]
    private Button Button_Close;

    #region  -> 控制方法

    [Space]
    [Header("控制方法")]

    [SerializeField]
    private Text Text_Controll_HowToPlay;

    #endregion
    #region  -> 水果圖片列表

    [Space]
    [Header("水果圖片列表")]

    [SerializeField] private Image Image_1;
    [SerializeField] private Image Image_2;
    [SerializeField] private Image Image_3;
    [SerializeField] private Image Image_4;
    [SerializeField] private Image Image_5;
    [SerializeField] private Image Image_6;
    [SerializeField] private Image Image_7;
    [SerializeField] private Image Image_8;

    [SerializeField] private Image Image_Heart;

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

    /// <summary>
    /// 顯示/關閉視窗
    /// </summary>
    /// <param name="show"></param>
    public void SetShowWindow(bool show) {
        canvasGroupWindow.SetShowWindow(show);
        if (show) {
            UpdateDisplayPage();
        }
    }

    /// <summary>
    /// 更新頁面
    /// </summary>
    private void UpdateDisplayPage() {
        //操作方法
        OptionsPage optionsPage = Core.Instance.optionsPage;
        string leftkey = Core.Instance.keyCodeNameComponent.GetKeyCodeName(optionsPage.keycode_MoveLeft);
        string rightkey = Core.Instance.keyCodeNameComponent.GetKeyCodeName(optionsPage.keycode_MoveRight);
        string putFruit = Core.Instance.keyCodeNameComponent.GetKeyCodeName(optionsPage.keycode_PutFruit);
        Text_Controll_HowToPlay.text = $"使用 {leftkey} 與 {rightkey} 左右移動\n按 {putFruit} 放下水果";
        //水果列表
        FruitFactory fruitFactory = Core.Instance.grapeGameCore.FruitFactory;
        FruitSpriteType fruitSpriteType = PlayerPrefHelper.GetSetting_FruitSpriteType();
        Image_1.sprite = fruitFactory.GetFruitSprite(fruitSpriteType, FruitType.Seed);
        Image_2.sprite = fruitFactory.GetFruitSprite(fruitSpriteType, FruitType.PieceOfGrape);
        Image_3.sprite = fruitFactory.GetFruitSprite(fruitSpriteType, FruitType.Grape);
        Image_4.sprite = fruitFactory.GetFruitSprite(fruitSpriteType, FruitType.Apple);
        Image_5.sprite = fruitFactory.GetFruitSprite(fruitSpriteType, FruitType.Pineapple);
        Image_6.sprite = fruitFactory.GetFruitSprite(fruitSpriteType, FruitType.Papaya);
        Image_7.sprite = fruitFactory.GetFruitSprite(fruitSpriteType, FruitType.Peach);
        Image_8.sprite = fruitFactory.GetFruitSprite(fruitSpriteType, FruitType.Watermelon);
        Image_Heart.sprite = fruitFactory.GetFruitSprite(fruitSpriteType, FruitType.Joker);
    }

    #endregion

}
