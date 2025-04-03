using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//왼쪽클릭 -> 노드 설치 or 노래 재생 위치 변경 && UI 클릭할때, 드래그할때, 땔때 노래 재생시간 및 볼륨 조절 가능
//오른쪽 클릭 -> 카메라 이동 가능 상태

public class MouseHandler : MonoBehaviour
{
    //오른쪽 마우스 클릭이 될 때
    public bool isRotating = false;

    public Vector3 mousePosition;

    private NodeContainer _nodeContainer;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isRotating = true;
        }
        else
        {
            isRotating = false;
        }
    }
}
