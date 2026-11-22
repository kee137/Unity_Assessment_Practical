using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Text killText;
    int kills;

    void Awake()
    {
        Instance = this;
        kills = PlayerPrefs.GetInt("Kills", 0);
    }

    void Start() { UpdateUI(); }

    public void AddKill()
    {
        kills++;
        PlayerPrefs.SetInt("Kills", kills);
        UpdateUI();
    }

    void UpdateUI() { killText.text = "击杀：" + kills; }
}