using System.Collections;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject objectToSpawn;  // 생성할 오브젝트 프리팹
    private float spawnInterval = 1; // 생성 간격(초)

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnObject());
    }

    private IEnumerator SpawnObject()
    {
        while (true)
        {
            // TODO 오브젝트 풀링

            // 오브젝트 생성
            Instantiate(objectToSpawn, transform.position, transform.rotation);
            // 지정된 시간만큼 대기
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
