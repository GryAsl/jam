using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public TutorialManager tutMan;
    public string tut;
    public bool alreadyTriggered;

    private void Start()
    {
        tutMan = GameObject.Find("GameManager").GetComponent<TutorialManager>();
    }

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        Debug.Log(other);
        if(other.gameObject == GameObject.Find("Player"))
        {
            if (alreadyTriggered)
                return;
            alreadyTriggered = true;
            tutMan.TriggerTut(tut);
        }
    }
}
