using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideEnemy : EnemyBase
{
    public enum ForwardEnemyBehaviour
    {
        NonStopTargetSpot, NonStopTargetPlayer, TargetPosThenMoveLeft
    };
    public enum FireStyle
    {
        Immediatly, WaitThenFire
    };

    [Header("Child Attributes")]
    [SerializeField] private FireStyle ShootingStyle;
    [SerializeField] private ForwardEnemyBehaviour EnemyBehaviour;
    [SerializeField] private bool AllowShooting;
    [SerializeField] private bool FixedWaitTime = true;
    private float WaitTime;
    [SerializeField] private float StartWaitTime = 1f;

    private Vector2 MoveSpot;
    private Transform MoveSpotTarget;
    private bool ArrivedAtMoveSpot = false;

    [Header("MoveSpot destination")]
    private bool TargetTopSpot; 
    private bool targetBottomSpot; 

    new void Start()
    {
        base.Start();

        FireTimer = Weapon.NextFire;
        OnStartEnemyType();
    }

    new void Update()
    {
        base.Update();
        //Shoot();
    }

    protected override void EnemyMovement()
    {
        EnemyType();
    }

    void OnStartEnemyType()
    {
        switch (EnemyBehaviour)
        {
            case ForwardEnemyBehaviour.NonStopTargetSpot:
                ReturnTopOrBottomTarget();
                GetMoveSpot();
                LookatTarget(MoveSpotTarget);
                break;

            case ForwardEnemyBehaviour.NonStopTargetPlayer:
                MoveSpot = player.position;
                MoveSpotTarget = player;
                LookatTarget(MoveSpotTarget);
                break;

            case ForwardEnemyBehaviour.TargetPosThenMoveLeft:
                if (!FixedWaitTime)
                {
                    StartWaitTime = Random.Range(1f, StartWaitTime);
                    WaitTime = StartWaitTime;
                }
                else
                {
                    WaitTime = StartWaitTime;
                }
                ReturnTopOrBottomTarget();
                GetMoveSpot();
                break;
        }
    }

    void EnemyType()
    {
        switch (EnemyBehaviour)
        {
            case ForwardEnemyBehaviour.NonStopTargetSpot:
                if(AllowShooting) CanShoot = true;
                LookAtPlayer = false;
                if (Vector2.Distance(transform.position, MoveSpot) < 0.1f)
                {
                    ResetRotation();
                    ArrivedAtMoveSpot = true;
                }
                else
                { 
                    GOTOMoveSpot(MoveSpot);
                }

                if(ArrivedAtMoveSpot) MoveLeft(Speed);

                break;

            case ForwardEnemyBehaviour.NonStopTargetPlayer:
                if (AllowShooting) CanShoot = true;
                LookAtPlayer = false;
                MoveInDirection(EnemySprite.transform.localRotation.normalized * Vector3.left);
                break;

            case ForwardEnemyBehaviour.TargetPosThenMoveLeft:
                LookAtPlayer = false;
                TargetThenMoveLeftMov();
                break;
        }
    }

    protected override void Shoot()
    {
        if (FireTimer <= 0 && CanShoot)
        {
            StartCoroutine(Weapon.Shoot());
            FireTimer = Weapon.NextFire;
        }
        else if (ArrivedAtMoveSpot && ShootingStyle == FireStyle.WaitThenFire)
        {
            FireTimer -= Time.deltaTime;
        }
        else if (ShootingStyle == FireStyle.Immediatly)
        {
            FireTimer -= Time.deltaTime;
        }
    }

    void GetMoveSpot()
    {
        int x;

        if (TargetTopSpot)
        {
            x = Random.Range(0, Points.GetComponent<PointsContainers>().TopDesPoints.Length);
            MoveSpot = Points.GetComponent<PointsContainers>().TopDesPoints[x].transform.position;
            MoveSpotTarget = Points.GetComponent<PointsContainers>().TopDesPoints[x].transform;
        }

        if (targetBottomSpot)
        {
            x = Random.Range(0, Points.GetComponent<PointsContainers>().BottomDesPoints.Length);
            MoveSpot = Points.GetComponent<PointsContainers>().BottomDesPoints[x].transform.position;
            MoveSpotTarget = Points.GetComponent<PointsContainers>().BottomDesPoints[x].transform;
        }

    }

    void ReturnTopOrBottomTarget()
    {
        if (transform.position.y >= 0)
        {
            targetBottomSpot = true;
            TargetTopSpot = false;
        }
        else
        {
            TargetTopSpot = true;
            targetBottomSpot = false;
        }
    }

    void GOTOMoveSpot(Vector2 MoveSpot)
    {
        if(!ArrivedAtMoveSpot)
        transform.position = Vector2.MoveTowards(transform.position, MoveSpot, Speed * Time.deltaTime);
    }

    void MoveInDirection(Vector2 Direction)
    {
        transform.Translate(Direction * Time.deltaTime * Speed, Space.World);
    }

    void TargetThenMoveLeftMov()
    {
        if (WaitTime <= 0)
        {
            if (AllowShooting) CanShoot = true;
            ResetRotation();
            MoveLeft(Speed);
        }
        else
        {
            WaitTime -= Time.deltaTime;
            LookatTarget(MoveSpotTarget);
            GOTOMoveSpot(MoveSpot);
        }
    }
}
