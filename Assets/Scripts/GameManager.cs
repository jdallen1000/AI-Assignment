using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Stores all the nodes
    [SerializeField] private Node[] nodes;
    //Stores reference to the player
    [SerializeField] private Player player;

    public Node[] Nodes { get { return nodes; } }
    public Player Player { get { return player; } }

    public GameObject gamaOvarText;

    public static GameManager Instance { get; private set; }

    /// <summary>
    /// Awake is called before Start is executed for the first time.
    /// </summary>
    private void Awake()
    { 
        if (Instance == null)
        {
            Instance = this;
        }
        FindObjectOfType<Enemy>().GameOverEvent += GameOver;
        gamaOvarText.SetActive(false);
    }

    /// <summary>
    /// Triggers the Restart Game coroutine.
    /// </summary>
    private void GameOver()
    {
        StartCoroutine(RestartGame());
    }

    /// <summary>
    /// Disables the player. Re-loads the active scene after 2 second delay.
    /// </summary>
    /// <returns></returns>
    IEnumerator RestartGame()
    {
        player.enabled = false;
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
            Debug.Log("Quiting");
        }
    }
}
