using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    Player playerScript;
    private ColorBlock colour;
    private Button button;

    void Start()
    {
       playerScript = GameObject.Find("Player").GetComponent<Player>();
       colour = GetComponent<Button>().colors;
       button = GetComponent<Button>();
    }

    public void buttonDisable()
    {
        if (playerScript.directionNotFound && !playerScript.moving)
        {
            this.GetComponent<Button>().interactable = false;
        }
        
    }
    void Update()
    {
        if (playerScript.moving)
        {
            this.GetComponent<Button>().interactable = true;
        }


    }
}
