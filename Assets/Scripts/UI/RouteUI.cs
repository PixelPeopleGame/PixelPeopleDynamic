using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UI;

public class RouteUI : MonoBehaviour
{
    [field: SerializeField]
    public Text TitleText { get; private set; }

    [field: SerializeField]
    public Text DescriptionText { get; private set; }

    [field: SerializeField]
    public RawImage RouteIcon { get; private set; }

    public ApiOfficialRoute Route { get; set; }

    public async void SetVariables(ApiOfficialRoute route)
    {
        this.Route = route;

        // Set Text
        TitleText.text = route.Name;
        DescriptionText.text = route.Description;

        // Set Texture
        if (route.ImageURL != "")
        {
            Texture2D texture = await ApiHandler.GetRemoteTexture(route.ImageURL);
            if (texture != null)
            {
                RouteIcon.texture = texture;
            }
            else
            {
                RouteIcon.texture = UIController.Instance.NoImageIcon.texture;
            }
        }
    }
}
