using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteRenderer : MonoBehaviour
{
    public static RouteRenderer Instance;
    private LineRenderer lineRenderer;

    private void Awake() {
        Instance = this;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.loop = true;
    }

    public void RenderRoute(List<GameObject> cityRoute) {
        lineRenderer.positionCount = cityRoute.Count;

        for (int i = 0; i < cityRoute.Count; i++) {
            Vector3 point = cityRoute[i].transform.position;    
            point.y = .2f;
            lineRenderer.SetPosition(i, point);
        }
    }

    public void ClearRoute() {
        lineRenderer.positionCount = 0;
    }
}
