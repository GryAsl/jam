using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public UIManager uiManager;
    public bool isGameOn;
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


}
