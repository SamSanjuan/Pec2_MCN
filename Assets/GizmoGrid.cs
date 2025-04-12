using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoGrid : MonoBehaviour
{
    public int width = 20;
    public int height = 20;
    public float cellSize = 1f;
    public Vector3 offset = Vector3.zero;
    public Color lineColor = Color.gray;

    private List<LineRenderer> lineRenderers = new List<LineRenderer>();

    void Start()
    {
        UpdateGrid(); 
    }

    void Update()
    {     
        //UpdateGrid();
    }

    private void UpdateGrid()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        lineRenderers.Clear();

        for (int x = 0; x <= width; x++)
        {
            CreateLine(
                new Vector3(x * cellSize, 0, 0) + offset,
                new Vector3(x * cellSize, height * cellSize, 0) + offset
            );
        }

        for (int y = 0; y <= height; y++)
        {
            CreateLine(
                new Vector3(0, y * cellSize, 0) + offset,
                new Vector3(width * cellSize, y * cellSize, 0) + offset
            );
        }
    }

    private void CreateLine(Vector3 start, Vector3 end)
    {
        GameObject lineObj = new GameObject("GridLine");
        lineObj.transform.parent = this.transform;

        LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2; 
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        lineRenderer.startWidth = 0.05f; 
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); 
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;

        lineRenderers.Add(lineRenderer); 
    }

    public void RefreshGrid()
    {
        UpdateGrid();
    }
}