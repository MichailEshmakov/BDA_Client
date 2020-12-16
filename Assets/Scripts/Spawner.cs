using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : Singleton<Spawner>
{

    [SerializeField] float width;
    [SerializeField] float spawnPeriod;
    [SerializeField] float spawnPeriodChangePerSpawn;
    [SerializeField] float minSpawnPeriod;
    [SerializeField] float badFallerChance;

    [SerializeField] Faller badFallerPrefab;
    [SerializeField] Faller goodFallerPrefab;

    [SerializeField] float fallerSpeed;
    [SerializeField] float fallerSpeedChangePerSpawn;
    [SerializeField] float maxFallerSpeed;

    private float timeUntilSpawn;

    protected override void Awake()
    {
        base.Awake();
        GameManager.OnStartPlay += OnStartPlay;
    }

    // Start is called before the first frame update
    void Start()
    {
        timeUntilSpawn = spawnPeriod;
        if (badFallerChance > 1 || badFallerChance < 0)
        {
            Debug.LogError("Вероятность выпадения плохого предмета должна быть от 0 до 1");
        }
    }

    private void OnStartPlay()
    {
        StartCoroutine(SpawnCoroutine()); 
    }

    private IEnumerator SpawnCoroutine()
    {
        while (GameManager.Instance.CurrentState == GameManager.gameStatus.play)
        {
            Spawn();
            yield return new WaitForSeconds(spawnPeriod);
            
        }
    }

    private void Spawn()
    {
        Faller newFaller = Instantiate(UnityEngine.Random.Range(0.0f, 1.0f) > badFallerChance ? goodFallerPrefab : badFallerPrefab,
            new Vector3(UnityEngine.Random.Range(-width, width), transform.position.y, transform.position.z),
            Quaternion.identity);
        newFaller.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -fallerSpeed);

        ChangeDifficulty();
    }

    private void ChangeDifficulty()
    {
        fallerSpeed = Mathf.Clamp(fallerSpeed * fallerSpeedChangePerSpawn, 0, maxFallerSpeed);
        spawnPeriod = Mathf.Clamp(spawnPeriod * spawnPeriodChangePerSpawn, minSpawnPeriod, float.MaxValue);
    }

    private void OnDisable()
    {
        GameManager.OnStartPlay -= OnStartPlay;
    }
}
