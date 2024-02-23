using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    [SerializeField] public int _width, _height;
    [SerializeField] Tile _tile;
    [SerializeField] Transform _camera;
    Dictionary<Vector2, Tile> _tiles;

    void Awake(){

        Instance = this;
    }


    public void GenerateGridWhite(){

        _tiles = new Dictionary<Vector2, Tile>();

        for(int y = 0; y < _height; y++){

            for(int x = 0; x < _width; x++){

                var spawnedTile = Instantiate(_tile, new Vector3(x, y), Quaternion.identity);

                spawnedTile.name = $"Tile {x + 1} {y + 1}";

                var isOffset = (x + y) % 2 == 1;
                spawnedTile.Init(isOffset);

                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }

        _camera.transform.position = new Vector3((float) _width / 2 - 0.5f, (float) _height / 2 - 0.5f, -10);

        GameManager.Instance.UpdateGameState(GameState.SpawnPiecesWhite);
    }

    public void GenerateGridBlack(){

        _tiles = new Dictionary<Vector2, Tile>();

        for(int y = _height - 1; y >= 0; y--){

            for(int x = _width - 1; x >= 0; x--){

                var spawnedTile = Instantiate(_tile, new Vector3(x, y), Quaternion.identity);

                spawnedTile.name = $"Tile {x + 1} {y + 1}";

                var isOffset = (x + y) % 2 == 1;
                spawnedTile.Init(isOffset);

                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }

        _camera.transform.position = new Vector3((float) _width / 2 - 0.5f, (float) _height / 2 - 0.5f, -10);

        GameManager.Instance.UpdateGameState(GameState.SpawnPiecesBlack);
    }

    public Tile GetPieceSpawnTile(int xtemp, int ytemp){
        
        return _tiles.Where(t => t.Key.x == xtemp && t.Key.y == ytemp).Select(t => t.Value).FirstOrDefault();
    }


    public Tile GetTile(Vector2 position){

        foreach(var kvp in _tiles){

            if(Vector2.Distance(kvp.Key, position) < 0.5f){
                return kvp.Value;
            }
        }

        return null;
    }
}
