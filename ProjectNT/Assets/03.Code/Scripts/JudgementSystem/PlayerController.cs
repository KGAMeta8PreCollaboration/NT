using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using Photon;
using Photon.Pun;

public delegate bool HapticDelegate(float amplitude, float duration);
public class PlayerController : MonoBehaviour
{
    public float velocityMagnitude;
    public float velocityMagnitudeThreshold;
    public float hitThreshold = 0.1f; // 판정을 위한 거리 허용 오차

    [SerializeField] private ParticleSystem triggerEffect;
    private ActionBasedController _controller;
    private XRRayInteractor rayInter;
    private Vector3 prevPos = new Vector3();

    public GameObject tmpPointPrefab;

    private WooferNetworkSync _wooferNetworkSync;
    //test
    public TextMeshProUGUI logText;
    public TextMeshProUGUI logText2;
    private ScoreUI _scoreUI;
    private HapticDelegate hapticDelegate;

    private Coroutine _collisionStayCoroutine = null;

    private void Awake()
    {
        _controller = GetComponentInParent<ActionBasedController>();
        rayInter = _controller.GetComponentInChildren<XRRayInteractor>();

    }
    private void OnEnable()
    {
        _controller.activateAction.action.performed += ParclePlay;
        _controller.activateAction.action.performed += OnTopNoteHit;
        hapticDelegate += _controller.SendHapticImpulse;
    }
    private void OnDisable()
    {
        _controller.activateAction.action.performed -= ParclePlay;
        _controller.activateAction.action.performed -= OnTopNoteHit;
        hapticDelegate -= _controller.SendHapticImpulse;
    }
    private void ParclePlay(InputAction.CallbackContext cnt)
    {
        triggerEffect.Play(true);
    }
    private void Start()
    {
        //=============test//=============
        logText = GameObject.Find("LogText")?.GetComponent<TextMeshProUGUI>();
        //logText2 = GameObject.Find("LogText2")?.GetComponent<TextMeshProUGUI>();
        _scoreUI = FindObjectOfType<ScoreUI>();
        //=============test=============

        prevPos = transform.position;

        if (GameManager.Instance.IsMulti) _wooferNetworkSync = FindObjectOfType<WooferNetworkSync>();
    }

    private void Update()
    {
        Vector3 deltaPos = transform.position - prevPos;

        velocityMagnitude = deltaPos.magnitude / Time.deltaTime;

    }

    private void LateUpdate()
    {
        prevPos = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<Woofer>(out Woofer woofer))
        {
            Collider other = collision.collider;

            Vector3 stickPosition = transform.position;

            //Vector3 closestPoint = other.ClosestPointOnBounds(stickPosition); //추후 고민해보자.
            Vector3 closestPoint = other.ClosestPoint(stickPosition);

            Vector3 wooferCenter = other.bounds.center;

            float wooferTopY = wooferCenter.y + (other.bounds.extents.y - hitThreshold);

            bool isDownwardHit = transform.position.y < prevPos.y; // 아래로 휘둘렀는지 확인
            bool isFastEnough = velocityMagnitude > velocityMagnitudeThreshold; // 일정 속도 이상 휘둘렀는지 확인
            bool isOnTop = closestPoint.y >= wooferTopY; // 윗면에서 충돌했는지 확인

            //print($"휘두른 속도: {velocityMagnitude}");
            //print($"아래로 휘둘렀는지: {isDownwardHit}, 속도는 충분했는지: {isFastEnough}, 윗면에 충돌했는지: {isOnTop}");

            if (isFastEnough && isDownwardHit && isOnTop)
            {
                if (!GameManager.Instance.IsMulti) woofer.Hit();
                else _wooferNetworkSync.SendHit(woofer, PhotonNetwork.LocalPlayer.NickName);
                if (_collisionStayCoroutine == null) _collisionStayCoroutine = StartCoroutine(OnCollisionStayCoroutine(woofer));
                Instantiate(tmpPointPrefab, closestPoint, Quaternion.identity);
                _scoreUI.tempHitCount++;
                //logText.text = "Hit Count: " + _scoreUI.tempHitCount + "\n 우퍼 번호: " + woofer.name;
                _controller.SendHapticImpulse(0.6f, 0.15f);
                print("우퍼와 상호작용 됨");
            }
        }
    }

    private IEnumerator OnCollisionStayCoroutine(Woofer woofer)
    {
        while (true)
        {
            logText.text = "우퍼에 닿는 중";
            if (!GameManager.Instance.IsMulti) woofer.Hold(hapticDelegate);
            else _wooferNetworkSync.SendHold(woofer, PhotonNetwork.LocalPlayer.NickName);
            yield return null;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.TryGetComponent<Woofer>(out Woofer woofer))
        {
            logText.text = "우퍼에서 뗏음";
            if (!GameManager.Instance.IsMulti) woofer.ReleaseLongNote();
            else _wooferNetworkSync.SendRelease(woofer, PhotonNetwork.LocalPlayer.NickName);
            if (_collisionStayCoroutine != null)
            {
                StopCoroutine(_collisionStayCoroutine);
                _collisionStayCoroutine = null;
            }
        }
    }

    private void OnTopNoteHit(InputAction.CallbackContext cnt)
    {
        if (rayInter.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            if (hit.collider.CompareTag("TopNote"))
            {
                TopNoteProjectile proj =
                PoolManager.Instance.topNoteProjPool.Pop();
                proj.transform.SetParent(transform, true);
                proj.gameObject.transform.position = transform.position;
                proj.Init(transform.position, hit.transform.position);
                _controller.SendHapticImpulse(0.8f, 0.15f);
            }
        }
    }
}
