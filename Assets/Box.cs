using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    private GameObject[] Walls;
    private GameObject[] Boxs;
    private gameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        Walls = GameObject.FindGameObjectsWithTag("Wall");
        Boxs = GameObject.FindGameObjectsWithTag("Box");
        gm = FindAnyObjectByType<gameManager>();
    }

    public bool MovementController(Vector2 direction)
    {
        if (ObjBlocked(transform.position, direction)) return false;
        else
        {
            transform.Translate(direction);
            gm.playerPushes++;
            return true;
        }
    }

    public bool ObjBlocked(Vector3 position, Vector2 direction)
    {
        Vector2 newPosition = new Vector2(position.x, position.y) + direction;

        foreach (var wall in Walls)
        {
            if (wall.transform.position.x == newPosition.x && wall.transform.position.y == newPosition.y)
            {
                return true;
            }
        }

        foreach (var box in Boxs)
        {
            if (box.transform.position.x == newPosition.x && box.transform.position.y == newPosition.y)
            {
                return true;
    
            }
        }
        return false;
    }
}
