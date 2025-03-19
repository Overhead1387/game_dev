using System.Collections;
using System.Collections.Generic;
using HyperCasual.Core;
using UnityEngine;
using UnityEngine.UI;

namespace HyperCasual.Runner
{
    /// <summary>
    /// Shop menu that provides access to in-game purchases and customization options.
    /// Contains a back button to return to the previous menu.
    /// </summary>
    public class ShopView : View
    {
        [SerializeField, Tooltip("Back button to return to previous menu")]
        HyperCasualButton m_Button;

        protected override void Awake()
        {
            base.Awake();
            ValidateComponents();
        }

        void ValidateComponents()
        {
            if (m_Button == null)
                Debug.LogError($"[{nameof(ShopView)}] Back Button reference is missing");
        }

        void OnEnable()
        {
            m_Button.AddListener(OnButtonClick);
        }

        void OnDisable()
        {
            m_Button.RemoveListener(OnButtonClick);
        }

        void OnButtonClick()
        {
            UIManager.Instance.GoBack();
        }
    }
}