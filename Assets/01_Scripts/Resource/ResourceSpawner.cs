using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private GameObject spawnRange;
    [SerializeField] private BoxCollider rangeCollider;
    [SerializeField] private GameObject[] resources;
    private Queue<GameObject> pool = new Queue<GameObject>();

    [SerializeField] private int initialSize = 30;
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

        StartCoroutine(Respawn());
    }

    public void InsertQueue(GameObject go)
    {
        pool.Enqueue(go);
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
                GameObject go = GetQueue();
                go.transform.position = GetRandomPosition();
                if (pool.Count <= 5)
                    dealy = spawnDleay;
            }
            yield return new WaitForSeconds(dealy);
        }
    }

    Vector3 GetRandomPosition()
    {
        Vector3 originPosition = spawnRange.transform.position;
  
        float rangeX = rangeCollider.bounds.size.x;
        float rangeZ = rangeCollider.bounds.size.z;

        rangeX = Random.Range((rangeX / 2) * -1, rangeX / 2);
        rangeZ = Random.Range((rangeZ / 2) * -1, rangeZ / 2);
        Vector3 RandomPostion = new Vector3(rangeX, 0f, rangeZ);

        Vector3 respawnPosition = originPosition + RandomPostion;
        return respawnPosition;
    }
}