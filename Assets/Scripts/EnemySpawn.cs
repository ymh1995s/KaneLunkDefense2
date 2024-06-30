using System.Collections;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject objectToSpawn;  // ������ ������Ʈ ������
    private float spawnInterval = 1; // ���� ����(��)

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnObject());
    }

    private IEnumerator SpawnObject()
    {
        while (true)
        {
            // TODO ������Ʈ Ǯ��

            // ������Ʈ ����
            Instantiate(objectToSpawn, transform.position, transform.rotation);
            // ������ �ð���ŭ ���
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
