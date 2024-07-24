using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
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
