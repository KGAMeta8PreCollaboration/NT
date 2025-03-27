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
        text.text = time.ToString() + "/" + time * secondsPerBPM;
        //text.transform.localScale = new Vector3(
        //     originalTextScale.x / transform.localScale.x,
        //     originalTextScale.y / transform.localScale.y,
        //     originalTextScale.z
        //     );
    }

    public void Test(float tmp)
    {
        text.text = tmp.ToString();
    }
}
