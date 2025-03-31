using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TmpCreateNotes : MonoBehaviour
{
    private NoteManager noteManager;
    [SerializeField] private Woofer[] _woofers;

    //LongNote Test
    public bool[] isHolding;
    //판정 평균 Test
    public double sum = 0;
    public double avg = 0;
    public int count = 0;

    public void AvgPerfetDiff()
    {
        avg = sum / count;
        print($"timDiff 퍼펙트 평균: {avg}");
    }

    private void Start()
    {
        noteManager = FindObjectOfType<NoteManager>();
        //StartCoroutine(CreateCoroutine());

        isHolding = new bool[_woofers.Length];
    }

    // public void Create(InputAction.CallbackContext context)
    // {
    // 	if (context.performed)
    // 	{
    // 		noteManager.CreateNote(int.Parse(context.control.name) - 1);
    // 	}
    // }

    public void Hit(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // Debug.Log($"누른 시간: {AudioSettings.dspTime}");

            int index = context.control.name == "z" ? 0 :
                context.control.name == "x" ? 1 :
                context.control.name == "c" ? 2 :
                context.control.name == "v" ? 3 : 0;

            isHolding[index] = true;
            _woofers[index].Hit();
        }
        if (context.canceled) // 키를 뗐을 때
        {
            int index = context.control.name == "z" ? 0 :
                context.control.name == "x" ? 1 :
                context.control.name == "c" ? 2 :
                context.control.name == "v" ? 3 : 0;
            if (isHolding[index] == true)
            {
                _woofers[index].ReleaseLongNote(); // 롱노트 종료
                isHolding[index] = false;
            }
        }
    }

    private void Update()
    {
        for (int i = 0; i < _woofers.Length; i++)
        {
            if (isHolding[i] == true)
            {
                _woofers[i].Hold();
            }
        }
    }

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
        // noteManager.CreateNote(Random.Range(0, noteManager.maxNoteRails));
    }
}
