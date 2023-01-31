using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfterSec : MonoBehaviour , IPooledObjects
{
    [SerializeField] private float LifeSpan = 1f;
    private float LifeSpanTimer;

    public virtual void OnObjectSpawn()
    {
        LifeSpanTimer = LifeSpan;
    }

    void Update()
    {
        Countdown();
    }

    void Countdown()
    {
        if (LifeSpanTimer <= 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            LifeSpanTimer -= Time.deltaTime;
        }
    }

}
