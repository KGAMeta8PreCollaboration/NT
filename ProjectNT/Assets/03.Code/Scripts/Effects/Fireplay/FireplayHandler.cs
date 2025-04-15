using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireplayHandler : MonoBehaviour
{
    [SerializeField] private List<GameObject> fireplayPrefabs;
    [SerializeField] private float spawnRate = 1f;
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 endPos;
    
    private List<FireplayPrefab> fireplayInstances = new List<FireplayPrefab>();
    private FireplayPrefab currentPrefab;

    private void Awake()
    {
        for (int i = 0; i < fireplayPrefabs.Count; i++)
        {
            GameObject prefab = Instantiate(fireplayPrefabs[i], transform.position, Quaternion.identity, transform);
            //생성 시 비활성화
            prefab.SetActive(false);
            FireplayPrefab vfxInstance = prefab.GetComponent<FireplayPrefab>();
            vfxInstance.SetDirection(startPos, endPos, spawnRate);
            fireplayInstances.Add(vfxInstance);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int randomNum = Random.Range(0, fireplayPrefabs.Count);

            currentPrefab = fireplayInstances[randomNum];
            currentPrefab.gameObject.SetActive(true);
            currentPrefab.Play();
        }
    }
}
