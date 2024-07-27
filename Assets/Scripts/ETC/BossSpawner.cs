using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    private string[] prefabNames = { "Monster/BOSS"};

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject objectToSpawn = Resources.Load<GameObject>(prefabNames[0]);
        Instantiate(objectToSpawn, transform.position, transform.rotation);
    }
}
