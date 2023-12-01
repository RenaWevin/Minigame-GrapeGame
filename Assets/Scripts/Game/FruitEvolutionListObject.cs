
using UnityEngine;
using UnityEngine.UI;

public class FruitEvolutionListObject : MonoBehaviour {

    [SerializeField]
    public FruitType fruitType;

    [SerializeField]
    private Image image_Fruit;

    [SerializeField]
    private Text text_FruitName;

    [SerializeField]
    private float preferredTextWidth = 195;

    public void SetSprite(Sprite sprite) {
        if (image_Fruit != null) {
            image_Fruit.sprite = sprite;
        }
    }

    public void SetFruitName(string name) {
        if (text_FruitName != null) {
            text_FruitName.text = name;
            if (preferredTextWidth > 0) {
                float pWidth = text_FruitName.preferredWidth;
                if (pWidth > preferredTextWidth) {
                    float scale = pWidth / preferredTextWidth;
                    text_FruitName.rectTransform.localScale = new Vector3(1, scale, 1);
                } else {
                    text_FruitName.rectTransform.localScale = Vector3.one;
                }
            }
        }
    }

    public void SetImageScale(Vector3 localScale) {
        if (image_Fruit != null) {
            image_Fruit.rectTransform.localScale = localScale;
        }
    }

}
