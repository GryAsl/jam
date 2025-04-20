using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public UIManager uiManager;
    public bool isGameOn;
    public bool isInventoryOn;
    public Player player;

    

    void Awake()
    {
        uiManager = gameObject.GetComponent<UIManager>();
        //sm = GameObject.Find("Court").GetComponent<ScoreManager>();
    }



    // Update is called once per frame
    void Update()
    {
    }

    public void StartGame()
    {
        isGameOn = true;
        Application.targetFrameRate = 60;
        Cursor.visible = false;
    }

    public void EndGame()
    {
        isGameOn = false;
        uiManager.EndGame();
    }

    public void ToggleInventory()
    {
        if (isInventoryOn)
        {
            uiManager.CloseInventory();
            player.TurnOffItems();
        }
        else
        {
            uiManager.OpenInventory();
            player.TurnOnItems();
        }
        isInventoryOn = !isInventoryOn;
    }

}
