
using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LeaderboardDataComponent : MonoBehaviour {

    #region 筆記

    /// 目前存讀檔的邏輯如下
    /// 讀檔：
    ///  會在遊戲初始化時就先讀取，以陣列的形式讀取，再個別塞入Dictionary中
    /// 存檔：
    ///  會在任何一次出現新紀錄時儲存一次，將Dictionary轉換成陣列再轉成json儲存
    ///  目前不直接從Dictionary轉成json的原因是他會固定把key轉成字串，而不是當下的型別

    #endregion
    #region 參數區

    private const string LeaderboardDataFileName = "leaderboard.grape";

    private string FilePath { get => Path.Combine(Application.persistentDataPath, LeaderboardDataFileName); }

    //第一名的名次Index
    public const int FirstRank = 1;
    //最後一個可以上榜的名次
    public const int LastRank = 3;

    public struct LeaderboardData {
        public int Rank;
        public string Name;
        public int Score;
    }

    private readonly Dictionary<int, LeaderboardData> datasDict = new Dictionary<int, LeaderboardData>();

    #endregion
    #region Awake

    private void Awake() {
        Load();
    }

    #endregion
    #region 儲存檔案

    private void Save() {
        var filePath = FilePath;
        LeaderboardData[] leaderboardDatas = new LeaderboardData[datasDict.Count];
        int index = -1;
        foreach (var item in datasDict) {
            index++;
            leaderboardDatas[index] = item.Value;
        }
        try {
            var dataJson = JsonMapper.ToJson(leaderboardDatas);
            File.WriteAllText(filePath, dataJson);
            Debug.Log("儲存完畢");
        } catch (Exception e) {
            Debug.LogError(e);
        }
    }

    #endregion
    #region 讀檔

    private void Load() {
        var filePath = FilePath;
        try {
            LeaderboardData[] dataArray = null;
            int dataCount = 0;
            if (File.Exists(filePath)) {
                string data_raw = File.ReadAllText(filePath);
                dataArray = JsonUtility.FromJson<LeaderboardData[]>(data_raw);
            }
            if (dataArray != null) {
                dataCount = dataArray.Length;
                for (int i = 0; i < dataArray.Length; i++) {
                    datasDict[dataArray[i].Rank] = dataArray[i];
                }
            }
            Debug.Log($"LeaderboardData讀取完畢，總計讀取到{dataCount}筆資料");
        } catch (Exception e) {
            Debug.LogError(e);
        }
    }

    #endregion
    #region 外部方法-嘗試取得指定Rank的資料

    /// <summary>
    /// 嘗試取得指定Rank的資料
    /// </summary>
    /// <param name="rank"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public bool TryGetLeaderboardData(int rank, out LeaderboardData data) {
        bool output = datasDict.TryGetValue(rank, out data);
        if (output && data.Name == null) {
            //有找到資料，但是沒有任何名字資料(就算不填寫也應該要是string.Empty)
            output = false;
        }
        return output;
    }

    #endregion
    #region 外部方法-檢查是否刷新榜次

    /// <summary>
    /// 檢查是否刷新榜次，同時輸出會是第幾名
    /// </summary>
    /// <param name="score"></param>
    /// <param name="rank"></param>
    /// <returns></returns>
    public bool IsRefreshHighScore(int score, out int rank) {
        for (int i = FirstRank; i <= LastRank; i++) {
            if (TryGetLeaderboardData(i, out var data)) {
                if (score > data.Score) {
                    //有比榜上名次分數好的時候
                    rank = data.Rank;
                    return true;
                }
            } else {
                //該榜還從缺的時候
                rank = i;
                return true;
            }
        }
        //完全沒上榜
        rank = -1;
        return false;
    }

    #endregion
    #region 外部方法-記錄新分數

    /// <summary>
    /// 記錄新分數
    /// </summary>
    /// <param name="newScore"></param>
    /// <param name="name"></param>
    public void SaveNewRecord(int newScore, string name) {
        if (IsRefreshHighScore(newScore, out int newRank)) {
            //將newRank後面的名次往後推移
            for (int i = LastRank; i > newRank; i--) {
                int iPrev = i - 1;
                if (datasDict.TryGetValue(iPrev, out var prevData)) {
                    //如果前一名有的話就往後移並刷新名次
                    prevData.Rank = i;
                    datasDict[i] = prevData;
                }
            }
            //將該名次資料存入字典
            if (name == null) {
                //假如名字為null，先parse成空白
                name = string.Empty;
            }
            LeaderboardData newLeaderboardData = new LeaderboardData() {
                Rank = newRank,
                Name = name,
                Score = newScore
            };
            datasDict[newRank] = newLeaderboardData;
            //儲存檔案
            Save();
        }
    }

    #endregion

}
