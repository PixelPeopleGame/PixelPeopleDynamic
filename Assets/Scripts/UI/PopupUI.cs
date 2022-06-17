using TMPro;
using UnityEngine;
using UnityEngine.UI;

using CurrentRoute;
using AllPopupTemplates;

namespace UI
{
    public class PopupUI : MonoBehaviour
    {
        [field: SerializeField, Header("Components")]
        public TextMeshProUGUI TitleText { get; private set; }

        [field: SerializeField]
        public TextMeshProUGUI DescriptionText { get; private set; }

        [field: SerializeField]
        public Image BackgroundImage { get; private set; }

        [field: SerializeField]
        public Image Icon { get; private set; }

        [field: SerializeField]
        public Button CloseButton { get; private set; }

        private RectTransform _transform;
        private PopupQuestionUI _popupQuestionUI;
        private PopupAnimation _popupAnimation;

        private void Awake()
        {
            _transform = GetComponent<RectTransform>();
            _popupQuestionUI = GetComponentInChildren<PopupQuestionUI>();
            _popupAnimation = GetComponent<PopupAnimation>();
        }

        public void SetPopupData(ApiPopup popup)
        {
            PopupTemplate template = PopupTemplates.Instance.GetPopupTemplate(popup.PopupType);

            _popupQuestionUI.DestroyChildren();
            TitleText.text = popup.Title;
            DescriptionText.text = popup.Description + "\n\n";
            DescriptionText.color = template.TextColor;
            Icon.sprite = template.Image;
            BackgroundImage.color = template.BackgroundColor;
        
            _transform.anchoredPosition = new Vector2(0, 1000);

            if (popup.PopupType != PopupType.Question)
                return;
        
            _popupQuestionUI.LoadQuestionAnswers(popup);
        }

        public void CloseButtonClick()
        {
            _popupAnimation.PlayAnimation(AnimationType.Hide);
        }
    }
}