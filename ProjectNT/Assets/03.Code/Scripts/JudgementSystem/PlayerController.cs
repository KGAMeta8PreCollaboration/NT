using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Woofer[] _woofers;

    private NoteManager _noteManager;

    [Range(1, 5)] public float hitSpeedThreshold = 1f;
    private ActionBasedController _controller;
    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _noteManager = FindObjectOfType<NoteManager>();
        _controller = GetComponentInParent<ActionBasedController>();

        _controller.activateAction.action.performed += TriggerButtonAction;

        StartCoroutine(CreateCoroutine());
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
            float speed = _rigidbody.velocity.magnitude;
            Vector3 direction = _rigidbody.velocity.normalized;
            float dotProduct = Vector3.Dot(direction, Vector3.down);

            if (speed >= hitSpeedThreshold && dotProduct > 0.7f) // 속도+방향 검사
            {
                Debug.Log("노트 히트 성공! (정확한 내리치기 판정)");
                Destroy(other.gameObject);
            }
            else
            {
                Debug.Log("속도 부족 또는 잘못된 방향 (히트 실패)");
            }
        }
    }

    //======================Test======================
    private IEnumerator CreateCoroutine()
    {
        while (true)
        {
            RandomCreate();
            yield return new WaitForSeconds(1.0f);
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
