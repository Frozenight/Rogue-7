using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField] private Transform enemiesHolderPlatform;
    [SerializeField] private Transform roomPlatform;
    [SerializeField] private GameObject[] roomEnemies;

    private Coroutine colorReadCoroutine;
    private bool roomIsActive = false;

    [Range(0f, 2f)]
    [SerializeField] private float enemiesColorReadFreq;

    private IEnumerator ReadColorsPeriodically()
    {
        while (true)
        {
            foreach (GameObject enemy in roomEnemies)
            {
                ColorReader colorReader = enemy.GetComponent<ColorReader>();
                if (colorReader != null)
                {
                    StartCoroutine(colorReader.GetColorUnderEnemy());
                    yield return new WaitForSeconds(enemiesColorReadFreq);
                }
            }
        }
    }

    private void OnDestroy()
    {
        if (colorReadCoroutine != null)
        {
            StopCoroutine(colorReadCoroutine);
            colorReadCoroutine = null;
        }
    }

    private IEnumerator RepositionAndActivateEnemies()
    {
        foreach (GameObject enemy in roomEnemies)
        {
            enemy.transform.position = roomPlatform.position;
            enemy.GetComponent<AIMovement>().enabled = true;
            enemy.GetComponent<ColorReader>().enabled = true;
        }
        yield return new WaitForSeconds(1f); // delay before starting movement

        foreach (GameObject enemy in roomEnemies)
        {
            enemy.GetComponent<AIMovement>()?.StartPhasing();
        }
    }

    private void DeactivateAndRepositionEnemies()
    {
        foreach (GameObject enemy in roomEnemies)
        {
            enemy.GetComponent<AIMovement>().StopPhasing();
            enemy.GetComponent<AIMovement>().enabled = false;
            enemy.GetComponent<ColorReader>().enabled = false;
            enemy.transform.position = enemiesHolderPlatform.position;
        }
    }

    public void StartAIPhasing()
    {
        if (roomIsActive)
            return;
        roomIsActive = true;
        StartCoroutine(RepositionAndActivateEnemies());
        colorReadCoroutine = StartCoroutine(ReadColorsPeriodically());
    }

    public void StopAIPhasing()
    {
        if (!roomIsActive)
            return;
        roomIsActive = false;
        DeactivateAndRepositionEnemies();
        if (colorReadCoroutine != null)
        {
            StopCoroutine(colorReadCoroutine);
            colorReadCoroutine = null;
        }
    }
}
