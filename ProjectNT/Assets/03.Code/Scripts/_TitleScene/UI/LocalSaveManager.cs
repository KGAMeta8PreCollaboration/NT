using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Purchasing.MiniJSON;
using UnityEngine.UI;

[Serializable]
public class PlayerLocalSaveData
{
    public int lavel;
    public string playerName;
    public float score;
    public string playerImageName;

    public PlayerLocalSaveData(string imageName, int lavel, string playerName, float score)
    {
        this.lavel = lavel;
        this.playerName = playerName;
        this.score = score;

        this.playerImageName = imageName;
    }
}

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
        filePath = Path.Combine(Application.persistentDataPath, "playerData");
    }

    //로컬에 저장된 데이터들을 로드
    public IEnumerator LocalDataLoad(string musicName)
    {

        string musicFilePath = Path.Combine(filePath, musicName + ".json");
        string directoryPath = Path.GetDirectoryName(musicFilePath);

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath); // 디렉토리 생성
        }

        if (File.Exists(musicFilePath))//파일이 존재하는지 확인
        {
            string json = "";

            yield return StartCoroutine(ReadFileAsync(musicFilePath, result => json = result));

            PlayerDataListWrapper dataWrapper = JsonUtility.FromJson<PlayerDataListWrapper>(json);
            if (dataWrapper != null && dataWrapper.playerDataList != null)
            {
                datas = dataWrapper.playerDataList; // 데이터 리스트를 가져옴
            }
            else
            {
                datas = new List<PlayerLocalSaveData>();
            }
        }
        else
        {
            datas = new List<PlayerLocalSaveData>();//파일없으므로 빈리스트로 초기화
            PlayerDataListWrapper dataWrapper = new PlayerDataListWrapper(datas);//래핑
            string json = JsonUtility.ToJson(dataWrapper);//json으로 변환
            File.WriteAllText(musicFilePath, json);//파일에 저장
        }
    }

    //파일을 비동기적으로 읽어오는 함수
    private IEnumerator ReadFileAsync(string path, Action<string> onComplete)
    {
        //파일을 읽는 작업을 코루틴으로 처리
        string fileContent = "";
        using (StreamReader reader = new StreamReader(path))
        {
            fileContent = reader.ReadToEnd();//파일 내용 읽기
        }

        //파일 읽기 완료 후 콜백으로 전달
        onComplete(fileContent);
        yield return null;
    }

    //로컬에 저장된 데이터들 점수순으로 정렬 후 50개까지만 유지
    public void SortLocalData(string musicName)
    {
        datas.Sort((player1, player2) => player2.score.CompareTo(player1.score));
        if (datas.Count > 50)
        {
            datas.RemoveRange(50, datas.Count - 50);
        }
    }

    //로컬에 데이터를 저장
    public IEnumerator LocalDataSave(string musicName, string playerName, int level, float score)
    {
        yield return StartCoroutine(LocalDataLoad(musicName));//저장하기전에 dats가 null일 수 있으므로 한번불러오기

        string imageName = "DefaultImage";//일단 모두 같은 이미지로 -> 뭘로할지모름
        PlayerLocalSaveData newData = new PlayerLocalSaveData(imageName, level, playerName, score);

        datas.Add(newData);

        string musicFilePath = Path.Combine(filePath, musicName + ".json");//노래 이름의 파일

        SortLocalData(musicName);//정렬 후 50개만

        PlayerDataListWrapper dataWrapper = new PlayerDataListWrapper(datas);
        string json = JsonUtility.ToJson(dataWrapper);

        File.WriteAllText(musicFilePath, json);

        yield return null;
    }

    //이 아래는 테스트용
    public Button testButton;
    public GameMusicSampleData gameMusicData;
    private string tsetName = "player";
    private bool istest = false;
    private void Awake()
    {
        testButton.onClick.AddListener(TestButton);
    }
    public void TestButton()
    {
        if (istest == false)
        {
            istest = true;
            StartCoroutine(TestSave(() => { istest = false; }));
        }
        else
        {
            Debug.Log("테스트세이브 진행중 버튼 클릭");
        }
    }
    public IEnumerator TestSave(Action action)
    {
        Debug.Log("테스트세이브 시작");
        foreach (TitleMusicData data in gameMusicData.titleMusicDatas)
        {
            int testRandomNum = UnityEngine.Random.Range(1, 5);
            for (int i = 0; i < testRandomNum; i++)
            {
                int testScore = UnityEngine.Random.Range(100, 100000);
                int testLevel = UnityEngine.Random.Range(20, 50);
                int testnum = UnityEngine.Random.Range(1, 10);
                string name = tsetName + testnum.ToString();
                yield return StartCoroutine(LocalDataSave(data.musicName, name, testLevel, testScore));
            }
        }
        Debug.Log("테스트세이브 종료");
        action.Invoke();
    }
}
