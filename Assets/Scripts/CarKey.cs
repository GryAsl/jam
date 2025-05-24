using UnityEngine;
using System.Collections;

public class CarKey : MonoBehaviour
{
    public float minIntensity = 0f;
    public float maxIntensity = 2f;
    public float speed = 2f; // De�i�im h�z�
    public float waitTime = 0f; // D�ng� sonunda bekleme s�resi

    Renderer rend;
    Material mat;
    Material mat2;
    bool alreadyTriggered;

    void Start()
    {

        rend = GetComponent<Renderer>();
        if (rend == null || rend.sharedMaterials.Length < 2)
        {
            Debug.LogError("Bu obje en az 2 material kullanmal�.");
            enabled = false;
            return;
        }

        // Material referans� almak i�in GetSharedMaterials kullan�yoruz
        Material[] mats = rend.materials; // .materials = Instance, .sharedMaterials = Asset
        mat = mats[1];
        mat2 = mats[2];
        StartCoroutine(FlickerEmission());
    }

    IEnumerator FlickerEmission()
    {
        float t = 0f;
        bool increasing = true;

        while (true)
        {
            float targetIntensity = increasing ? maxIntensity : minIntensity;
            float startIntensity = increasing ? minIntensity : maxIntensity;
            float elapsed = 0f;

            while (elapsed < speed)
            {
                float emission = Mathf.Lerp(startIntensity, targetIntensity, elapsed / speed);
                mat.SetColor("_EmissionColor", Color.white * emission);
                mat2.SetColor("_EmissionColor", Color.white * emission);
                elapsed += Time.deltaTime;
                yield return null;
            }

            // De�eri tam oturtmak i�in
            mat.SetColor("_EmissionColor", Color.white * targetIntensity);
            if (waitTime > 0f) yield return new WaitForSeconds(waitTime);
            increasing = !increasing;
        }
    }

    public void TriggerKey()
    {
        if (alreadyTriggered)
            return;
        alreadyTriggered = true;
        Debug.Log("1");
        GetComponent<AudioSource>().Play();
        Debug.Log("2");
        StartCoroutine(ScaleDown());
        Material[] mats = rend.materials; // Instance array
        for (int i = 0; i < mats.Length; i++)
        {
            mats[i] = mat;
        }
        rend.materials = mats;
    }



    IEnumerator ScaleDown()
    {
        yield return new WaitForSeconds(1f);

        float t = 0f;
        while (t < 1.0f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, t);
            t += .001f;
            yield return new WaitForFixedUpdate();
        }
    }

}