using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //Define delegate types and events here

    public Node CurrentNode { get; private set; }
    public Node TargetNode { get; private set; }

    public bool directionNotFound = false;

    private LayerMask mask;

    public int ID_out;


    [SerializeField] private float speed = 4;

    public bool moving = false;
    //similar movement to ai without the pathfinding algorithm, wasd
    private Vector3 currentDir;

    // Start is called before the first frame update
    void Start()
    {
        //access game manager instnace and it goes throguh all nodes that contain nodes list adn checks if the amount of children is equal to 0, essentially grab one nodes 6
        //for instance has three parents
        foreach (Node node in GameManager.Instance.Nodes)
        {
            if (node.Parents.Length > 2 && node.Children.Length == 0)
            {
                CurrentNode = node;
                break;
            }
        }
        mask = LayerMask.GetMask("Ignore Raycast");
        ID_out = -1;
    }

    // Update is called once per frame
    void Update()
    {
       // Debug.Log(moving);

        if (moving == false)
        {
            //detect if movement
            //check if any events receieved from the buttons if so, which node is the current node, set moving to true. if moving is true well go to else, say that our distance.
            //if distance is greater than 0.25f. When the game starts the player will be sitting, moving will set to false until a button is pressed.
            //Implement inputs and event-callbacks here\
            PlayerMoveInput();
        }
        else
        {
            if (Vector3.Distance(transform.position, TargetNode.transform.position) > 0.25f)
            {
                transform.Translate(currentDir * speed * Time.deltaTime);
            }
            else
            {
                moving = false;
                CurrentNode = TargetNode;
            }
        }
    }

    //Implement mouse interaction method here
    //kinda relates to raycasting, how can you have a raycast from our cursor position where it is on screen and work out if mouse is covering something. mouse distance from 3d object.


    /// <summary>
    /// Sets the players target node and current directon to the specified node.
    /// </summary>
    /// <param name="node"></param>
    public void MoveToNode(Node node)
    {
        if (moving == false)
        {
            TargetNode = node;
            currentDir = TargetNode.transform.position - transform.position;
            currentDir = currentDir.normalized;
            moving = true;
        }
    }


    /*
     * horizontal and vertical inputs are on an axis from 1 to -1 
     * if else statement looks for player input and calls the method with the corisponding directional vector3
     */
    public void PlayerMoveInput()
    {
        if (Input.GetAxis("Horizontal") < 0)  //left
        {
            CheckForNode(-Vector3.right);
            ID_out = 3;
        }
        else if (Input.GetAxis("Horizontal") > 0)  // right
        {
            CheckForNode(Vector3.right);
            ID_out = 1;
        }
        else if (Input.GetAxis("Vertical") < 0) //down
        {
            CheckForNode(-Vector3.forward);
            ID_out = 2;
        }
        else if (Input.GetAxis("Vertical") > 0) //up
        {
            CheckForNode(Vector3.forward);
            ID_out = 0;
        }
    }

    public void PlayerMouseInput(int direction)
    {
        /*take in int to determine direction
        * 0 = North
        * 1 = East
        * 2 = South
        * 3 = West
        * 
        * for button integration
        */

        switch (direction)
        {
            case 0:
                CheckForNode(Vector3.forward);
                return;
            case 1:
                CheckForNode(Vector3.right);
                return;
            case 2:
                CheckForNode(-Vector3.forward);
                return;
            case 3:
                CheckForNode(-Vector3.right);
                return;
        }
    }

    public void CheckForNode(Vector3 checkDirection)
    {

        RaycastHit hit;
        Node node;

        if (Physics.Raycast(transform.position, checkDirection, out hit, 50f))  // cast raycast in direction of the direction passed throught the method
        {
            if (hit.collider.TryGetComponent<Node>(out node)) //checks if hit a collider with the node script attatched
            {
                Debug.DrawRay(transform.position, checkDirection * 1000, Color.white, mask); // debug ray
                MoveToNode(node);
                Debug.Log("HIT NODE");
            }
        }
        else
        {
            Debug.Log("NO HIT NODE");
            directionNotFound = true;  //for use with button script
        }
    }
}
    


