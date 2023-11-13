
using UnityEngine;

public class FruitObject : MonoBehaviour {

    #region 參數參考區

    /// <summary>
    /// 本體的水果種類
    /// </summary>
    [SerializeField]
    private FruitType m_fruitType;

    /// <summary>
    /// 本體的水果種類
    /// </summary>
    public FruitType FruitType { get { return m_fruitType; } }

    /// <summary>
    /// 已經接觸過其他水果了
    /// </summary>
    public bool isTouchedAnotherFruit { get; private set; } = false;

    #endregion
    #region Inspector資源區

    [SerializeField]
    private SpriteRenderer mySpriteRenderer;

    [SerializeField]
    private Collider2D myCollider2D;

    [SerializeField]
    private Rigidbody2D myRigidbody2D;

    #endregion
    #region 外部方法-清除此水果接觸資訊

    /// <summary>
    /// 清除此水果接觸資訊，用於生成水果時恢復成出廠狀態
    /// </summary>
    public void ClearTouchedInfo() {
        isTouchedAnotherFruit = false;
    }

    #endregion
    #region 外部方法-檢查自己與另一個水果是否可以結合 

    /// <summary>
    /// 檢查自己與另一個水果是否可以結合
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool CanCombineWith(FruitObject other) {
        //規則：兩個水果種類相同，或者兩者其一為Joker
        bool sameType = this.FruitType == other.FruitType;
        bool containsJoker = (this.FruitType == FruitType.Joker || other.FruitType == FruitType.Joker);
        return sameType || containsJoker;
    }

    #endregion
    #region 外部方法-啟用/關閉此水果的物理系統

    /// <summary>
    /// 啟用/關閉此水果的物理系統
    /// </summary>
    /// <param name="value"></param>
    public void SetEnablePhysics(bool value) {
        myCollider2D.enabled = value;
        myRigidbody2D.simulated = value;
    }

    #endregion
    #region 外部方法-設定Sprite

    /// <summary>
    /// 設定Sprite
    /// </summary>
    /// <param name="sprite"></param>
    public void SetSprite(Sprite sprite) {
        mySpriteRenderer.sprite = sprite;
    }

    #endregion
    #region 外部方法-設定顏色

    /// <summary>
    /// 設定顏色
    /// </summary>
    /// <param name="color"></param>
    public void SetSpriteColor(Color color) {
        mySpriteRenderer.color = color;
    }

    #endregion
    #region 擴展方法-取得兩個水果中屬於正常水果範圍的ID

    /// <summary>
    /// 取得兩個水果中屬於正常水果範圍的ID
    /// </summary>
    /// <returns></returns>
    public static FruitType GetNormalTypeFromObjects(FruitObject obj1, FruitObject obj2) {
        //這裡會屏除Joker與Disappear
        bool IsNormalType(FruitObject obj) {
            return (obj.FruitType != FruitType.Joker) && (obj.FruitType != FruitType.Disappear);
        }
        if (IsNormalType(obj1)) {
            return obj1.FruitType;
        }
        if (IsNormalType(obj2)) {
            return obj2.FruitType;
        }
        if (obj1.FruitType == FruitType.Joker && obj2.FruitType == FruitType.Joker) {
            //兩者都是Joker，合成後直接消失
            return FruitType.Disappear;
        }
        Log.Error($"有兩個水果的類型都不是NormalType: {obj1.name} 與 {obj2.name}");
        return FruitType.Grape;
    }

    #endregion
    #region 碰撞箱碰撞事件

    private void OnCollisionEnter2D(Collision2D collision) {
        //檢查水果種類
        FruitObject otherComponent = collision.gameObject.GetComponent<FruitObject>();
        if (otherComponent != null) {
            isTouchedAnotherFruit = true;
            if (this.CanCombineWith(otherComponent)) {
                //發現與對方可以結合，向遊戲核心申請結合
                Core.Instance.grapeGameCore.ApplyForFruitCombine(this, otherComponent);
            }
        }
    }

    #endregion

}
