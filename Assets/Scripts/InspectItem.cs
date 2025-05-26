using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class InspectItem : MonoBehaviour
{
    private GameObject item;
    private Vector3 targetPosition;
    private Vector3 targetRotation;
    public float moveSpeed = 5f;
    public float rotateSpeed = 150f;

    private bool inspecting = false;
    private bool arrived = false;
    private bool returning = false;

    private Vector3 startPos;
    private Quaternion startRot;

    public string text;
    
    

    public void StartInspect(GameObject go, Vector3 targetPosition_, Vector3 targetRotation_)
    {
        if(gameObject.TryGetComponent<AudioSource>(out var audioSource))
        {
            audioSource.Play();
            StartCoroutine(GameObject.Find("GameManager").GetComponent<UIManager>().SubOn(text, audioSource.time));
        }

        item = go;
        item.GetComponent<BoxCollider>().enabled = false;

        startPos = item.transform.position;
        startRot = item.transform.rotation;

        targetPosition = targetPosition_;
        targetRotation = targetRotation_;

        inspecting = true;
        arrived = false;
        returning = false;
    }

    public void StopInspect()
    {
        inspecting = false;
        returning = true;
        arrived = false;
    }

    void Update()
    {
        if (item == null) return;

        // Ýleriye giderken
        if (inspecting && !arrived)
        {
            item.transform.position = Vector3.Lerp(item.transform.position, targetPosition, Time.deltaTime * moveSpeed);
            item.transform.rotation = Quaternion.Lerp(item.transform.rotation, Quaternion.Euler(targetRotation), Time.deltaTime * moveSpeed);

            if (Vector3.Distance(item.transform.position, targetPosition) < 0.01f &&
                Quaternion.Angle(item.transform.rotation, Quaternion.Euler(targetRotation)) < 1f)
            {
                arrived = true;
            }
        }
        // Ýnceleme sýrasýnda mouse ile döndür
        else if (inspecting && arrived)
        {
            float mouseX = -Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime;
            item.transform.Rotate(Vector3.up, mouseX, Space.Self);
            item.transform.Rotate(Vector3.right, mouseY, Space.Self);
        }
        // Geri dönüþ sýrasýnda
        else if (returning)
        {
            item.transform.position = Vector3.Lerp(item.transform.position, startPos, Time.deltaTime * moveSpeed);
            item.transform.rotation = Quaternion.Lerp(item.transform.rotation, startRot, Time.deltaTime * moveSpeed);

            if (Vector3.Distance(item.transform.position, startPos) < 0.01f &&
                Quaternion.Angle(item.transform.rotation, startRot) < 1f)
            {
                returning = false;
            }
        }
    }
}
