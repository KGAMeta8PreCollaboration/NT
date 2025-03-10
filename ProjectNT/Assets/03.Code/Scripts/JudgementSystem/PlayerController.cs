using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerController : MonoBehaviour
{
    //=======PC 용=========
    [SerializeField] private Woofer[] _woofers;

    private NoteManager _noteManager;
    //=======PC 용=========
    public float velocityMagnitude;
    public float hitThreshold = 0.1f; // 판정을 위한 거리 허용 오차
    private ActionBasedController _controller;
    private Vector3 prevPos = new Vector3();

    public GameObject tmpPointPrefab;

    private void Start()
    {
        _noteManager = FindObjectOfType<NoteManager>();
        _controller = GetComponentInParent<ActionBasedController>();

        _controller.activateAction.action.performed += TriggerButtonAction;

        prevPos = transform.position;

        StartCoroutine(CreateCoroutine());
    }

    private void Update()
    {
        Vector3 deltaPos = transform.position - prevPos;

        velocityMagnitude = deltaPos.magnitude / Time.deltaTime;

        prevPos = transform.position;
    }

    private void TriggerButtonAction(InputAction.CallbackContext context)
    {
        //상단 노트 상호작용, 일시정지, 확인 버튼 등등
    }

    public void Create(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _noteManager.CreateNote(int.Parse(context.control.name) - 1);
        }
    }

    public void Hit(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            InputControl control = context.control;
            int index = control.name.Equals("a") ? 0 :
                control.name.Equals("s") ? 1 :
                control.name.Equals("d") ? 2 :
                control.name.Equals("f") ? 3 : -1;

            if (index == -1) return;

            _woofers[index].Hit();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Woofer>(out Woofer woofer))
        {
            Vector3 stickPosition = transform.position;

            //Vector3 closestPoint = other.ClosestPointOnBounds(stickPosition); //추후 고민해보자.
            Vector3 closestPoint = other.ClosestPoint(stickPosition);

            Vector3 wooferCenter = other.bounds.center;

            float wooferTopY = wooferCenter.y + (other.bounds.extents.y - hitThreshold);

            Instantiate(tmpPointPrefab, closestPoint, Quaternion.identity);

            bool isDownwardHit = transform.position.y < prevPos.y; // 아래로 휘둘렀는지 확인
            bool isFastEnough = velocityMagnitude > 1.5f; // 일정 속도 이상 휘둘렀는지 확인
            bool isOnTop = closestPoint.y >= wooferTopY; // 윗면에서 충돌했는지 확인

            print($"아래로 휘둘렀는지: {isDownwardHit}, 속도는 충분했는지: {isFastEnough}, 윗면에 충돌했는지: {isOnTop}");

            if (isFastEnough && isDownwardHit && isOnTop)
            {
                woofer.Hit();
            }
        }
    }

    //======================Test======================
    private IEnumerator CreateCoroutine()
    {
        while (true)
        {
            RandomCreate();
            yield return new WaitForSeconds(1.5f);
        }
    }

    private void RandomCreate()
    {
        _noteManager.CreateNote(Random.Range(0, _noteManager.maxNoteRails));
    }
    //======================Test======================

    private void OnDestroy()
    {
        _controller.activateAction.action.performed -= TriggerButtonAction;
    }
}
