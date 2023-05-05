using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Button randomizeCities;
    [SerializeField] private TextMeshProUGUI numberOfCitiesText;
    [SerializeField] private Slider numberOfCitiesSlider;


    private void Start() {
        numberOfCitiesText.text = $"{Mathf.RoundToInt(numberOfCitiesSlider.value)}";

        randomizeCities.onClick.AddListener(() => {
            CityManager.Instance.SpawnCities(Mathf.RoundToInt(numberOfCitiesSlider.value));
        });

        numberOfCitiesSlider.onValueChanged.AddListener((v) => {
            numberOfCitiesText.text = $"{Mathf.RoundToInt(numberOfCitiesSlider.value)}";
        });
    }
}
