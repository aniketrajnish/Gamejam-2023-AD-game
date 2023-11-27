using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Tilemaps;
[CreateAssetMenu(fileName = "Prefab Brush", menuName = "Brushes/Prefab Brush")]
[CustomGridBrush(false, true, false, "Prefab Brush")]
public class PrefabBrush : GameObjectBrush
{
    public override void Erase(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
    {
        //base.Erase(gridLayout, brushTarget, position);
        if(brushTarget.layer == 31)
        {
            return;
        }
        
        Transform erased = 
            GetObjectInCell(gridLayout, brushTarget.transform, new Vector3Int(position.x, position.y ,0));

        if(erased != null)
        {
             Undo.DestroyObjectImmediate(erased.gameObject);
        }
    }

    private static Transform GetObjectInCell(GridLayout gridLayout, Transform parent, Vector3Int position)
    {
        if (parent == null)
            return null;

        Vector3 min = gridLayout.LocalToWorld(gridLayout.CellToLocalInterpolated(position));
        Vector3 max = gridLayout.LocalToWorld(gridLayout.CellToLocalInterpolated(position + Vector3Int.one));
        Bounds bounds = new Bounds((max + min) * .5f, max - min);

        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            
            if (bounds.Contains(child.position))
            {
                return child;
            }
        }
        return null;
    }
}
#endif
