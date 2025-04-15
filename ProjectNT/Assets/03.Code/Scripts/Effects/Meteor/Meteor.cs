using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    private Vector3 _startPos;
    private Vector3 _endPos;
    private float _duration;

    private float _elaspedTime;
    private bool _isMoving = false;
    private ParticleSystem _ps;

    private void Awake()
    {
        _ps = GetComponent<ParticleSystem>();   
    }

    public void Move(Vector3 startPos, Vector3 endPos, float duration)
    {
        _startPos = startPos;
        _endPos = endPos;
        _duration = duration;
        StartCoroutine(HandleMove());
    }

    private IEnumerator HandleMove()
    {
        _isMoving = true;
        _ps.Play();
        _elaspedTime = 0;

        while (_elaspedTime < _duration)
        {
            _elaspedTime += Time.deltaTime;
            float t = Mathf.Clamp01(_elaspedTime / _duration);
            Vector3 pos = Vector3.Lerp(_startPos, _endPos, t);
            transform.position = pos;

            yield return null;
        }

        _isMoving = false;
        _ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        transform.position = _startPos;
    }
}
