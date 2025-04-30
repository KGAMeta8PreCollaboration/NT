using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class FireplayPrefab : MonoBehaviour
{
    private VisualEffect _vfx;

    private float _spawnRate;
    private Vector3 _startPos;
    private Vector3 _endPos;
    private Vector2 _scale;

    private Vector3[] randColors = { new Vector3(11, 111, 111), new Vector3(111, 11, 111), new Vector3(111, 111, 11) };
    //private Vector3[] randColors = { new Vector3(1f, 0.2f, 0.2f),    // 빨강
    //new Vector3(0.2f, 1f, 0.2f),    // 초록
    //new Vector3(0.2f, 0.2f, 1f),    // 파랑
    //new Vector3(1f, 1f, 0.2f),      // 노랑
    //new Vector3(1f, 0.2f, 1f),      // 보라
    //new Vector3(0.2f, 1f, 1f)
    //        };// 청록

    private void Awake()
    {
        _vfx = GetComponent<VisualEffect>();
    }

    public void SetDirection(Vector3 startPos, Vector3 endPos, float spawnRate = 10)
    {
        _spawnRate = spawnRate;
        _startPos = startPos;
        _endPos = endPos;
        _vfx.SetFloat("NoOfSpawnParticles", _spawnRate);
        _vfx.SetVector3("StartPos", _startPos);
        _vfx.SetVector3("EndPos", _endPos);
        //_vfx.SetVector2("Scale", _scale);
    }

    //켜진다는 것은 터진다는 것...
    public void Play()
    {
        int rand = Random.Range(0, randColors.Length);
        Vector3 randColor = randColors[rand];
        _vfx.SetVector3("Color", randColor);
        _vfx.Play();
        StartCoroutine(HandleExplosion());
    }

    private IEnumerator HandleExplosion()
    {
        yield return new WaitForSeconds(2f);
        _vfx.Stop();
    }
}
