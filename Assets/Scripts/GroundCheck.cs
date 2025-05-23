using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("onGround: " + other );
        GameObject.Find("Player").GetComponent<Player>().isGrounded = true;
        GameObject.Find("Player").GetComponent<Player>().jumpCharges = 1;
    }
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("onGround: " + other);
        GameObject.Find("Player").GetComponent<Player>().isGrounded = true;
        GameObject.Find("Player").GetComponent<Player>().jumpCharges = 1;
    }


    private void OnTriggerExit(Collider other)
    {
        GameObject.Find("Player").GetComponent<Player>().isGrounded = false;
    }
}
