using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class JudgementSystem : MonoBehaviour
{
    [SerializeField] private Transform[] _timingTrans; //Perfact, Great, Good의 Transform
    [SerializeField] private TextMeshProUGUI _judgementText;

    private Vector2[] _timingBoxs;

    private Woofer _woofer;

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        _timingBoxs = new Vector2[_timingTrans.Length];
        for (int i = 0; i < _timingTrans.Length; i++)
        {
            float minX = _timingTrans[i].position.x - (_timingTrans[i].localScale.x / 2f);
            float maxX = _timingTrans[i].position.x + (_timingTrans[i].localScale.x / 2f);

            _timingBoxs[i].Set(minX, maxX);

            float diff = Mathf.Abs(minX - maxX);
            print(diff);
            print(_timingBoxs[i].x + ", " + _timingBoxs[i].y);
        }
    }

    public NoteType CheckTiming()
    {
        float notePosX = _woofer.notes[0].transform.position.x;
        for (int i = 0; i < _timingBoxs.Length; i++)
        {
            if (_timingBoxs[i].x <= notePosX && notePosX <= _timingBoxs[i].y)
            {
                NoteType noteType = i == 0 ? NoteType.Perfect :
                    i == 1 ? NoteType.Good :
                    i == 2 ? NoteType.Cool : NoteType.Bad;
                _judgementText.text = noteType.ToString() + "!";
                print(noteType.ToString() + "!");
                return noteType;
            }
        }
        _judgementText.text = "Miss!";
        print("미스!");
        return NoteType.Bad;
    }

    private void Init()
    {
        _woofer = GetComponent<Woofer>();
    }
}
