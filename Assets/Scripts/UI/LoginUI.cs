using UnityEngine;
using UnityEngine.UI;

using CurrentRoute;

namespace UI
{
    public class LoginUI : MonoBehaviour
    {
        [SerializeField] private InputField _nameInput;
        [SerializeField] private InputField _mailInput;
    
        [SerializeField] private Button _enterButton;
        [SerializeField] private Button _userAgreementButton;

        [SerializeField] private Toggle _toggle;

        [SerializeField] private GameObject _userAgreement;
        [SerializeField] private GameObject _userExistPanel;
    
        public void EnterClick()
        {
            if (_toggle.isOn)
            {
                Saves.SaveGameContoller.SetMail(_mailInput.text);

                // Start Route
                RouteController.Instance.StartRoute();

                // Disable Login UI
                UIController.Instance.LoginUI.SetActive(false);
            }
        }

        public void UserAgreementToggleChanged()
        {
            _userAgreement.SetActive(true);
        }
    }
}
