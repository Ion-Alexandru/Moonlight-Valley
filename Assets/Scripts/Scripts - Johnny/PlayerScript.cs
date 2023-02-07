using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using static System.Net.Mime.MediaTypeNames;
using UnityEngine.SceneManagement;

public class PlayerScript : Mover
{
    GameManager gameManager;
    public static PlayerScript instance;
    public float speed;
    public GameObject outline;
    public int seedIndex = 1;
    public int weaponIndex = 0;
    public string[] seeds = {"None", "Cucumber", "Tomato", "Carrot", "Pumpkin", "Cabbage"};
    public string[] weapons = {"Hand", "Gardening Hoe", "Seeds Pack", "Scythe"};
    public UnityEngine.UI.Text currentSeed;
    public UnityEngine.UI.Text currentWeapon;
    public Vector2 lastDirection = Vector2.zero;
    public Tilemap tilemap;
    public GardeningBehaviour gardeningBehaviour;
    public Transform position;
    public float x;
    public float y;
    float attackInterval = 0.5f;
    float nextAttackTime = 0;
    
    public int hitpoints = 10; // starting hitpoints
    public int maxHitpoints = 10;
    public float attackRange = 0.2f;
    public int attackDamage = 2;

    // protected override void ReceiveDamage(Damage dmg)
    // {
    //     base.ReceiveDamage(dmg);
    //     GameManager.instance.OnHitpointChange();
    // }

    private void Update()
    {
        Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        x = Input.GetAxisRaw("Horizontal"); // Returns -1,1 or 0 | -1 if holding A, 0 not holding any keys, 1 if holding D
        y = Input.GetAxisRaw("Vertical"); // Returns -1,1 or 0 | -1 if holding S, 0 not holding any keys, 1 if holding

        UpdatedMotor(new Vector3(x, y, 0));

        Vector2 direction = new Vector2(x, y);
        if (direction.sqrMagnitude > 0) {
            lastDirection = direction;
            gardeningBehaviour.selectTile( );
        }

        if(Input.GetKeyDown(KeyCode.UpArrow) && seedIndex < seeds.Length - 1){
        seedIndex += 1;

        Debug.Log("Current seed: " + seeds[seedIndex]);
        
        currentSeed.text = "Current Seed: " + seeds[seedIndex];

        } else if (Input.GetKeyDown(KeyCode.DownArrow) && seedIndex > 1){
            seedIndex -= 1;

            Debug.Log("Current seed: " + seeds[seedIndex]);
            currentSeed.text = "Current Seed: " + seeds[seedIndex];
        }
        
        float Scroll = Input.GetAxis("Mouse ScrollWheel");

        if (Scroll > 0 && weaponIndex < weapons.Length - 1) // forward
        {
            weaponIndex += 1;

            currentWeapon.text = "Current Weapon: " + weapons[weaponIndex];
        } else if (Scroll < 0 && weaponIndex > 0) // backwards
        {
            weaponIndex -= 1;

            currentWeapon.text = "Current Weapon: " + weapons[weaponIndex];
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time >= nextAttackTime)
        {
            AttackEnemy(transform.position, mousePosition);

            // Reset next attack time
            nextAttackTime = Time.time + attackInterval;
        }

    }

    public virtual void ReceiveDamage(int damage)
    {
        GameManager.instance.ShowText("- " + damage + " damage", 30, Color.white, transform.position, Vector3.zero, 0.5f);

        hitpoints -= damage;
        GameManager.instance.OnHitpointChange();  //notify game manager of change in hitpoints
        if (hitpoints <= 0) playerDeath();
    }

    private void playerDeath() {
        //player has died, handle death and restart as necessary
        if (hitpoints <= 0){
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void AttackEnemy(Vector2 attackOrigin, Vector2 attackDirection) {                                                                           
        RaycastHit2D hit = Physics2D.Raycast(attackOrigin, attackDirection, attackRange);
        Debug.DrawRay(transform.position, Vector2.right * hit.distance, Color.red);
        
        //check if we hit collider                                                           
        if (hit.collider != null) {  
            //check if it's an enemy                                     
            foreach (Enemy enemy in gardeningBehaviour.enemyList){
                if(enemy!=null) {
                    Debug.Log("Enemy Hit!!");
                    enemy.EnemyReceiveDamage(attackDamage, enemy); 
                }
            }
        }
    }

    // private void AttackEnemy(Vector3 attackOrigin, Vector3 mousePosition)
    // {
    //     Vector3 attackDirection = mousePosition - attackOrigin;
    //     Collider2D enemyCollider = Physics2D.OverlapCircle(attackOrigin + attackDirection * attackRange, 0.1f, 1 << LayerMask.NameToLayer("Enemy"));
    //     if (enemyCollider != null)
    //     {
    //         // Deal damage to enemy
    //     }
    //     // Check if weapon hits an obstacle using a raycast
    //     RaycastHit2D wallHit = Physics2D.Raycast(attackOrigin, attackDirection, attackRange, 1 << LayerMask.NameToLayer("Wall"));
    //     if (wallHit.collider != null)
    //     {
    //         // Stop weapon when it hits an obstacle
    //         Vector3 newWeaponPosition = wallHit.point - attackDirection.normalized * 0.1f;
    //         weapon.position = newWeaponPosition;
    //     }
    //     else
    //     {
    //         weapon.position = attackOrigin + attackDirection * attackRange;
    //     }
    // }
}
