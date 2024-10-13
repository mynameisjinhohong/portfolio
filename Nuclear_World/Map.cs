using Array2DEditor;
using CodingSystem_HJH;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapRow : CellRow<Cell> { }

[System.Serializable]
public class Map : Array2D<Cell>
{
    [SerializeField]
    MapRow[] cells = new MapRow[Consts.defaultGridSize];

    protected override CellRow<Cell> GetCellRow(int idx)
    {
        return cells[idx];
    }
}
