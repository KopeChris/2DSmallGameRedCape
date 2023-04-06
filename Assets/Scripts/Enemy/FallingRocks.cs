using System.Collections;
using UnityEngine;

public class FallingRocks : MonoBehaviour
{
    public GameObject rockPrefab;
    public float spawnIntervalMin;
    public float spawnIntervalMax;

    float spawnInterval;

    void Start()
    {
        StartCoroutine(SpawnRocks());
    }

    IEnumerator SpawnRocks()
    {
        while (true)
        {
            spawnInterval = Random.Range(spawnIntervalMin, spawnIntervalMax);
            yield return new WaitForSeconds(spawnInterval);
            Instantiate(rockPrefab, transform.position, Quaternion.identity);
        }
    }

}
