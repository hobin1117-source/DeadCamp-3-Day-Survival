using UnityEngine;

public class Resource : MonoBehaviour
{
    public ItemData itemToGive;
    public int quantityPerHit = 1;
    public int capacity;

    private ResourceSpawner resourceSpawner;

    private void Start()
    {
        resourceSpawner = Managers.Resource;
    }

    public void Gathering(Vector3 hitPoint, Vector3 hitNormal)
    {
        for (int i = 0; i < quantityPerHit; i++)
        {
            if (capacity <= 0)
            {
                Release();
                break;
            }
            capacity -= 1;
            Instantiate(itemToGive.dropPrefabs, hitPoint + Vector3.up, Quaternion.LookRotation(hitNormal, Vector3.up));

        }
    }

    private void Release()
    {
        resourceSpawner.InsertQueue(gameObject);
    }
}
