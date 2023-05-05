using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationUIController : MonoBehaviour
{
    public static GenerationUIController Instance { get; private set; }
    [SerializeField] private Transform routePrefab;
    [SerializeField] private Transform cityPrefab;
    [SerializeField] private Transform routeContent;
    private int currentGenIndex;


    private void Awake() {
        Instance = this;
    }

    private void Start() {
        Hide();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            UpdateRoutes(currentGenIndex);
        }

        if (Input.GetKeyDown(KeyCode.Z)) currentGenIndex--;
        if (Input.GetKeyDown(KeyCode.X)) currentGenIndex++;
        currentGenIndex = Mathf.Clamp(currentGenIndex, 0, TravelingManager.Instance.generationList.Count - 1);
    }

    private void UpdateRoutes(int generationIndex) {
        ClearContent();

        foreach (Route route in TravelingManager.Instance.generationList[generationIndex].routes) {
            Transform newRouteUI = Instantiate(routePrefab, routeContent);

            foreach (GameObject city in route.cityRoute) {
                CityUI newCityUI = Instantiate(cityPrefab, newRouteUI.GetComponent<RouteUIController>().cityContent).GetComponent<CityUI>();
                newCityUI.cityId.text = $"C{city.GetComponent<City>().cityId}";
            }
        }
    }

    private void ClearContent() {
        foreach (Transform child in routeContent) {
            Destroy(child.gameObject);
        }
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}
