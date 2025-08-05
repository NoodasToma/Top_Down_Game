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
    public Image fireballimg;
    public float fireballCDTime = 5f;
    public bool fireballOnCooldown = false;
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
        fireballimg.fillAmount = 1;
        scoreText.text = "Kills: 0";
        gameOverUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        isPaused = false;

    }

    // Update is called once per frame
    void Update()
    {
        FireballCD();
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

    public IEnumerator FireballCooldown()
    {
        if (fireballOnCooldown) yield break;
        fireballOnCooldown = true;
        fireballimg.fillAmount = 1;
        float timer = fireballCDTime;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            fireballimg.fillAmount = timer / fireballCDTime;
            yield return null;
        }

        fireballimg.fillAmount = 1;
        fireballOnCooldown = false;
    }

    void FireballCD()
    {
        if (Input.GetKeyUp(fireballKeyCode) && !fireballOnCooldown)
        {
            StartCoroutine(FireballCooldown());
        }
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
