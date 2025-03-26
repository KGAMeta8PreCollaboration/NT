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
        protected PopupManager _popupManager;

        protected virtual void OnDestroy()
        {
            closeButton?.onClick.RemoveListener(CloseButtonClick);
        }

        public virtual void CloseButtonClick()
        {
            _popupManager.ClosePopup(this);
            closeAction?.Invoke();
        }

        /// <summary>
        /// PopupManager가 Popup을 찾는 과정에서 Init도 호출합니다.
        /// </summary>
        public virtual void Init(PopupManager popupManager)
        {
            _popupManager = popupManager;
            closeButton?.onClick.AddListener(CloseButtonClick);
        }
    }
}
