using System.Collections;
using UnityEngine;

public class Case : MonoBehaviour
{
    public GameObject go;
    public bool rotate;
     public   Vector3 rot;
    public float anan;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (rotate)
        {
            StartCoroutine(Turn());
            rotate = false;
            GetComponent<AudioSource>().Play();
        }
    }

    IEnumerator Turn()
    {
        float t = 0f;
        while(t <= 1)
        {
            t += Time.deltaTime;

            rot = new Vector3(0f, t * 90f * anan, 0);
            go.transform.localRotation = Quaternion.Euler(rot);
            yield return null;
        }
    }
}
