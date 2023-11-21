using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Script that manages tilemap data and overlay.
/// </summary>
public class MapManager : SimpleSingleton<MapManager>
{
    [SerializeField] private Tilemap tilemapPathLayer;
    [SerializeField] private Tilemap tilemapMiddleLayer;
    [SerializeField] private Tilemap tilemapTopLayer;

    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private GameObject nodeContainer;

    [SerializeField] private Dictionary<Vector2Int, NodeTile> map;
    public Dictionary<Vector2Int, NodeTile> Map { get { return map; } }

    public void Init()
    {
        map = new Dictionary<Vector2Int, NodeTile>();
        List<TileInfo> tileInfoList = tilemapPathLayer.GetTileMapInfo();
        int index = 0;

        foreach (TileInfo tileInfo in tileInfoList)
        {
            if (tilemapPathLayer.HasTile(tileInfo.Coordinates))
            {
                if (!map.ContainsKey(tileInfo.Coordinates2D))
                {
                    GameObject nodeTile = Instantiate(nodePrefab, nodeContainer.transform);

                    nodeTile.transform.position = tileInfo.WorldPoint;
                    //overlayTile.GetComponent<SpriteRenderer>().sortingOrder = tilemapMiddleLayer.GetComponent<TilemapRenderer>().sortingOrder + 1;
                    nodeTile.gameObject.GetComponent<NodeTile>().GridLocation = tileInfo.Coordinates;
                    nodeTile.gameObject.name = "NodeTile" + index;

                    map.Add(tileInfo.Coordinates2D, nodeTile.gameObject.GetComponent<NodeTile>());
                    index++;
                }
            }
        }
    }

    public Vector3 GetNearestTileWorldPosition(Vector3 position)
    {
        Vector3Int posToCell = new Vector3Int();
        Vector3 cellWorldPos = new Vector3();

        posToCell = tilemapPathLayer.WorldToCell(position);
        cellWorldPos = tilemapPathLayer.CellToWorld(posToCell);

        return cellWorldPos;
    }

    public NodeTile GetNearestNodeTile(Vector3 position)
    {
        Vector3Int posToCell = new Vector3Int();
        Vector2Int posToCell2D = new Vector2Int();
        NodeTile result;

        posToCell = tilemapPathLayer.WorldToCell(position);
        posToCell2D = new Vector2Int(posToCell.x, posToCell.y);

        if (map.ContainsKey(posToCell2D))
        {
            result = map[posToCell2D];
        }
        else
        {
            result = null;
        }

        return result;
    }

    public NodeTile GetRandomWalkableTile()
    {
        NodeTile result = null;
        List<NodeTile> randList = new List<NodeTile>();

        foreach (NodeTile tile in map.Values)
        {
            if (!tile.IsBlocked && !tile.IsOccupied)
            {
                randList.Add(tile);
            }
        }

        CommonTools.Shuffle(randList);

        if(randList.Count > 0)
        {
            result = randList[0];
        }      
        return result;
    }
}
