
using System.Collections.Generic;
using UnityEngine;

public class FruitFactory : MonoBehaviour {

    #region 資源區

    /// <summary>
    /// 水果生成來源
    /// </summary>
    [SerializeField]
    private FruitObject fruitPrefab_Original;

    /// <summary>
    /// 水果物件池
    /// </summary>
    private Queue<FruitObject> objPool_Fruits;

    #endregion
    #region 初始化



    #endregion
    #region 外部-生成水果


    public void SpawnFruit() {

    }

    #endregion
    #region 外部-回收水果

    /// <summary>
    /// 回收水果
    /// </summary>
    public void DisposeFruit() {

    }

    #endregion

}
