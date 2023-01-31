using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targetbullet : Bullet
{
    private Transform MoveSpotTarget;
    public override void OnObjectSpawn()
    {
        base.OnObjectSpawn();
        MoveSpot = player.position;
        MoveSpotTarget = player;
        LookatTarget(MoveSpotTarget);
    }

    new void Update()
    {
        base.Update();
        
    }
    public override void Movement()
    {
        TargetMov(BulletSprite.transform.localRotation.normalized * Vector3.left);
    }

    void TargetMov(Vector2 Direction)
    {
        transform.Translate(Direction * Time.deltaTime * Speed, Space.World);
    }
}
