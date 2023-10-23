
using System.Collections.Generic;
using UnityEngine;

public class GrapeGameCore : MonoBehaviour {

    #region Component

    /// <summary>
    /// 水果工廠
    /// </summary>
    [SerializeField]
    private FruitFactory fruitFactory;

    #endregion
    #region 容器-水果配對

    private struct PendingFruitsPair {
        public FruitObject fruit1;
        public FruitObject fruit2;
    }

    #endregion
    #region 遊戲物件參照區

    /// <summary>
    /// 移動游標左側邊界
    /// </summary>
    [SerializeField]
    private Transform Trans_LeftBound;

    /// <summary>
    /// 移動游標右側邊界
    /// </summary>
    [SerializeField]
    private Transform Trans_RightBound;

    /// <summary>
    /// 水果重生點
    /// </summary>
    [SerializeField]
    private Transform spawnPoint;

    /// <summary>
    /// 水果物件放置容器
    /// </summary>
    [SerializeField]
    private Transform trans_FruitContainer;

    #endregion
    #region 參數參考區

    private Vector3 spawnPointOriginal;

    private float moveSpeed = 3f;

    //目前在場上已生成的水果
    private readonly List<FruitObject> fruitsInScene = new List<FruitObject>();

    //已申請合成的水果列表
    private readonly List<PendingFruitsPair> pendingCombines = new List<PendingFruitsPair>();
    //每次檢查時，已經被合成過的水果的HashSet
    private readonly HashSet<FruitObject> appliedCombinedFruits = new HashSet<FruitObject>();

    #endregion
    #region Unity內建方法

    #region  -> Awake

    void Awake() {
        //初始化重生點位置
        spawnPointOriginal = spawnPoint.position;
    }

    #endregion
    #region  -> Update

    void Update() {
        if (Input.GetKeyDown(KeyCode.S)) {
            int typeId = Random.Range((int)FruitType.Grape, (int)FruitType.HealingLuka_Uhhuh);
            //var newFruit = fruitFactory.SpawnFruit((FruitType)typeId);
            //newFruit.transform.SetParent(parent: trans_FruitContainer, worldPositionStays: true);
            //newFruit.transform.position = spawnPoint.position;
            //newFruit.gameObject.name += System.DateTime.Now.Second.ToString();
            //fruitsInScene.Add(newFruit);
            SpawnFruit(
                type: (FruitType)typeId,
                parent: trans_FruitContainer,
                worldPosition: spawnPoint.position
            );
        }
        if (Input.GetKeyDown(KeyCode.D)) {
            //刪除水果
            if (fruitsInScene.Count > 0) {
                var toDispose = fruitsInScene[0];
                DisposeFruit(toDispose);
            }
        }

        if (Input.GetKey(KeyCode.LeftArrow)) {
            float newX = spawnPoint.position.x;
            newX -= (Time.deltaTime * moveSpeed);
            newX = Mathf.Max(newX, Trans_LeftBound.position.x);
            spawnPoint.position = new Vector3(newX, spawnPointOriginal.y, spawnPointOriginal.z);
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
            float newX = spawnPoint.position.x;
            newX += (Time.deltaTime * moveSpeed);
            newX = Mathf.Min(newX, Trans_RightBound.position.x);
            spawnPoint.position = new Vector3(newX, spawnPointOriginal.y, spawnPointOriginal.z);
        }
    }

    #endregion
    #region  -> FixedUpdate

    private void FixedUpdate() {
        ExecuteAllCombineApplies();
    }

    #endregion

    #endregion
    #region 水果結合相關

    #region  -> 檢查並實際處理所有結合要求

    /// <summary>
    /// 檢查並實際處理所有結合要求
    /// </summary>
    private void ExecuteAllCombineApplies() {
        //先清空已經被合成過的水果
        appliedCombinedFruits.Clear();
        //檢查是否有任何水果組合需要處理
        if (pendingCombines.Count > 0) {
            for (int i = 0; i < pendingCombines.Count; i++) {
                var pair = pendingCombines[i];
                //檢查這兩個水果是否有任一個已經被處理過了
                bool isApplied = (appliedCombinedFruits.Contains(pair.fruit1) || appliedCombinedFruits.Contains(pair.fruit2));
                if (isApplied) {
                    //已經有被處理過了，跳過此要求
                    continue;
                }
                //處理這兩個水果
                //先檢查是否真的可以結合
                if (pair.fruit1.CanCombineWith(pair.fruit2)) {
                    //先將這兩者加入已經被合成的列表上
                    appliedCombinedFruits.Add(pair.fruit1);
                    appliedCombinedFruits.Add(pair.fruit2);
                    //檢查下一階水果
                    int nextPhaseFruitId = (int)FruitObject.GetNormalTypeFromObjects(pair.fruit1, pair.fruit2);
                    nextPhaseFruitId += 1;
                    FruitType nextPhaseFruitType = (FruitType)nextPhaseFruitId;
                    if (nextPhaseFruitType != FruitType.Disappear) {
                        //水果並不是單純消失
                        //在這兩個水果的位置中間生成一個下一階的水果
                        Vector3 nextFruitPos = (pair.fruit1.transform.position + pair.fruit2.transform.position) / 2;
                        SpawnFruit(
                            type: nextPhaseFruitType,
                            parent: trans_FruitContainer,
                            worldPosition: nextFruitPos
                        );
                    }
                    //移除這兩個水果
                    DisposeFruit(pair.fruit1);
                    DisposeFruit(pair.fruit2);
                    //執行加分★
                    //(未實作)
                    Log.Info($"合成成功！分數增加 {nextPhaseFruitId}");
                } else {
                    Log.Error($"以下兩個水果不能融合卻被加入到了合成請求中：\n{pair.fruit1.name} 與 {pair.fruit2.name}");
                    continue;
                }
            }
        }
        //最後清除所有需求
        pendingCombines.Clear();
    }

    #endregion
    #region  -> 外部申請結合

    /// <summary>
    /// 申請結合
    /// </summary>
    public void ApplyForFruitCombine(FruitObject self, FruitObject target) {
        pendingCombines.Add(new PendingFruitsPair() {
            fruit1 = self,
            fruit2 = target
        });
    }

    #endregion

    #endregion
    #region 水果生成刪除相關

    /// <summary>
    /// 從水果工廠生成指定種類水果
    /// </summary>
    /// <param name="type"></param>
    /// <param name="parent"></param>
    /// <param name="worldPosition"></param>
    /// <returns></returns>
    private FruitObject SpawnFruit(FruitType type, Transform parent, Vector3 worldPosition) {
        var newFruit = fruitFactory.SpawnFruit(type);
        newFruit.transform.SetParent(parent: parent, worldPositionStays: true);
        newFruit.transform.position = worldPosition;
        newFruit.gameObject.name += System.DateTime.Now.Second.ToString();
        fruitsInScene.Add(newFruit);

        return newFruit;
    }

    /// <summary>
    /// 刪除指定水果
    /// </summary>
    /// <param name="toDispose"></param>
    private void DisposeFruit(FruitObject toDispose) {
        if (fruitsInScene.Contains(toDispose)) {
            fruitsInScene.Remove(toDispose);
            fruitFactory.DisposeFruit(toDispose);
        } else {
            Log.Error($"水果{toDispose.name}已經不在場上了！");
        }
    }

    #endregion

}
