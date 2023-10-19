
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
        Debug.Log($"{this.gameObject.name} : {collision.gameObject.name}");
    }

}
