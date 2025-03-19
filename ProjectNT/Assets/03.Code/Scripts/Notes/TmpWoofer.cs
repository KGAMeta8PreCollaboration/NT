using UnityEngine;

public class TmpWoofer : MonoBehaviour
{
    public LongNote currentLongNote;
    private bool isHoldingLongNote = false;

    public bool IsCurrentNoteLong()
    {
        return currentLongNote != null;
    }

    public void StartLongNote()
    {
        if (IsCurrentNoteLong())
        {
            isHoldingLongNote = true;
            Debug.Log("롱노트 시작!");
            currentLongNote.Hit(NoteType.Perfect);
        }
    }

    public void Hold()
    {
        if (isHoldingLongNote && currentLongNote != null)
        {
            if (currentLongNote.Hold()) // 특정 시간에 맞춰 판정
            {
                Debug.Log("롱노트 판정 성공!");
            }

            // 롱노트가 종료되었는지 확인
            if (AudioSettings.dspTime >= currentLongNote.endTargetDspTime)
            {
                ReleaseLongNote();
            }
        }
    }

    public void ReleaseLongNote()
    {
        if (isHoldingLongNote)
        {
            currentLongNote.Release();
            isHoldingLongNote = false;
            currentLongNote = null;
        }
    }

    public void Hit()
    {
        Debug.Log("단노트 히트!");
    }
}
