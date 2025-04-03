using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaBlender : MonoBehaviour
{
    public float alpha;
    public List<Renderer> rends;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Renderer rend in rends)
        {
            rend.material.color = new Color(1, 1, 1, alpha);
        }
    }
}
