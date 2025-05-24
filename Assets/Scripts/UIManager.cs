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
    public GameObject PasswordCanvas;
    public GameObject currentCanvas;
    public float canvasSpeed;
    public AudioSource audioS;
    public Slider slider;
    public TextMeshProUGUI password;
    public TextMeshProUGUI storyText;
    public TextMeshProUGUI bigStoryText;
    public Image storyBackgroundImage;
    public Image startBackgroundImage;
    public string passwordText;
    public GameObject storyPlane;

    public Camera MenuCam;
    public Camera PlayCam;


    void Start()
    {
        gm = gameObject.GetComponent<GameManager>();
        currentCanvas = MainCanvas;
        storyPlane = GameObject.Find("Story");
        storyPlane.transform.localPosition = new Vector3(storyPlane.transform.localPosition.x, -.55f, storyPlane.transform.localPosition.z);
        PlayCam.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(StoryTelling());
        }
    }


    public void StartGame()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        StartCoroutine(OpenCanvas(GameCanvas));
        StartCoroutine(CloseCanvas(MainCanvas));
        StartCoroutine(StartGameUI());
        gm.StartGame();
        MenuCam.enabled = false;
        PlayCam.enabled = true;
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
    
    public void One()
    {
        passwordText += "1";
        password.text = passwordText;
    }
    public void Two()
    {
        passwordText += "2";
        password.text = passwordText;
    }
    public void Three()
    {
        passwordText += "3";
        password.text = passwordText;
    }

    public void Four()
    {
        passwordText += "4";
        password.text = passwordText;
    }

    public void Five()
    {
        passwordText += "5";
        password.text = passwordText;
    }

    public void Six()
    {
        passwordText += "6";
        password.text = passwordText;
    }

    public void Seven()
    {
        passwordText += "7";
        password.text = passwordText;
    }

    public void Eight()
    {
        passwordText += "8";
        password.text = passwordText;
    }

    public void Nine()
    {
        passwordText += "9";
        password.text = passwordText;
    }
    public void Zero()
    {
        passwordText += "0";
        password.text = passwordText;
    }

    public void TryPassword()
    {
        if(passwordText == "2206")
        {
            password.color = Color.green;
            Invoke("TurnOffPasswordUI", 1f);
            GameObject.Find("kasa2").GetComponent<Case>().rotate = true;
        }
    }

    void TurnOffPasswordUI()
    {
        Cursor.visible = false;
        StartCoroutine(OpenCanvas(GameCanvas));
        StartCoroutine(CloseCanvas(PasswordCanvas));
    }

    public void TurnOnPasswordUI()
    {
        Cursor.visible = true;
        StartCoroutine(OpenCanvas(PasswordCanvas));
        StartCoroutine(CloseCanvas(GameCanvas));
    }

    IEnumerator StoryTelling()
    {
        float t = 0f;
        

        t = 0f;
        Color col = storyBackgroundImage.color;
        storyPlane.GetComponent<MyAnimation>().Play = true;
        while (t <= 1)
        {
            t += Time.deltaTime;
            storyText.alpha += .01f;
            Debug.Log(col);
            col.a += .008f;
            storyBackgroundImage.color = col;
            storyPlane.transform.localPosition = Vector3.Lerp(storyPlane.transform.localPosition, new Vector3(storyPlane.transform.localPosition.x, -.15f, storyPlane.transform.localPosition.z), t);

            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator StartGameUI()
    {
        float t = 0f;
        //yield return new WaitForSeconds(1f);
        //while (t <= 1)
        //{
        //    t += .02f;
        //    bigStoryText.alpha = Mathf.Lerp(0f, 1f, t);

        //    yield return new WaitForFixedUpdate();
        //}
        //yield return new WaitForSeconds(.1f); // BUNU 10 YAPMAN LAZIM
        //t = 0;
        //while (t <= 1)
        //{
        //    t += .05f;
        //    bigStoryText.alpha = Mathf.Lerp(1f, 0f, t);

        //    yield return new WaitForFixedUpdate();
        //}
        //yield return new WaitForSeconds(1f);

        t = 0f;
        float lastA = 1f;
        float waitTime = .03f;
        Color col = Color.black;
        while (t <= .55f)
        {
            t += .003f;
            col.a = Mathf.Lerp(lastA, 0f, t);
            startBackgroundImage.color = col;
            Debug.Log("0: " + col);
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(waitTime * .5f);
        t = 0;
        lastA = col.a;
        Debug.LogWarning("1: " + lastA);
        while (t <= .95f)
        {
            t += .015f;
            col.a = Mathf.Lerp(lastA, 1f, t);
            startBackgroundImage.color = col;
            Debug.Log("1: " + col);
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(waitTime);
        lastA = col.a;
        t = 0;
        Debug.LogWarning("2: " + lastA);
        while (t <= .85f)
        {
            t += .01f;
            col.a = Mathf.Lerp(lastA, 0f, t);
            startBackgroundImage.color = col;
            Debug.Log(col);
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(waitTime);
        t = 0;
        lastA = col.a;
        Debug.LogWarning("3: " + lastA);
        while (t <= .85f)
        {
            t += .015f;
            col.a = Mathf.Lerp(lastA, 1f, t);
            startBackgroundImage.color = col;
            Debug.Log(col);
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(waitTime);
        lastA = col.a;
        t = 0;
        GameObject.Find("kapsül2").GetComponent<Case>().rotate = true;
        Debug.LogWarning("4: " + lastA);
        while (t <= 1f)
        {
            t += .015f;
            col.a = Mathf.Lerp(lastA, 0f, t);
            startBackgroundImage.color = col;
            Debug.Log(col);
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(.1f);
        
    }

    public void UseItem()
    {
        Debug.LogWarning("X");
        GameObject.Find("Player").GetComponent<Player>().UseItem();
    }


    public IEnumerator TutOn(TextMeshProUGUI tutText, Image tutImage)
    {
        float t = 0f;
        Color col = tutImage.color;
        while (t <= 1)
        {
            t += Time.fixedDeltaTime;
            tutText.alpha += .01f;
            col.a += .01f;
            tutImage.color = col;
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSecondsRealtime(3f);
        while (t >= 0)
        {
            t -= Time.fixedDeltaTime;
            col.a -= .01f;
            tutText.alpha -= .01f;
            tutImage.color = col;
            yield return new WaitForFixedUpdate();
        }

    }
}
