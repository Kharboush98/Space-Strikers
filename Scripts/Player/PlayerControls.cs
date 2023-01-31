using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [Header("Attributes")]
    public int Speed;
    public int SpeedWhileFiring1W = 10;
    public int SpeedWhileFiringBothW = 10;
    private int OriginalSpeed;
    private Rigidbody2D rb;
    private Vector2 MoveDirection;
    private PlayerShooting PlayerS;



    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        PlayerS = GetComponent<PlayerShooting>();
        OriginalSpeed = Speed;
    }


    void Update()
    {
        if (!Pause.isGamePaused)
        {
            ProcessInputs();
            //Bound();
            SpeedChange();
        }
    }
    void FixedUpdate()
    {
        Movement();
    }


    //Movement and Boundries
    private void ProcessInputs()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        MoveDirection = new Vector2(x,y).normalized;
    }

    private void Movement()
    {
        rb.velocity = new Vector2(MoveDirection.x * Speed, MoveDirection.y * Speed) * Time.deltaTime;
    }

    private void SpeedChange()
    {
        if((PlayerS.IsFiringW1 && !PlayerS.IsFiringW2) || (PlayerS.IsFiringW2 && !PlayerS.IsFiringW1))
        {
            Speed = SpeedWhileFiring1W;
        } else if (PlayerS.IsFiringW1 && PlayerS.IsFiringW2)
        {
            Speed = SpeedWhileFiringBothW;
        }
        else
        {
            Speed = OriginalSpeed;
        }
    }

    private void Bound()
    {
        Vector2 min = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        transform.position = new Vector2(Mathf.Clamp(transform.position.x, min.x + 1.75f, max.x - 1.75f),
            Mathf.Clamp(transform.position.y, min.y + 1.75f, max.y - 1.75f));
    }
}
