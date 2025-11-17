using UnityEngine;

public class Resource : MonoBehaviour
{
    public ItemData itemToGive;
    public int quantityPerHit = 1;
    public int capacity;

    private ResourceSpawner resourceSpawner;

    private void Start()
    {
        resourceSpawner = FindAnyObjectByType<ResourceSpawner>();
    }

    public void Gathering(Vector3 hitPoint, Vector3 hitNormal)
    {
        for (int i = 0; i < quantityPerHit; i++)
        {
            capacity -= 1;
            Instantiate(itemToGive.dropPrefabs, hitPoint + Vector3.up, Quaternion.LookRotation(hitNormal, Vector3.up));
            if (capacity <= 0)
            {
                Release();
                break;
            }
        }
    }

    private void Release()
    {
        resourceSpawner.InsertQueue(gameObject);
    }
}
