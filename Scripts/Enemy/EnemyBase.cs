using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour , IDamagable
{
    [Header("Hit Effects")]
    [SerializeField] private GameObject HitEffect;
    [SerializeField] protected int PointOnDeath = 100;

    [Header("Main Attributes")]
    [SerializeField]private int MaxHealth;
    protected int CurrentHealth;
    
    [SerializeField] protected bool LookAtPlayer = false;
    protected GameObject EnemySprite;


    [Space]
    [Tooltip("Speed of Enemy if FixedSpeed = T , else acts as MinSpeed")]
    [SerializeField] protected float Speed;
    [SerializeField] protected bool FixedSpeed = true;
    [SerializeField] float MaxSpeed;
    
    protected bool  CanShoot = false;
    private bool Dead = false;

    [Space]
    protected float FireTimer;
    protected Weapon Weapon;
    protected Weapon OnDeathWeapon;

    protected Transform player;
    private GameObject _Whandler;
    protected GameObject Points;
    //private GameObject LevelManager;

    protected TrailRenderer[] Trail;
    private GameObject TrailContainer;
    private bool TrailEmitting = true;

    protected void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Points = GameObject.FindGameObjectWithTag("GridPoints");
        Weapon = GetComponent<Weapon>();
    }

    protected void Start()
    {
        CurrentHealth = MaxHealth;
        //LevelManager = GameObject.FindGameObjectWithTag("LevelManager");

        //TrailContainer = transform.Find("Trail").gameObject;
        //Trail = TrailContainer.GetComponentsInChildren<TrailRenderer>();

        _Whandler = GameObject.FindGameObjectWithTag("WaveHandler");
        EnemySprite = gameObject.transform.GetChild(0).gameObject;


        SetSpeed();
        GetOnDeathChild();
    }

    protected void Update()
    {
        if (!Pause.isGamePaused)
        {
            isAlive();
            if (LookAtPlayer) LookatTarget(player);
            EnemyMovement();
            bound();
            Shoot();
        }
    }

    protected abstract void EnemyMovement();
    protected abstract void Shoot();


    //MainFunctions
    #region mainFunctions(AllEnimies)
    

    protected void GetOnDeathChild()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).gameObject.name == "FireOnDeath")
            {
                OnDeathWeapon = transform.GetChild(i).GetComponent<Weapon>();
            }
        }
    }

    protected virtual void isAlive()
    {
        if (CurrentHealth <= 0 && !Dead)
        {
            // do stuff on death here

            //AudioManager.instance.Play("Hit");
            if(OnDeathWeapon != null) StartCoroutine(OnDeathWeapon.Shoot());

            if (_Whandler != null)
            {
                _Whandler.GetComponent<WaveHandler>().NumOfEnemyDestroyed++;
                _Whandler.GetComponent<WaveHandler>().AddScore(PointOnDeath);
            }
            Dead = true;
            Destroy(gameObject);
        }
    }

    private void bound()
    {
        //Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        //max.x -= 2;
        if (transform.position.x < -9f)
        {
            if (_Whandler != null) {
                _Whandler.GetComponent<WaveHandler>().NumOfEnemyDestroyed++;
                //Destroy(gameObject);
            }
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int Damage)
    {
        AudioManager.instance.Play("Hit");
        Instantiate(HitEffect, transform.position, Quaternion.identity);
        CurrentHealth -= Damage;
    }

    protected void LookatTarget(Transform target)
    {
        Vector3 dir = target.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.AngleAxis(angle - 180, Vector3.forward);
        EnemySprite.transform.rotation = Quaternion.AngleAxis(angle - 180, Vector3.forward);
    }

    protected void ResetRotation()
    {
        Vector3 dir = new Vector3(0, 0, 0);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        EnemySprite.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    protected void StopGeneratingTrail()
    {
        if (TrailEmitting)
        {
            for (int i = 0; i < Trail.Length; i++)
            {
                Trail[i].emitting = false;
                TrailEmitting = false;
            }
        }
    }

    protected void GenerateTrail()
    {
        if (!TrailEmitting)
        {
            for (int i = 0; i < Trail.Length; i++)
            {
                Trail[i].emitting = true;
                TrailEmitting = true;
            }
        }
    }

    #endregion

    //Enemy Attributes
    #region CommonEnemyAttributes

    private void SetSpeed()
    {
        if (!FixedSpeed)
        {
            Speed = Random.Range(Speed, MaxSpeed);
            //Debug.Log("Speed = " + Speed);
        }
    }

    protected void MoveLeft(float MovementSpeed)
    {
        transform.Translate(Vector2.left * Time.deltaTime * MovementSpeed, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.CompareTag("PlayerProjectile"))
        //{
        //    collision.gameObject.GetComponent<Bullet>().TakeDamage(1);
        //    //Instantiate(HitEffect, transform.position, Quaternion.identity);
        //    TakeDamage(collision.gameObject.GetComponent<Bullet>().Damage);
        //}
        
        if (collision.CompareTag("PlayerExplosion"))
        {
            Instantiate(HitEffect, transform.position, Quaternion.identity);
            TakeDamage(5);
        }
    }

    #endregion
}
