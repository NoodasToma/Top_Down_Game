using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ui_script : MonoBehaviour
{
     Slider healthBar;

    void Start()
    {
        healthBar = gameObject.GetComponent<Slider>();
        float maxHp = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Movement>().playerHP;
        healthBar.maxValue = maxHp;
        setHpBar(maxHp);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setHpBar(float val)
    {
        healthBar.value = val;
    }
    

}
