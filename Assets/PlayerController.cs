using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject[] Walls;
    public GameObject[] Boxs;
    private gameManager gm;
    private Vector3 actualPosition;
    private Vector3 newPosition;

    private bool canMove = false;
    // Start is called before the first frame update
    void Start()
    {
        gm = FindAnyObjectByType<gameManager>();
        actualPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        movementInput.Normalize();

        if (movementInput.sqrMagnitude > 0.5)
        {
            if (canMove)
            {
                canMove = false;
                MovementController(movementInput);

                if (transform.position != newPosition)
                {
                    newPosition = transform.position;
                    gm.playerMoves++;
                }
            } 
        }
        else canMove = true;
    }
    
    public bool MovementController(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) < 0.5) direction.x = 0;
        else
        {
            direction.y = 0;

        }
        direction.Normalize();

        if (Blocked(transform.position, direction)) return false;
        else
        {
            // transform.Translate(direction);
            transform.position += new Vector3(direction.x, direction.y, 0);
            return true;
        } 
    }

    public bool Blocked(Vector3 position, Vector2 direction)
    {
        Vector2 newPosition = new Vector2(position.x, position.y) + direction;
         Walls = GameObject.FindGameObjectsWithTag("Wall");

        foreach (var wall in Walls)
        {
            if(wall.transform.position.x == newPosition.x && wall.transform.position.y == newPosition.y)
            {
                return true;
            }
        }

        Boxs = GameObject.FindGameObjectsWithTag("Box");
        foreach (var box in Boxs)
        {
            if(box.transform.position.x == newPosition.x && box.transform.position.y == newPosition.y)
            {
                Box objBox = box.GetComponent<Box>();
                if (objBox && objBox.MovementController(direction)) return false;
                else return true;
            }
        } return false;
    }
}
