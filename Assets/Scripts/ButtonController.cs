using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    Player playerScript;
    private ColorBlock colour;
    private Button button;

    private bool isRunning = false;
    public int ID;

    void Start()
    {
        playerScript = GameObject.Find("Player").GetComponent<Player>();
        colour = GetComponent<Button>().colors;
        button = GetComponent<Button>();
    }

    IEnumerator ButtonColourChange()
    {
        if (playerScript.directionNotFound && !playerScript.moving)  // detects if there is not a valid node in  that diretion and falshes the button red if there is not
        {
            isRunning = true;
            colour.selectedColor = Color.red;
            button.colors = colour;
            yield return new WaitForSeconds(0.2f);
            colour.selectedColor = Color.white;
            button.colors = colour;
            isRunning = false;
        }
        else
        {
            isRunning = true;
            colour.selectedColor = Color.green;
            button.colors = colour;
            yield return new WaitForSeconds(0.2f);
            colour.selectedColor = Color.white;
            button.colors = colour;
            isRunning = false;
        }

    }
    void Update()
    {
        if (playerScript.moving)
        {
            this.GetComponent<Button>().interactable = true;
        }

        if (playerScript.ID_out == ID) // allows the button to know what button to reference when changing the colour of the individual directions when there is a input that is not the mouse
        {
            buttonPress();
            //Debug.Log(ID);
            playerScript.ID_out = -1;
        }
    }

    public void buttonPress()
    {
        if (isRunning == false)
        {
            StartCoroutine(ButtonColourChange());
        }
    }
}
