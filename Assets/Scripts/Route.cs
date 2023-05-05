using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Route : MonoBehaviour
{
    public float totalDistance;
    public List<GameObject> cityRoute = new List<GameObject>();

    private void Awake() {
        PopulateRoute();
    }

    private void Start() {

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
        for (int i = 0; i < cityRoute.Count; i++) {
            if (i == cityRoute.Count-1) {
                totalDistance += Vector3.Distance(cityRoute[i].transform.position, cityRoute[0].transform.position);
                Debug.Log($"{i} -> 0 - Total distance: {totalDistance}");
            }
            else {
                totalDistance += Vector3.Distance(cityRoute[i].transform.position, cityRoute[i + 1].transform.position);
                Debug.Log($"{i} -> {i+1} - Total distance: {totalDistance}");
            }
        }

    }

    public void PopulateRouteCrossover(Route parentFirst, Route parentSecond) {
        var count = cityRoute.Count;
        Route child = new Route();
        child.cityRoute.Clear();

        var startCut = Random.Range(0, count);
        var endCut = Random.Range(startCut+1, count);
        var slice = this.cityRoute.Skip(startCut).Take(endCut - startCut).ToList();

        for (int i = 0; i < count; i++) {
            if (!slice.Contains(parentFirst.cityRoute[i])) {
                slice.Add(parentFirst.cityRoute[i]);
            }
        }
        child.cityRoute = slice;

        cityRoute = child.cityRoute;
    }
}
