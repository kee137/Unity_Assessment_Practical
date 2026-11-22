using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject panel;
    bool paused;

    void Start() { panel.SetActive(false); }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused) Resume(); else Pause();
        }
    }

    void Pause()
    {
        paused = true;
        panel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume() // hook to Resume button OnClick
    {
        paused = false;
        panel.SetActive(false);
        Time.timeScale = 1f;
    }
}