using System.Collections.Generic;
using UnityEngine;

public class Generation : MonoBehaviour
{
    [SerializeField] private GameObject routePrefab;
    private Route parentFirst, parentSecond;
    public List<Route> routes = new List<Route>();
    private Route fittestRoute;
    private Route secondFittestRoute;

    private readonly int routeAmount = 10;

    public void PopulateGen() {
        for (int i = 0; i < routeAmount; i++) {
            Route newRoute = Instantiate(routePrefab, this.transform).GetComponent<Route>();
            routes.Add(newRoute);
        }
    }

    public void PopulateGenCrossover() {
        for (int i = 0; i < routeAmount; i++) {
            Route newRoute = Instantiate(routePrefab, this.transform).GetComponent<Route>();
            routes.Add(newRoute);
            newRoute.PopulateRouteCrossover(parentFirst, parentSecond);
        }
    }

    public void SetFittests() {
        fittestRoute = routes[0];

        for (int i = 0; i < routes.Count; i++) {
            if (fittestRoute.totalDistance > routes[i].totalDistance) fittestRoute = routes[i];
        }

        for (int i = 0; i < routes.Count; i++) {
            if (routes[i] == fittestRoute) continue;

            if (secondFittestRoute == null) secondFittestRoute = routes[i];

            if (secondFittestRoute.totalDistance > routes[i].totalDistance) secondFittestRoute = routes[i];
        }
        //Debug.Log($"1st: {fittestRoute.totalDistance}\n2nd: {secondFittestRoute.totalDistance}");
    }

    public void Clear() {
        for (int i = 0; i < routes.Count; i++) {
            Destroy(routes[i].gameObject);
        }
        routes.Clear();
    }

    public void SetFittestParents(Route parentFirst, Route parentSecond) {
        this.parentFirst = parentFirst;
        this.parentSecond = parentSecond;
    }

    public Route GetFit() => fittestRoute;
    public Route GetSecondFit() => secondFittestRoute;
}