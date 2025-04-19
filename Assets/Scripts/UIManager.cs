using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameManager gm;
    public GameObject GameCanvas;
    public GameObject MainCanvas;
    public GameObject SettingsCanvas;
    public GameObject EndCanvas;
    public GameObject InventoryCanvas;
    public GameObject currentCanvas;
    public float canvasSpeed;
    public AudioSource audioS;
    public Slider slider;

    void Start()
    {
        gm = gameObject.GetComponent<GameManager>();
        currentCanvas = MainCanvas;

    }


    public void StartGame()
    {
        //Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        StartCoroutine(OpenCanvas(GameCanvas));
        StartCoroutine(CloseCanvas(MainCanvas));
        gm.StartGame();
    }

    public void OpenSettingsMenu()
    {
        StartCoroutine(OpenCanvas(SettingsCanvas));
        StartCoroutine(CloseCanvas(MainCanvas));
    }

    public void OpenMainMenu()
    {
        StartCoroutine(OpenCanvas(MainCanvas));
        StartCoroutine(CloseCanvas(SettingsCanvas));
        StartCoroutine(CloseCanvas(EndCanvas));
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator OpenCanvas(GameObject canvas)
    {
        canvas.SetActive(true);
        CanvasGroup cg = canvas.GetComponent<CanvasGroup>();
        while (cg.alpha < 1f)
        {
            cg.alpha += canvasSpeed;
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator CloseCanvas(GameObject canvas)
    {
        CanvasGroup cg = canvas.GetComponent<CanvasGroup>();
        while (cg.alpha > 0f)
        {
            cg.alpha -= canvasSpeed;
            yield return new WaitForFixedUpdate();
        }
        canvas.SetActive(false);
    }





    public void EndGame()
    {
        StartCoroutine(OpenCanvas(EndCanvas));
        StartCoroutine(CloseCanvas(GameCanvas));
    }

    public void ChangeVolume()
    {
        audioS.volume = slider.value;
    }

    public void OpenInventory()
    {
        Cursor.visible = true;
        StartCoroutine(OpenCanvas(InventoryCanvas));
        StartCoroutine(CloseCanvas(GameCanvas));
    }

    public void CloseInventory()
    {
        Cursor.visible = false;
        StartCoroutine(OpenCanvas(GameCanvas));
        StartCoroutine(CloseCanvas(InventoryCanvas));
    }

    public void MoveItemsRight()
    {
        StartCoroutine(GameObject.Find("Player").GetComponent<Player>().MoveRight());

    }

    public void MoveItemsLeft()
    {
        StartCoroutine(GameObject.Find("Player").GetComponent<Player>().MoveLeft());
    }
}
