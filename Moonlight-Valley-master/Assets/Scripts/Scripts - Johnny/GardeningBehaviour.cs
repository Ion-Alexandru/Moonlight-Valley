using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using static System.Net.Mime.MediaTypeNames;

public class GardeningBehaviour : MonoBehaviour
{   
    public GameManager gameManager;
    public static GardeningBehaviour instance;
    public List<TileData> tileDataList;
    public Canvas canvas;   
    Vector3Int selectedTilePosition;

    public List<Enemy> enemyList = new List<Enemy>();
    public Sprite mudSprite;
    public Sprite grassSprite;
    public Sprite seedSprite;
    public Sprite matureSprite;

    public Sprite[] harvestableSprites;

    public int[] vegetables = new int[] {0, 0, 0, 0, 0};

    public float timeToGrow;
    public float timeToEvil;
    public GameObject tomatoPrefab;
    public GameObject cucumberPrefab;
    public GameObject carrotPrefab;
    public GameObject pumpkinPrefab;
    public GameObject cabbagePrefab;
    public GameObject readySignPrefab;
    public int enemySpeed = 5;
    List<ReadySign> readySignList = new List<ReadySign>();
    public PlayerScript player;

    

    private void Start() {
        enemyList = new List<Enemy>();

        tileDataList = new List<TileData>();

        if (player.currentWeapon != null)
        {
            player.currentWeapon.text = "Current Weapon: " + player.weapons[player.weaponIndex];
        }
       
        BoundsInt bounds = player.tilemap.cellBounds;
        TileBase[] allTiles = player.tilemap.GetTilesBlock(bounds);

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null)
                {
                    Vector3Int tilePosition = new Vector3Int(x + bounds.x, y + bounds.y, 0);
                    tileDataList.Add(new TileData(tilePosition, 0, 0));
                }
            }
        }
    }

    private void Update() {
        
        if (readySignList.Count > 0)
        {
            for (int y = readySignList.Count - 1; y >= 0; y--)
            {
                ReadySign readySign = readySignList[y];

                for (int x = 0; x < tileDataList.Count; x++)
                {
                    if (tileDataList[x].seed == 0 && tileDataList[x].position == readySign.position)
                    {
                        Destroy(readySign.sprite);
                        readySignList.RemoveAt(y);
                        break;
                    }
                }
            }
        }

        if (player.weaponIndex == 0){

            player.outline.SetActive(false);

            player.currentSeed.text = "";
        }else if (player.weaponIndex == 1){
            player.outline.transform.position = player.tilemap.GetCellCenterWorld(selectedTilePosition);

            player.outline.SetActive(true);

            player.currentSeed.text = "";
            makeTerrainMud();
        }else if (player.weaponIndex == 2){
            player.outline.transform.position = player.tilemap.GetCellCenterWorld(selectedTilePosition);

            player.outline.SetActive(true);

            player.currentSeed.text = "Current seed: " + player.seeds[player.seedIndex];
            plantSeed(selectedTilePosition, player.seedIndex);
        } else if (player.weaponIndex == 3) {
            player.outline.transform.position = player.tilemap.GetCellCenterWorld(selectedTilePosition);
            
            player.outline.SetActive(true);

            player.currentSeed.text = "";
            harvestPlant(selectedTilePosition);
        }
    }

    public void selectTile() {
        Vector3Int cellPosition = player.tilemap.WorldToCell(player.transform.position);
        selectedTilePosition = cellPosition + Vector3Int.RoundToInt(player.lastDirection);
    }

    public void makeTerrainMud() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            // Get the current tile at the selected tile position
            TileBase currentTile = player.tilemap.GetTile(selectedTilePosition);

            // Create a new tile that uses the new sprite
            Tile newTile = ScriptableObject.CreateInstance<Tile>();
            newTile.sprite = mudSprite;

            // Change the sprite of the selected tile to the new sprite
            for (int j = 0; j < tileDataList.Count; j++)
            {
                if (tileDataList[j].position == selectedTilePosition && tileDataList[j].state == 0 && tileDataList[j].seed == 0)
                {
                    tileDataList[j].state = 1;
                    tileDataList[j].seed = 0;
                    player.tilemap.SetTile(selectedTilePosition, newTile);
                    break;
                }
            }

        }
    }

    public void plantSeed(Vector3Int tilePosition, int i) {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Vector3Int storedTilePosition = tilePosition;

            // Get the current tile at the selected tile position
            TileBase currentTile = player.tilemap.GetTile(storedTilePosition);

            // Change the sprite of the selected tile to the new sprite
            for (int j = 0; j < tileDataList.Count; j++)
            {
                if (tileDataList[j].position == storedTilePosition && tileDataList[j].state == 1 && tileDataList[j].seed == 0)
                {
                    // Create a new tile that uses the new sprite
                    Tile newTile = ScriptableObject.CreateInstance<Tile>();
                    newTile.sprite = seedSprite;
                    tileDataList[j].state = 2;
                    tileDataList[j].seed = i;
                    player.tilemap.SetTile(storedTilePosition, newTile);
                    StartCoroutine(WaitAndChangeTileToMature(timeToGrow, storedTilePosition, i));
                    break;
                }
            }

        }
    }

    private IEnumerator WaitAndChangeTileToMature(float waitTime, Vector3Int tilePosition, int i) {
        yield return new WaitForSeconds(waitTime);

        // Get the current tile at the selected tile position
        TileBase currentTile = player.tilemap.GetTile(tilePosition);

        // Create a new tile that uses the new sprite
        Tile newTile = ScriptableObject.CreateInstance<Tile>();

        newTile.sprite = matureSprite;
        player.tilemap.SetTile(tilePosition, newTile);

        // Make the plant harvestable
        growToHarvestable(tilePosition, i);
    }
    
    private void growToHarvestable(Vector3Int tilePosition, int i){
        StartCoroutine(WaitAndChangeTileToHarvestable(timeToGrow, tilePosition, i));
    }

    private IEnumerator WaitAndChangeTileToHarvestable(float waitTime, Vector3Int tilePosition, int i) {
        yield return new WaitForSeconds(waitTime);

        // Get the current tile at the selected tile position
        TileBase currentTile = player.tilemap.GetTile(tilePosition);

        // Create a new tile that uses the new sprite
        Tile newTile = ScriptableObject.CreateInstance<Tile>();

        // Change the sprite of the selected tile to the new sprite
        newTile.sprite = harvestableSprites[i-1];

        foreach (TileData tileData in tileDataList)
        {
            if (tileData.position == tilePosition && tileData.state == 2 && tileData.seed != 0)
            {
                SpawnReadySign(tilePosition);

                Debug.Log("Plant " + tilePosition + " is harvestable!");
                tileData.seed = i;
                tileData.state = 3;
                player.tilemap.SetTile(tilePosition, newTile);
                break;
            }
        }

        StartCoroutine(WaitAndChangeCropToEvil(timeToEvil, tilePosition));
    }

    private IEnumerator WaitAndChangeCropToEvil(float waitTime, Vector3Int tilePosition) {
        yield return new WaitForSeconds(waitTime);

        // Get the current tile at the selected tile position
        TileBase currentTile = player.tilemap.GetTile(tilePosition);

        // Create a new tile that uses the new sprite
        Tile newTile = ScriptableObject.CreateInstance<Tile>();

        // Change the sprite of the selected tile to the new sprite
        newTile.sprite = mudSprite;

        for (int j = 0; j < tileDataList.Count; j++)
        {
            if (tileDataList[j].position == tilePosition && tileDataList[j].state == 3)
            {
                SpawnEnemy(tileDataList[j].seed, tilePosition);
                Debug.Log("Plant " + tilePosition + " is now EVIL!"); 
                tileDataList[j].state = 0;
                tileDataList[j].seed = 0;
                player.tilemap.SetTile(tilePosition, newTile);
                break;
            }
        }
    }

    public void harvestPlant(Vector3Int tilePosition){

        if (Input.GetKeyDown(KeyCode.Space) && player.weaponIndex == 3)
        {
            // Get the current tile at the selected tile position
            TileBase currentTile = player.tilemap.GetTile(tilePosition);

            // Create a new tile that uses the new sprite
            Tile newTile = ScriptableObject.CreateInstance<Tile>();

            newTile.sprite = mudSprite;

            for (int j = 0; j < tileDataList.Count; j++)
            {
                if (tileDataList[j].position == tilePosition && tileDataList[j].state == 3)
                {
                    vegetables[tileDataList[j].seed - 1] += Random.Range(1, 3);
                    Debug.Log("You now have " + player.seeds[tileDataList[j].seed] + ": " + vegetables[tileDataList[j].seed - 1]);

                    gameManager.ShowText("Collected: " + player.seeds[tileDataList[j].seed], 30, Color.white, transform.position, Vector3.zero, 0.5f);

                    tileDataList[j].state = 1;
                    tileDataList[j].seed = 0;
                    player.tilemap.SetTile(tilePosition, newTile);
                    break;
                } 
            }
        }
    }
    
    public void SpawnEnemy(int vegetableType, Vector3Int spawnPosition)
    {
        Vector3 tilePosition = spawnPosition;
        GameObject spritePrefab = null;
        
        switch (vegetableType)
        {
            case 1:
                spritePrefab = cucumberPrefab;
                break;
            case 2:
                spritePrefab = tomatoPrefab;
                break;
            case 3:
                spritePrefab = carrotPrefab;
                break;
            case 4:
                spritePrefab = pumpkinPrefab;
                break;
            case 5:
                spritePrefab = cabbagePrefab;
                break;
        }
        
        GameObject spawnedSprite = Instantiate(spritePrefab, tilePosition, Quaternion.identity);
    
        Enemy enemy = new Enemy();
        enemy.vegetableType = vegetableType;
        enemy.sprite = spawnedSprite;
        enemy.targetPos = player.transform.position;
        enemy.step = enemySpeed * Time.deltaTime;
        enemy.isAttacking = false;
        enemy.attackTime = 1.5f;
        enemy.attackDamage = 2;
        StartCoroutine(FollowAndAttackPlayer(enemy));
    
        enemyList.Add(enemy);
    }
    
    IEnumerator FollowAndAttackPlayer(Enemy enemy)
    {
        while (enemy.sprite != null)
        {
            Vector3 direction = player.transform.position - enemy.sprite.transform.position;
            if (direction.magnitude > enemy.attackRange) // Enemy is not close enough to attack
            {
                enemy.sprite.transform.position += direction.normalized * enemy.step;
                //enemy.sprite.transform.position = Vector3.MoveTowards(enemy.sprite.transform.position, player.transform.position, enemySpeed);
            }
            else //if (!enemy.isAttacking)
            {
                player.ReceiveDamage(enemy.attackDamage);
                // enemy.isAttacking = true;
                
                yield return new WaitForSeconds(enemy.attackTime);
                // enemy.isAttacking = false;
            }
            yield return new WaitForEndOfFrame(); 
        }
    }
    
    void SpawnReadySign(Vector3Int spawnPosition)
    {
        Vector3 worldPos = player.tilemap.CellToWorld(spawnPosition) + new Vector3(1, 1, 0);
        GameObject signSprite = Instantiate(readySignPrefab, worldPos, Quaternion.identity);

        ReadySign readySign = new ReadySign();
        readySign.position = spawnPosition;
        readySign.sprite = signSprite;

        readySignList.Add(readySign);
    }
}