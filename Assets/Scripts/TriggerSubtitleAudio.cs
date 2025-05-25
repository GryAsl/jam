using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(BoxCollider))]
public class TriggerSubtitleAudio : MonoBehaviour
{
    public AudioClip voiceClip;
    public string subtitleText;
    public float subtitleDuration = 3f;
    public UIManager uiMan;

    public TextMeshProUGUI tmpText; // TMP için

    bool triggered = false;
    AudioSource audioSource;



    private void Start()
    {
        uiMan = GameObject.Find("GameManager").GetComponent<UIManager>();
        audioSource = GetComponent<AudioSource>();

    }

    void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (other.gameObject == GameObject.Find("Player"))
        {
            triggered = true;
            if (audioSource && voiceClip)
                audioSource.PlayOneShot(voiceClip);

            tmpText.text = subtitleText;
            StartCoroutine(uiMan.SubOn(subtitleText, subtitleDuration));
        }




    }

}