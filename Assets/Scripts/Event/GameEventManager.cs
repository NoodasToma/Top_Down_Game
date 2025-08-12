using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    public static event Action OnEnemyKilled;


    public static void EnemyKilled()
    {
        OnEnemyKilled?.Invoke();
    }

   
}
