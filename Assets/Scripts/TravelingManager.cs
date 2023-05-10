using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelingManager : MonoBehaviour
{
    public static TravelingManager Instance { get; private set; }

    public List<Generation> generationList = new List<Generation>();
    [SerializeField] private GameObject generationPrefab;
    private Generation lastGeneration;
    public Generation currentGeneration;
    private GameObject first;
    private bool autoplay = false;
    public float bestRouteFoundDistance;

    private void Awake() {
        Instance = this;
        bestRouteFoundDistance = 9999f;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            if (GenerationUIController.Instance.gameObject.activeInHierarchy) GenerationUIController.Instance.Hide();
            else GenerationUIController.Instance.Show();
        }

        if (Input.GetKeyDown(KeyCode.Q)) {
            first = Instantiate(generationPrefab, this.transform);
            currentGeneration = first.GetComponent<Generation>();
            generationList.Add(currentGeneration);
            currentGeneration.PopulateGen();
            Generation firstGen = first.GetComponent<Generation>();
            firstGen.SetFittests();
        }

        if (Input.GetKeyDown(KeyCode.W)) {
            Generation firstGen = first.GetComponent<Generation>();
            firstGen.SetFittests();
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            NextGeneration();
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            currentGeneration.SetFittests();
        }

        if (Input.GetKeyDown(KeyCode.Space)) autoplay = !autoplay;

        if (autoplay) {
            NextGeneration();
            currentGeneration.SetFittests();
        }
    }

    private void NextGeneration() {
        lastGeneration = currentGeneration;

        GameObject newGenObject = Instantiate(generationPrefab, this.transform);

        Generation newGen = newGenObject.GetComponent<Generation>();

        newGen.SetFittestParents(currentGeneration.GetFit(), currentGeneration.GetSecondFit());

        currentGeneration = newGen;

        currentGeneration.PopulateGenCrossover();

        generationList.Add(currentGeneration);
    }
}

/*foreach (Transform child in newGenObject.transform) {
            Generation childGen = child.GetComponent<Generation>();
            childGen.Clear();
        }*/