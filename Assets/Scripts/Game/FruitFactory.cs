
using System;
using System.Collections.Generic;
using UnityEngine;

public class FruitFactory : MonoBehaviour {

    #region Inspector資源區

    /// <summary>
    /// 各種水果生成來源參照(僅供Inspector編輯)
    /// </summary>
    [SerializeField]
    private List<FruitObject> fruitPrefab_OriginalList = new List<FruitObject>();

    /// <summary>
    /// 物件池本體Transform
    /// </summary>
    [SerializeField]
    private Transform trans_ObjectPool;

    #endregion
    #region 程式碼控制資源區

    /// <summary>
    /// 各種水果生成來源字典(生成時請參照此)
    /// </summary>
    private readonly Dictionary<FruitType, FruitObject> fruitPrefab_OriginalDict = new Dictionary<FruitType, FruitObject>();

    /// <summary>
    /// 水果物件池
    /// </summary>
    private readonly Dictionary<FruitType, Queue<FruitObject>> objPool_Fruits = new Dictionary<FruitType, Queue<FruitObject>>();

    /// <summary>
    /// 生成水果時的位置
    /// </summary>
    private Transform trans_Spawn { get { return this.transform; } }

    #endregion
    #region 初始化

    private void Awake() {
        //處理生成水果來源參照
        fruitPrefab_OriginalDict.Clear();
        for (int i = 0; i < fruitPrefab_OriginalList.Count; i++) {
            var item = fruitPrefab_OriginalList[i];
            if (fruitPrefab_OriginalDict.ContainsKey(item.FruitType)) {
                Log.Error($"有重複FruitType({item.FruitType})的水果被加入到參照 (FruitPrefab_OriginalList)\n水果的Type設定是否有設定錯誤？");
                continue;
            }
            fruitPrefab_OriginalDict.Add(item.FruitType, item);
        }
        //水果物件池初始化
        objPool_Fruits.Clear();
        foreach (FruitType type in Enum.GetValues(typeof(FruitType))) {
            objPool_Fruits.Add(type, new Queue<FruitObject>());
        }
    }

    #endregion
    #region 外部-生成水果

    /// <summary>
    /// 生成水果
    /// </summary>
    /// <returns></returns>
    public FruitObject SpawnFruit(FruitType type) {
        if (type == FruitType.Disappear) {
            Log.Warning("指定生成的水果種類為FruitType.Disappear，將不生成任何水果");
            return null;
        }
        FruitObject output;
        if (objPool_Fruits.ContainsKey(type)) {
            if (objPool_Fruits[type].Count > 0) {
                //有水果在物件池裡
                output = objPool_Fruits[type].Dequeue();
            } else {
                //沒有水果在物件池裡
                if (fruitPrefab_OriginalDict.TryGetValue(type, out FruitObject newObj)) {
                    output = Instantiate<FruitObject>(original: newObj, parent: trans_Spawn);
                } else {
                    Log.Error($"指定的水果種類{type}({(int)type})在生成字典中不存在！");
                    return null;
                }
            }
        } else {
            Log.Error($"指定的水果種類{type}({(int)type})無效！");
            return null;
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
        FruitType type = toDispose.FruitType;
        if (objPool_Fruits.ContainsKey(type)) {
            if (objPool_Fruits[type].Contains(toDispose)) {
                //已經回收好這個水果了，跳過
                return;
            }
            toDispose.gameObject.SetActive(false);
            toDispose.transform.SetParent(parent: trans_ObjectPool, worldPositionStays: true);
            objPool_Fruits[type].Enqueue(toDispose);
        } else {
            Log.Error("水果物件池尚未初始化完成就有水果嘗試進入物件池！");
        }
    }

    #endregion

}
