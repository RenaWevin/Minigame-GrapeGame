
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
    private readonly Queue<FruitObject> objPool_Fruits = new Queue<FruitObject>();

    /// <summary>
    /// 物件池本體Transform
    /// </summary>
    [SerializeField]
    private Transform trans_ObjectPool;

    /// <summary>
    /// 生成水果時的位置
    /// </summary>
    private Transform trans_Spawn { get { return this.transform; } }

    #endregion
    #region 初始化



    #endregion
    #region 外部-生成水果

    /// <summary>
    /// 生成水果
    /// </summary>
    /// <returns></returns>
    public FruitObject SpawnFruit() {
        FruitObject output;
        if (objPool_Fruits.Count > 0) {
            //有水果在物件池裡
            output = objPool_Fruits.Dequeue();
        } else {
            //沒有水果在物件池裡
            output = Instantiate<FruitObject>(original: fruitPrefab_Original, parent: trans_Spawn);
        }
        output.gameObject.SetActive(true);
        return output;
    }

    #endregion
    #region 外部-回收水果

    /// <summary>
    /// 回收水果
    /// </summary>
    /// <param name="toDispose"></param>
    public void DisposeFruit(FruitObject toDispose) {
        toDispose.gameObject.SetActive(false);
        toDispose.transform.SetParent(parent: trans_ObjectPool, worldPositionStays: true);
    }

    #endregion

}
