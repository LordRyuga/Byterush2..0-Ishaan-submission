using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public Canvas mainMenu;
    public Canvas InstructionsObj;
    public void Instructions()
    {
        InstructionsObj.enabled = true;
        mainMenu.enabled = false;
    }
    public void Back()
    {
        InstructionsObj.enabled = false;
        mainMenu.enabled = true;
    }
}
