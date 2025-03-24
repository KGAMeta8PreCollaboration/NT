using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TopNote : Note
{

    private XRSimpleInteractable xRSimInter;

    public override void Init(Transform target, NoteSpawnData noteSpawnData)
    {
        base.Init(target, noteSpawnData);

        xRSimInter = GetComponent<XRSimpleInteractable>();
    }

    public override void Hit(JudgementType noteType)
    {
        Debug.Log("HIT");
    }

    protected override void PostJudgement()
    {
        Debug.Log("PostJudgement");
    }

}
