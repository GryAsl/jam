using UnityEngine;

public class LightOptimizer : MonoBehaviour
{
    Transform player;
    public float distance;
    public float turnOffDistance;

    void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(Camera.main.transform.position, transform.position);
        if (distance > turnOffDistance)
        {
            GetComponent<Light>().enabled = false;
        }
        else
            GetComponent<Light>().enabled = true;
    }
}
