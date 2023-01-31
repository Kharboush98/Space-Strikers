using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [HideInInspector] public bool IsFiringW1 = false;
    [HideInInspector] public bool IsFiringW2 = false;

    [Header("PrimaryWeapon")]
    public int PrimaryWeaponID = 0;
    public KeyCode K1;
    [SerializeField] private GameObject WeaponChild;
    private Weapon weapon;
    private float FireTimer;

    [Header("SecondaryWeapon")]
    public int SecondaryWeaponID = 1;
    public KeyCode K2;
    [SerializeField] private GameObject WeaponChild2;
    private Weapon weapon2;
    private float FireTimer2;

    private void Awake()
    {
        GetWeapons();
    }

    void Update()
    {
        if (!Pause.isGamePaused)
        {
            PrimaryFire();
            SecondaryFire();
        }
    }

    private void GetWeapons()
    {
        WeaponChild = gameObject.transform.Find("Weapon Container").transform.GetChild(PrimaryWeaponID).gameObject;
        if (WeaponChild != null)
        {
            weapon = WeaponChild.GetComponent<Weapon>();
            FireTimer = weapon.NextFire;
        }

        WeaponChild2 = gameObject.transform.Find("Weapon Container").transform.GetChild(SecondaryWeaponID).gameObject;

        if (WeaponChild2 != null)
        {
            weapon2 = WeaponChild2.GetComponent<Weapon>();
            FireTimer2 = weapon2.NextFire;
        }
    }

    private void PrimaryFire()
    {
        if (Input.GetKey(K1) && Time.time > FireTimer)
        {
            IsFiringW1 = true;
            FireTimer = Time.time + weapon.NextFire;
            Attack(weapon);
        }

        if (Input.GetKeyUp(K1))
        {
            IsFiringW1 = false;
        }
    }

    //Secondary Weapon :-
    private void SecondaryFire()
    {
        if (Input.GetKey(K2) && Time.time > FireTimer2)
        {
            IsFiringW2 = true;
            FireTimer2 = Time.time + weapon2.NextFire;
            Attack(weapon2);
            //Attack2();
        }

        if (Input.GetKeyUp(K2))
        {
            IsFiringW2 = false;
        }
    }

    private void Attack(Weapon W)
    {
        StartCoroutine(W.Shoot());
    }

}
