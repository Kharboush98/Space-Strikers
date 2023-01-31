using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour , IPooledObjects , IDamagable
{
    [Header("Main Attributes")]
    [SerializeField] private int MaxHealth = 1;
    protected int CurrentHealth;
    protected float Speed;
    public int Damage = 1;
    [SerializeField] protected bool isDestructible = true;
    [SerializeField] protected bool LookAtPlayer = false;
    protected bool ProjBlocker = false;
    //[SerializeField] protected bool HasAnimator = false;
    protected GameObject BulletSprite;


    [Space]
    [Header("Rotation Attributes")]
    [SerializeField] protected  bool Rotate = false;
    [SerializeField] protected float RotateDegree;

    [Space]
    [Header("Hit Effects")]
    [SerializeField] protected GameObject HitEffect;

    //[Header("Life Span")]
    //[SerializeField] protected bool HasALifeSpan = false;
    //[SerializeField] protected float LifeSpan = 1;
    //protected bool LifeSpanEnded = false;

    protected ObjectPooler OP;
    protected Animator animator;
    protected float LifeSpanTimer;
    protected Vector2 PlayerTargetPos;
    protected Transform player;
    protected Vector3 Direction;
    protected Vector2 MoveSpot;


    //protected SpriteRenderer SR;

    public virtual void OnObjectSpawn()
    {
        OP = ObjectPooler.Instance;

        animator = GetComponent<Animator>();
        
        player = GameObject.FindGameObjectWithTag("Player").transform;
        PlayerTargetPos = new Vector2(player.position.x, player.position.y);
        
        CurrentHealth = MaxHealth;
        //LifeSpanTimer = LifeSpan;
        BulletSprite = gameObject.transform.GetChild(0).gameObject;
    }

    protected void Update()
    {
        isAlive();
        Bound();
        if (Rotate) Rotation();
        if (LookAtPlayer) LookatTarget(player);

        //StartTimeSpanCountdown();
    }

    protected void FixedUpdate()
    {
        Movement();
    }

    public abstract void Movement();

    public virtual void TakeDamage(int Damage)
    {
        CurrentHealth -= Damage;
        if (HitEffect != null)
        {
            Instantiate(HitEffect, transform.position, Quaternion.identity);
        }
    }

    void isAlive()
    {
        if (isDestructible && CurrentHealth <= 0)
        {
            // Particle Effects and such here
            gameObject.SetActive(false);
        }
    }

    public void SetMoveDir(Vector3 dir)
    {
        Direction = dir;    
    }

    public void SetBulletAngle(float Dir)
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Dir));
    }

    public void SetSpeed(float speed)
    {
        Speed = speed;
    }

    public void SetMoveSpot(Vector2 moveSpot)
    {
        MoveSpot = moveSpot;
    }

    void Rotation()
    {
        transform.Rotate(0, 0, RotateDegree * Time.deltaTime, Space.Self);
    }

    public bool DestructStatus()
    {
        return isDestructible;
    }
    public bool ProjBlockerStatus()
    {
        return ProjBlocker;
    }

    public void LookatTarget(Transform target)
    {
        Vector3 dir = target.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.AngleAxis(angle - 180, Vector3.forward);
        BulletSprite.transform.rotation = Quaternion.AngleAxis(angle - 180, Vector3.forward);
    }

    //public void StartTimeSpanCountdown()
    //{
    //    if (HasALifeSpan)
    //    {
    //        if (LifeSpanTimer <= 0)
    //        {
    //            //gameObject.SetActive(false);
    //            LifeSpanEnded = true;
    //        }
    //        else
    //        {
    //            LifeSpanTimer -= Time.deltaTime;
    //        }
    //    }
    //}

    protected virtual void Bound()
    {
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        max.x -= 2;
        if (transform.position.x < max.x)
        {
            gameObject.SetActive(false);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("StageCollider"))
        {
            AudioManager.instance.Play("BulletHit");
            Instantiate(HitEffect, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
        }
    }

}
