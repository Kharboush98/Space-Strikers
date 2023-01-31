using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAbilities : MonoBehaviour
{
    [SerializeField] private int MeterStocks = 1;
    [SerializeField] private int MeterGainThreshold = 500;
    [SerializeField] private Image[] MeterStockImg;
    private int MaxStocks = 6;
    private int MeterGainEveryThreshold;

    [System.Serializable]
    public class Abilities {
        public int AbilityID ;
        public KeyCode K1;
        public GameObject WeaponChild;
        [HideInInspector] public Ability Ability;
        [HideInInspector] public float FireTimer;
    }
    [Space]
    [Header("Abilities")]
    [SerializeField] private Abilities[] AbilityController ;

    private int AbilityArray;
    private Transform AbilityContainer;
    private GameObject _Whandler;

    private void Awake()
    {
        _Whandler = GameObject.FindGameObjectWithTag("WaveHandler");
        AbilityContainer = gameObject.transform.Find("Ability Container");
        AbilityArray = AbilityContainer.childCount;
        GetAbilities();
        SetStocksUI();
        MeterGainEveryThreshold = MeterGainThreshold;
    }

    void Update()
    {
        PrimaryFire();
        UpdateMeterStocksUI();
        if(_Whandler != null) IncMeterStocks();
    }

    private void GetAbilities()
    {
        for (int i = 0; i < AbilityArray; i++) {
            AbilityController[i].WeaponChild = AbilityContainer.GetChild(i).gameObject;
            if (AbilityController[i].WeaponChild != null)
            {
                AbilityController[i].AbilityID = i;
                AbilityController[i].Ability = AbilityController[i].WeaponChild.GetComponent<Ability>();
                AbilityController[i].FireTimer = AbilityController[i].Ability.CooldownTimer;
            }
        }
    }

    private void PrimaryFire()
    {
        for(int i = 0; i < AbilityArray; i++)
        {
            if (Input.GetKey(AbilityController[i].K1))
            {
                ReductCostofAbility(i);
            }
        }
    }

    private void ReductCostofAbility(int AbilityNum)
    {
        if (Time.time > AbilityController[AbilityNum].FireTimer && AbilityController[AbilityNum].Ability.StockRequirment <= MeterStocks)
        {
            MeterStocks -= AbilityController[AbilityNum].Ability.StockRequirment;
            
            Debug.Log("Used");
            UseAbility(AbilityNum);
            AbilityController[AbilityNum].FireTimer = Time.time + AbilityController[AbilityNum].Ability.CooldownTimer;
        }
    }
    private void UseAbility(int AbilityNum)
    {
        AbilityController[AbilityNum].Ability.ActivateAbility();
    }

    //--------------- Stocks UI Functions

    void SetStocksUI()
    {
        for (int i = 0; i < MeterStockImg.Length; i++)
        {
            if (i < MeterStocks)
            {
                MeterStockImg[i].enabled = true;
            }
            else
            {
                MeterStockImg[i].enabled = true;
            }
        }
    }

    void UpdateMeterStocksUI()
    {
        if (MeterStocks > MaxStocks) MeterStocks = MaxStocks;

        for(int i = 0; i < MaxStocks ;i++)
        {
            if (i < MeterStocks)
            {
                MeterStockImg[i].enabled = true;
            }
            else
            {
                MeterStockImg[i].enabled = false;
            }
        }
    }

    void IncMeterStocks()
    {
        if (_Whandler.GetComponent<WaveHandler>().GetScore() >= MeterGainThreshold )
        {
            AudioManager.instance.Play("MeterGain");
            MeterStocks += 1;
            MeterGainThreshold += MeterGainEveryThreshold;
        }
    }

}
