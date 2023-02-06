using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Mover
{
    // Experience
    public int seedsAmount = 1;

    // Logicx
    public float triggerLength = 1;
    public float chaseLength = 5;
    private bool chasing;
    private bool collidingWithPlayer;
    private Transform playerTransform;
    private Vector3 startingPosition;

    // Hitbox
    public ContactFilter2D filter;
    private BoxCollider2D hitbox;
    private Collider2D[] hits = new Collider2D[10];

    protected override void Start()
    {
        base.Start();
        playerTransform = GameManager.instance.player.transform;
        startingPosition = transform.position;
        hitbox = transform.GetChild(0).GetComponent<BoxCollider2D>();

    }

    private void FixedUpdate()
    {
        // Is the player in range?
        if(Vector3.Distance(playerTransform.position, startingPosition) < chaseLength)
        {
            if (Vector3.Distance(playerTransform.position, startingPosition) < triggerLength)
                chasing = true;

            if (chasing)
            {
                if (!collidingWithPlayer)
                {
                    UpdatedMotor((playerTransform.position - transform.position).normalized);
                }
            }
            else
            {
                UpdatedMotor(startingPosition - transform.position);
            }
        }
        else
        {
            UpdatedMotor(startingPosition - transform.position);
            chasing = false;
        }

        // Check for overlaps
        collidingWithPlayer = false;
        boxCollider.OverlapCollider(filter, hits);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == null)
                continue;

          if(hits[i].tag == "Fighter" && hits[i].name == "Player")
            {
                collidingWithPlayer = true;
            }
        }

    }

    protected override void Death()
    {
        Destroy(gameObject);
        GameManager.instance.seeds += seedsAmount;
        GameManager.instance.ShowText("+" + seedsAmount + "tomato seeds", 30, Color.magenta, transform.position, Vector3.up * 40, 1.0f);
    }

}
