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

    void Start()
    {
        playerScript = GameObject.Find("Player").GetComponent<Player>();
        colour = GetComponent<Button>().colors;
        button = GetComponent<Button>();
    }

    IEnumerator ButtonColourChange()
    {
        if (playerScript.directionNotFound && !playerScript.moving)
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

    }

    public void buttonPress()
    {
        if (isRunning == false)
        {
            StartCoroutine(ButtonColourChange());
        }
    }
}
