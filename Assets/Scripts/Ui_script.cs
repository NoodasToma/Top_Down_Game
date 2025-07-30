using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Ui_script : MonoBehaviour
{
    Slider healthBar;
    //cooldown for fireball skill
    public Image fireballimg;
    public float fireballCDTime = 5f;
    public bool fireballOnCooldown = false;
    public KeyCode fireballKeyCode;
    public Text scoreText; //kill counter
    private int killCount = 0;
    //Game Over Screen
    public GameObject gameOverUI;
    public SpawnEnemies spawnScript;




    void Start()
    {
        healthBar = gameObject.GetComponent<Slider>();
        float maxHp = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Movement>().playerHP;
        healthBar.maxValue = maxHp;
        setHpBar(maxHp);
        //fireball cooldown 
        fireballimg.fillAmount = 1;
        scoreText.text = "Kills: 0";
        gameOverUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        FireballCD();
        if (Input.GetKeyDown(KeyCode.R) && gameOverUI)  restart();
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
        if (Input.GetKey(fireballKeyCode) && !fireballOnCooldown)
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
    }

}
