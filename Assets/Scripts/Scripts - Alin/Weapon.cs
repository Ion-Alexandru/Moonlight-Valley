using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Collidable
{
    // Damage struct
    public int damagePoint = 1;
    public float pushForce = 2.0f;

    // Upgrade
    public int weaponLevel = 0;
    private SpriteRenderer spriteRenderer;

    // Swing 
    private Animator anim;
    private float cooldown = 0.5f;
    private float lastSwing;

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    protected void Update()
    {
        //base.Update();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(Time.time - lastSwing > cooldown)
            {
                lastSwing = Time.time;
                Swing();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Harvest();
        }
    }

    private void Swing()
    {
        anim.SetTrigger("Swing");
    }

    private void Harvest()
    {
        anim.SetTrigger("Harvest");
    }

}
