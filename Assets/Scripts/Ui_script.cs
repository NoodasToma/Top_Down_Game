using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ui_script : MonoBehaviour
{
     Slider healthBar;
    public Image fireballimg;
    public float fireballCDTime = 5f;
    public bool fireballOnCooldown = false;
    public KeyCode fireballKeyCode;




    void Start()
    {
        healthBar = gameObject.GetComponent<Slider>();
        float maxHp = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Movement>().playerHP;
        healthBar.maxValue = maxHp;
        setHpBar(maxHp);
        //fireball cooldown 
        fireballimg.fillAmount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        FireballCD();
    }

    public IEnumerator FireballCooldown() {
        if (fireballOnCooldown) yield break;
    fireballOnCooldown = true;
    fireballimg.fillAmount = 1; 
    float timer = fireballCDTime;

    while (timer > 0) {
        timer -= Time.deltaTime;
        fireballimg.fillAmount = timer / fireballCDTime; 
        yield return null;
    }

    fireballimg.fillAmount = 1; 
    fireballOnCooldown = false;
}

void FireballCD() {
    if (Input.GetKey(fireballKeyCode) && !fireballOnCooldown) {
        StartCoroutine(FireballCooldown());
    }
}

    public void setHpBar(float val)
    {
        healthBar.value = val;
    }
    

}
