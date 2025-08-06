using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class Ui_script : MonoBehaviour
{

    public Animator playerAnimator;
    Slider healthBar;
    //cooldown for fireball skill
    public Image skillImg;
    public bool skillOnCooldown = false;
    public KeyCode fireballKeyCode;
    public TextMeshProUGUI scoreText; //kill counter
    private int killCount = 0;
    //Game Over Screen
    public GameObject gameOverUI;
    public SpawnEnemies spawnScript;
    //Pause Menu Screen
    public GameObject pauseMenuUI;
    public bool isPaused;

    public GameObject HotBar;




    void Start()
    {
        healthBar = gameObject.GetComponent<Slider>();
        float maxHP = GameObject.FindGameObjectWithTag("Player").GetComponent<StatsManager>().maxHP;
        scoreText = GameObject.FindGameObjectWithTag("killCounter").GetComponent<TextMeshProUGUI>();
        Debug.Log(scoreText);

        healthBar.maxValue = maxHP;
        setHpBar(maxHP);
        //fireball cooldown 
        skillImg.fillAmount = 1;


        scoreText.text = "Kills: 0";
        gameOverUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        isPaused = false;

    }
    public  void SetSkillIcon(Sprite icon)
    {
        skillImg.sprite = icon;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && gameOverUI.activeSelf) restart();
        if (Input.GetKeyDown(KeyCode.Escape) && !gameOverUI.activeSelf)
        {
            if (!isPaused)
                pause();
            else
                resume();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape pressed");
            Debug.Log("gameOverUI active: " + gameOverUI.activeSelf);
            Debug.Log("pauseMenuUI active: " + pauseMenuUI.activeSelf);
        }
    }

    public IEnumerator SkillCooldown(float skillCD)
    {
        if (skillOnCooldown) yield break;
        skillOnCooldown = true;
        skillImg.fillAmount = 1;
        float timer = skillCD;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            skillImg.fillAmount = timer / skillCD;
            yield return null;
        }

        skillImg.fillAmount = 1;
        skillOnCooldown = false;
    } 
    public void SkillCD(float skillCD)
    {
        StartCoroutine(SkillCooldown(skillCD));
    }

    public void setHpBar(float val)
    {
        healthBar.value = val;
    }
    //kill count
    public void AddKill()
    {
        killCount++;
        Debug.Log("Kill added: " + killCount);
        scoreText.text = "Kills: " + killCount;
    }


    public void gameOver()
    {
        gameOverUI.SetActive(true);
        spawnScript.stopSpawning = true;
    }

    public void restart()
    {
        Debug.Log("Button clicked!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        if (isPaused) { Time.timeScale = 1f; }
    }
    public void pause()
    {
        pauseMenuUI.SetActive(true);
        isPaused = true;
        Time.timeScale = 0f;
    }
    public void resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void UseConsumable(ConsumableSO item)
{
    var player = GameObject.FindGameObjectWithTag("Player");
    if (player != null)
    {
        var handler = player.GetComponent<ConsumableHandler>();
        if (handler != null)
        {
            handler.Consume(item);
        }
    }
}


}
