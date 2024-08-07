using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    private GameObject objectToSpawn;  // ������ ������Ʈ ������
    private float spawnInterval = 5f; // ���� ����(��)
    private float changeInterval = 60f; // ������ ���� ����(��) 2�о� 5���ؼ� �ϴ� 10��
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

            // X * Y ���� ���·� ������Ʈ ����
            for (int x = 0; x < 2; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    // �� ������Ʈ�� ��ġ�� ����
                    Vector3 position = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z);
                    Instantiate(objectToSpawn, position, transform.rotation);
                }
            }


            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private IEnumerator ChangePrefabPeriodically()
    {
        while (true)
        {
            // changeInterval �� ���� ���
            yield return new WaitForSeconds(changeInterval);

            // ���� ���������� 5�ʾ� �߰�
            changeInterval += 5;

            // ���� ������ �ε����� �������� ����
            currentPrefabIndex = (currentPrefabIndex + 1) % prefabNames.Length;
            // ���ο� ������ �ε�
            objectToSpawn = Resources.Load<GameObject>(prefabNames[currentPrefabIndex]);

            // �������� �ε���� �ʾ��� ��� ���� �α� ���
            if (objectToSpawn == null)
            {
                Debug.LogError("�������� ã�� �� �����ϴ�: " + prefabNames[currentPrefabIndex]);
            }

            // �迭�� ������ �ε����� �����ϸ� �ڷ�ƾ ����
            if (currentPrefabIndex == prefabNames.Length - 1)
            {
                Debug.Log("�迭�� ������ �����տ� �����߽��ϴ�. �ڷ�ƾ�� �����մϴ�.");
                yield break;
            }
        }
    }
}
