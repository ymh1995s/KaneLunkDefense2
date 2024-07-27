using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    private GameObject objectToSpawn;  // 생성할 오브젝트 프리팹
    private float spawnInterval = 1; // 생성 간격(초)
    private float changeInterval = 3f; // 프리팹 변경 간격(초) 2분씩 5개해서 일단 10개
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
            // TODO 오브젝트 풀링
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
            // changeInterval 초 동안 대기
            yield return new WaitForSeconds(changeInterval);

            // 현재 프리팹 인덱스를 다음으로 변경
            currentPrefabIndex = (currentPrefabIndex + 1) % prefabNames.Length;
            // 새로운 프리팹 로드
            objectToSpawn = Resources.Load<GameObject>(prefabNames[currentPrefabIndex]);

            // 프리팹이 로드되지 않았을 경우 에러 로그 출력
            if (objectToSpawn == null)
            {
                Debug.LogError("프리팹을 찾을 수 없습니다: " + prefabNames[currentPrefabIndex]);
            }
        }
    }
}
