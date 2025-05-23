using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlickering : MonoBehaviour
{
    public Renderer targetRenderer;      // Flicker uygulanacak Renderer (mesh vs.)
    public Color emissionColor = Color.white; // Temel emission rengi
    public float minEmission = 0.5f;    // Minimum emission intensity
    public float maxEmission = 2.5f;    // Maksimum emission intensity
    public float flickerSpeedMin = 0.02f;
    public float flickerSpeedMax = 0.1f;

    List<Material> sharedMats = new List<Material>();

    void Start()
    {
        if (targetRenderer == null)
            targetRenderer = GetComponent<Renderer>();

        // Instance alýyoruz ki diðer objelerin materyali etkilenmesin
         targetRenderer.GetSharedMaterials(sharedMats);

        StartCoroutine(FlickerEmissionCoroutine());
    }

    IEnumerator FlickerEmissionCoroutine()
    {
        emissionColor = sharedMats[2].GetColor("_EmissionColor");
        while (true)
        {
            // Emission intensity’i belirle
            float intensity = Random.Range(minEmission, maxEmission);
            Color finalColor = emissionColor * intensity;

            sharedMats[2].SetColor("_EmissionColor", finalColor);

            yield return new WaitForSeconds(Random.Range(flickerSpeedMin, flickerSpeedMax));
        }
    }
}
