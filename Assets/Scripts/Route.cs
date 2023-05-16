using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Route : MonoBehaviour
{
    public float totalDistance;
    public List<GameObject> cityRoute = new List<GameObject>();
    private int bestSnippetIndex;

    private void Awake() {
        PopulateRoute();
    }

    public void PopulateRoute() {
        foreach (GameObject city in CityManager.Instance.cities) {
            cityRoute.Add(city);
        }
        ShuffleList();
        CalculateTotalDistance();
    }

    private void ShuffleList() {
        for (int i = 0; i < cityRoute.Count; i++) {
            GameObject temp = cityRoute[i];
            int rand = Random.Range(i, cityRoute.Count);
            cityRoute[i] = cityRoute[rand];
            cityRoute[rand] = temp;
        }
    }

    public void CalculateTotalDistance() {
        totalDistance = 0;

        for (int i = 0; i < cityRoute.Count; i++) {
            if (i == cityRoute.Count-1) {
                totalDistance += Vector3.Distance(cityRoute[i].transform.position, cityRoute[0].transform.position);
                //Debug.Log($"{i} -> 0 - Total distance: {totalDistance}");
            }
            else {
                totalDistance += Vector3.Distance(cityRoute[i].transform.position, cityRoute[i + 1].transform.position);
                //Debug.Log($"{i} -> {i+1} - Total distance: {totalDistance}");
            }
        }

        if (totalDistance < TravelingManager.Instance.bestRouteFoundDistance) {
            TravelingManager.Instance.bestRouteFoundDistance = totalDistance;
            TravelingManager.Instance.bestRouteFoundGeneration = GenerationUIController.Instance.currentGenIndex+1;
        }
    }

    public void PopulateRouteCrossover(Route parentFirst, Route parentSecond) {
        EmplaceSnippetAt(GetBestSnippet(parentSecond), bestSnippetIndex);

        EmplaceSnippetAt(GetBestSnippet(parentFirst), bestSnippetIndex);

        if (Random.Range(0, 100) > 65){
            Mutate(1);
        }

        if (Random.Range(0, 100) > 80){
            Mutate(3);
        }

        if (Random.Range(0, 100) > 95) {
            Mutate(5);
        }

        CalculateTotalDistance();
    }

    private void Mutate(int amountToMutate) {
        for (int i = 0; i < amountToMutate; i++) {
            int randI = Random.Range(0, cityRoute.Count);
            GameObject temp = cityRoute[randI];
            int rand = Random.Range(0, cityRoute.Count);
            cityRoute[randI] = cityRoute[rand];
            cityRoute[rand] = temp;
        }
    }

    private void EmplaceSnippetAt(List<GameObject> snippet, int index) {
        for (int i = 0; i < snippet.Count; i++) {
            int elementCurrentIndex = cityRoute.FindIndex(j => j.gameObject.GetComponent<City>().cityId == snippet[i].gameObject.GetComponent<City>().cityId);
            //Debug.Log($"Element C{snippet[i].gameObject.GetComponent<City>().cityId} is at {elementCurrentIndex}");
            var temp = cityRoute[CycleX(index+i, cityRoute.Count)];
            cityRoute[CycleX(index + i, cityRoute.Count)] = snippet[i];
            cityRoute[elementCurrentIndex] = temp;
        }
    }

    private List<GameObject> GetBestSnippet(Route route) {
        List<GameObject> bestSnippet = new List<GameObject>();
        List<GameObject> tempSnippet = new List<GameObject>();
        int snippetSize = (cityRoute.Count-2);

        for (int i = 0; i < route.cityRoute.Count; i++) {
            tempSnippet.Clear();
            bestSnippetIndex = i;
            for (int j = 0; j < snippetSize; j++) {
                tempSnippet.Add(route.cityRoute[CycleX(i + j, route.cityRoute.Count)]);
            }

            if (bestSnippet.Count == 0) bestSnippet = tempSnippet;

            if (GetSnippetTotalDistance(tempSnippet) < GetSnippetTotalDistance(bestSnippet)) bestSnippet = tempSnippet;
        }

        //for (int i = 0; i < bestSnippet.Count; i++) {
            //Debug.Log($"Best snippet[{i}] = C{bestSnippet[i].gameObject.GetComponent<City>().cityId}");
        //}

        return bestSnippet;
    }

    private float GetSnippetTotalDistance(List<GameObject> snippet) {
        float snippetTotalDist= 0;

        for (int i = 0; i < snippet.Count-1; i++) {
            snippetTotalDist += Vector3.Distance(snippet[i].transform.position, snippet[i + 1].transform.position);
        }

        return snippetTotalDist;
    }

    private int CycleX(int i, int cycleLength) {
        return (i % cycleLength);
    }
}