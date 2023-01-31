using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    public enum PlayerBulletType
    {
        Basic , RandomTarget, Unstoppable
    };
    public PlayerBulletType BulletType;

    [Header("RandomExp")]
    [SerializeField] private GameObject Explosion;
    private bool SpawnedExplosion = false;
    private GameObject Points;

    public override void OnObjectSpawn()
    {
        base.OnObjectSpawn();
        Points = GameObject.FindGameObjectWithTag("GridPoints");
        SpawnedExplosion = false;
        GetMoveSpot();
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
            case PlayerBulletType.Basic:
                Basic();
                break;

            case PlayerBulletType.RandomTarget:
                RandomTargetMov();
                break;

            case PlayerBulletType.Unstoppable:
                Basic();
                break;
        }
    }

    public override void TakeDamage(int Damage)
    {
        CurrentHealth -= Damage;
    }

    void Basic()
    {
        transform.Translate(Direction * Time.deltaTime * Speed, Space.World);
    }

    void GetMoveSpot()
    {
        float x, y;
        float a, b;

        x = Points.GetComponent<PointsContainers>().XaxisGrid[14].transform.position.x;
        y = Points.GetComponent<PointsContainers>().XaxisGrid[9].transform.position.x;

        a = Points.GetComponent<PointsContainers>().YaxisGrid[0].transform.position.y;
        b = Points.GetComponent<PointsContainers>().YaxisGrid[6].transform.position.y;

        MoveSpot = new Vector2(Random.Range(x, y), Random.Range(a, b));
    }

    void RandomTargetMov()
    {
        transform.position = Vector2.MoveTowards(transform.position, MoveSpot, Speed * Time.deltaTime);

        if (transform.position.x == MoveSpot.x && transform.position.y == MoveSpot.y)
        {
            SpawnExp();
        }
    }

    void SpawnExp()
    {
        if (!SpawnedExplosion)
        {
            if (Explosion != null)
            {
                Instantiate(Explosion, transform.position, Quaternion.identity);
                SpawnedExplosion = true;
            }
            gameObject.SetActive(false);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (BulletType == PlayerBulletType.Basic)
        {
            if (collision.CompareTag("StageCollider"))
            {
                Instantiate(HitEffect, transform.position, Quaternion.identity);
                AudioManager.instance.Play("BulletHit");
                gameObject.SetActive(false);
            }
            else if (collision.CompareTag("EnemyProjectile")
                && collision.gameObject.GetComponent<Bullet>().DestructStatus())
            {
                AudioManager.instance.Play("Hit");
                collision.gameObject.GetComponent<Bullet>().TakeDamage(1);
                TakeDamage(1);
            }
            else if (collision.CompareTag("EnemyProjectile")
                && collision.gameObject.GetComponent<Bullet>().ProjBlockerStatus())
            {
                TakeDamage(1);
                AudioManager.instance.Play("BulletHit");
                Instantiate(HitEffect, transform.position, Quaternion.identity);
            }
            else if (collision.CompareTag("Enemy"))
            {
                TakeDamage(1);
                collision.gameObject.GetComponent<EnemyBase>().TakeDamage(1);
            }
        }
        else if (BulletType == PlayerBulletType.RandomTarget)
        {
            if (collision.CompareTag("StageCollider"))
            {
                Instantiate(HitEffect, transform.position, Quaternion.identity);
                AudioManager.instance.Play("BulletHit");
                gameObject.SetActive(false);
            }
        }
        else if (BulletType == PlayerBulletType.Unstoppable)
        {
            if (collision.CompareTag("StageCollider"))
            {
                //Instantiate(HitEffect, transform.position, Quaternion.identity);
                //AudioManager.instance.Play("BulletHit");
                gameObject.SetActive(false);
            }
            else if (collision.CompareTag("EnemyProjectile"))
            {
                AudioManager.instance.Play("Hit");
                Instantiate(HitEffect, transform.position, Quaternion.identity);
                collision.gameObject.SetActive(false);
            }
            else if (collision.CompareTag("Enemy"))
            {
                collision.gameObject.GetComponent<EnemyBase>().TakeDamage(10);
            }
        }
    }

}
