using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Camera")]
    public float minX = -60f;
    public float maxX = 60f;
    public float camSens = 1f;
    public int normalFOV = 80;
    public int maxFOV = 90;
    Camera cam;

    float rotY = 0f;
    public float rotX = 0f;


    [Header("Movement")]
    public float speedMultiplier;
    public float runSpeed;
    public float walkSpeed;
    public float maxSpeed;
    CharacterController controller;
    Vector3 move;
    Vector3 input;
    bool climbingInput;
    public Transform groundCheck;
    Vector3 yVelocity;
    public bool isGrounded;
    public float jumpHeight;
    public int jumpCharges;
    public float gravity;
    public float normalGravity;
    public LayerMask groundMask;


    [Header("Envanter")]
    public Collider boxCollider;
    public RaycastHit[] hits;
    public List<GameObject> items = new();
    public GameObject item;

    [Header("Other")]
    public GameObject flashlight;
    public GameManager gm;
    public GameObject[] points;
    private GameObject point;
    private GameObject pointLeft;
    private GameObject pointRight;
    public GameObject note;
    bool isNoteOn;
    Vector3 noteStartTrans;
    Vector3 noteStartTransS;
    Quaternion noteStartTransR;
    public GameObject noteGO;
    public int inventoryIndex;
    public TextMeshProUGUI nameText;
    public bool puzlleDone;
    public MeshRenderer meshR;
    public Material greenM;

    public GameObject postPorrrrr;
    public GameObject glass;




    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        controller = GetComponent<CharacterController>();
        cam = Camera.main;
        cam.fieldOfView = normalFOV;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("zort");
            glass.GetComponent<BoxCollider>().enabled = false;
            postPorrrrr.SetActive(true);

        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            GameObject.Find("myDoor2").GetComponent<MeshDestroy>().DestroyMesh();
            GameObject.Find("myDoor2.1").GetComponent<MeshDestroy>().DestroyMesh();
            GameObject.Find("myDoor2.2").GetComponent<MeshDestroy>().DestroyMesh();
            GameObject.Find("myDoor2.3").GetComponent<MeshDestroy>().DestroyMesh();
        }

        HandleMovementInput();
        if (gm.isInventoryOn)
            return;
        rotY += Input.GetAxis("Mouse X") * camSens;
        rotX += Input.GetAxis("Mouse Y") * camSens;
        rotX = Mathf.Clamp(rotX, minX, maxX);

        if (Cursor.visible == false)
            transform.localEulerAngles = new Vector3(0, rotY, 0);
        //cam.transform.localEulerAngles = new Vector3(-rotX, 0, 0);


        GroundedMov();
        ApplyGravity();
        controller.Move(move * Time.deltaTime);
    }

    void HandleMovementInput()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));

        input = transform.TransformDirection(input);
        input = Vector3.ClampMagnitude(input, 1f);

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            maxSpeed = runSpeed;
            StartCoroutine(IncreaseFOV());
            GetComponent<Headbob>().amplitude = GetComponent<Headbob>().amplitudeRun;
            GetComponent<Headbob>()._frequency = 15f;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            maxSpeed = walkSpeed;
            StartCoroutine(DecreaseFOV());
            GetComponent<Headbob>().amplitude = GetComponent<Headbob>().amplitudeNormal;
            GetComponent<Headbob>()._frequency = 10f;
        }

        if (Input.GetKeyDown(KeyCode.E)) //Etkilisime gecme
        {
            hits = Physics.BoxCastAll(boxCollider.bounds.center, boxCollider.bounds.size * 1.5f, transform.forward, transform.rotation, 2f);
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.CompareTag("puzzle"))
                {
                    if (!puzlleDone)
                    {
                        Debug.LogWarning(hit.collider);
                        gm.uiManager.TurnOnPasswordUI();
                        puzlleDone = true;
                        Cursor.visible = true;
                        return;
                    }
                }
                if (hit.collider.CompareTag("climb"))
                {
                    Debug.Log(hit.collider);
                    climbingInput = true; //Bunu bi ara GetKey ile yapmam lazim
                }
                else if (hit.collider.CompareTag("note"))
                {
                    Debug.Log(hit.collider);
                    note = hit.collider.gameObject;
                    if (!isNoteOn)
                    {
                        noteStartTrans = note.transform.position;
                        noteStartTransS = note.transform.localScale;
                        noteStartTransR = note.transform.rotation;
                        StartCoroutine(Note());
                        isNoteOn = true;
                    }
                    else
                    {
                        StartCoroutine(NoteBack());
                        isNoteOn = false;
                    }
                    break;
                }
                else if (hit.collider.CompareTag("item"))
                {
                    Debug.LogWarning(hit.collider.GetComponent<Item>().itemName);
                    if (puzlleDone)
                    {
                        
                        items.Add(hit.collider.gameObject);
                        hit.collider.gameObject.GetComponent<Item>().StartAnim();
                    }
                    else if (hit.collider.name == "fener")
                    {
                        Debug.LogWarning(hit.collider);
                        items.Add(hit.collider.gameObject);
                        hit.collider.gameObject.GetComponent<Item>().StartAnim();
                    }
                    else if (hit.collider.GetComponent<Item>().itemName == "Wrench")
                    {
                        Debug.LogWarning(hit.collider);
                        items.Add(hit.collider.gameObject);
                        hit.collider.gameObject.GetComponent<Item>().StartAnim();
                    }
                }
                if (hit.collider.CompareTag("door"))
                {

                }
                if (hit.collider.CompareTag("platform"))
                {
                    Debug.Log("platform");
                    GetComponent<MoveObjects>().ToggleObject(hit.collider.gameObject);
                    break;
                }

            }
        }

        //if (Input.GetKeyUp(KeyCode.E))
        //{
        //    climbingInput = false;
        //}

        if (Input.GetKeyDown(KeyCode.I))
        {
            gm.ToggleInventory();
            SetItemTransform(0);
            SetItemTransform(1);
            //SetItemTransform(2);

        }

        if (Input.GetKeyUp(KeyCode.Space) && jumpCharges > 0)
        {
            Jump(1f);
        }

    }

    void GroundedMov()
    {
        if (input.x != 0)
            move.x += input.x * speedMultiplier;
        else move.x = 0;

        if (input.z != 0)
            move.z += input.z * speedMultiplier;
        else move.z = 0;

        move = Vector3.ClampMagnitude(move, maxSpeed);
    }

    IEnumerator IncreaseFOV()
    {
        while (cam.fieldOfView <= maxFOV)
        {
            if (Input.GetKeyUp(KeyCode.LeftShift))
                yield break;
            cam.fieldOfView += 50f * Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator DecreaseFOV()
    {
        while (cam.fieldOfView >= normalFOV)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
                yield break;
            cam.fieldOfView -= 50f * Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator MoveRight()
    {
        float t = 0;

        if (items.Count == 2)
        {
            Vector3 scaleMdiddleStart = items[0].transform.localScale;
            Vector3 scaleRightStart = items[1].transform.localScale;
            while (t <= 1)
            {
                if (inventoryIndex == 0)
                {
                    //SetItemTransformbyTransform(1, items[1].transform.position, Vector3.Lerp(scaleRightStart, Vector3.zero, t), Quaternion.Lerp(items[1].transform.rotation, points[1].transform.rotation, t));
                    //SetItemTransformbyTransform(0, Vector3.Lerp(items[0].transform.position, points[1].transform.position, t), Vector3.Lerp(scaleMdiddleStart, points[1].transform.localScale, t));
                    nameText.text = items[1].GetComponent<Item>().itemName;
                }
                else if (inventoryIndex == -1)
                {
                    SetItemTransformbyTransform(1, Vector3.Lerp(items[1].transform.position, points[1].transform.position, t), Vector3.Lerp(scaleRightStart, points[1].transform.localScale, t), Quaternion.Lerp(items[1].transform.rotation, points[1].transform.rotation, t));
                    SetItemTransformbyTransform(0, Vector3.Lerp(items[0].transform.position, points[0].transform.position, t), Vector3.Lerp(scaleMdiddleStart, points[0].transform.localScale, t), Quaternion.Lerp(items[0].transform.rotation, points[0].transform.rotation, t));
                    nameText.text = items[0].GetComponent<Item>().itemName;
                }



                if (t < .4f)
                    t += Time.deltaTime;
                else t = 1.01f;
                yield return null;
            }
            //if (inventoryIndex == 0)
            //{
            //    scaleRightStart = items[1].transform.localScale;
            //    t = 0;
            //    while (t <= 1)
            //    {
            //        SetItemTransformbyTransform(1, points[0].transform.position, Vector3.Lerp(Vector3.zero, points[0].transform.localScale, t));
            //        Debug.LogWarning("2: " + t);

            //        t += Time.deltaTime * 2f;
            //        yield return null;
            //    }
            //}
            if (inventoryIndex == 0)
            {
                inventoryIndex = 2;
                item = items[1];
            }
            else if (inventoryIndex == -1)
            {
                inventoryIndex = 0;
                item = items[0];
            }
        }
        else if (items.Count == 3)
        {
            Vector3 scaleMdiddleStart = items[0].transform.localScale;
            Vector3 scaleRightStart = items[1].transform.localScale;
            Vector3 scaleLeftStart = items[2].transform.localScale;
            while (t <= 1)
            {
                //SetItemTransformbyTransform(1, items[1].transform.position, Vector3.Lerp(scaleRightStart, Vector3.zero, t));
                //SetItemTransformbyTransform(0, Vector3.Lerp(items[0].transform.position, points[1].transform.position, t), Vector3.Lerp(scaleMdiddleStart, points[1].transform.localScale, t));
                //SetItemTransformbyTransform(2, Vector3.Lerp(items[2].transform.position, points[0].transform.position, t), Vector3.Lerp(scaleLeftStart, points[0].transform.localScale, t));

                t += Time.deltaTime;
                yield return null;
            }
            Debug.Log("Bitti");
            scaleRightStart = items[1].transform.localScale;
            t = 0;
            while (t <= 1)
            {
                //SetItemTransformbyTransform(1, points[2].transform.position, Vector3.Lerp(scaleRightStart, points[2].transform.localScale, t));

                t += Time.deltaTime;
                yield return null;
            }

        }
        //else if(items.Count > 3)
        //{
        //    while (t <= 1)
        //    {
        //        SetItemTransformbyTransform(1, items[1].transform.position, Vector3.Lerp(scaleRightStart, Vector3.zero, t));
        //        SetItemTransformbyTransform(0, Vector3.Lerp(items[0].transform.position, points[1].transform.position, t), Vector3.Lerp(scaleMdiddleStart, points[1].transform.localScale, t));
        //        SetItemTransformbyTransform(2, Vector3.Lerp(items[2].transform.position, points[0].transform.position, t), Vector3.Lerp(scaleLeftStart, points[0].transform.localScale, t));

        //        t += Time.deltaTime;
        //        yield return null;
        //    }
        //}

    }

    public IEnumerator MoveLeft()
    {
        float t = 0;

        if (items.Count == 2)
        {
            Vector3 scaleMdiddleStart = items[0].transform.localScale;
            Vector3 scaleRightStart = items[1].transform.localScale;
            while (t <= 1)
            {
                if (inventoryIndex == 0)
                {
                    SetItemTransformbyTransform(1, Vector3.Lerp(items[1].transform.position, points[0].transform.position, t), Vector3.Lerp(scaleRightStart, points[0].transform.localScale, t), Quaternion.Lerp(items[1].transform.rotation, points[0].transform.rotation, t));
                    SetItemTransformbyTransform(0, Vector3.Lerp(items[0].transform.position, points[2].transform.position, t), Vector3.Lerp(scaleMdiddleStart, points[2].transform.localScale, t), Quaternion.Lerp(items[0].transform.rotation, points[2].transform.rotation, t));
                    nameText.text = items[1].GetComponent<Item>().itemName;
                }
                else if (inventoryIndex == -1)
                {
                    SetItemTransformbyTransform(1, Vector3.Lerp(items[1].transform.position, points[1].transform.position, t), Vector3.Lerp(scaleRightStart, points[1].transform.localScale, t), Quaternion.Lerp(items[1].transform.rotation, points[1].transform.rotation, t));
                    SetItemTransformbyTransform(0, Vector3.Lerp(items[0].transform.position, points[0].transform.position, t), Vector3.Lerp(scaleMdiddleStart, points[0].transform.localScale, t), Quaternion.Lerp(items[0].transform.rotation, points[0].transform.rotation, t));
                    nameText.text = items[0].GetComponent<Item>().itemName;
                }
                else if (inventoryIndex == 1)
                {
                    SetItemTransformbyTransform(1, Vector3.Lerp(items[1].transform.position, points[2].transform.position, t), Vector3.Lerp(scaleRightStart, points[2].transform.localScale, t), Quaternion.Lerp(items[1].transform.rotation, points[2].transform.rotation, t));
                    SetItemTransformbyTransform(0, Vector3.Lerp(items[0].transform.position, points[0].transform.position, t), Vector3.Lerp(scaleMdiddleStart, points[0].transform.localScale, t), Quaternion.Lerp(items[0].transform.rotation, points[0].transform.rotation, t));
                    nameText.text = items[0].GetComponent<Item>().itemName;
                }
                if (t < .4f)
                    t += Time.deltaTime;
                else t = 1.01f;
                yield return null;
            }


        }
        else if (items.Count == 3)
        {
            Vector3 scaleMdiddleStart = items[0].transform.localScale;
            Vector3 scaleRightStart = items[1].transform.localScale;
            Vector3 scaleLeftStart = items[2].transform.localScale;
            while (t <= 1)
            {
                //SetItemTransformbyTransform(1, items[1].transform.position, Vector3.Lerp(scaleRightStart, Vector3.zero, t));
                //SetItemTransformbyTransform(0, Vector3.Lerp(items[0].transform.position, points[1].transform.position, t), Vector3.Lerp(scaleMdiddleStart, points[1].transform.localScale, t));
                //SetItemTransformbyTransform(2, Vector3.Lerp(items[2].transform.position, points[0].transform.position, t), Vector3.Lerp(scaleLeftStart, points[0].transform.localScale, t));

                t += Time.deltaTime;
                yield return null;
            }
            Debug.Log("Bitti");
            scaleRightStart = items[1].transform.localScale;
            t = 0;
            while (t <= 1)
            {
                //SetItemTransformbyTransform(1, points[2].transform.position, Vector3.Lerp(scaleRightStart, points[2].transform.localScale, t));

                t += Time.deltaTime;
                yield return null;
            }

        }
        //else if(items.Count > 3)
        //{
        //    while (t <= 1)
        //    {
        //        SetItemTransformbyTransform(1, items[1].transform.position, Vector3.Lerp(scaleRightStart, Vector3.zero, t));
        //        SetItemTransformbyTransform(0, Vector3.Lerp(items[0].transform.position, points[1].transform.position, t), Vector3.Lerp(scaleMdiddleStart, points[1].transform.localScale, t));
        //        SetItemTransformbyTransform(2, Vector3.Lerp(items[2].transform.position, points[0].transform.position, t), Vector3.Lerp(scaleLeftStart, points[0].transform.localScale, t));

        //        t += Time.deltaTime;
        //        yield return null;
        //    }
        //}
        if (inventoryIndex == 0)
        {
            inventoryIndex = -1;
            item = items[1];
        }
        else if (inventoryIndex == -1)
        {

        }
        else if (inventoryIndex == 1)
        {
            inventoryIndex = 0;
            item = items[0];
        }

    }

    void SetItemTransform(int index)
    {
        Debug.Log(index);
        if (items[index].GetComponent<Item>().itemName == "Wrench")
        {
            items[index].transform.position = points[index].transform.position;
            items[index].transform.localScale = new Vector3(.3f,.3f,.3f);
            items[index].transform.rotation = points[index].transform.rotation;
        }
        else
        {
            items[index].transform.position = points[index].transform.position;
            items[index].transform.localScale = points[index].transform.localScale;
            items[index].transform.rotation = points[index].transform.rotation;
        }
    }

    void SetItemTransformbyTransform(int index, Vector3 pos, Vector3 scale, Quaternion rotation)
    {
        items[index].transform.position = pos;
        items[index].transform.localScale = scale;
        items[index].transform.rotation = rotation;
    }

    void SetObjectTransformbyTransform(GameObject go, Vector3 pos, Vector3 scale, Quaternion rot)
    {
        go.transform.position = pos;
        go.transform.localScale = scale;
        go.transform.rotation = rot;
    }

    void ApplyGravity()
    {
        if (climbingInput)
        {
            Jump(.31f);
        }
        else
        {
            gravity = normalGravity;
            yVelocity.y += gravity * -2f * Time.deltaTime;
            yVelocity.y = Mathf.Clamp(yVelocity.y, -normalGravity, 999);
            controller.Move(yVelocity * Time.deltaTime);
        }

    }


    void Jump(float multiplier)
    {
        yVelocity.y = Mathf.Sqrt(jumpHeight * 10f * multiplier * normalGravity);
        controller.Move(yVelocity * Time.deltaTime);
        jumpCharges = 0;
    }

    IEnumerator Note()
    {
        float t = 0f;
        while (t <= 1)
        {
            SetObjectTransformbyTransform(note, Vector3.Lerp(note.transform.position, noteGO.transform.position, t), Vector3.Lerp(note.transform.localScale, noteGO.transform.localScale, t), Quaternion.Lerp(note.transform.rotation, noteGO.transform.rotation, t));
            if (t < .4f)
                t += Time.deltaTime;
            else t = 1.01f;
            yield return null;
        }
    }

    IEnumerator NoteBack()
    {
        float t = 0f;
        while (t <= 1)
        {
            SetObjectTransformbyTransform(note, Vector3.Lerp(note.transform.position, noteStartTrans, t), Vector3.Lerp(note.transform.localScale, noteStartTransS, t), Quaternion.Lerp(note.transform.rotation, noteStartTransR, t));
            if (t < .4f)
                t += Time.deltaTime;
            else t = 1.01f;
            yield return null;
        }
    }

    public void UseItem()
    {
        Debug.LogWarning("ah");
        hits = Physics.BoxCastAll(boxCollider.bounds.center, boxCollider.bounds.size * 2f, transform.forward, transform.rotation, 2f);
        GetComponent<AudioSource>().Play();
        meshR.material = greenM;
        foreach (RaycastHit hit in hits)
        {
            Debug.Log(hit.collider);
            if (hit.collider.CompareTag("door") && puzlleDone)
            {
                Debug.Log("door: " + hit.collider);
                StartCoroutine(UseItemVisual());
                //GameObject.Find("Metal Kapı (5)").GetComponent<MoveDoor>().MoveDoorX();
                //GameObject.Find("Metal Kapı (4)").GetComponent<MoveDoor>().MoveDoorX();
                GameObject.Find("myDoor").GetComponent<MeshDestroy>().DestroyMesh();

            }
        }
    }

    IEnumerator UseItemVisual()
    {
        yield return new WaitForSeconds(.5f);
        gm.ToggleInventory();
        items.Clear();
    }

    public void TurnOnItems()
    {
        foreach (GameObject go in items)
        {
            go.SetActive(true);
        }
    }
    public void TurnOffItems()
    {
        foreach (GameObject go in items)
        {
            go.SetActive(false);
        }
    }
}
