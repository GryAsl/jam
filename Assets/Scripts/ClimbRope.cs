//using UnityEngine;
//using StarterAssets;

//public class ClimbRope : MonoBehaviour
//{
//    private bool isNearRope = false;
//    private bool isClimbing = false;
//    private CharacterController characterController;
//    private ThirdPersonController thirdPersonController;

//    public float climbSpeed = 3f;

//    private void Start()
//    {
//        characterController = GetComponent<CharacterController>();
//        thirdPersonController = GetComponent<ThirdPersonController>();
//    }

//    private void Update()
//    {
//        // E�er yak�ndaysa ve bo�luk tu�una bas�l�yorsa t�rmanmaya ba�la
//        if (isNearRope && Input.GetKey(KeyCode.Space))
//        {
//            isClimbing = true;
//        }
//        // Bo�luk b�rak�l�rsa t�rmanmay� b�rak
//        else if (Input.GetKeyUp(KeyCode.Space))
//        {
//            isClimbing = false;
//        }

//        // T�rmanma aktifse yukar� hareket ettir
//        if (isClimbing)
//        {
//            Vector3 climb = new Vector3(0, climbSpeed, 0);
//            characterController.Move(climb * Time.deltaTime);
//            if (thirdPersonController != null)
//            {
//                thirdPersonController.Gravity = 0;
//            }

//            Debug.Log("T�rman�yor: " + climb);
//        }
//        else
//        {
//            // T�rmanm�yorsa gravity'yi a�
//            if (thirdPersonController != null && thirdPersonController.Gravity == 0)
//            {
//                thirdPersonController.Gravity = -15f;
//                Debug.Log("Gravity geri a��ld�.");
//            }
//        }
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.CompareTag("Climbable"))
//        {
//            isNearRope = true;
//            Debug.Log("Climbable objeye yakla��ld�.");
//        }
//    }

//    private void OnTriggerExit(Collider other)
//    {
//        if (other.CompareTag("Climbable"))
//        {
//            isNearRope = false;
//            isClimbing = false;

//            if (thirdPersonController != null)
//            {
//                thirdPersonController.Gravity = -15f;
//            }

//            Debug.Log("Climbable objeden uzakla��ld�.");
//        }
//    }
//}
