using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class DissolveObject : MonoBehaviour
{
    [SerializeField] private float noiseStrength = 0.25f;
    [SerializeField] private float objectHeight = 1.0f;

    private Material material;
    public bool dissolve;
    public KeyCode keyCode;

    private void Awake()
    {
        material = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        //var time = Time.time * Mathf.PI * 0.25f;

        //float height = transform.position.y;
        //height += Mathf.Sin(time) * (objectHeight / 2.0f);
        if (Input.GetKeyDown(keyCode))
        {
            dissolve = true;
            GetComponent<BoxCollider>().enabled = false;
        }
        if (dissolve)
        {
            objectHeight -= Time.deltaTime * 1.5f;
        }
        SetHeight();
    }

    private void SetHeight()
    {
        material.SetFloat("_CutoffHeight", objectHeight);
        material.SetFloat("_NoiseStrength", noiseStrength);
    }
}
