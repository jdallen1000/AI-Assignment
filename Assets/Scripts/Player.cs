using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //Define delegate types and events here

    public Node CurrentNode { get; private set; }
    public Node TargetNode { get; private set; }

    [SerializeField] private float speed = 4;
   
    private bool moving = false;
    //similar movement to ai without the pathfinding algorithm, wasd
    private Vector3 currentDir;

    // Start is called before the first frame update
    void Start()
    {
        //access game manager instnace and it goes throguh all nodes that contain nodes list adn checks if the amount of children is equal to 0, essentially grab one nodes 6
        //for instance has three parents
        foreach (Node node in GameManager.Instance.Nodes)
        {
            if(node.Parents.Length > 2 && node.Children.Length == 0)
            {
                CurrentNode = node;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (moving == false)
        {
                //detect if movement
                //check if any events receieved from the buttons if so, which node is the current node, set moving to true. if moving is true well go to else, say that our distance.
                //if distance is greater than 0.25f. When the game starts the player will be sitting, moving will set to false until a button is pressed.
                //Implement inputs and event-callbacks here
            
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

    public void MoveUp()
    {
        moving = true;
        CurrentNode.Parents[0] = TargetNode;
        
    }
}
