using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class PlayerDataListWrapper//JsonUtility는 리스트를 직접 직렬화 할 수 없기때문에 필요
{
    public List<PlayerLocalSaveData> playerDataList;

    public PlayerDataListWrapper(List<PlayerLocalSaveData> playerDataList)
    {
        this.playerDataList = playerDataList;
    }
}

public class LocalSaveManager : MonoBehaviour
{
    public List<PlayerLocalSaveData> datas = new List<PlayerLocalSaveData>();

    private string filePath;//저장경로

    void Start()
    {
        filePath = Application.persistentDataPath + "playerData.json";
    }

    //로컬에 저장된 데이터들을 로드
    public IEnumerator LocalDataLoad()
    {
        if (File.Exists(filePath))//파일이 존재하는지 확인
        {
            //파일을 비동기적으로 읽기
            string json = "";
            yield return StartCoroutine(ReadFileAsync(filePath, result => json = result));
            //filePath(불러온파일)을 읽고 그 결과(ReadFileAsync함수에서 받아온 result)를
            //json(빈객체)에 콜백으로 넣음

            //json 파싱하여 데이터 저장
            PlayerDataListWrapper dataWrapper = JsonUtility.FromJson<PlayerDataListWrapper>(json);
            datas = dataWrapper.playerDataList;  //데이터를 리스트에 저장
        }
        else
        {
            //파일이 없음
            datas.Clear();
        }
    }

    //파일을 비동기적으로 읽어오는 함수
    private IEnumerator ReadFileAsync(string path, Action<string> onComplete)
    {
        //파일을 읽는 작업을 코루틴으로 처리
        string fileContent = "";
        using (StreamReader reader = new StreamReader(path))
        {
            fileContent = reader.ReadToEnd();
        }

        //파일 읽기 완료 후 콜백으로 전달
        onComplete(fileContent);
        yield return null;
    }

    //로컬에 저장된 데이터들 점수순으로 정렬 후 50개까지만 유지
    public void SortLocalData()
    {
        datas.Sort((player1, player2) => player2.score.CompareTo(player1.score));//점수순으로 정렬
        if (datas.Count > 50)//50개 넘어가면
        {
            datas.RemoveRange(50, datas.Count - 50);//51번째부터는 삭제
        }
    }

    //로컬에 데이터를 저장
    public IEnumerator LocalDataSave(string playerName, int level, float score)
    {
        yield return StartCoroutine(LocalDataLoad());//저장하기전에 dats가 null일 수 있으므로 한번불러오기

        string imageName = "DefaultImage";//일단 모두 같은 이미지로 -> 뭘로할지모름
        PlayerLocalSaveData newData = new PlayerLocalSaveData(imageName, level, playerName, score);
        datas.Add(newData);

        string json = JsonUtility.ToJson(new PlayerDataListWrapper(datas));//json으로 변환하여 저장
        File.WriteAllText(filePath, json);//파일에 json 저장

        SortLocalData();//정렬 후 50개만

        json = JsonUtility.ToJson(new PlayerDataListWrapper(datas));
        File.WriteAllText(filePath, json);

        yield return null;
    }

    string tsetName = "player";
    int testLevel = 1;
    float tsetScore = 100;
    public void TestSave()
    {
        testLevel++;
        string name = tsetName + testLevel.ToString();
        tsetScore += 100f;
        StartCoroutine(LocalDataSave(name, testLevel, tsetScore));
    }
}
