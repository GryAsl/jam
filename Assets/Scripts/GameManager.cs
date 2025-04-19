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
    }

    public void EndGame()
    {
        isGameOn = false;
        uiManager.EndGame();
    }

    public void ToggleInventory()
    {
        if (isInventoryOn)
            uiManager.CloseInventory();
        else uiManager.OpenInventory();
        isInventoryOn = !isInventoryOn;
    }

}
