using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faller : MonoBehaviour
{
    [SerializeField] bool isGood;

    private void Awake()
    {
        GameManager.OnGameover += OnGameover;
    }

    private void OnMouseDown()
    {
        if (isGood)
        {
            GameManager.Instance.AddScore();
        }
        else
        {
            GameManager.Instance.MinusLife();
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Floor") && isGood)
        {
            GameManager.Instance.MinusLife();
        }
    }

    private void OnGameover()
    {
        Destroy(gameObject);
    }

    private void OnDisable()
    {
        GameManager.OnGameover -= OnGameover;
    }
}
