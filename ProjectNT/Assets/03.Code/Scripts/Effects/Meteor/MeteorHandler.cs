using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeteorHandler : MonoBehaviour
{
    [SerializeField] private List<GameObject> meteorPrefabs;
    [SerializeField] private Transform startPos;
    [SerializeField] private Transform endPos;
    [SerializeField] private float duration;
    [SerializeField] private Vector2 scaleRange; //1 ~ 2 사이가 적당한 듯 함
    [SerializeField] private Vector2 countRange; //5 ~ 8개로 해볼 예정

    private List<Meteor> meteors = new List<Meteor>();

    private void Awake()
    {
        //최대 범위 값으로 생성
        for (int m = 0; m < meteorPrefabs.Count; m++)
        {
            string name = "meteor" + m;
            GameObject tmp = new GameObject(name);
            tmp.transform.SetParent(transform);

            //각각 그 개수만큼 생성
            for (int i = 0; i < countRange.y * 2; i++)
            {
                GameObject meteorObj = Instantiate(meteorPrefabs[m], tmp.transform);
                meteorObj.SetActive(false);
                Meteor meteor = meteorObj.GetComponent<Meteor>();
                meteors.Add(meteor);
            }
        }
    }

    private void Start()
    {
        ParticleScalingModeChange();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            PlayMeteor();
        }
    }

    //운석 소환 시 이거 쓰면 됨
    public void PlayMeteor()
    {
        int spawnCount = Random.Range((int)countRange.x , (int)countRange.y);
        int used = 0;
                float scale = Random.Range(scaleRange.x, scaleRange.y);

        foreach (var meteor in meteors)
        {
            //하이어라키 상에서 꺼져있고, 사용개수가 스폰되는 숫자보다 작아야함
            if (!meteor.gameObject.activeInHierarchy && used < spawnCount)
            {
                meteor.transform.localScale = Vector3.one * scale;
                ParticleSystem[] particleSystemList = meteor.GetComponentsInChildren<ParticleSystem>(true);
                for (int i = 0; i < particleSystemList.Length; i++)
                {
                    particleSystemList[i].transform.localScale = Vector3.one * scale;
                }

                meteor.gameObject.SetActive(true);

                float randomX = Random.Range(-30, 30);
                float randomY = Random.Range(-30f, 30f);

                Vector3 randStart = startPos.position + Vector3.up * randomY + Vector3.right * randomX;
                Vector3 randEnd = endPos.position + Vector3.up * randomY + Vector3.right * randomX;
                meteor.Move(randStart, randEnd, duration);

                used++;
            }

            if (used >= spawnCount) break;
        }
        //print($"PlayMeteor에 들어옴"); 
        //foreach (var meteor in meteors)
        //{
        //    float randomY = Random.Range(-30f, 30f);
        //    Vector3 randStart = startPos.position + Vector3.up * randomY;
        //    Vector3 randEnd = endPos.position + Vector3.up * randomY;
        //    meteor.Move(randStart, randEnd, duration);
        //}
    }

    //메테오 프리팹에 있는 모든 ParitcleScalingMode를 Heircy로 바꿔야함
    private void ParticleScalingModeChange()
    {
        for (int i = 0; i < meteorPrefabs.Count; i++)
        {
            // true를 하면 비활성화 되어있는 오브젝트도 찾아준다.
            ParticleSystem[] particleSystemList = meteorPrefabs[i].GetComponentsInChildren<ParticleSystem>(true);

            for (int p = 0; p < particleSystemList.Length; p++)
            {
                ParticleSystem.MainModule particle = particleSystemList[i].main;
                particle.scalingMode = ParticleSystemScalingMode.Hierarchy;
            }
        }
    }
}
