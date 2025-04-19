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
//        // Eðer yakýndaysa ve boþluk tuþuna basýlýyorsa týrmanmaya baþla
//        if (isNearRope && Input.GetKey(KeyCode.Space))
//        {
//            isClimbing = true;
//        }
//        // Boþluk býrakýlýrsa týrmanmayý býrak
//        else if (Input.GetKeyUp(KeyCode.Space))
//        {
//            isClimbing = false;
//        }

//        // Týrmanma aktifse yukarý hareket ettir
//        if (isClimbing)
//        {
//            Vector3 climb = new Vector3(0, climbSpeed, 0);
//            characterController.Move(climb * Time.deltaTime);
//            if (thirdPersonController != null)
//            {
//                thirdPersonController.Gravity = 0;
//            }

//            Debug.Log("Týrmanýyor: " + climb);
//        }
//        else
//        {
//            // Týrmanmýyorsa gravity'yi aç
//            if (thirdPersonController != null && thirdPersonController.Gravity == 0)
//            {
//                thirdPersonController.Gravity = -15f;
//                Debug.Log("Gravity geri açýldý.");
//            }
//        }
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.CompareTag("Climbable"))
//        {
//            isNearRope = true;
//            Debug.Log("Climbable objeye yaklaþýldý.");
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

//            Debug.Log("Climbable objeden uzaklaþýldý.");
//        }
//    }
//}
