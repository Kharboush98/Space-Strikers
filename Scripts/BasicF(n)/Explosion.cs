using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private Animator animator;
    [SerializeField] protected float LifeSpan = 1;
    private float LifeSpanTimer;
    protected bool StartCountdownTimer = false;


    private void Start()
    {
        animator = transform.GetComponent<Animator>();
        LifeSpanTimer = LifeSpan;

    }

    private void Update()
    {
        if(StartCountdownTimer) StartCountdown();
    }

    //--------------------------------------------------
    private void PlayNextAim(string Anim)
    {
        animator.SetTrigger(Anim);
    }

    private void DisableOnEnd()
    {
        gameObject.SetActive(false);
    }

    private void CountdownStart()
    {
        StartCountdownTimer = true;
    }

    private void Dest_GO()
    {
        Destroy(gameObject);
    }

    void StartCountdown()
    {
        if (LifeSpanTimer <= 0)
        {
            PlayNextAim("Explode");
        }
        else
        {
            LifeSpanTimer -= Time.deltaTime;
        }
    }

}
