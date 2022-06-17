using TMPro;
using UnityEngine;
using UnityEngine.UI;

using CurrentRoute;
using SpecialControllers;

namespace UI
{
    public class PopupQuestionUI : MonoBehaviour
    {
        [SerializeField] private GameObject _questionAnswers;
        [SerializeField] private Transform _answerHolder;

        public delegate void OnVariableChangeDelegate();

        public event OnVariableChangeDelegate OnRightAnswerClick;
        public event OnVariableChangeDelegate OnWrongAnswerClick;

        public void LoadQuestionAnswers(ApiPopup popup)
        {
            foreach (string answer in popup.QuestionAnswers)
            {
                GameObject answerPrefab = Instantiate(_questionAnswers, _answerHolder);
                answerPrefab.GetComponentInChildren<TextMeshProUGUI>().text = answer;
                answerPrefab.GetComponent<Button>().onClick.AddListener(() =>
                {
                    CheckCorrectAnswer(answer, popup.CorrectAnswer);
                });
            }
        }

        private void CheckCorrectAnswer(string answer, string correctAnswer)
        {
            if (answer == correctAnswer)
            {
                AudioController.Instance.RequestSounds("right");
                DestroyChildren();

                OnRightAnswerClick?.Invoke();
            }
            else
            {
                AudioController.Instance.RequestSounds("wrong");
                OnWrongAnswerClick?.Invoke();
            }
        }

        public void DestroyChildren()
        {
            foreach (Transform childTransform in _answerHolder.GetComponentsInChildren<Transform>())
            {
                if (childTransform != transform)
                    Destroy(childTransform.gameObject);
            }
        }
    }
}