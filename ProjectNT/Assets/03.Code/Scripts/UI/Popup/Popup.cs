using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Popup : MonoBehaviour
    {
        [SerializeField] protected Button closeButton;
        protected Action closeAction;

        protected virtual void OnEnable()
        {
            closeButton.onClick.AddListener(CloseButtonClick);
        }

        protected virtual void OnDisable()
        {
            closeButton.onClick.RemoveListener(CloseButtonClick);
        }

        public void CloseButtonClick()
        {
            PopupManager.Instance.ClosePopup(this);
            closeAction?.Invoke();
        }

        /// <summary>
        /// PopupManager가 Popup을 찾는 과정에서 Init도 호출합니다.
        /// </summary>
        public virtual void Init() { }
    } 
}
