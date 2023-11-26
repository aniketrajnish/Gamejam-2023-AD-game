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
    [SerializeField] private List<Tilemap> tilemapPathLayers;

    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private GameObject nodeContainer;

    [SerializeField] private List<Dictionary<Vector2Int, NodeTile>> mapList;

    private Tilemap currentPathTilemap;
    private Dictionary<Vector2Int, NodeTile> currentMap;

    public Dictionary<Vector2Int, NodeTile> Map { get { return currentMap; } }

    public void Init()
    {
        mapList = new List<Dictionary<Vector2Int, NodeTile>>();
        GeneratePathTiles();
    }

    public Vector3 GetNearestTileWorldPosition(Vector3 position)
    {
        Vector3Int posToCell = new Vector3Int();
        Vector3 cellWorldPos = new Vector3();

        posToCell = currentPathTilemap.WorldToCell(position);
        cellWorldPos = currentPathTilemap.CellToWorld(posToCell);

        return cellWorldPos;
    }

    public NodeTile GetNearestNodeTile(Vector3 position)
    {
        Vector3Int posToCell = new Vector3Int();
        Vector2Int posToCell2D = new Vector2Int();
        NodeTile result;

        posToCell = currentPathTilemap.WorldToCell(position);
        posToCell2D = new Vector2Int(posToCell.x, posToCell.y);

        if (currentMap.ContainsKey(posToCell2D))
        {
            result = currentMap[posToCell2D];
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

        foreach (NodeTile tile in currentMap.Values)
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

    public void ChangePathMap(int index)
    {
        currentMap = mapList[index];
        currentPathTilemap = tilemapPathLayers[index];
    }

    private void GeneratePathTiles()
    {
        for(int i = 0; i<tilemapPathLayers.Count; i++)
        {
            var map = new Dictionary<Vector2Int, NodeTile>();
            List<TileInfo> tileInfoList = tilemapPathLayers[i].GetTileMapInfo();
            
            int index = 0;
            foreach (TileInfo tileInfo in tileInfoList)
            {
                if (tilemapPathLayers[i].HasTile(tileInfo.Coordinates))
                {
                    if (!map.ContainsKey(tileInfo.Coordinates2D))
                    {
                        GameObject nodeTile = Instantiate(nodePrefab, nodeContainer.transform);

                        nodeTile.transform.position = tileInfo.WorldPoint;
                        nodeTile.gameObject.GetComponent<NodeTile>().GridLocation = tileInfo.Coordinates;
                        nodeTile.gameObject.name = "NodeTile" + index;

                        map.Add(tileInfo.Coordinates2D, nodeTile.gameObject.GetComponent<NodeTile>());
                        index++;
                    }
                }
            }
            mapList.Add(map);
        }

        currentPathTilemap = tilemapPathLayers[0];
        currentMap = mapList[0];
    }
}
