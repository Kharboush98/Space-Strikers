using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//requires StaticWeapon to shoot
public class ExplosiveBullet : Bullet
{
    public enum ExplosiveBulletType
    {
        RandomTarget, TargetPlayer, StaticMine, Obstacle, Follow
    };

    [Header("Child Attributes")]
    [SerializeField] private ExplosiveBulletType BulletType;
    [SerializeField] private GameObject Explosion;

    [Header("Follow Specific Var")]
    [SerializeField] private bool HasLifeSpan = true;
    [SerializeField] private float LifeSpan = 1.5f;

    private GameObject Points;
    private bool SpawnedExplosion = false;

    public override void OnObjectSpawn()
    {
        base.OnObjectSpawn();
        Points = GameObject.FindGameObjectWithTag("GridPoints");
        SpawnedExplosion = false;
        LifeSpanTimer = LifeSpan;
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
            case ExplosiveBulletType.RandomTarget:
                RandomTargetMov();
                break;

            case ExplosiveBulletType.TargetPlayer:
                TargetPlayerMov();
                break;

            case ExplosiveBulletType.StaticMine:
                StaticMinemov();
                break;

            case ExplosiveBulletType.Obstacle:
                Obstaclemov();
                break;

            case ExplosiveBulletType.Follow:
                LookAtPlayer = true;
                FollowMov();
                break;
        }
    }

    void GetMoveSpot()
    {
        float x, y;
        float a, b;

        x = Points.GetComponent<PointsContainers>().XaxisGrid[0].transform.position.x;
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

    void TargetPlayerMov()
    {
        transform.position = Vector2.MoveTowards(transform.position, PlayerTargetPos, Speed * Time.deltaTime);

        if (transform.position.x == PlayerTargetPos.x && transform.position.y == PlayerTargetPos.y)
        {
            SpawnExp();
        }
    }

    void StaticMinemov()
    {
        //static actions
        SpawnExp();
    }

    void Obstaclemov()
    {
        transform.position = Vector2.MoveTowards(transform.position, MoveSpot, Speed * Time.deltaTime);

        if (transform.position.x == MoveSpot.x && transform.position.y == MoveSpot.y)
        {
            if (HasLifeSpan)
            {
                if (LifeSpanTimer <= 0)
                {
                    gameObject.SetActive(false);
                }
                else
                {
                    Rotate = true;
                    ProjBlocker = true;
                    LifeSpanTimer -= Time.deltaTime;
                }
            }
            else
            {
                Rotate = true;
                isDestructible = true;
            }
        }
    }

    void FollowMov()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, Speed * Time.deltaTime);

        if (LifeSpanTimer <= 0)
        {
            //instantiate particle exp
            //SpawnExp();
            if (!SpawnedExplosion && HitEffect != null)
            {
                Instantiate(HitEffect, transform.position, Quaternion.identity);
                SpawnedExplosion = true;
            }
            gameObject.SetActive(false);
        }
        else
        {
            LifeSpanTimer -= Time.deltaTime;
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

}