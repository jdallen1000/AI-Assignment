using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Tooltip("Movement speed modifier.")]
    [SerializeField] private float speed = 3;
    private Node currentNode;
    private Vector3 currentDir;
    private bool playerCaught = false;

    public delegate void GameEndDelegate();
    public event GameEndDelegate GameOverEvent = delegate { };





    // Start is called before the first frame update
    void Start()
    {
        InitializeAgent();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCaught == false)
        {
            if (currentNode != null)
            {
                //If within 0.25 units of the current node.
                if (Vector3.Distance(transform.position, currentNode.transform.position) > 0.25f)
                {
                    currentDir = currentNode.transform.position - transform.position;
                    currentDir = currentDir.normalized;
                    transform.Translate(currentDir * speed * Time.deltaTime);
                }
                //Implement path finding algorithm here + invoke the method: call it here
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    DFS();
                }
                if (playerScript.moving)
                {
                    DFS();
                }
            }
            else
            {
                Debug.LogWarning($"{name} - No current node");
            }

            Debug.DrawRay(transform.position, currentDir, Color.cyan);
           // Debug.Log(currentNode.transform.position);
        }
    }

    //Called when a collider enters this object's trigger collider.
    //Player or enemy must have rigidbody for this to function correctly.
    private void OnTriggerEnter(Collider other)
    {
        if (playerCaught == false)
        {
            if (other.tag == "Player")
            {
                playerCaught = true;
                GameOverEvent.Invoke(); //invoke the game over event
            }
        }
    }

    /// <summary>
    /// Sets the current node to the first in the Game Managers node list.
    /// Sets the current movement direction to the direction of the current node.
    /// </summary>
    void InitializeAgent()
    {
        //refer to the game manager without having reference just saying game.managerinstance index 0
        currentNode = GameManager.Instance.Nodes[0];
        //Vector3.direction, working out direction of one vector to another
        currentDir = currentNode.transform.position - transform.position;
        //giving vectro with a magnitude of 1, takes all values and it becomes a float between 0 and 1
        //its important to normalize because it gives consistent 
        currentDir = currentDir.normalized;
        //finding two vectors for direction is dir = b - a
        //finding two vector for distance is distance = a - b
    }
    private Node currentNodeSearch;
    private bool targetFound = false;
    private List<Node> unsearchedNodes = new List<Node>();

    Player playerScript;


    private void Awake()
    {
        playerScript = GameObject.Find("Player").GetComponent<Player>();    
    }

    void DFS()
    {
        targetFound = false;
        unsearchedNodes.Add(GameManager.Instance.Nodes[0]);

        while (!targetFound)                                                                                    //WHILE TARGET FOUND IS FALSE, CONTINUE THE LOOP
        {
            currentNodeSearch = unsearchedNodes[unsearchedNodes.Count - 1];                                     //1. TAKE LAST ITEM IN 'UNSEARCHED NODES' LIST AND ASSIGN IT TO 'NODE CURRENTLY BEING SEARCHED'

            if (currentNodeSearch = playerScript.TargetNode)                                                   //CHANGE SCRIPT WHEN YOU DECIDE WHAT THE TARGET IS GOING TO BE
                                                                                                                //2. CHECK IF 'NODE CURRENTLY BEING SEARCHED' IS THE SAME AS *EITHER*
                                                                                                                     //THE TARGET NODE OF THE PLAYER (NODE THEY ARE HEADING TOWARDS)
                                                                                                                     //THE CURRENT NODE OF THE PLAYER (THE LAST NODE THEY VISITED)
            {
                currentNode = currentNodeSearch;                                                                //ASSIGN 'NODE CURRENTLY BEING SEARCHED' AS 'CURRENTNODE'
                //Debug.Log(currentNode.transform.position);
                targetFound = true; 
                unsearchedNodes.Clear();                                                                        // ADDED A STEP TO CLEAR THE LIST UNSURE IF NESSESSARY
                break;                                                                                          //BREAK THE LOOP AND FINISH THIS METHOD
            }

            for (int i = 0; i < currentNodeSearch.Children.Length; i++)                                         //3. USE A FOR LOOP TO ADD EACH CHILD OF 'NODE CURRENTLY BEING SEARCHED' TO UNSEARCHED NODES LIST
            {
                unsearchedNodes.Add(currentNodeSearch.Children[i]);
            }

            unsearchedNodes.Remove(currentNodeSearch);                                                          //4. REMOVE 'NODE CURRENTLY BEING SEARCHED FROM UNSEARCHED NODES LIST
        }
    }
}
    

