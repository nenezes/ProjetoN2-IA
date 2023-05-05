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

    private void Awake() {
        Instance = this;
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
        }

        if (Input.GetKeyDown(KeyCode.W)) {
            Generation firstGen = first.GetComponent<Generation>();
            firstGen.SetFittests();
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            lastGeneration = currentGeneration;
            NextGeneration();
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            currentGeneration.PopulateGenCrossover();
        }
    }

    private void NextGeneration() {
        GameObject newGenObject = Instantiate(generationPrefab, this.transform);

        Generation newGen = newGenObject.GetComponent<Generation>();

        newGen.SetFittestParents(currentGeneration.GetFit(), currentGeneration.GetSecondFit());

        currentGeneration = newGen;
    }
}

/*foreach (Transform child in newGenObject.transform) {
            Generation childGen = child.GetComponent<Generation>();
            childGen.Clear();
        }*/