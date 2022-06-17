using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using CurrentRoute;
using UI;

public class SelectRouteUI : MonoBehaviour
{
    [field: SerializeField]
    public ApiOfficialRoutes Routes { get; private set; }

    public ApiOfficialRoute SelectedRoute { get; set; }

    [field: SerializeField]
    public RectTransform OfficialRoutesCollection { get; private set; }

    [field: SerializeField]
    public GameObject OfficialRoutePrefab { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        GetRoutes();
    }

    async void GetRoutes()
    {
        Routes = JsonHelperNew<ApiOfficialRoutes>.FromJSON(
                await ApiHandler.GetRequest($"https://{""}pixelpeople.nl/PixelPeopleAPI/OfficialRoutes.php/"));

        PopulateOfficialRoutes();
    }

    public void PopulateOfficialRoutes()
    {
        // Instantiate all prefabs
        for (int i = 0; i < Routes.Routes.Count; i++)
        {
            GameObject prefab = Instantiate(OfficialRoutePrefab, OfficialRoutesCollection.transform);
            prefab.GetComponent<RouteUI>().SetVariables(Routes.Routes[i]);
            prefab.GetComponentInChildren<Button>().onClick.AddListener(delegate { RouteClick(prefab); });
        }
    }

    public void RouteClick(GameObject prefab)
    {
        SelectedRoute = prefab.GetComponent<RouteUI>().Route;
    }

    public void StartRouteClick()
    {
        RouteController.Instance.GetRoute(SelectedRoute.URL);
        UIController.Instance.SelectRouteUI.SetActive(false);
    }
}
