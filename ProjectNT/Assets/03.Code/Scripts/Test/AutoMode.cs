using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMode : MonoBehaviour
{
    public double perfectThreshold;

    public Woofer[] woofers;
    public NoteRail[] rails;
    public LinkedList<Note>[] noteList;

    private void Start()
    {
        noteList = new LinkedList<Note>[rails.Length];

        for (int i = 0; i < rails.Length; i++)
        {
            noteList[i] = rails[i].GetNoteList();
            StartCoroutine(AutoModeRail(i, i < 4 ? false : true));
        }
    }

    public IEnumerator AutoModeRail(int index, bool topRail)
    {
        while (true)
        {
            if (noteList[index].Count > 0)
            {
                Note note = noteList[index].First.Value;
                double currentTime = AudioSettings.dspTime;
                double targetTime = note.GetTargetDspTime();
                double timeDiff = Math.Abs(currentTime - targetTime);

                bool isLongNote = note is LongNote;

                // 첫 정박 타이밍 처리
                if (timeDiff < perfectThreshold && !note.isHit)
                {
                    if (!topRail)
                    {
                        woofers[index].Hit();
                    }
                    else
                    {
                        TopNote topNote = note as TopNote;
                        topNote?.AutoHit(new UnityEngine.InputSystem.InputAction.CallbackContext());
                    }

                    Debug.Log($"첫 Hit 처리됨 - 시간차: {timeDiff:f2}");
                }
                // 롱노트면 홀드 처리
                else if (isLongNote && note.isHit)
                {
                    woofers[index].Hold();

                    Debug.Log($"Hold 처리 중 - 현재: {currentTime:f2}");
                }
            }

            yield return null;
        }
    }

}
