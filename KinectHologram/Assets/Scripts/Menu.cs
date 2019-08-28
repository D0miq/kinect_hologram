using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private GameObject activeMenu;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            this.LoadServerScene();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            this.LoadClientScene();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            this.LoadLocalScene();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            this.ExitApplication();
        }
    }

    public void LoadLocalScene()
    {
        SceneManager.LoadScene("LocalClientScene");
    }

    public void LoadServerScene()
    {
        SceneManager.LoadScene("ServerScene");
    }

    public void LoadClientScene()
    {

        SceneManager.LoadScene("ClientScene");
    }

    public void ChangeMenu(GameObject menu)
    {
        this.activeMenu.SetActive(false);
        this.activeMenu = menu;
        this.activeMenu.SetActive(true);
    }

    public void ExitApplication()
    {
        Application.Quit();
    }
}
