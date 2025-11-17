using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    private Queue<GameObject> pool = new Queue<GameObject>();

    [SerializeField] private GameObject[] resources;
    private List<Transform> spawnPool = new List<Transform>();

    [SerializeField] private int initialSize = 20;

    [SerializeField] private float spawnDleay = 30;

    public void Start()
    {
        for (int i = 0; i < initialSize; i++)
        {
            GameObject resource = resources[Random.Range(0, resources.Length)];
            GameObject go = Instantiate(resource, gameObject.transform);
            go.transform.Rotate(new Vector3(0, Random.Range(-180, 180), 0));
            pool.Enqueue(go);
            go.SetActive(false);
        }

        InitSpawnPool();

        StartCoroutine(Respawn());
    }

    private void InitSpawnPool()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            spawnPool.Add(spawnPoints[i]);
        }
    }

    public void InsertQueue(GameObject go, int index)
    {
        pool.Enqueue(go);
        spawnPool.Add(spawnPoints[index]);
        go.SetActive(false);
    }

    public GameObject GetQueue()
    {
        GameObject go = pool.Dequeue();
        go.SetActive(true);

        return go;
    }

    IEnumerator Respawn()
    {
        float dealy = 0.01f;
        while (true)
        {
            if (pool.Count != 0)
            {
                int index = Random.Range(0, spawnPool.Count);
                spawnPool.Remove(spawnPoints[index]);

                GameObject go = GetQueue();
                go.transform.position = spawnPoints[index].position;
                go.GetComponent<Resource>().spawnIndex = index;

                if (pool.Count <= 5)
                    dealy = spawnDleay;
                else
                    dealy = 0.01f;
            }
            yield return new WaitForSeconds(dealy);
        }
    }
}