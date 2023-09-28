using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Button randomizeCitiesButton;
    [SerializeField] private Button toggleRoutesButton;
    [SerializeField] private Button nextGenerationButton;
    [SerializeField] private Button autoplayButton;
    [SerializeField] private TextMeshProUGUI numberOfCitiesText;
    [SerializeField] private Slider numberOfCitiesSlider;

    [SerializeField] private TextMeshProUGUI shortestRouteDistance;
    [SerializeField] private TextMeshProUGUI shortestRouteGeneration;
    [SerializeField] private TextMeshProUGUI totalGenerationsText;


    private void Start() {
        numberOfCitiesText.text = $"{Mathf.RoundToInt(numberOfCitiesSlider.value)}";

        randomizeCitiesButton.onClick.AddListener(() => {
            CityManager.Instance.SpawnCities(Mathf.RoundToInt(numberOfCitiesSlider.value));
            TravelingManager.Instance.SetupFirst();
            toggleRoutesButton.interactable = true;
            nextGenerationButton.interactable = true;
            autoplayButton.interactable = true;
        });

        toggleRoutesButton.onClick.AddListener(() => {
            GenerationUIController.Instance.Toggle();
        });

        nextGenerationButton.onClick.AddListener(() => {
            TravelingManager.Instance.NextGeneration();
            TravelingManager.Instance.currentGeneration.SetFittests();
            GenerationUIController.Instance.currentGenIndex++;
        });

        autoplayButton.onClick.AddListener(() => {
            TravelingManager.Instance.autoplay = !TravelingManager.Instance.autoplay;
        });

        numberOfCitiesSlider.onValueChanged.AddListener((v) => {
            numberOfCitiesText.text = $"{Mathf.RoundToInt(numberOfCitiesSlider.value)}";
        });
    }

    private void Update() {
        totalGenerationsText.text = $"CURRENT GENERATION: {Mathf.Clamp(TravelingManager.Instance.generationList.Count-1, 0, Mathf.Infinity)}";
        shortestRouteDistance.text = $"{TravelingManager.Instance.bestRouteFoundDistance}m";
        shortestRouteGeneration.text = $"Found on generation {TravelingManager.Instance.bestRouteFoundGeneration}";

        if (TravelingManager.Instance.generationList.Count < 1) return;

        if (TravelingManager.Instance.autoplay) {
            toggleRoutesButton.interactable = false;
            nextGenerationButton.interactable = false;
            randomizeCitiesButton.interactable = false;
        }
        else {
            toggleRoutesButton.interactable = true;
            nextGenerationButton.interactable = true;
            randomizeCitiesButton.interactable = true;
        }
    }
}
