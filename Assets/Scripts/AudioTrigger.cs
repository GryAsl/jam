using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(BoxCollider))]
public class AudioTrigger : MonoBehaviour
{
    public string subtitle;
    public bool alreadyPlayed;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (alreadyPlayed)
            return;
        alreadyPlayed = true;
        GetComponent<AudioSource>().Play();
        StartCoroutine(GameObject.Find("GameManager").GetComponent<UIManager>().SubOn(subtitle, GetComponent<AudioSource>().clip.length));
    }
}
