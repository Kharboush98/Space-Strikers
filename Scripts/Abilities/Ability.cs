using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    public int StockRequirment = 1;
    public float CooldownTimer = 0.5f;
    [SerializeField] protected Transform[] firePos;

    protected ObjectPooler OP;

    protected void Start()
    {
        OP = ObjectPooler.Instance;
    }

    public abstract void ActivateAbility();

}
