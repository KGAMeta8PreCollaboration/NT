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

    private void Awake()
    {
        _vfx = GetComponent<VisualEffect>();
        print($"불꽃 프리팹 생성됨 이름은 : {gameObject.name}");
    }

    public void SetDirection(Vector3 startPos, Vector3 endPos, float spawnRate = 10)
    {
        _spawnRate = spawnRate;
        _startPos = startPos;   
        _endPos = endPos;
        _vfx.SetFloat("NoOfSpawnParticles", _spawnRate);
        _vfx.SetVector3("StartPos", _startPos);
        _vfx.SetVector3("EndPos", _endPos);
    }

    //켜진다는 것은 터진다는 것...
    public void Play()
    {
        _vfx.Play();
        print($"{gameObject.name}의 프리팹 실행 됨");
        StartCoroutine(HandleExplosion());
    }

    private IEnumerator HandleExplosion()
    {
        yield return new WaitForSeconds(2f);
        _vfx.Stop();
    }
}
