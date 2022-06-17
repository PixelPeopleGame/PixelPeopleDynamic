using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AllPopupTemplates
{
    public class PopupTemplates : Singleton<PopupTemplates>
    {
        [SerializeField]
        private List<PopupTemplate> _popupTemplates = new List<PopupTemplate>();

        public PopupTemplate GetPopupTemplate(PopupType type)
        {
            return _popupTemplates.Find(x => x.Type == type);
        }
    }

    [System.Serializable]
    public class PopupTemplate
    {
        public PopupType Type;
        public Sprite Image;
        public Color TextColor;
        public Color BackgroundColor = Color.black;
    }
}

