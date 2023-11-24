using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A* pathfinding depending on overlay tiles.
/// </summary>
public class PathFinder
{
    public List<NodeTile> FindPath(NodeTile start, NodeTile end, bool diagonal = false, bool blockable = true)
    {
        List<NodeTile> openList = new List<NodeTile>();
        List<NodeTile> closedList = new List<NodeTile>();
        NodeTile currentNodeTile = null;
              
        start.G = 0;
        start.H = GetDistance(start, end, diagonal);
        start.Previous = null;
        openList.Add(start);

        while (openList.Count > 0)
        {
            //Apply penalty cost for changing direction to avoid too many zig-zag.
            if (diagonal)
            {
                foreach (NodeTile candidate in openList)
                {
                    if (currentNodeTile != null)
                    {
                        NodeTile preTile = currentNodeTile.Previous;
                        if (preTile != null)
                        {
                            Vector3 curDistance = candidate.GridLocation - currentNodeTile.GridLocation;
                            Vector3 preDistance = currentNodeTile.GridLocation - preTile.GridLocation;
                            Vector3 curDirection = curDistance.normalized;
                            Vector3 preDirection = preDistance.normalized;

                            if (curDirection != preDirection)
                            {
                                candidate.P += 5;
                            }
                        }
                    }
                }
            }          

            currentNodeTile = openList.OrderBy(x => x.F).First();

            if (currentNodeTile == end)
            {
                return GetFinishedList(start, end);
            }

            openList.Remove(currentNodeTile);
            closedList.Add(currentNodeTile);

            foreach (var tile in GetNeightbourNodeTiles(currentNodeTile, diagonal))
            {
                if ((blockable && tile.IsBlocked) || tile.IsOccupied || closedList.Contains(tile))
                {
                    continue;
                }

                int tentativeGCost = currentNodeTile.G + GetDistance(currentNodeTile, tile, diagonal);
                if (tentativeGCost < tile.G || !openList.Contains(tile))
                {
                    tile.Previous = currentNodeTile;
                    tile.G = GetDistance(start, tile, diagonal);
                    tile.H = GetDistance(end, tile, diagonal);

                    if (!openList.Contains(tile))
                    {
                        openList.Add(tile);
                    }
                }            
            }
        }

        return new List<NodeTile>();
    }

    public List<NodeTile> GetNeightbourNodeTiles(NodeTile currentNodeTile, bool diagonal = false)
    {
        var map = MapManager.Instance.Map;

        List<NodeTile> neighbours = new List<NodeTile>();
        int[] hasNeighbours = {0, 0, 0, 0, 0, 0, 0, 0};

        //Top
        Vector2Int locationToCheck = new Vector2Int(
            currentNodeTile.GridLocation.x,
            currentNodeTile.GridLocation.y + 1
        );
        if (map.ContainsKey(locationToCheck))
        {
            map[locationToCheck].P = 0;
            neighbours.Add(map[locationToCheck]);
            hasNeighbours[1] = 1;
        }

        //Right
        locationToCheck = new Vector2Int(
            currentNodeTile.GridLocation.x + 1,
            currentNodeTile.GridLocation.y
        );
        if (map.ContainsKey(locationToCheck))
        {
            map[locationToCheck].P = 0;
            neighbours.Add(map[locationToCheck]);
            hasNeighbours[3] = 1;
        }

        //Bottom
        locationToCheck = new Vector2Int(
            currentNodeTile.GridLocation.x,
            currentNodeTile.GridLocation.y - 1
        );
        if (map.ContainsKey(locationToCheck))
        {
            map[locationToCheck].P = 0;
            neighbours.Add(map[locationToCheck]);
            hasNeighbours[5] = 1;
        }

        //Left
        locationToCheck = new Vector2Int(
            currentNodeTile.GridLocation.x - 1,
            currentNodeTile.GridLocation.y
        );
        if (map.ContainsKey(locationToCheck))
        {
            map[locationToCheck].P = 0;
            neighbours.Add(map[locationToCheck]);
            hasNeighbours[7] = 1;
        }

        if (diagonal)
        {
            //Top right
            locationToCheck = new Vector2Int(
                currentNodeTile.GridLocation.x + 1,
                currentNodeTile.GridLocation.y + 1
            );
            if (map.ContainsKey(locationToCheck))
            {
                map[locationToCheck].P = 0;
                //Avoid cutting corner of impassable tiles.
                if (hasNeighbours[1] == 1 && hasNeighbours[3] == 1)
                {
                    neighbours.Add(map[locationToCheck]);
                }               
            }

            //Top left
            locationToCheck = new Vector2Int(
                currentNodeTile.GridLocation.x - 1,
                currentNodeTile.GridLocation.y + 1
            );
            if (map.ContainsKey(locationToCheck))
            {
                map[locationToCheck].P = 0;
                //Avoid cutting corner of impassable tiles.
                if (hasNeighbours[1] == 1 && hasNeighbours[7] == 1)
                {
                    neighbours.Add(map[locationToCheck]);
                }                
            }

            //Bottom right
            locationToCheck = new Vector2Int(
                currentNodeTile.GridLocation.x + 1,
                currentNodeTile.GridLocation.y - 1
            );
            if (map.ContainsKey(locationToCheck))
            {
                map[locationToCheck].P = 0;
                //Avoid cutting corner of impassable tiles.
                if (hasNeighbours[3] == 1 && hasNeighbours[5] == 1)
                {
                    neighbours.Add(map[locationToCheck]);
                }        
               
            }

            //Bottom left
            locationToCheck = new Vector2Int(
                currentNodeTile.GridLocation.x - 1,
                currentNodeTile.GridLocation.y - 1
            );
            if (map.ContainsKey(locationToCheck))
            {
                map[locationToCheck].P = 0;
                //Avoid cutting corner of impassable tiles.
                if (hasNeighbours[5] == 1 && hasNeighbours[7] == 1)
                {
                    neighbours.Add(map[locationToCheck]);
                }
               
            }
        }

        CommonTools.Shuffle(neighbours);
        return neighbours;
    }

    private List<NodeTile> GetFinishedList(NodeTile start, NodeTile end)
    {
        List<NodeTile> finishedList = new List<NodeTile>();
        NodeTile currentTile = end;

        while (currentTile != start)
        {
            finishedList.Add(currentTile);
            currentTile = currentTile.Previous;
        }

        finishedList.Reverse();
        return finishedList;
    }

    private int GetDistance(NodeTile start, NodeTile tile, bool diagonal)
    {
        if (diagonal)
        {
            return GetDiagonalDistance(start, tile);
        }
        else
        {
            return GetManhattenDistance(start, tile);
        }
    }

    //The distance between two points is reprsented by
    //the sum of the absolute differences of their coordinates.
    //This works only with 4 direction movement.
    private int GetManhattenDistance(NodeTile start, NodeTile tile)
    {
        var result = Mathf.Abs(start.GridLocation.x - tile.GridLocation.x) + Mathf.Abs(start.GridLocation.y - tile.GridLocation.y);
        return result;
    }

    //Take diagonal movement into account.
    private int GetDiagonalDistance(NodeTile start, NodeTile tile)
    {
        int deltaX = Mathf.Abs(start.GridLocation.x - tile.GridLocation.x);
        int deltaY = Mathf.Abs(start.GridLocation.y - tile.GridLocation.y);
        int remaining = Mathf.Abs(deltaX - deltaY);

        int straightCost = 10; 
        int diagonalCost = 14; //square root of 200 approximately, more effective than just square root of 2
        int result = diagonalCost * Mathf.Min(deltaX, deltaY) + straightCost * remaining;

        return result;
    }

}
