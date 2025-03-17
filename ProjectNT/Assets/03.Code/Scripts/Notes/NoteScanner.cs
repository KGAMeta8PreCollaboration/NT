using System;
using System.Collections.Generic;
using UnityEngine;

public class NoteScanner : MonoBehaviour
{
    public event Action<Note> OnNoteEnter; // 노트가 감지될 때 이벤트
    public event Action<Note> OnNoteExit;  // 노트가 범위를 벗어날 때 이벤트

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Note note))
        {
            OnNoteEnter?.Invoke(note);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Note note))
        {
            OnNoteExit?.Invoke(note);
        }
    }
}
