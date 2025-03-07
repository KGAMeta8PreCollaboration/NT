using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class TitleController : MonoBehaviour
{
    public XRRayInteractor rayInteractor;
    public ActionBasedController abcController;
    public LayerMask layerMask;

    private XRController controller;
    private GameObject curObject = null;
    private OutlineTrigger curOutlineTrigger = null;
    private Button curButton = null;

    void Start()
    {
        Initialization();
    }

    private void Initialization()
    {
        abcController = GetComponent<ActionBasedController>();
        controller = abcController.GetComponent<XRController>();
    }

    
}
