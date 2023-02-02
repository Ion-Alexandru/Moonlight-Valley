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

    public float speed;

    private void Start() {
    }

    private void Update() {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 direction = new Vector2(horizontal, vertical);
        if (direction.sqrMagnitude > 0) {
            lastDirection = direction;
            selectTile();
        }

        transform.position = transform.position + new Vector3(horizontal, vertical, 0) * speed * Time.deltaTime;

        changeTileSprite();
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
}