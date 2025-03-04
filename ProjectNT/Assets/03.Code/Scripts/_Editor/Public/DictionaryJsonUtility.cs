using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Data_Serializable_Class

[Serializable]
public class DataDictionary<TKey, TValue>
{
    public TKey Key;
    public TValue Value;
}

[Serializable]
public class JsonDataArray<TKey, TValue>
{
    public List<DataDictionary<TKey, TValue>> data;
}

#endregion

public static class DictionaryJsonUtility
{
    #region Dictionary_Data_Save
    public static string ToJson<TKey, TValue>(Dictionary<TKey, TValue> jsonDicData, bool pretty = false)
    {
        List<DataDictionary<TKey, TValue>> dataList = new List<DataDictionary<TKey, TValue>>();
        DataDictionary<TKey, TValue> dictionaryData;
        foreach (TKey key in jsonDicData.Keys)
        {
            dictionaryData = new DataDictionary<TKey, TValue>();
            dictionaryData.Key = key;
            dictionaryData.Value = jsonDicData[key];
            dataList.Add(dictionaryData);
        }
        JsonDataArray<TKey, TValue> arrayJson = new JsonDataArray<TKey, TValue>();
        arrayJson.data = dataList;
        return JsonUtility.ToJson(arrayJson, pretty);
    }
    #endregion

    #region Dictionary_Data_Load
    public static Dictionary<TKey, TValue> FromJson<TKey, TValue>(string jsonData)
    {

        JsonDataArray<TKey, TValue> dataList = JsonUtility.FromJson<JsonDataArray<TKey, TValue>>(jsonData);

        Dictionary<TKey, TValue> returnDictionary = new Dictionary<TKey, TValue>();

        for (int i = 0; i < dataList.data.Count; i++)
        {

            DataDictionary<TKey, TValue> dictionaryData = dataList.data[i];
            if (dictionaryData.Key != null)
                returnDictionary[dictionaryData.Key] = dictionaryData.Value;
        }
        return returnDictionary;

    }
    #endregion
}
