using System.Collections;
using UnityEngine;

public class MeshDestroyAfter : MonoBehaviour
{
    float timer = 4f;

    void Start()
    {
        StartCoroutine(ScaleDown());
        StartCoroutine(SlowDown());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ScaleDown()
    {
        yield return new WaitForSeconds(timer);

        float t = 0f;
        while(t < 1.0f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, t);
            t += .001f;
            yield return new WaitForFixedUpdate();
        }
    }
    IEnumerator SlowDown()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        float t = 0f;
        while (t < 1.0f)
        {
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, t);
            t += .0001f;
            yield return new WaitForFixedUpdate();
        }
    }
}
