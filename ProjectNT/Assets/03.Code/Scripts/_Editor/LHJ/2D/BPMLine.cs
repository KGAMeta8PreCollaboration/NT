using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BPMLine : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;
    private Vector3 originalTextScale;

    private void Awake()
    {
        originalTextScale = text.transform.localScale;
        //print($"로컬 사이즈 : {originalTextScale}");
    }

    public void SetBPMText(float time, float secondsPerBPM)
    {
        string timeText = SetSongLengthText(time * secondsPerBPM);
        text.text = time.ToString() + "/" + timeText;
        //text.transform.localScale = new Vector3(
        //     originalTextScale.x / transform.localScale.x,
        //     originalTextScale.y / transform.localScale.y,
        //     originalTextScale.z
        //     );
    }
    private string SetSongLengthText(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        int milliseconds = Mathf.FloorToInt((time * 1000) % 1000);
        return string.Format("{0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);
    }
}
