using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBullet : Bullet
{
    public enum BasicBulletType
    {
        Basic, Static
    };
    public BasicBulletType BulletType;

    public override void OnObjectSpawn()
    {
        base.OnObjectSpawn();
    }

    new void Update()
    {
        base.Update();
    }

    public override void Movement()
    {
        SwitchMov();
    }

    void SwitchMov()
    {
        switch (BulletType)
        {
            case BasicBulletType.Basic:
                Basic();
                break;

            case BasicBulletType.Static:
                Speed = 0;
                break;
        }
    }

    void Basic()
    {
        transform.Translate(Direction * Time.deltaTime * Speed, Space.World);
    }

    //protected override void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("PlayerProjectile") && isDestructible)
    //    {
    //        collision.gameObject.GetComponent<Bullet>().TakeDamage(1);
    //        TakeDamage(1);
    //    }
    //}
}
