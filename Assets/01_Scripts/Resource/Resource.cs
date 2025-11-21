using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] private ItemData itemToGive;
    [SerializeField] private int quantityPerHit = 1;
    [SerializeField] private int MaxCapacity;
    [SerializeField] private int curCapacity;

    public int spawnIndex;

    private ResourceSpawner resourceSpawner;

    public void Init(ResourceSpawner resourceSpawner)
    {
        this.resourceSpawner = resourceSpawner;
        curCapacity = MaxCapacity;
    }

    public void Gathering(Vector3 hitPoint, Vector3 hitNormal)
    {
        for (int i = 0; i < quantityPerHit; i++)
        {
            curCapacity -= 1;
            Instantiate(itemToGive.dropPrefabs, hitPoint + Vector3.up, Quaternion.LookRotation(hitNormal, Vector3.up));
            if (curCapacity <= 0)
            {
                Release();
                break;
            }
        }
    }

    private void Release()
    {
        resourceSpawner.InsertQueue(gameObject, spawnIndex);
    }
}
