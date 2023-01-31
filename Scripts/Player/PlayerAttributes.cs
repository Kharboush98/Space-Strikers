using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerAttributes : MonoBehaviour , IDamagable
{
    [Header("Health")]
    public int MaxHealth;
    [SerializeField] private int CurrentHealth;
    [SerializeField] private float InvunrabilityTime;
    private float IVtimer;
    private bool isInvul = false;
    private bool Alive;
    [SerializeField] private Image[] HealthStocks;


    [Header("Effects")]
    [SerializeField] private GameObject HitEffect;

    private GameObject PlayerSprite;
    private Shake CameraGO;

    private bool RunOnce = false;

    //[Header("Energy")]
    //[SerializeField] private int MaxEnergy;
    //public int CurrentEnergy;
    //private GameManager GM;

    private void Awake()
    {
        CurrentHealth = MaxHealth;
        //CurrentEnergy = MaxEnergy;

        IVtimer = InvunrabilityTime;
        Alive = true;
    }

    void Start()
    {
        //GM = GameManager.GetInstance();
        //MaxHealth = GM.PlayerMaxHealth;
        CameraGO = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Shake>();
        SetHealthUI();
        PlayerSprite = gameObject.transform.GetChild(0).gameObject;
    }

    void Update()
    {
        isAlive();
        InvulCountdown();
        UpdateHealthUI();
    }

    void SetHealthUI()
    {
        for (int i = 0; i < HealthStocks.Length; i++)
        {
            if (i < MaxHealth)
            {
                HealthStocks[i].enabled = true;
            }
            else
            {
                HealthStocks[i].enabled = false;
            }
        }
    }

    void UpdateHealthUI()
    {
        if (CurrentHealth > MaxHealth) CurrentHealth = MaxHealth;

        for (int i = 0; i < MaxHealth; i++)
        {
            if (i < CurrentHealth)
            {
                HealthStocks[i].enabled = true;
            }
            else
            {
                HealthStocks[i].enabled = false;
            }
        }
    }

    #region Health Functions
    public void SetMaxHealth(int Health)
    {
        MaxHealth = Health;
    }

    private void isAlive()
    {
        if (CurrentHealth <= 0 && !RunOnce)
        {
            RunOnce = true;
            Alive = false;
            Instantiate(HitEffect, transform.position, Quaternion.identity);
            PlayerSprite.SetActive(false);
            //Debug.Log("Player DED");
        }
        else
        {
            Alive = true;
        }
    }

    public bool isPlayerAlive()
    {
        return Alive;
    }

    public void TakeDamage(int Damage)
    {
        if (!isInvul)
        {
            isInvul = true;
            AudioManager.instance.Play("Hit");
            Instantiate(HitEffect, transform.position, Quaternion.identity);
            CurrentHealth -= Damage;
        }
    }
    void InvulCountdown()
    {
        if (isInvul && IVtimer > 0)
        {
            //invunrable to damage
            IVtimer -= Time.deltaTime;
        }
        else
        {
            //Can Take Damage
            isInvul = false;
            IVtimer = InvunrabilityTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!SceneLoader.Instance.inTransition)
        {
            if (collision.CompareTag("Enemy"))
            {
                collision.gameObject.GetComponent<EnemyBase>().TakeDamage(1);
                StartCoroutine(StopShake());
                TakeDamage(1);
            }

            if (collision.CompareTag("EnemyProjectile"))
            {
                //collision.gameObject.GetComponent<Bullet>().TakeDamage(1);
                StartCoroutine(StopShake());
                collision.gameObject.SetActive(false);
                TakeDamage(1);
            }

            if (collision.CompareTag("Explosion"))
            {
                StartCoroutine(StopShake());
                TakeDamage(1);
            }
        }
    }

    IEnumerator StopShake()
    {
        GamePause.PauseGame();
        if(CameraGO != null) CameraGO.CamShake();
        yield return new WaitForSecondsRealtime(0.1f);
        GamePause.ResumeGame();
    }
    #endregion
}
