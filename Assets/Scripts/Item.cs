using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour
{
    Vector3 startLoc;
    public string itemName;
    void Start()
    {
        startLoc = transform.position;
    }

    public void StartAnim()
    {
        StartCoroutine(Anim());
        GetComponent<MeshCollider>().enabled = false;
        GetComponent<AudioSource>().Play();

    }

    IEnumerator Anim()
    {
        float t = 0;
        while (t <= 1)
        {
            transform.position = Vector3.Lerp(startLoc, GameObject.Find("Player").gameObject.transform.position, t * .75f);
            transform.localScale = new Vector3(Mathf.Lerp(transform.localScale.x, 0, t), Mathf.Lerp(transform.localScale.y, 0, t), Mathf.Lerp(transform.localScale.z, 0, t));
            t += Time.deltaTime;
            yield return null;
        }
        GameObject.Find("Player").GetComponent<Player>().TurnOffItems();
    }
}
