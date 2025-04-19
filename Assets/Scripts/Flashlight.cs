using System.Collections;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public Light light_;
    public float currentPower;
    public float maxPower;

    public Coroutine cor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentPower = maxPower;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            light_.enabled = false;

        if(currentPower > 0f)
        {
            if(currentPower < .1f && cor == null)
            {
                cor = StartCoroutine(FlashEffect());
            }
            else currentPower -= .05f * Time.deltaTime;
        }
        else light_.enabled = false;
    }

    IEnumerator FlashEffect()
    {
        while (currentPower > 0f)
        {
            if(light_.enabled == true)
                light_.enabled = false;
            else light_.enabled = true;

            yield return new WaitForSeconds(Random.Range(.15f,.35f));
        }

    }
}
