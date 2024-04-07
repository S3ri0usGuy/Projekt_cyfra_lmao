using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerResources : MonoBehaviour
{
    [SerializeField] int baseHp;
    int hp;
    [SerializeField] TextMeshProUGUI HpText;

    AudioMenager AudioMenager;

    private void Awake()
    {
        hp = baseHp;
        HpText.text = "HP: " + hp + " / " + baseHp;
        AudioMenager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioMenager>();
    }

    public void TakeDamage(int damage)
    {
        hp-= damage;
        AudioMenager.PlaySFX(AudioMenager.damage);
        HpText.text = "HP: " + hp + " / " + baseHp;
        if (hp <= 0)
        {
            AudioMenager.PlaySFX(AudioMenager.death);
            Invoke(nameof(RestartGame), 1f);
        }
    }

    void RestartGame()
    {
        SceneManager.LoadScene(1);
    }
}