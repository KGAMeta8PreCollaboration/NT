using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorHandler : MonoBehaviour
{
    [SerializeField] private List<GameObject> meteorPrefabs;
    [SerializeField] private Transform startPos;
    [SerializeField] private Transform endPos;
    [SerializeField] private float duration;

    private List<Meteor> meteors = new List<Meteor>();

    private void Awake()
    {
        for (int i = 0; i < meteorPrefabs.Count; i++)
        {
            GameObject meteorObj = Instantiate(meteorPrefabs[i], transform);
            Meteor meteor = meteorObj.GetComponent<Meteor>();
            meteors.Add(meteor);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            foreach (var meteor in meteors)
            {
                float randomY = Random.Range(-10f, 10f);
                Vector3 randStart = startPos.position + Vector3.up * randomY;
                Vector3 randEnd = endPos.position + Vector3.up * randomY;
                meteor.Move(randStart, randEnd, duration);
            }
        }
    }

    //운석 소환 시 이거 쓰면 됨
    public void PlayMeteor()
    {
        print($"PlayMeteor에 들어옴"); 
        foreach (var meteor in meteors)
        {
            float randomY = Random.Range(-30f, 30f);
            Vector3 randStart = startPos.position + Vector3.up * randomY;
            Vector3 randEnd = endPos.position + Vector3.up * randomY;
            meteor.Move(randStart, randEnd, duration);
        }
    }
}
