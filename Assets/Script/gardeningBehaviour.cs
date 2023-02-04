using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class gardeningBehaviour : MonoBehaviour
{
    public Tilemap tilemap;
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
    int i = 0;
    private bool isHoed;
    private bool isHoeInHand = false;
    private bool isSeedInHand = false;
    private bool isScytheInHand = false;
    float startTime;

    private void Start() {
    }

    private void Update() {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if(Input.GetKeyDown(KeyCode.UpArrow) && i < 5){
            i += 1;
            Debug.Log("Current seed is:" + i);
        } else if (Input.GetKeyDown(KeyCode.DownArrow) && i > 0){
            i -= 1;
            Debug.Log("Current seed is:" + i);
        }

        Vector2 direction = new Vector2(horizontal, vertical);
        if (direction.sqrMagnitude > 0) {
            lastDirection = direction;
            selectTile();
        }

        transform.position = transform.position + new Vector3(horizontal, vertical, 0) * speed * Time.deltaTime;

        if (isHoeInHand){
            mudOutline.transform.position = tilemap.GetCellCenterWorld(selectedTilePosition);

            changeTileSprite();
        }

        if (isSeedInHand){
            seedOutline.transform.position = tilemap.GetCellCenterWorld(selectedTilePosition);

            plantSeed(selectedTilePosition, i);
        }

        if(Input.GetKeyDown(KeyCode.Alpha1) && isHoeInHand == false){
            isHoeInHand = true;
            mudOutline.SetActive(true);

        } else if(Input.GetKeyDown(KeyCode.Alpha1) && isHoeInHand) {
            isHoeInHand = false;
            mudOutline.SetActive(false);
        }

        if(Input.GetKeyDown(KeyCode.Alpha2) && isSeedInHand == false){
            isSeedInHand = true;
            seedOutline.SetActive(true);

        } else if(Input.GetKeyDown(KeyCode.Alpha2) && isSeedInHand) {
            isSeedInHand = false;
            seedOutline.SetActive(false);
        }

        if(Input.GetKeyDown(KeyCode.Alpha3) && isScytheInHand == false){
            isScytheInHand = true;
            mudOutline.SetActive(true);

        } else if(Input.GetKeyDown(KeyCode.Alpha3) && isScytheInHand) {
            isScytheInHand = false;
            mudOutline.SetActive(false);
        }
    }

    private void selectTile() {
        Vector3Int cellPosition = tilemap.WorldToCell(transform.position);
        selectedTilePosition = cellPosition + Vector3Int.RoundToInt(lastDirection);
    }

    private void changeTileSprite() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            // Get the current tile at the selected tile position
            TileBase currentTile = tilemap.GetTile(selectedTilePosition);

            // Create a new tile that uses the new sprite
            Tile newTile = ScriptableObject.CreateInstance<Tile>();
            newTile.sprite = mudSprite;

            // Change the sprite of the selected tile to the new sprite
            tilemap.SetTile(selectedTilePosition, newTile);

            Debug.Log("Tile position is: " + selectedTilePosition);
        } else if (Input.GetKeyDown(KeyCode.Backspace)){
            // Get the current tile at the selected tile position
            TileBase currentTile = tilemap.GetTile(selectedTilePosition);

            // Create a new tile that uses the new sprite
            Tile newTile = ScriptableObject.CreateInstance<Tile>();
            newTile.sprite = grassSprite;

            // Change the sprite of the selected tile to the new sprite
            tilemap.SetTile(selectedTilePosition, newTile);
        }
    }

    private void plantSeed(Vector3Int tilePosition, int i) {
        if (Input.GetKeyDown(KeyCode.F)) {
            // Get the current tile at the selected tile position
            TileBase currentTile = tilemap.GetTile(tilePosition);

            // Create a new tile that uses the new sprite
            Tile newTile = ScriptableObject.CreateInstance<Tile>();
            newTile.sprite = seedSprite;

            // Change the sprite of the selected tile to the new sprite
            tilemap.SetTile(tilePosition, newTile);

            StartCoroutine(WaitAndChangeTileToMature(10f, tilePosition, i));

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
        Debug.Log("Plant " + tilePosition + " is mature!");

        // Make the plant harvestable
        harvestPlant(tilePosition, i);
    }
    
    private void harvestPlant(Vector3Int tilePosition, int i){
        // Get the current tile at the selected tile position
        TileBase currentTile = tilemap.GetTile(tilePosition);

        // Create a new tile that uses the new sprite
        Tile newTile = ScriptableObject.CreateInstance<Tile>();
        newTile.sprite = seedSprite;

        // Change the sprite of the selected tile to the new sprite
        tilemap.SetTile(selectedTilePosition, newTile);

        StartCoroutine(WaitAndChangeTileToHarvestable(10f, tilePosition, i));
    }

    private IEnumerator WaitAndChangeTileToHarvestable(float waitTime, Vector3Int tilePosition, int i) {
        yield return new WaitForSeconds(waitTime);

        // Get the current tile at the selected tile position
        TileBase currentTile = tilemap.GetTile(tilePosition);

        // Create a new tile that uses the new sprite
        Tile newTile = ScriptableObject.CreateInstance<Tile>();

        // Change the sprite of the selected tile to the new sprite
        newTile.sprite = harvestableSprites[i];

        tilemap.SetTile(tilePosition, newTile);
        Debug.Log("Plant " + tilePosition + " is harvestable!");

        if(tilemap.GetTile(selectedTilePosition) == currentTile && Input.GetKeyDown(KeyCode.E) && isScytheInHand){
            newTile.sprite = mudSprite;

            tilemap.SetTile(tilePosition, newTile);
            Debug.Log("Hurray!");
        }
    }
}