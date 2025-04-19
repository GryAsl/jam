using System.Collections;
using UnityEngine;

public class MyAnimation : MonoBehaviour
{
    public Texture2D anim1;
    public Texture2D anim2;
    public Material mat;
    public bool Play;
    public bool AnimAlreadyPlayed;
    Renderer renderer_;

    void Start()
    {
        StartCoroutine(PlayAnim());
        renderer_ = GetComponent<Renderer>();
        renderer_.material.SetTexture("_Photo", anim1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Play = true;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            Play = false;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            AnimAlreadyPlayed = true;
        }
    }

    IEnumerator PlayAnim()
    {
        while (!AnimAlreadyPlayed)
        {
            if (Play)
            {
                renderer_.material.SetTexture("_Photo", anim2);
                yield return new WaitForSeconds(.5f);
                renderer_.material.SetTexture("_Photo", anim1);
                yield return new WaitForSeconds(.5f);
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
