using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PopupDetail
{

}

public class PopUp : MonoBehaviour
{
   public string[] strings = new string[]
    {
        "저장할 경로를 선택해주세요.",
        "파일을 저장하는데 실패했습니다",
        "작업물의 이름이 지정되지않았습니다.",
        "작업물의 아티스트가 지정되지않았습니다.",
        "작업물의 표지가 지정되지않았습니다.",
        "작업을 수행하면 저장하지 않은 작업물이 제거될 수 있습니다.\n먼저 저장하시겠습니까?",
        "파일을 불러오지 못 했습니다.",
        "더 이상 페이즈를 늘릴 수 없습니다."
    };

   private void Awake()
   {

   }

}
