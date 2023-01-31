using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public Animator CamAnim;

    private void Start()
    {
        CamAnim = gameObject.GetComponent<Animator>();
    }

    public void CamShake()
    {
        CamAnim.SetTrigger("Shake");
    }

}
