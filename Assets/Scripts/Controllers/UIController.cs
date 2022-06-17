using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIController : Singleton<UIController>
    {
        [field: SerializeField, Header("Regular UI")]
        public GameObject MinimapUI { get; private set; }

        [field: SerializeField]
        public GameObject LoginUI { get; private set; }

        [field: SerializeField]
        public GameObject SelectRouteUI { get; private set; }

        [field: SerializeField]
        public GameObject CloseVideoButton { get; private set; }

        [field: SerializeField]
        public GameObject CloseWebButton { get; private set; }

        [field: SerializeField]
        public GameObject SettingsButton { get; private set; }

        [field: SerializeField]
        public GameObject PopupUI { get; private set; }


        [field: SerializeField, Header("Special UI")]
        public GameObject DummyUI { get; private set; }

        [field: SerializeField]
        public GameObject MoodMeUI { get; private set; }


        [field: SerializeField, Header("Images")]
        public Sprite NoImageIcon { get; private set; }

        // Methods
        // event for video laying or something
    }
}
