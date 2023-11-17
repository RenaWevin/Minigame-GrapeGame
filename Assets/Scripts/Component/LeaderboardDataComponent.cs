
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LeaderboardDataComponent : MonoBehaviour {

    #region 參數區

    private const string LeaderboardDataFileName = "leaderboard.grape";

    private struct LeaderboardData {
        public int Rank;
        public string Name;
        public int Score;
    }

    #endregion
    #region Awake

    private void Awake() {
        Load();
    }

    #endregion
    #region 儲存檔案

    private void Save() {
        //以下為範例code

        //int hp = int.Parse(InputField_HP_int.text);
        //float atkSuccess = float.Parse(InputField_AtkSuccess_float.text);
        //var data_raw = new PlayerData() {
        //    Name = InputField_Name.text,
        //    HP = hp,
        //    AtkSuccess = atkSuccess,
        //    Numbers = new int[] { 1, 2, 3 }
        //};
        //PlayerData[] playerDatas = new PlayerData[3];
        //playerDatas[0] = (data_raw);
        //playerDatas[1] = (data_raw);
        //playerDatas[2] = (data_raw);
        //var dataJson = JsonUtility.ToJson(playerDatas);
        //Debug.Log(dataJson);

        //var filePath = Path.Combine(Application.persistentDataPath, playerDataFileName);
        //try {
        //    File.WriteAllText(filePath, dataJson);
        //    Debug.Log("儲存完畢");
        //} catch (Exception e) {
        //    Debug.LogError(e);
        //}
    }

    #endregion
    #region 讀檔

    private void Load() {
        var filePath = Path.Combine(Application.persistentDataPath, LeaderboardDataFileName);
        try {
            if (File.Exists(filePath)) {
                string data_raw = File.ReadAllText(filePath);
                LeaderboardData data = JsonUtility.FromJson<LeaderboardData>(data_raw);
                Debug.Log("LeaderboardData讀取完畢");
            } else {
                //檔案不存在

            }
        } catch (Exception e) {
            Debug.LogError(e);
        }
    }

    #endregion

    ///// <summary>
    ///// 記錄新分數
    ///// </summary>
    //public static void SaveNewRecord(int newScore) {
    //
    //}

}
