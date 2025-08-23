using System.Collections;
using System.Collections.Generic;
using Combat;
using UnityEngine;

  [CreateAssetMenu(menuName = "Combat/Combo")]
    public  class Combo : ScriptableObject
    {



        [SerializeField] float cooldownBetweenAttacks;
        [SerializeField] float cooldownBetweenCombos;
        [SerializeField] float comboResetTime;

        [SerializeField] int comboLimit;
        [SerializeField] Attack[] attacks; 

        public float GetCooldownBetweenCombos => cooldownBetweenCombos;
        public float GetComboResetTime => comboResetTime;
        public int GetComboLimit => comboLimit;
        public Attack[] GetAttacks => attacks;
        public float GetCooldownBetweenAttacks => cooldownBetweenAttacks;
        public void startCombo(GameObject attacker, int comboIndex)
  {
    if (comboIndex <= comboLimit) attacks[comboIndex].Execute(attacker);
  }

    }
