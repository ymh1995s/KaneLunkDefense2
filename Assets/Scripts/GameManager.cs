using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private float startTime; // 씬이 시작된 시간

    public BasePlayer player;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        FindPlayer();
    }


    void Start()
    {
        startTime = Time.time; // 씬 시작 시점을 기록
        Gatcha.GameStart();
    }

    void Update()
    {
        UpdateTimer(); // 매 프레임 시간 업데이트
    }

    private void FindPlayer()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.GetComponent<BasePlayer>();
        }
        else
        {
            Debug.LogError("Player object not found!");
        }
    }

    public void DebugBtnTimePlus()
    {
        Time.timeScale += 0.5f;
    }

    public void DebugBtnTimeMinus()
    {
        Time.timeScale -= 0.5f;
    }

    void UpdateTimer()
    {
        float elapsedTime = Time.time - startTime; // 경과 시간 계산
    }
}
