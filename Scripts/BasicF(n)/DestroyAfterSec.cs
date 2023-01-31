using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSec : MonoBehaviour
{
    [SerializeField] private float LifeSpan = 1f;
    private float LifeSpanTimer;

    // Start is called before the first frame update
    void Start()
    {
        LifeSpanTimer = LifeSpan;
    }

    // Update is called once per frame
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
