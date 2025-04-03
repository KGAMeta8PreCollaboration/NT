using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class PlayerLocalSaveData//세이브 데이터
{
    public int score;
    public string playerName;
    public int combo;
    public string gameMusicName;
    public string difficulty;

    public PlayerLocalSaveData(int score, string playerName, int combo, string gameMusicName, string difficulty)
    {
        this.score = score;
        this.playerName = playerName;
        this.combo = combo;
        this.gameMusicName = gameMusicName;
        this.difficulty = difficulty;
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
    public List<PlayerLocalSaveData> datas = new List<PlayerLocalSaveData>();//노래 이름의 랭킹 데이터

    private string filePath;//저장경로

    void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "playerData.json");
        //랭킹 데이터 저장 주소
    }

    //로컬에 저장된 데이터들을 로드
    public IEnumerator LocalDataLoad()
    {
        if (filePath == null)
        {
            filePath = Path.Combine(Application.persistentDataPath, "playerData.json");
        }
        string directoryPath = Path.GetDirectoryName(filePath);

        if (!Directory.Exists(directoryPath))//폴더가 없으면
        {
            Directory.CreateDirectory(directoryPath); //폴더 생성
            Debug.Log("폴더 없음 -> 생성");
        }

        if (File.Exists(filePath))//노래 이름의 파일이 존재하는지 확인
        {
            Debug.Log("폴더 있음");
            string json = "";

            yield return StartCoroutine(ReadFileAsync(filePath, result => json = result));//노래 이름의 저장 데이터를 string json에 넣기

            PlayerDataListWrapper dataWrapper = JsonUtility.FromJson<PlayerDataListWrapper>(json);
            if (dataWrapper != null && dataWrapper.playerDataList != null)
            {
                datas = dataWrapper.playerDataList;//데이터 리스트를 가져옴
                Debug.Log("폴더 있음 -> 데이터 리스트 가져옴");
            }
            else
            {
                datas = new List<PlayerLocalSaveData>();
                Debug.Log("폴더 없음 -> 데이터 리스트 새로 생성");
            }
        }
        else//저장된 랭킹 데이터 파일이 없으면 새로 생성
        {
            Debug.Log("폴더 없음 랭킹 데이터 파일 새로 생성");
            datas = new List<PlayerLocalSaveData>();//파일없으므로 빈리스트로 초기화
            PlayerDataListWrapper dataWrapper = new PlayerDataListWrapper(datas);//래핑
            string json = JsonUtility.ToJson(dataWrapper);//json으로 변환
            File.WriteAllText(filePath, json);//파일에 저장
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
    private void SortLocalData()
    {
        //점수로 정렬하고 점수가 같으면 콤보로 정렬 한번더
        datas.Sort((player1, player2) =>
        {
            int scoreComparison = player2.score.CompareTo(player1.score); //점수 비교
            if (scoreComparison == 0) //점수가 같으면 콤보로 비교
            {
                return player2.combo.CompareTo(player1.combo);
            }
            return scoreComparison;
        });

        if (datas.Count > 50) //50개만 남기기
        {
            datas.RemoveRange(50, datas.Count - 50);
        }
    }

    //로컬에 데이터를 저장
    public IEnumerator LocalDataSave(PlayerLocalSaveData newData)
    {
        yield return StartCoroutine(LocalDataLoad());//저장하기전에 dats가 null일 수 있으므로 한번불러오기

        datas.Add(newData);

        SortLocalData();//정렬 후 50개만

        PlayerDataListWrapper dataWrapper = new PlayerDataListWrapper(datas);
        string json = JsonUtility.ToJson(dataWrapper);

        File.WriteAllText(filePath, json);

        yield return null;
    }

    ////이 아래는 테스트용
    //public Button testButton;
    //public GameMusicSampleData gameMusicData;
    //private string tsetName = "player";
    //private bool istest = false;
    //private void Awake()
    //{
    //    testButton.onClick.AddListener(TestButton);
    //}
    //public void TestButton()
    //{
    //    if (istest == false)
    //    {
    //        istest = true;
    //        StartCoroutine(TestSave(() => { istest = false; }));
    //    }
    //    else
    //    {
    //        Debug.Log("테스트세이브 진행중 버튼 클릭");
    //    }
    //}
    //public IEnumerator TestSave(Action action)
    //{
    //    Debug.Log("테스트세이브 시작");
    //    foreach (TitleMusicData data in gameMusicData.titleMusicDatas)
    //    {
    //        int testRandomNum = UnityEngine.Random.Range(1, 5);
    //        for (int i = 0; i < testRandomNum; i++)
    //        {
    //            int testScore = UnityEngine.Random.Range(100, 100000);
    //            int testLevel = UnityEngine.Random.Range(20, 50);
    //            int testnum = UnityEngine.Random.Range(1, 10);
    //            string name = tsetName + testnum.ToString();
    //            yield return StartCoroutine(LocalDataSave(data.musicName, name, testLevel, testScore));
    //        }
    //    }
    //    Debug.Log("테스트세이브 종료");
    //    action.Invoke();
    //}
}
