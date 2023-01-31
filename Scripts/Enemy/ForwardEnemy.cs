using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardEnemy : EnemyBase
{
    public enum ForwardEnemyBehaviour
    {
        NonStop , NonStopWshooting, Stop_Move ,StopCycleOnYaxis, Cycle
    };
    public enum FireStyle
    {
        Immediatly ,WaitThenFire
    };

    [Header("Child Attributes")]
    [SerializeField] private FireStyle ShootingStyle;
    [SerializeField] private float MoveLeftSpeed;
    [SerializeField] private ForwardEnemyBehaviour EnemyBehaviour;
    [SerializeField] private bool FixedWaitTime = true;
    private float WaitTime;
    [SerializeField] private float StartWaitTime;
    
    private Vector2 MoveSpot;
    private bool ArrivedAtMoveSpot = false;

    [Space]
    [Header("Sway Variables")]
    [SerializeField] private bool ApplySway = false;
    private float SwayXSpeed;
    private float SwayYSpeed;
    [SerializeField] private bool XaxisSway;
    [SerializeField] private bool YaxisSway;

    [Space]
    [Header("StopIdle Variables")]
    [SerializeField] private int Cycles = 3;
    private int CurrentCycleValue = 0;

    new void Start()
    {
        base.Start();

        FireTimer = Weapon.NextFire;

        if (EnemyBehaviour == ForwardEnemyBehaviour.Stop_Move
            || EnemyBehaviour == ForwardEnemyBehaviour.Cycle
            || EnemyBehaviour == ForwardEnemyBehaviour.StopCycleOnYaxis)
        {
            //WhereToStop();
            if (!FixedWaitTime) StartWaitTime = Random.Range(3f,StartWaitTime);
            if(ApplySway) RandomSway();
            VerticalMoveSpot();
        }

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

    void EnemyType()
    {
        switch (EnemyBehaviour)
        {
            case ForwardEnemyBehaviour.NonStop:
                MoveLeft(Speed);
                CanShoot = false;
                LookAtPlayer = false;
                break;
            case ForwardEnemyBehaviour.NonStopWshooting:
                MoveLeft(Speed);
                CanShoot = true;
                LookAtPlayer = false;
                break;
            case ForwardEnemyBehaviour.Stop_Move:
                Stop();
                break;
            case ForwardEnemyBehaviour.StopCycleOnYaxis:
                ApplySway = false;
                MoveOnYaxis();
                break;
            case ForwardEnemyBehaviour.Cycle:
                ApplySway = false;
                CycleEnemyBehaviour();
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
        else if(ArrivedAtMoveSpot && ShootingStyle == FireStyle.WaitThenFire)
        {
            FireTimer -= Time.deltaTime;
        }
        else if(ShootingStyle == FireStyle.Immediatly)
        {
            FireTimer -= Time.deltaTime;
        }
    }

    void VerticalMoveSpot()
    {
        float x,y;
        WaitTime = StartWaitTime;
        x = Points.GetComponent<PointsContainers>().XaxisGrid[8].transform.position.x;
        y = Points.GetComponent<PointsContainers>().XaxisGrid[14].transform.position.x;
        MoveSpot = new Vector2(Random.Range(x ,y),transform.position.y);
    }

    void Stop()
    {
        if (!ArrivedAtMoveSpot)
        {
            GOTOMoveSpot(MoveSpot);
            CanShoot = false;
        }
        else
        {
            if (WaitTime <= 0)
            {
                CanShoot = false;
                LookAtPlayer = false;
                ResetRotation();
                MoveLeft(MoveLeftSpeed);
            }
            else
            {
                WaitTime -= Time.deltaTime;
                CanShoot = true;
                SwayMovement();
            }
        }

        if(Vector2.Distance(transform.position, MoveSpot) < 0.1f)
        {
            ArrivedAtMoveSpot = true;
        }

    }

    void MoveOnYaxis()
    {
        if (!ArrivedAtMoveSpot)
        {
            GOTOMoveSpot(MoveSpot);
            CanShoot = false;
        }
        else if (CurrentCycleValue >= Cycles)
        {
            if (WaitTime <= 0)
            {
                CanShoot = false;
                LookAtPlayer = false;
                ResetRotation();
                MoveLeft(MoveLeftSpeed);
            }
            else
            {
                WaitTime -= Time.deltaTime;
                CanShoot = true;
            }
        }

        if (Vector2.Distance(transform.position, MoveSpot) < 0.1f)
        {
            if (WaitTime <= 0 && CurrentCycleValue < Cycles)
            {
                //GetRandomPos();
                GetRandomYPos();
                CurrentCycleValue++;
                CanShoot = false;
                WaitTime = StartWaitTime;
                ArrivedAtMoveSpot = false;
            }
            else
            {
                ArrivedAtMoveSpot = true;
                WaitTime -= Time.deltaTime;
                CanShoot = true;
            }
        }
    }

    void CycleEnemyBehaviour()
    {
        if (!ArrivedAtMoveSpot)
        {
            GOTOMoveSpot(MoveSpot);
            CanShoot = false;
        }   
        else if (CurrentCycleValue >= Cycles)
        {
            if (WaitTime<= 0)
            {
                CanShoot = false;
                LookAtPlayer = false;
                ResetRotation();
                MoveLeft(MoveLeftSpeed);
            }
            else
            {
                WaitTime -= Time.deltaTime;
                CanShoot = true;
            }
        }

        if(Vector2.Distance(transform.position, MoveSpot) < 0.1f)
        {
            if (WaitTime <= 0 && CurrentCycleValue < Cycles)
            {
                //GetRandomPos();
                GetAnyMovSpot();
                CurrentCycleValue++;
                CanShoot = false;
                WaitTime = StartWaitTime;
                ArrivedAtMoveSpot = false;
            }
            else
            {
                ArrivedAtMoveSpot = true;
                WaitTime -= Time.deltaTime;
                CanShoot = true;
            }
        }

    }
    
    void GetRandomYPos()
    {
        int Y;
        //X = Random.Range(7, Points.GetComponent<PointsContainers>().XaxisGrid.Length);
        //MoveSpot.x = Points.GetComponent<PointsContainers>().XaxisGrid[X].transform.position.x;

        Y = Random.Range(0, Points.GetComponent<PointsContainers>().YaxisGrid.Length);
        MoveSpot.y = Points.GetComponent<PointsContainers>().YaxisGrid[Y].transform.position.y;

        MoveSpot = new Vector2(transform.position.x,MoveSpot.y);
    }

    void GetAnyMovSpot()
    {
        float x, y;
        float a, b;

        x = Points.GetComponent<PointsContainers>().XaxisGrid[9].transform.position.x;
        y = Points.GetComponent<PointsContainers>().XaxisGrid[14].transform.position.x;

        a = Points.GetComponent<PointsContainers>().YaxisGrid[0].transform.position.y;
        b = Points.GetComponent<PointsContainers>().YaxisGrid[6].transform.position.y;

        MoveSpot = new Vector2(Random.Range(x, y), Random.Range(a, b));
    }

    void GOTOMoveSpot(Vector2 MoveSpot)
    {
        transform.position = Vector2.MoveTowards(transform.position, MoveSpot, Speed * Time.deltaTime);
        //transform.position = Vector2.Lerp(transform.position, MoveSpot, Speed * Time.deltaTime);
    }

    //Sway F(n)
    void SwayMovement()
    {
        if (YaxisSway)
            transform.localPosition += Vector3.up * Mathf.Sin(Time.time) * SwayYSpeed * Time.deltaTime;

        if (XaxisSway)
            transform.localPosition += Vector3.right * Mathf.Sin(Time.time) * SwayXSpeed * Time.deltaTime;
    }

    void RandomSway()
    {
        SwayXSpeed = Random.Range(0.1f, 0.15f);
        SwayYSpeed = Random.Range(0.1f, 0.15f);
    }
}
