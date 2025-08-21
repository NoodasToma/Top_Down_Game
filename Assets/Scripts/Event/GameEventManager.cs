using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    public static event Action OnEnemyKilled;

    private static Coroutine frameFreezer;

    private static GameEventManager gameEventManager;


    void Start()
    {
        gameEventManager = this;
    }


    public static void EnemyKilled()
    {
        OnEnemyKilled?.Invoke();
    }

    public static void CameraShake(float duration, float magnitude)
    {
        GameObject cameraHolder = GameObject.FindGameObjectWithTag("CameraHolder");
        Vector3 orignalPos = cameraHolder.transform.localPosition;
        gameEventManager.StartCoroutine(gameEventManager.screenShaker(duration, magnitude, cameraHolder, orignalPos));
    }

    IEnumerator screenShaker(float duration, float magnitude, GameObject cameraholder, Vector3 originalPos)
    {
        float timePassed = 0f;

        while (timePassed < duration)
        {

            float x = UnityEngine.Random.Range(-1f, 1f) * magnitude;
            float y = UnityEngine.Random.Range(-1f, 1f) * magnitude;

            cameraholder.transform.localPosition = originalPos + new Vector3(x, y, 0f);

            timePassed += Time.deltaTime;

            yield return null;
        }

        cameraholder.transform.localPosition = originalPos;
    }

    public static void freezeFrame(float duration)
    {
        if(frameFreezer==null) frameFreezer = gameEventManager.StartCoroutine(gameEventManager.frameFreeze(duration));
    }

     IEnumerator frameFreeze(float duration)
    {
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(duration);
        Time.timeScale = 1f;
        frameFreezer = null;
    }

   
}
