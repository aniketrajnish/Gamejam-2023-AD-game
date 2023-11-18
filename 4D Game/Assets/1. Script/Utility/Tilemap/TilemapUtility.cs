using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class TileInfo
{
    public Vector3Int Coordinates;
    public Vector2Int Coordinates2D;
    public Vector3 WorldPoint;
    public Tile Tile;
}

/// <summary>
/// Extension of tilemap for receiving more tile information.
/// </summary>
public static class TileMapUtility
{
    public static List<TileInfo> GetTileMapInfo(this Tilemap tilemap)
    {
        List<TileInfo> tileInfoList = new List<TileInfo>();

        for (int y = tilemap.origin.y; y < (tilemap.origin.y + tilemap.size.y); y++)
        {
            for (int x = tilemap.origin.x; x < (tilemap.origin.x + tilemap.size.x); x++)
            {
                TileInfo tileInfo = new TileInfo();
                Vector3Int coordinates = new Vector3Int(x, y, tilemap.origin.z);
                Tile tile = tilemap.GetTile<Tile>(coordinates);

                if (tile != null)
                {
                    tileInfo.Coordinates = coordinates;
                    tileInfo.Coordinates2D = new Vector2Int(coordinates.x, coordinates.y);

                    Vector3 worldPoint = tilemap.CellToWorld(coordinates);
                    tileInfo.WorldPoint = worldPoint;

                    tileInfo.Tile = tile;

                    tileInfoList.Add(tileInfo);
                }
            }
        }

        return tileInfoList;
    }
}

