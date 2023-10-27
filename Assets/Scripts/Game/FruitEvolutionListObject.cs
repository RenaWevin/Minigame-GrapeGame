
using UnityEngine;
using UnityEngine.UI;

public class FruitEvolutionListObject : MonoBehaviour {

    [SerializeField]
    public FruitType fruitType;

    [SerializeField]
    private Image image_Fruit;

    [SerializeField]
    private Text text_FruitName;

    public void SetSprite(Sprite sprite) {
        if (image_Fruit != null) {
            image_Fruit.sprite = sprite;
        }
    }

    public void SetFruitName(string name) {
        if (text_FruitName != null) {
            text_FruitName.text = name;
        }
    }

    public void SetImageScale(Vector3 localScale) {
        if (image_Fruit != null) {
            image_Fruit.rectTransform.localScale = localScale;
        }
    }

}
