
using System.Collections.Generic;
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

    #endregion

    private void OnCollisionEnter2D(Collision2D collision) {
        //檢查水果種類
        FruitObject otherComponent = collision.gameObject.GetComponent<FruitObject>();
        if (otherComponent != null) {
            if (this.FruitType == otherComponent.FruitType) {
                //同種水果，向遊戲核心申請結合
                Log.Warning($"{this.gameObject.name}與{collision.gameObject.name}可以結合");
                Core.Instance.grapeGameCore.ApplyForFruitCombine(this, otherComponent);
            }
        }
        //測試
        Log.Info($"{this.gameObject.name} : {collision.gameObject.name}");
    }

}
