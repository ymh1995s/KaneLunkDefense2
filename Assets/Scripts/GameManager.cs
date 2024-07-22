using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Gatcha.GameStart();
    }

    public void DebugBtnTimePlus()
    {
        Time.timeScale += 0.5f;
    }

    public void DebugBtnTimeMinus()
    {
        Time.timeScale -= 0.5f;
    }
}
