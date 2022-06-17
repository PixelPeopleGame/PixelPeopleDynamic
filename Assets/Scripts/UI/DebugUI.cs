using TMPro;
using UnityEngine;

namespace UI
{
    public class DebugUI : Singleton<DebugUI>
    {
        [SerializeField] private TextMeshProUGUI _distanceText;
        [SerializeField] private TextMeshProUGUI _longLatText;
        [SerializeField] private TextMeshProUGUI _angleText;
        [SerializeField] private TextMeshProUGUI _northText;
        [SerializeField] private TextMeshProUGUI _needleAngleText;
        [SerializeField] private GameObject _parent;

        public void UpdateDistanceText(double distance)
        {
            if(_parent.activeSelf == true) { 
            _distanceText.text = $"{distance:F2} M";
            }
        }

        public void UpdateLongLatText(double longitude, double latitude)
        {
            if (_parent.activeSelf == true)
            {
                _longLatText.text = $"Long:{longitude}\nLat: {latitude}";
            }
        }

        public void UpdateAngleText(float angle)
        {
            if (_parent.activeSelf == true)
            {
                _angleText.text = angle.ToString();
            }
        }

        public void UpdateNorth(float north)
        {
            if (_parent.activeSelf == true)
            {
                _northText.text = north.ToString();
            }
        }


        public void UpdateNeedleAngle(float angle)
        {
            if (_parent.activeSelf == true)
            {
                _needleAngleText.text = angle.ToString();
            }
        }
    }
}