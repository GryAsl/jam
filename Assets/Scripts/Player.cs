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
    float rotX = 0f;


    [Header("Movement")]
    public float speedMultiplier;
    public float runSpeed;
    public float walkSpeed;
    public float maxSpeed;
    CharacterController controller;
    Vector3 move;
    Vector3 input;
    public bool climbingInput;
    public Transform groundCheck;
    public Vector3 yVelocity;
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

    [Header("Other")]
    public GameObject flashlight;
    public GameManager gm;
    public GameObject[] points;
    public GameObject point;
    public GameObject pointLeft;
    public GameObject pointRight;
    public int inventoryIndex;
    public TextMeshProUGUI nameText;




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


            HandleMovementInput();
        if (gm.isInventoryOn)
            return;
        rotY += Input.GetAxis("Mouse X") * camSens;
        rotX += Input.GetAxis("Mouse Y") * camSens;
        rotX = Mathf.Clamp(rotX, minX, maxX);

        transform.localEulerAngles = new Vector3(0, rotY, 0);
        cam.transform.localEulerAngles = new Vector3(-rotX, 0, 0);


        GroundedMov();
        CheckGround();
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
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            maxSpeed = walkSpeed;
            StartCoroutine(DecreaseFOV());
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            hits = Physics.BoxCastAll(boxCollider.bounds.center, boxCollider.bounds.size, transform.forward, transform.rotation, 1f);
            foreach(RaycastHit hit in hits)
            {
                if (hit.collider.CompareTag("item"))
                {
                    Debug.Log(hit.collider);
                    items.Add(hit.collider.gameObject);
                    hit.collider.gameObject.GetComponent<Item>().StartAnim();
                }
                else if (hit.collider.CompareTag("climb"))
                {
                    Debug.Log(hit.collider);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            gm.ToggleInventory();
            SetItemTransform(0);
            SetItemTransform(1);
            SetItemTransform(2);

        }

        if(Input.GetKeyUp(KeyCode.Space) && jumpCharges > 0)
        {
            Jump();
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
            Debug.Log("+");
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
            Debug.Log("-");
            cam.fieldOfView -= 50f * Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator MoveRight()
    {
        float t = 0;

        if(items.Count == 2)
        {
            Vector3 scaleMdiddleStart = items[0].transform.localScale;
            Vector3 scaleRightStart = items[1].transform.localScale;
            while (t <= 1)
            {
                if (inventoryIndex == 0)
                {
                    SetItemTransformbyTransform(1, items[1].transform.position, Vector3.Lerp(scaleRightStart, Vector3.zero, t));
                    SetItemTransformbyTransform(0, Vector3.Lerp(items[0].transform.position, points[1].transform.position, t), Vector3.Lerp(scaleMdiddleStart, points[1].transform.localScale, t));
                    nameText.text = items[1].GetComponent<Item>().itemName;
                }
                else if (inventoryIndex == -1)
                {
                    SetItemTransformbyTransform(1, Vector3.Lerp(items[1].transform.position, points[1].transform.position, t), Vector3.Lerp(scaleRightStart, points[1].transform.localScale, t));
                    SetItemTransformbyTransform(0, Vector3.Lerp(items[0].transform.position, points[0].transform.position, t), Vector3.Lerp(scaleMdiddleStart, points[0].transform.localScale, t));
                    nameText.text = items[0].GetComponent<Item>().itemName;
                }



                Debug.LogWarning(t);
                if (t < .4f)
                    t += Time.deltaTime;
                else t = 1.01f;
                    yield return null;
            }
            Debug.Log("Bitti");
            if (inventoryIndex == 0)
            {
                scaleRightStart = items[1].transform.localScale;
                t = 0;
                while (t <= 1)
                {
                    SetItemTransformbyTransform(1, points[0].transform.position, Vector3.Lerp(Vector3.zero, points[0].transform.localScale, t));
                    Debug.LogWarning("2: " + t);

                    t += Time.deltaTime * 2f;
                    yield return null;
                }
            }
        }
        else if (items.Count == 3){
            Vector3 scaleMdiddleStart = items[0].transform.localScale;
            Vector3 scaleRightStart = items[1].transform.localScale;
            Vector3 scaleLeftStart = items[2].transform.localScale;
            while (t <= 1)
            {
                SetItemTransformbyTransform(1, items[1].transform.position, Vector3.Lerp(scaleRightStart, Vector3.zero, t));
                SetItemTransformbyTransform(0, Vector3.Lerp(items[0].transform.position, points[1].transform.position, t), Vector3.Lerp(scaleMdiddleStart, points[1].transform.localScale, t));
                SetItemTransformbyTransform(2, Vector3.Lerp(items[2].transform.position, points[0].transform.position, t), Vector3.Lerp(scaleLeftStart, points[0].transform.localScale, t));

                t += Time.deltaTime;
                yield return null;
            }
            Debug.Log("Bitti");
            scaleRightStart = items[1].transform.localScale;
            t = 0;  
            while (t <= 1)
            {
                SetItemTransformbyTransform(1, points[2].transform.position, Vector3.Lerp(scaleRightStart, points[2].transform.localScale, t));

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
            inventoryIndex++;
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
                    SetItemTransformbyTransform(1, Vector3.Lerp(items[1].transform.position, points[0].transform.position, t), Vector3.Lerp(scaleRightStart, points[0].transform.localScale, t));
                    SetItemTransformbyTransform(0, Vector3.Lerp(items[0].transform.position, points[2].transform.position, t), Vector3.Lerp(scaleMdiddleStart, points[2].transform.localScale, t));
                    nameText.text = items[1].GetComponent<Item>().itemName;
                }
                else if( inventoryIndex == -1)
                {
                    SetItemTransformbyTransform(1, Vector3.Lerp(items[1].transform.position, points[1].transform.position, t), Vector3.Lerp(scaleRightStart, points[1].transform.localScale, t));
                    SetItemTransformbyTransform(0, Vector3.Lerp(items[0].transform.position, points[0].transform.position, t), Vector3.Lerp(scaleMdiddleStart, points[0].transform.localScale, t));
                    nameText.text = items[0].GetComponent<Item>().itemName;
                }
                else if (inventoryIndex == 1)
                {
                    SetItemTransformbyTransform(1, Vector3.Lerp(items[1].transform.position, points[2].transform.position, t), Vector3.Lerp(scaleRightStart, points[2].transform.localScale, t));
                    SetItemTransformbyTransform(0, Vector3.Lerp(items[0].transform.position, points[0].transform.position, t), Vector3.Lerp(scaleMdiddleStart, points[0].transform.localScale, t));
                    nameText.text = items[0].GetComponent<Item>().itemName;
                }
                Debug.LogWarning(t);
                if (t < .4f)
                    t += Time.deltaTime;
                else t = 1.01f;
                yield return null;
            }
            Debug.Log("Bitti");
            scaleRightStart = items[1].transform.localScale;
            t = 0;
            
        }
        else if (items.Count == 3)
        {
            Vector3 scaleMdiddleStart = items[0].transform.localScale;
            Vector3 scaleRightStart = items[1].transform.localScale;
            Vector3 scaleLeftStart = items[2].transform.localScale;
            while (t <= 1)
            {
                SetItemTransformbyTransform(1, items[1].transform.position, Vector3.Lerp(scaleRightStart, Vector3.zero, t));
                SetItemTransformbyTransform(0, Vector3.Lerp(items[0].transform.position, points[1].transform.position, t), Vector3.Lerp(scaleMdiddleStart, points[1].transform.localScale, t));
                SetItemTransformbyTransform(2, Vector3.Lerp(items[2].transform.position, points[0].transform.position, t), Vector3.Lerp(scaleLeftStart, points[0].transform.localScale, t));

                t += Time.deltaTime;
                yield return null;
            }
            Debug.Log("Bitti");
            scaleRightStart = items[1].transform.localScale;
            t = 0;
            while (t <= 1)
            {
                SetItemTransformbyTransform(1, points[2].transform.position, Vector3.Lerp(scaleRightStart, points[2].transform.localScale, t));

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
        inventoryIndex--;
    }

    void SetItemTransform(int index)
    {
        items[index].transform.position = points[index].transform.position;
        items[index].transform.localScale = points[index].transform.localScale;
        items[index].transform.rotation = points[index].transform.rotation;
    }

    void SetItemTransformbyTransform(int index, Vector3 pos, Vector3 scale)
    {
        items[index].transform.position = pos;
        items[index].transform.localScale = scale;
    }

    void ApplyGravity()
    {
        gravity = normalGravity;
        yVelocity.y += gravity * -2f * Time.deltaTime;
        controller.Move(yVelocity * Time.deltaTime);
    }

    void CheckGround()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 20f, groundMask);
        if (isGrounded)
        {
            jumpCharges = 1;
        }
    }


    void Jump()
    {
        Debug.Log("ALLAH B�Y�K");
       yVelocity.y = Mathf.Sqrt(jumpHeight * -2000f * normalGravity);
    }
}
