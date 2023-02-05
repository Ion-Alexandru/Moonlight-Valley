using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using static System.Net.Mime.MediaTypeNames;

public class gardeningBehaviour : MonoBehaviour
{
    public Tilemap tilemap;
    public List<TileData> tileDataList;
    private Vector3Int selectedTilePosition;
    private Vector2 lastDirection = Vector2.zero;

    public Sprite mudSprite;
    public Sprite grassSprite;
    public Sprite seedSprite;
    public Sprite matureSprite;

    public Sprite[] harvestableSprites;

    public float speed;
    public GameObject seedOutline;
    public GameObject mudOutline;
    public GameObject harvestOutline;
    int seedIndex = 1;
    int weaponIndex = 0;
    public int[] vegetables = {0, 0};

    public float timeToGrow;
    public float timeToEvil;

    private string[] seeds = {"None", "Carrot", "Tomato"};
    private string[] weapons = {"Hand", "Hoe", "Seeds Pack", "Scythe"};
    public UnityEngine.UI.Text currentSeed;
    public UnityEngine.UI.Text currentWeapon;
    public GameObject spritePrefab;
    public Transform player;
    private GameObject spawnedSprite;
    private bool enemySpawned = false;

    private void Start() {
        tileDataList = new List<TileData>();

        currentSeed.text = "Current Seed: " + seeds[seedIndex];
        currentWeapon.text = "Current Weapon: " + weapons[weaponIndex];

        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

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

        Debug.Log(tileDataList.Count);
    }

    private void Update() {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // spawn enemy
        if (enemySpawned){
            Vector3 targetPos = player.position;
            float step = speed * Time.deltaTime;
            spawnedSprite.transform.position = Vector3.MoveTowards(spawnedSprite.transform.position, targetPos, step);
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

        Vector2 direction = new Vector2(horizontal, vertical);
        if (direction.sqrMagnitude > 0) {
            lastDirection = direction;
            selectTile();
        }

        transform.position = transform.position + new Vector3(horizontal, vertical, 0) * speed * Time.deltaTime;

        if (weaponIndex == 0){
            harvestOutline.SetActive(false);
            seedOutline.SetActive(false);
            mudOutline.SetActive(false);
        }else if (weaponIndex == 1){
            mudOutline.transform.position = tilemap.GetCellCenterWorld(selectedTilePosition);

            harvestOutline.SetActive(false);
            seedOutline.SetActive(false);
            mudOutline.SetActive(true);

            makeTerrainMud();
        }else if (weaponIndex == 2){
            seedOutline.transform.position = tilemap.GetCellCenterWorld(selectedTilePosition);

            harvestOutline.SetActive(false);
            mudOutline.SetActive(false);
            seedOutline.SetActive(true);

            plantSeed(selectedTilePosition, seedIndex);
        } else if (weaponIndex == 3) {
            harvestOutline.transform.position = tilemap.GetCellCenterWorld(selectedTilePosition);
            
            seedOutline.SetActive(false);
            mudOutline.SetActive(false);
            harvestOutline.SetActive(true);

            harvestPlant(selectedTilePosition);
        }
    }

    private void selectTile() {
        Vector3Int cellPosition = tilemap.WorldToCell(transform.position);
        selectedTilePosition = cellPosition + Vector3Int.RoundToInt(lastDirection);
    }

    private void makeTerrainMud() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            // Get the current tile at the selected tile position
            TileBase currentTile = tilemap.GetTile(selectedTilePosition);

            // Create a new tile that uses the new sprite
            Tile newTile = ScriptableObject.CreateInstance<Tile>();
            newTile.sprite = mudSprite;

            // Change the sprite of the selected tile to the new sprite
            for (int j = 0; j < tileDataList.Count; j++)
            {
                if (tileDataList[j].position == selectedTilePosition && tileDataList[j].state == 0)
                {
                    tileDataList[j].state = 1;
                    tileDataList[j].seed = 0;
                    tilemap.SetTile(selectedTilePosition, newTile);
                    break;
                }
            }

        } 
        // else if (Input.GetKeyDown(KeyCode.Backspace)){
        //     // Get the current tile at the selected tile position
        //     TileBase currentTile = tilemap.GetTile(selectedTilePosition);

        //     // Create a new tile that uses the new sprite
        //     Tile newTile = ScriptableObject.CreateInstance<Tile>();
        //     newTile.sprite = grassSprite;

        //     // Change the sprite of the selected tile to the new sprite
        //     tilemap.SetTile(selectedTilePosition, newTile);
        // }
    }

    private void plantSeed(Vector3Int tilePosition, int i) {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Vector3Int storedTilePosition = tilePosition;

            // Get the current tile at the selected tile position
            TileBase currentTile = tilemap.GetTile(storedTilePosition);

            // Create a new tile that uses the new sprite
            Tile newTile = ScriptableObject.CreateInstance<Tile>();
            newTile.sprite = seedSprite;

            // Change the sprite of the selected tile to the new sprite
            for (int j = 0; j < tileDataList.Count; j++)
            {
                if (tileDataList[j].position == storedTilePosition && tileDataList[j].state == 1)
                {
                    tileDataList[j].state = 1;
                    tileDataList[j].seed = i;
                    tilemap.SetTile(storedTilePosition, newTile);
                    break;
                }
            }

            StartCoroutine(WaitAndChangeTileToMature(timeToGrow, storedTilePosition, i));
        }
    }

    private IEnumerator WaitAndChangeTileToMature(float waitTime, Vector3Int tilePosition, int i) {
        yield return new WaitForSeconds(waitTime);

        // Get the current tile at the selected tile position
        TileBase currentTile = tilemap.GetTile(tilePosition);

        // Create a new tile that uses the new sprite
        Tile newTile = ScriptableObject.CreateInstance<Tile>();

        newTile.sprite = matureSprite;
        tilemap.SetTile(tilePosition, newTile);

        // Make the plant harvestable
        growToHarvestable(tilePosition, i);
    }
    
    private void growToHarvestable(Vector3Int tilePosition, int i){
        StartCoroutine(WaitAndChangeTileToHarvestable(timeToGrow, tilePosition, i));
    }

    private IEnumerator WaitAndChangeTileToHarvestable(float waitTime, Vector3Int tilePosition, int i) {
        yield return new WaitForSeconds(waitTime);

        // Get the current tile at the selected tile position
        TileBase currentTile = tilemap.GetTile(tilePosition);

        // Create a new tile that uses the new sprite
        Tile newTile = ScriptableObject.CreateInstance<Tile>();

        // Change the sprite of the selected tile to the new sprite
        newTile.sprite = harvestableSprites[i-1];

        for (int j = 0; j < tileDataList.Count; j++)
        {
            if (tileDataList[j].position == tilePosition)
            {
                Debug.Log("Plant " + tilePosition + " is harvestable!");
                tileDataList[j].state = 2;
                tilemap.SetTile(tilePosition, newTile);
                break;
            }
        }

        StartCoroutine(WaitAndChangeCropToEvil(timeToEvil, tilePosition));
    }

    private IEnumerator WaitAndChangeCropToEvil(float waitTime, Vector3Int tilePosition) {
        yield return new WaitForSeconds(waitTime);

        // Get the current tile at the selected tile position
        TileBase currentTile = tilemap.GetTile(tilePosition);

        // Create a new tile that uses the new sprite
        Tile newTile = ScriptableObject.CreateInstance<Tile>();

        // Change the sprite of the selected tile to the new sprite
        newTile.sprite = mudSprite;

        for (int j = 0; j < tileDataList.Count; j++)
        {
            if (tileDataList[j].position == tilePosition && tileDataList[j].state == 2)
            {
                Debug.Log("Plant " + tilePosition + " is now EVIL!"); 
                tileDataList[j].state = 0;
                tileDataList[j].seed = 0;
                tilemap.SetTile(tilePosition, newTile);

                spawnedSprite = Instantiate(spritePrefab, tilePosition, Quaternion.identity);
                enemySpawned = true;
                break;
            }
        }
    }

    private void harvestPlant(Vector3Int tilePosition){

        if (Input.GetKeyDown(KeyCode.Space) && weaponIndex == 3)
        {
            // Get the current tile at the selected tile position
            TileBase currentTile = tilemap.GetTile(tilePosition);

            // Create a new tile that uses the new sprite
            Tile newTile = ScriptableObject.CreateInstance<Tile>();

            newTile.sprite = mudSprite;

            for (int j = 0; j < tileDataList.Count; j++)
            {
                if (tileDataList[j].position == tilePosition && tileDataList[j].state == 2 && tileDataList[j].seed == 1 || tileDataList[j].seed == 2)
                {   
                    vegetables[tileDataList[j].seed - 1] += Random.Range(1, 3);
                    Debug.Log("You now have " + seeds[tileDataList[j].seed] + ": " + vegetables[tileDataList[j].seed - 1]);

                    tileDataList[j].state = 0;
                    tileDataList[j].seed = 0;
                    tilemap.SetTile(tilePosition, newTile);
                    break;
                }
            }
        }
    }

    public class TileData
    {
        public Vector3Int position;
        public int state;
        public int seed;

        public TileData(Vector3Int position, int state, int seed)
        {
            this.position = position;
            this.state = state;
            this.seed = seed;
        }
    }
}

