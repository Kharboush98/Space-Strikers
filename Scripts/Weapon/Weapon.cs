using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected string bulletType;
    [Space]

    public float FireRate;
    [HideInInspector] public float NextFire;
    [SerializeField] private bool FixedFireRate = true;
    [SerializeField] private float MinFireRate;
    [Space]

    [SerializeField] protected Transform[] firePos;
    [Header("Bullet Manager Attributes")]
    [SerializeField] protected int NumOfBullets;
    [Space]
    [SerializeField] protected bool RandomNumOfBullets = false;
    [SerializeField] protected int MinBullets , MaxBullets;
    [Space]

    [Header("Bullet Speed Attributes")]
    [SerializeField] protected float BulletSpeed;
    [SerializeField] public bool FixedbulletSpeed = true;
    [SerializeField] protected float MinSpeed, MaxSpeed;
    [Space]

    //[Header("Bullet Size Attributes")]
    //[SerializeField] private float bulletSize;
    //protected Vector3 V3;


    [HideInInspector] public bool isFiring = false;
    protected ObjectPooler OP;

    protected void Start()
    {
        OP = ObjectPooler.Instance;
        //V3 = new Vector3(bulletSize, bulletSize, 1);
        SetFireRate();
        RandomBulletSpeeds();
    }

    protected void RandomBulletSpeeds()
    {
        if (!FixedbulletSpeed)
        {
            BulletSpeed = Random.Range(MinSpeed , MaxSpeed);
        }
    }

    private void SetFireRate()
    {
        if (FixedFireRate)
        {
            NextFire = FireRate;
        }
        else
        {
            NextFire = Random.Range(MinFireRate, FireRate);
        }
    }

    protected void GetRandomNumOFBullets()
    {
        NumOfBullets = Random.Range(MinBullets , MaxBullets);
    } 

    public abstract IEnumerator Shoot();
}
