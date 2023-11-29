
using UnityEngine;
using UnityEngine.UI;

public class HowToPlayPage : MonoBehaviour {

    #region UI物件

    [SerializeField]
    private CanvasGroupWindow canvasGroupWindow;

    [SerializeField]
    private Button Button_Close;

    [SerializeField]
    private ScrollRect ScrollView_HowToPlay;

    #region  -> 控制方法

    [Space]
    [Header("控制方法")]

    [SerializeField]
    private Text Text_Controll_HowToPlay;

    #endregion
    #region  -> 水果圖片列表

    [Space]
    [Header("水果圖片列表")]

    //圖片
    [SerializeField] private Image Image_1;
    [SerializeField] private Image Image_2;
    [SerializeField] private Image Image_3;
    [SerializeField] private Image Image_4;
    [SerializeField] private Image Image_5;
    [SerializeField] private Image Image_6;
    [SerializeField] private Image Image_7;
    [SerializeField] private Image Image_8;

    [SerializeField] private Image Image_Heart;

    [Header("水果圖片檢視按鈕")]

    //檢視按鈕
    [SerializeField] private Button ZoomInButton_3;
    [SerializeField] private Button ZoomInButton_4;
    [SerializeField] private Button ZoomInButton_5;
    [SerializeField] private Button ZoomInButton_6;
    [SerializeField] private Button ZoomInButton_7;
    [SerializeField] private Button ZoomInButton_8;

    [SerializeField] private Button ZoomInButton_Heart;

    #endregion

    #endregion
    #region Awake

    private void Awake() {
        //關閉按鈕
        Button_Close.onClick.AddListener(delegate {
            Core.Instance.audioComponent.PlaySound(SoundId.Click_Close);
            SetShowWindow(false);
        });
        //檢視圖片按鈕
        ZoomInButton_3.onClick.AddListener(delegate { OnClick_ZoomInButton(Image_3, new Vector2(512, 512)); });
        ZoomInButton_4.onClick.AddListener(delegate { OnClick_ZoomInButton(Image_4, new Vector2(512, 512)); });
        ZoomInButton_5.onClick.AddListener(delegate { OnClick_ZoomInButton(Image_5, new Vector2(1024, 1024)); });
        ZoomInButton_6.onClick.AddListener(delegate { OnClick_ZoomInButton(Image_6, new Vector2(512, 512)); });
        ZoomInButton_7.onClick.AddListener(delegate { OnClick_ZoomInButton(Image_7, new Vector2(512, 512)); });
        ZoomInButton_8.onClick.AddListener(delegate { OnClick_ZoomInButton(Image_8, new Vector2(512, 512)); });
        ZoomInButton_Heart.onClick.AddListener(delegate { OnClick_ZoomInButton(Image_Heart, new Vector2(512, 512)); });
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
            ScrollView_HowToPlay.verticalNormalizedPosition = 1f;
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
        //水果按鈕
        bool isButtonEnable = fruitSpriteType != FruitSpriteType.Normal;
        ZoomInButton_3.interactable = isButtonEnable;
        ZoomInButton_4.interactable = isButtonEnable;
        ZoomInButton_5.interactable = isButtonEnable;
        ZoomInButton_6.interactable = isButtonEnable;
        ZoomInButton_7.interactable = isButtonEnable;
        ZoomInButton_8.interactable = isButtonEnable;
        ZoomInButton_Heart.interactable = isButtonEnable;
    }

    /// <summary>
    /// 按下檢視圖片按鈕
    /// </summary>
    /// <param name="image"></param>
    /// <param name="size"></param>
    private void OnClick_ZoomInButton(Image image, Vector2 size) {
        Core.Instance.audioComponent.PlaySound(SoundId.Click_Normal);
        ImageViewerPage imageViewerPage = Core.Instance.imageViewerPage;
        imageViewerPage.SetSpriteAndSize(image, size);
        imageViewerPage.SetShowWindow(true);
    }

    #endregion

}
