using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GenerationUIController : MonoBehaviour
{
    public static GenerationUIController Instance { get; private set; }
    [SerializeField] private Transform routePrefab;
    [SerializeField] private Transform cityPrefab;
    [SerializeField] private Transform routeContent;
    [SerializeField] private TextMeshProUGUI generationDrawn;
    private CanvasGroup canvasGroup;
    public int currentGenIndex;


    private void Awake() {
        Instance = this;
        canvasGroup = this.GetComponent<CanvasGroup>();
    }

    private void Start() {
        //Hide();
    }

    private void Update() {

        if (TravelingManager.Instance.generationList.Count < 1) return;

        if (TravelingManager.Instance.autoplay) currentGenIndex = TravelingManager.Instance.generationList.Count - 1;

        UpdateRoutes(currentGenIndex);

        if (Input.GetKeyDown(KeyCode.Z)) {
            currentGenIndex--;
            currentGenIndex = Mathf.Clamp(currentGenIndex, 0, TravelingManager.Instance.generationList.Count - 1);
            UpdateRoutes(currentGenIndex);
        }

        if (Input.GetKeyDown(KeyCode.X)) {
            currentGenIndex++;
            currentGenIndex = Mathf.Clamp(currentGenIndex, 0, TravelingManager.Instance.generationList.Count - 1);
            UpdateRoutes(currentGenIndex);
        }

        if (Input.GetKeyDown(KeyCode.Y)) {
            currentGenIndex = TravelingManager.Instance.generationList.Count - 1;
            UpdateRoutes(currentGenIndex);
        }
    }

    private void UpdateRoutes(int generationIndex) {
        ClearContent();

        generationDrawn.text = $"{generationIndex}";

        foreach (Route route in TravelingManager.Instance.generationList[generationIndex].routes) {
            Transform newRouteUI = Instantiate(routePrefab, routeContent);

            if (route == TravelingManager.Instance.generationList[generationIndex].GetFit()) {
                Outline newRouteUIOutline = newRouteUI.gameObject.AddComponent<Outline>();
                newRouteUIOutline.effectColor = Color.green;
                newRouteUIOutline.useGraphicAlpha = false;
                newRouteUIOutline.effectDistance = new Vector2(3, 3);
            }

            foreach (GameObject city in route.cityRoute) {
                CityUI newCityUI = Instantiate(cityPrefab, newRouteUI.GetComponent<RouteUIController>().cityContent).GetComponent<CityUI>();
                newCityUI.cityId.text = $"C{city.GetComponent<City>().cityId}";
            }
        }

        RouteRenderer.Instance.RenderRoute(TravelingManager.Instance.generationList[generationIndex].GetFit().cityRoute);
    }

    private void ClearContent() {
        foreach (Transform child in routeContent) {
            Destroy(child.gameObject);
        }
    }

    public void Toggle() {
        if (canvasGroup.alpha == 1) Hide();
        else Show();
    }

    public void Show() {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void Hide() {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
    
    public void SetCurrentGenIndex(int index) {
        currentGenIndex = index;
        UpdateRoutes(currentGenIndex);
        Debug.Log($"Generation {currentGenIndex}'s best route is: {TravelingManager.Instance.generationList[currentGenIndex].GetFit().totalDistance}m.");
    }
}
