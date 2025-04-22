using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class LazerHandler : MonoBehaviour
{
    //싱글
    private GameObject _S_2;
    private GameObject _S_3;

    //멀티 왼쪽
    private GameObject _M_L_2;
    private GameObject _M_L_3;
    
    //멀티 오른쪽
    private GameObject _M_R_2;
    private GameObject _M_R_3;

    private void Awake()
    {
        _S_2 = GameObject.Find("Particle L_S");
        _S_3 = GameObject.Find("Particle L_S_3");

        _M_L_2 = GameObject.Find("Particle L_L_M");
        _M_L_3 = GameObject.Find("Particle L_L_3");

        _M_R_2 = GameObject.Find("Particle L_R_M");
        _M_R_3 = GameObject.Find("Particle L_R_3");

        _S_2.SetActive(false);
        _S_3.SetActive(false);

        _M_L_2.SetActive(false);
        _M_L_3.SetActive(false);

        _M_R_2.SetActive(false);
        _M_R_3.SetActive(false);
    }

    ParticleSystem[] _S_P_2;
    ParticleSystem[] _S_P_3;

    ParticleSystem[] _M_L_P_2;
    ParticleSystem[] _M_L_P_3;

    ParticleSystem[] _M_R_P_2;
    ParticleSystem[] _M_R_P_3;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        _S_P_2 = _S_2.GetComponentsInChildren<ParticleSystem>(true);
        _S_P_3 = _S_3.GetComponentsInChildren<ParticleSystem>(true);

        _M_L_P_2 = _M_L_2.GetComponentsInChildren<ParticleSystem>(true);
        _M_L_P_3 = _M_L_3.GetComponentsInChildren<ParticleSystem>(true);

        _M_R_P_2 = _M_R_2.GetComponentsInChildren<ParticleSystem>(true);
        _M_R_P_3 = _M_R_3.GetComponentsInChildren<ParticleSystem>(true);

        switch (GameManager.Instance.PlayMode)
        {
            case Enums.PlayMode.Single:
                _S_2.SetActive(true);
                _S_3.SetActive(true);
                break;
            case Enums.PlayMode.Player1:
                _M_L_2.SetActive(true);
                _M_L_3.SetActive(true);
                break;
            case Enums.PlayMode.Player2:
                _M_R_2.SetActive(true);
                _M_R_3.SetActive(true);
                break;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            for(int i = 0; i < _S_P_2.Length; i++)
            {
                _S_P_2[i].Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            for (int i = 0; i < _S_P_3.Length; i++)
            {
                _S_P_3[i].Play();
            }
        }
    }
}
