using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GameObject gridLinePrefab;
    [SerializeField] private Transform gridContainer;
    [SerializeField] private Material gridLineMaterial;

    private GridSetting setting;
    private List<GameObject> _gridLines = new List<GameObject>();

    public void CreatGrid(GridSetting setting)
    {
        this.setting = setting;
        ClearGrid();

        //가로 선 생성
        for (int i = 0; i < setting.row; i++)
        {
            float yPos = i * setting.cellSize;
            CreateGridLine(new Vector3(0, yPos, 0),
                new Vector3(setting.column * setting.cellSize, yPos, 0));
        }

        //새로 선 생성
        for (int i = 0; i < setting.column; i++)
        {
            float xPos = i * setting.cellSize;
            CreateGridLine(new Vector3(xPos, 0, 0),
                         new Vector3(xPos, setting.row * setting.cellSize, 0));
        }
    }

    private void CreateGridLine(Vector3 start, Vector3 end)
    {
        GameObject line = Instantiate(gridLinePrefab, gridContainer);
        LineRenderer renderer = line.GetComponent<LineRenderer>();

        renderer.material = gridLineMaterial;
        renderer.startWidth = 0.02f;
        renderer.endWidth = 0.02f;
        renderer.SetPosition(0, start);
        renderer.SetPosition(1, end);

        _gridLines.Add(line);
    }

    private void ClearGrid()
    {
        foreach (var line in _gridLines)
        {
            Destroy(line);
        }
        _gridLines.Clear();
    }
}
