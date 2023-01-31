using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcBullet : Bullet
{
    [Space]
    [Header("Child Attributes")]
    [SerializeField] private float arcHeight;
    [SerializeField] private float BSpeed;
    [SerializeField] private GameObject Crosshair;
    private GameObject CrossHairOBJ;
    private Vector3 StartPos;
    private float StepScale;
    private float Progress;

    public override void OnObjectSpawn()
    {
        ResetValues();
        base.OnObjectSpawn();
        Initializations();
        LookAtPlayer = false;

        arcHeight = Random.Range(-2.0f,2.0f);
    }

    new void Update()
    {
        base.Update();
    }

    void Initializations()
    {
        StartPos = transform.position;
        float dist = Vector3.Distance(StartPos,PlayerTargetPos);
        StepScale = BSpeed / dist;

        //CrossHairOBJ = Instantiate(Crosshair,PlayerTargetPos,Quaternion.identity);
        CrossHairOBJ = OP.SpawnFromPool(Crosshair.name,PlayerTargetPos,Quaternion.identity);
    }

    public override void Movement()
    {
        Progress = Mathf.Min(Progress + Time.deltaTime * StepScale, 1.0f);
        float parabola = 1.0f - 4.0f * (Progress - 0.5f) * (Progress - 0.5f);
        Vector2 nextPos = Vector3.Lerp(StartPos, PlayerTargetPos, Progress);
        nextPos.y += parabola * arcHeight;

        LookAtNextPos(nextPos);
        transform.position = nextPos;

        if (Progress == 1.0f)
        {
            //instantiate explosion or sth
            CrossHairOBJ.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }

    }

    private void LookAtNextPos(Vector3 target)
    {
        Vector3 dir = target - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 180, Vector3.forward);
    }

    private void ResetValues()
    {
        StepScale = 0f;
        Progress = 0f;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.CompareTag("Crosshair"))
        //{
        //    collision.gameObject.SetActive(false);
        //}

        //if (collision.CompareTag("PlayerProjectile") && isDestructible)
        //{
        //    collision.gameObject.GetComponent<Bullet>().TakeDamage(1);
        //    TakeDamage(1);
        //}
    }

}
