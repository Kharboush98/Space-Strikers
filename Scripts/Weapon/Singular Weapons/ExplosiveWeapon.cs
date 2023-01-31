using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveWeapon : Weapon
{
    [Header("Child Attributes")]
    [SerializeField] protected float IntervalBetweenBullets;

    new void Start()
    {
        base.Start();
    }

    public override IEnumerator Shoot()
    {
        if (RandomNumOfBullets)
            GetRandomNumOFBullets();

        for (int i = 0; i < NumOfBullets; i++)
        {
            for (int j = 0; j < firePos.Length; j++)
            {
                GameObject bul = OP.SpawnFromPool(bulletType, firePos[j].position, Quaternion.identity);
                bul.GetComponent<Bullet>().SetMoveDir(firePos[j].transform.localRotation.normalized * Vector3.right);
                bul.GetComponent<Bullet>().SetSpeed(BulletSpeed);
                //BulletsSpeed();
            }
            yield return new WaitForSeconds(IntervalBetweenBullets);
        }
    }

}
