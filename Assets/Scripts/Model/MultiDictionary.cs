
using System.Collections.Generic;

public class MultiDictionary<TKey1, TKey2, TValue> : Dictionary<TKey1, Dictionary<TKey2, TValue>> {
    
    /// <summary>
    /// 嘗試新增值到指定的雙重key中，當指定的key已經存在時會加入失敗
    /// </summary>
    /// <param name="key1"></param>
    /// <param name="key2"></param>
    /// <param name="value"></param>
    /// <returns>是否有增加成功</returns>
    public bool TryAdd(TKey1 key1, TKey2 key2, TValue value) {
        Dictionary<TKey2, TValue> subDict;
        if (!this.TryGetValue(key1, out subDict)) {
            subDict = new Dictionary<TKey2, TValue>();
            this.Add(key1, subDict);
        }
        if (subDict.ContainsKey(key2)) {
            return false;
        } else {
            subDict.Add(key2, value);
            return true;
        }
    }

    /// <summary>
    /// 嘗試取得值
    /// </summary>
    /// <param name="key1"></param>
    /// <param name="key2"></param>
    /// <param name="value"></param>
    /// <returns>是否有取得值</returns>
    public bool TryGetValue(TKey1 key1, TKey2 key2, out TValue value) {
        if (this.TryGetValue(key1, out Dictionary<TKey2, TValue> subDict)) {
            return subDict.TryGetValue(key2, out value);
        } else {
            value = default(TValue);
            return false;
        }
    }

    /// <summary>
    /// 檢查此雙重key是否存在
    /// </summary>
    /// <param name="key1"></param>
    /// <param name="key2"></param>
    /// <returns>雙重key是否存在</returns>
    public bool ContainsKey(TKey1 key1, TKey2 key2) {
        if (this.TryGetValue(key1, out Dictionary<TKey2, TValue> subDict)) {
            return subDict.ContainsKey(key2);
        } else {
            return false;
        }
    }

    /// <summary>
    /// 移除雙重key底下的值
    /// </summary>
    /// <param name="key1"></param>
    /// <param name="key2"></param>
    public void Remove(TKey1 key1, TKey2 key2) {
        if (this.TryGetValue(key1, out Dictionary<TKey2, TValue> subDict)) {
            subDict.Remove(key2);
        }
    }

}
