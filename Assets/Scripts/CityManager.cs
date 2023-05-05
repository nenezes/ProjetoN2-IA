using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityManager : MonoBehaviour
{
    public static CityManager Instance { get; private set; }

    [SerializeField] private float spawnRange;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask cityLayer;
    [SerializeField] private List<GameObject> cityPrefabs;
    public List<GameObject> cities = new List<GameObject>();


    private void Awake() {
        Instance = this;
    }

    private void Update() {

    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawSphere(Vector3.zero, spawnRange);
    }

    public void SpawnCities(int citiesToSpawn) {
        DespawnCities();
        for (int i = 0; i < citiesToSpawn; i++) {
            Vector3 randomPos = GetRandomSpawnPosition();

            GameObject newCity = Instantiate(cityPrefabs[Random.Range(0, cityPrefabs.Count)], randomPos, Quaternion.identity, this.transform);

            City city = newCity.GetComponent<City>();

            city.cityId = i;

            cities.Add(newCity);
        }
    }

    public void DespawnCities() {
        foreach (Transform child in this.transform) {
            Destroy(child.gameObject);
        }

        cities.Clear();
    }

    private Vector3 GetRandomSpawnPosition() {
        bool gotProperPos = false;
        Vector3 randomPos = new Vector3();
        int att = 0;

        while (!gotProperPos) {
            if (att >= 100) {
                Debug.Log("Max tries exceeded.");
                gotProperPos = true;
            }
            att++;
            randomPos = Random.insideUnitSphere * spawnRange;
            randomPos.y = this.transform.position.y;

            if (Physics.Raycast(randomPos, Vector3.down, Mathf.Infinity, groundLayer) && !Physics.Raycast(randomPos, Vector3.down, Mathf.Infinity, cityLayer)) gotProperPos = true;
        }
        randomPos.y = 0;

        return randomPos;
    }
}
