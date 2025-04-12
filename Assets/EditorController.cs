using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using System.Collections;
using System;

public class EditorController : MonoBehaviour
{
    [Header ("Prefabs Instanciables")]
    public GameObject playerPrefab;
    public GameObject wallPrefab;
    public GameObject boxPrefab;
    public GameObject goalPrefab;

    public TMP_InputField levelInputField;
    public TextMeshProUGUI levelDisplay;
    public TextMeshProUGUI saveDisplay;

    public enum ToolType { Player, Wall, Box, Goal, Eraser }
    public ToolType currentTool = ToolType.Wall;

    private Dictionary<Vector2, GameObject> placedTiles = new();
    public BoxCollider2D placementArea;

    public gameManager gm;

    private int currentLevel = 1;
    string FileName => $"Level{currentLevel}.json";

    public GizmoGrid gizmoGrid;

    void Start()
    {
        UpdateLevelDisplay();
        gm = FindAnyObjectByType<gameManager>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 gridPos = new Vector2(
                Mathf.Round(worldPos.x),
                Mathf.Round(worldPos.y)
            );


            if (!GridZone(worldPos))
            {
                Debug.Log("Fuera del grid");
                return;
            }

            /*
            if (!placementArea.OverlapPoint(gridPos))
            {
                Debug.Log("Zona fuera del Grid");
                return;
            }*/

            if (currentTool == ToolType.Eraser)
            {
                if (placedTiles.ContainsKey(gridPos))
                {
                    Destroy(placedTiles[gridPos]);
                    placedTiles.Remove(gridPos);
                }
            }
            else
            {
                if (placedTiles.ContainsKey(gridPos))
                {
                    Destroy(placedTiles[gridPos]);
                    placedTiles.Remove(gridPos);
                }

                GameObject prefab = GetPrefabFromTool(currentTool);
                if (prefab != null)
                {
                    GameObject placed = Instantiate(prefab, gridPos, Quaternion.identity);
                    placedTiles[gridPos] = placed;
                }
            }
        }
    }

    bool GridZone(Vector2 pos)
    {
        Vector2 localPos = pos - (Vector2)gizmoGrid.offset;

        float maxX = gizmoGrid.width * gizmoGrid.cellSize;
        float maxY = gizmoGrid.height * gizmoGrid.cellSize;

        return localPos.x >= 0 && localPos.x < maxX && localPos.y >= 0 && localPos.y < maxY;
    }
    GameObject GetPrefabFromTool(ToolType tool)
    {
        return tool switch
        {
            ToolType.Player => playerPrefab,
            ToolType.Wall => wallPrefab,
            ToolType.Box => boxPrefab,
            ToolType.Goal => goalPrefab,
            _ => null,
        };
    }

    string GetTypeFromPrefabName(string prefabName)
    {
        return prefabName switch
        {
            "Player" => "Player",
            "Wall" => "Wall",
            "Box" => "Box",
            "Goal" => "Goal",
            _ => "Unknown"
        };
    }

    GameObject GetPrefabFromName(string name)
    {
        return name switch
        {
            "Player" => playerPrefab,
            "Wall" => wallPrefab,
            "Box" => boxPrefab,
            "Goal" => goalPrefab,
            _ => null
        };
    }

    public void SaveLevel()
    {
        LevelData level = new();

        foreach (var pair in placedTiles)
        {
            GameObject obj = pair.Value;
            string type = GetTypeFromPrefabName(obj.name.Replace("(Clone)", "").Trim());

            TileData tile = new TileData
            {
                position = pair.Key,
                type = type
            };

            level.tiles.Add(tile);
        }

        string json = JsonUtility.ToJson(level, true);
        File.WriteAllText(Application.dataPath + "/Resources/" + FileName, json);
        StartCoroutine(FeedBackLoadAndSaving(FileName, 1));
        Debug.Log("Nivel guardado en " + FileName);
    }

    IEnumerator FeedBackLoadAndSaving(string fileName, int type)
    {
        if (type == 1) saveDisplay.text = ("Save in " + fileName);
        else saveDisplay.text = ("Load level " + fileName);
        saveDisplay.gameObject.SetActive(true);

        yield return new WaitForSeconds(2.23f);
        saveDisplay.gameObject.SetActive(false);
    }


    public void LoadLevel()
    {
        string path = Application.dataPath + "/Resources/" + FileName;
        if (!File.Exists(path))
        {
            Debug.LogWarning("Archivo no encontrado: " + path);
            return;
        }

        string json = File.ReadAllText(path);
        LevelData level = JsonUtility.FromJson<LevelData>(json);

        ClearLevel();

        foreach (TileData tile in level.tiles)
        {
            GameObject prefab = GetPrefabFromName(tile.type);
            if (prefab != null)
            {
                GameObject placed = Instantiate(prefab, tile.position, Quaternion.identity);
                placedTiles[tile.position] = placed;
            }
        }
        StartCoroutine(FeedBackLoadAndSaving(FileName, 2));
        Debug.Log("Nivel cargado desde " + FileName);
    }

    public void LoadLevelInit()
    {
        StartCoroutine(LoadLevelCor());
    }


    private IEnumerator LoadLevelCor()
    {
        ClearLevel();
        yield return new WaitForSeconds(.32f);
        LoadLevel();
    }

    public void ClearLevel()
    {
        foreach (var tile in placedTiles.Values)
        {
            Destroy(tile);
        }
        placedTiles.Clear();
    }

    public void SetTool(string toolName)
    {
        if (System.Enum.TryParse(toolName, out ToolType tool))
        {
            currentTool = tool;
        }
    }

    public void ConfirmLevel()
    {
        if (int.TryParse(levelInputField.text, out int newLevel) && newLevel > 0 && newLevel <= 10)
        {
            currentLevel = newLevel;
            gm.actualLevel.text = currentLevel.ToString();
            UpdateLevelDisplay();
        }
        else
        {
            Debug.LogWarning("Número de nivel inválido.");
        }
    }

    void UpdateLevelDisplay()
    {
        if (levelDisplay != null)
        {
            levelDisplay.text = $"Current level: {currentLevel}";
        }
    }

    public void ResetCounters()
    {
        gm.playerMoves = 0;
        gm.playerPushes = 0;
        gm.time = 0;
    }
}

