using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalWeapon : Weapon
{
    [Header("Child Attributes")]
    [SerializeField] protected float IntervalBetweenBullets;

    new void Start()
    {
        base.Start();
    }

    //after FireRate secs it will fire NumOfBullets with intervalofBullets between them
    public override IEnumerator Shoot()
    {
        //yield return new WaitForSeconds(FireRate);
        if(RandomNumOfBullets)
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

}
