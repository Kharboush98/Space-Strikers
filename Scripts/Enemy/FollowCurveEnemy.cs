using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCurveEnemy : EnemyBase
{
    [SerializeField] private Transform[] Routes;

    private int RouteToGo;
    private float tParam;
    private Vector2 EnemyPos;
    private bool CoroutineAllowed;

    new void Start()
    {
        base.Start();
        SetParam();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }

    protected override void EnemyMovement()
    {
        if (CoroutineAllowed)
            StartCoroutine(GoByTheRoute(RouteToGo));
    }

    protected override void Shoot()
    {
        throw new System.NotImplementedException();
    }

    private void SetParam()
    {
        RouteToGo = 0;
        tParam = 0f;
        //speed modifier = 0.25f
        CoroutineAllowed = true;
    }

    private IEnumerator GoByTheRoute(int RouteNumber)
    {
        CoroutineAllowed = false;

        Vector2 p0 = Routes[RouteNumber].GetChild(0).position;
        Vector2 p1 = Routes[RouteNumber].GetChild(1).position;
        Vector2 p2 = Routes[RouteNumber].GetChild(2).position;
        Vector2 p3 = Routes[RouteNumber].GetChild(3).position;


        while (tParam < 1)
        {
            tParam += Time.deltaTime * Speed;

            EnemyPos = Mathf.Pow(1 - tParam, 3) * p0 +
                3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 +
                3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 +
                Mathf.Pow(tParam, 3) * p3;

            transform.position = EnemyPos;
            yield return new WaitForFixedUpdate();
        }

        tParam = 0f;
        RouteToGo += 1;

        //for looping
        if (RouteToGo > Routes.Length - 1)
        {
            //RouteToGo = 0;
            CoroutineAllowed = false;
        }
        else
        {
            CoroutineAllowed = true;
        }
    }

}
