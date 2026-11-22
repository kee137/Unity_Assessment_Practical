using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float maxHp = 100f;
    public Slider healthBar;
    float hp;

    void Start()
    {
        hp = maxHp;
        healthBar.maxValue = maxHp;
        healthBar.value = hp;
    }

    public void TakeDamage(float dmg)
    {
        hp = Mathf.Max(0, hp - dmg);
        healthBar.value = hp;
        if (hp <= 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}