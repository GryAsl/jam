using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CameraMoveSpline : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public Transform camTransform;
    public Transform pointC;
    public Transform pointD;
    public bool firstOne = true;
    public float interpolateAmount;
    public float speed;
    public Image menuBackgroundImage;
    bool coroutineAlreadyStart;

    private void FixedUpdate()
    {
        if(interpolateAmount >= .9f && !coroutineAlreadyStart)
        {
            coroutineAlreadyStart = true;
            StartCoroutine(Trans());
            Debug.Log("AHHH");
        }
        interpolateAmount += speed;
        if(firstOne)
            camTransform.position = Vector3.Lerp(pointA.position, pointB.position, interpolateAmount);
        else camTransform.position = Vector3.Lerp(pointC.position, pointD.position, interpolateAmount);
    }

    IEnumerator Trans()
    {
        float t = 0f;
        Color col = Color.black;
        while (t <= 1f)
        {
            Debug.LogWarning("1: " + t);
            t += .05f;
            col.a = Mathf.Lerp(0f, 1f, t);
            menuBackgroundImage.color = col;
            yield return new WaitForFixedUpdate();
        }
        interpolateAmount = 0f;
        firstOne = !firstOne;
        while (t >= 0f)
        {
            Debug.LogWarning("2: " + t + " / " + col.a);
            t -= .025f;
            col.a = Mathf.Lerp(0f, 1f, t);
            menuBackgroundImage.color = col;
            yield return new WaitForFixedUpdate();
        }
        coroutineAlreadyStart = false;
        Debug.LogWarning("3");
    }

}

