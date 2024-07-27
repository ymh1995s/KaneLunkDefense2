using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    private GameObject objectToSpawn;  // ������ ������Ʈ ������
    private float spawnInterval = 1; // ���� ����(��)
    private float changeInterval = 3f; // ������ ���� ����(��) 2�о� 5���ؼ� �ϴ� 10��
    private int currentPrefabIndex = 0;
    private string[] prefabNames = { "Monster/LV1", "Monster/LV2", "Monster/LV3", "Monster/LV4", "Monster/LV5"};


    void Start()
    {
        objectToSpawn = Resources.Load<GameObject>(prefabNames[currentPrefabIndex]);
        StartCoroutine(SpawnObject());
        StartCoroutine(ChangePrefabPeriodically());
    }

    private IEnumerator SpawnObject()
    {
        while (true)
        {
            // TODO ������Ʈ Ǯ��
            GameObject bossObject = Instantiate(objectToSpawn, transform.position, transform.rotation);
            //Instantiate(objectToSpawn, transform.position, transform.rotation);
            //Instantiate(objectToSpawn, transform.position, transform.rotation);
            //Instantiate(objectToSpawn, transform.position, transform.rotation);

            yield return new WaitForSeconds(spawnInterval);
        }
    }
    private IEnumerator ChangePrefabPeriodically()
    {
        while (true)
        {
            // changeInterval �� ���� ���
            yield return new WaitForSeconds(changeInterval);

            // ���� ������ �ε����� �������� ����
            currentPrefabIndex = (currentPrefabIndex + 1) % prefabNames.Length;
            // ���ο� ������ �ε�
            objectToSpawn = Resources.Load<GameObject>(prefabNames[currentPrefabIndex]);

            // �������� �ε���� �ʾ��� ��� ���� �α� ���
            if (objectToSpawn == null)
            {
                Debug.LogError("�������� ã�� �� �����ϴ�: " + prefabNames[currentPrefabIndex]);
            }
        }
    }
}
