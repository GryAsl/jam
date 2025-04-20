using System.Collections;
using UnityEngine;

public class MoveDoor : MonoBehaviour
{
    public float loc;
    public void MoveDoorX()
    {
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        float t = 0;
        while (t <= 1)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(loc, transform.position.y, transform.position.z), t);
            t += Time.deltaTime * .25f;
            yield return null;
        }
    }
}
