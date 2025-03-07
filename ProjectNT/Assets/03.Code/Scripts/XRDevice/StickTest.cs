using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickTest : MonoBehaviour
{
    public SphereCollider m_coll;

    private void Awake()
    {
        if (m_coll == null) m_coll = GetComponentInChildren<SphereCollider>();
    }
}
