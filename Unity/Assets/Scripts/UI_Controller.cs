using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Controller : MonoBehaviour
{
    [SerializeField]private GameObject GameUI;
    [SerializeField]private GameObject MenuUI;

    public void GoToMainMenu()
    {
        GameUI.SetActive(false);
        MenuUI.SetActive(true);
    }

    public void GoToGame()
    {
        GameUI.SetActive(true);
        MenuUI.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
