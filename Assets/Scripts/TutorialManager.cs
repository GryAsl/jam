using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public TextMeshProUGUI tutText;
    public Image tutImage;
    public UIManager uiMan;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void TriggerTut(string text)
    {
        tutText.text = text.ToString();
        StartCoroutine(uiMan.TutOn(tutText, tutImage));
    }
}
