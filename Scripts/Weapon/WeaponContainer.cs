using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponContainer : Weapon
{
    public enum WeaponType
    {
        SetMoveSpeedAngle, SetSpeed, SetArcBullet,
    };

    [Header("Child Attributes")]
    [SerializeField] private WeaponType Type;
    [SerializeField] protected float IntervalBetweenBullets;

    private Transform Player;

    new void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
    }

    void TypeChoice()
    {
        switch (Type)
        {
            case WeaponType.SetMoveSpeedAngle:
                StartCoroutine(BasicShoot());
                break;

            case WeaponType.SetSpeed:
                StartCoroutine(ExpShoot());
                break;

            case WeaponType.SetArcBullet:
                StartCoroutine(ArcShoot());
                break;

            //case WeaponType.TargetPlayer:
            //    StartCoroutine(TargetPlayerShoot());
            //    break;

        }
    }

    public override IEnumerator Shoot()
    {
        TypeChoice();
        yield return null;
    }

    IEnumerator BasicShoot()
    {
        //yield return new WaitForSeconds(FireRate);
        if (RandomNumOfBullets)
            GetRandomNumOFBullets();

        for (int i = 0; i < NumOfBullets; i++)
        {
            for (int j = 0; j < firePos.Length; j++)
            {
                GameObject bul = OP.SpawnFromPool(bulletType, firePos[j].position, Quaternion.identity);
                bul.GetComponent<Bullet>().SetMoveDir(firePos[j].transform.localRotation.normalized * Vector3.right);
                bul.GetComponent<Bullet>().SetBulletAngle(firePos[j].rotation.eulerAngles.z);
                bul.GetComponent<Bullet>().SetSpeed(BulletSpeed);
                //bul.GetComponent<Transform>().localScale = V3;
            }
            yield return new WaitForSeconds(IntervalBetweenBullets);
        }
    }

    IEnumerator ExpShoot()
    {
        if (RandomNumOfBullets)
            GetRandomNumOFBullets();

        for (int i = 0; i < NumOfBullets; i++)
        {
            for (int j = 0; j < firePos.Length; j++)
            {
                GameObject bul = OP.SpawnFromPool(bulletType, firePos[j].position, Quaternion.identity);
                bul.GetComponent<Bullet>().SetSpeed(BulletSpeed);
                RandomBulletSpeeds();
            }
            yield return new WaitForSeconds(IntervalBetweenBullets);
        }
    }

    IEnumerator ArcShoot()
    {
        if (RandomNumOfBullets)
            GetRandomNumOFBullets();

        for (int i = 0; i < NumOfBullets; i++)
        {
            for (int j = 0; j < firePos.Length; j++)
            {
                OP.SpawnFromPool(bulletType, firePos[j].position, Quaternion.identity);
            }
            yield return new WaitForSeconds(IntervalBetweenBullets);
        }
    }

    //IEnumerator TargetPlayerShoot()
    //{
    //    if (RandomNumOfBullets)
    //        GetRandomNumOFBullets();

    //    for (int j = 0; j < firePos.Length; j++)
    //    {
    //        GameObject bul = OP.SpawnFromPool(bulletType, firePos[j].position, Quaternion.identity);
    //        bul.GetComponent<Bullet>().SetSpeed(BulletSpeed);
    //        RandomBulletSpeeds();
    //    }
    //    yield return new WaitForSeconds(IntervalBetweenBullets);
    //}

}
