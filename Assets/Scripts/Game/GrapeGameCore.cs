
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
    
    #endregion
    #region Unity內建方法

    void Start() {
        spawnPointOriginal = spawnPoint.position;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.S)) {
            int typeId = Random.Range((int)FruitType.Grape, (int)FruitType.HealingLuka_Uhhuh);
            var newFruit = fruitFactory.SpawnFruit((FruitType)typeId);
            newFruit.transform.SetParent(parent: trans_FruitContainer, worldPositionStays: true);
            newFruit.transform.position = spawnPoint.position;
            newFruit.gameObject.name += System.DateTime.Now.Second.ToString();
            fruitsInScene.Add(newFruit);
        }
        if (Input.GetKeyDown(KeyCode.D)) {
            //刪除水果
            if (fruitsInScene.Count > 0) {
                var toDispose = fruitsInScene[0];
                fruitsInScene.Remove(toDispose);
                fruitFactory.DisposeFruit(toDispose);
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

}
