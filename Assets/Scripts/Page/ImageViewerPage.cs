
using UnityEngine;
using UnityEngine.UI;

public class ImageViewerPage : MonoBehaviour {

    #region UI物件參考區

    [SerializeField]
    private CanvasGroupWindow canvasGroupWindow;

    [SerializeField]
    private Button Button_Close;

    [SerializeField]
    private ScrollRect ScrollView_ImageViewer;

    [SerializeField]
    private RectTransform RectTransform_Content;

    [SerializeField]
    private Image Image_Content;

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
    }

    /// <summary>
    /// 從Image物件設定Sprite
    /// </summary>
    /// <param name="image"></param>
    public void SetSpriteAndSize(Image image, Vector2 size) {
        Image_Content.sprite = image.sprite;
        RectTransform_Content.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
        RectTransform_Content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
    }

    #endregion

}
