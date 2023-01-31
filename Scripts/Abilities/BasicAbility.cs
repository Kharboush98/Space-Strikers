using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAbility : Ability
{
    public enum WeaponType
    {
        Basic, Exp,
    };

    [Header("Child Attributes")]
    [SerializeField] protected WeaponType Type;
    [SerializeField] protected string BulletName;
    [SerializeField] protected float BulletSpeed;
    [SerializeField] protected int NumOfBullets;
    [SerializeField] protected float IntervalBetweenBullets = 0.1f;
    //[SerializeField] private float bulletSize;


    new void Start()
    {
        base.Start();
    }

    public override void ActivateAbility()
    {
        if(Type == WeaponType.Basic)
        {
            StartCoroutine(SetAll());
        }else if (Type == WeaponType.Exp)
        {
            StartCoroutine(ExpShoot());
        }
    }

    IEnumerator SetAll()
    {
        for (int i = 0; i < NumOfBullets; i++)
        {
            for (int j = 0; j < firePos.Length; j++)
            {
                GameObject bul = OP.SpawnFromPool(BulletName, firePos[j].position, Quaternion.identity);
                bul.GetComponent<Bullet>().SetMoveDir(firePos[j].transform.localRotation.normalized * Vector2.right);
                bul.GetComponent<Bullet>().SetSpeed(BulletSpeed);
            }
            yield return new WaitForSeconds(IntervalBetweenBullets);
        }
    }

    IEnumerator ExpShoot()
    {
        for (int i = 0; i < NumOfBullets; i++)
        {
            for (int j = 0; j < firePos.Length; j++)
            {
                GameObject bul = OP.SpawnFromPool(BulletName, firePos[j].position, Quaternion.identity);
                bul.GetComponent<Bullet>().SetSpeed(BulletSpeed);
            }
            yield return new WaitForSeconds(IntervalBetweenBullets);
        }
    }

}
