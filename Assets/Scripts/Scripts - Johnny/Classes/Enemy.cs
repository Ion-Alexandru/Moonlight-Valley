using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy
{
    PlayerScript player;
    GardeningBehaviour gardeningBehaviour;
    public int vegetableType;
    public GameObject sprite;
    public Vector3 targetPos;
    public float step;
    public bool isAttacking = false;
    public float attackDelay = 1.5f;
    public float attackTime = 0;
    public float attackRange = 0.3f;
    public int damage;
    public int enemyHitpoints;
    public int attackDamage = 2 ;

    public void Update()
    {
        if (!isAttacking)
        {
            MoveTowardsPlayer();
        }
        else
        {
            UpdateAttack();
        }
    }
    
    private void MoveTowardsPlayer()
    {
        //Move towards player position
        sprite.transform.position = Vector3.MoveTowards(sprite.transform.position, targetPos, step);
    }

    private void UpdateAttack()
    {
        //Reduce attackTime by deltaTime
        attackTime -= Time.deltaTime;
        if (attackTime <= 0)
        {
            DealDamage();
            attackTime = attackDelay;
        }
    }

    private void DealDamage()
    {
        player.ReceiveDamage(2);
    }

    public virtual void EnemyReceiveDamage(int damage, Enemy enemy)
    {
        enemyHitpoints -= damage;
        GameManager.instance.OnHitpointChange();  //notify game manager of change in hitpoints
        if (enemyHitpoints <= 0) enemyDeath(enemy);
    }
    void enemyDeath(Enemy enemy) {
        UnityEngine.Object.Destroy(enemy.sprite);
        //gardeningBehaviour.enemyList.Remove(enemy); // Given that the enemy you want to destroy has a component Enemy attached to it
    }
} 