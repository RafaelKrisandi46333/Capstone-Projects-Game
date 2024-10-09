using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; //gerakan disetting default
    public float jumpForce = 5f; //lompatan disetting default 
    public Transform cameraTransform;
    public Vector3 cameraOffset = new Vector3(0, 5, -10); 
    public LayerMask groundLayer; 
    public Transform groundCheck; 
    public float groundDistance = 0.4f; 
    private CharacterController ChCon; // untuk gereakan slope lebih dinamis
    public float speedJump;
    public float rotationSpeed;
    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        // Jika kamera belum diatur, cari kamera utama di scene
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform; // Memastikan kamera utamanya udh diambil
        }

        // Jika groundCheck belum diassign di Unity editor, coba cari dari child object
        if (groundCheck == null)
        {
            groundCheck = transform.Find("GroundCheck"); // Nama objek "GroundCheck" harus ada
            if (groundCheck == null)
            {
                Debug.LogError("GroundCheck object is not assigned or found. Please assign it in the inspector.");
            }
        }

        rb = GetComponent<Rigidbody>();
        ChCon = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Mengambil input dari keyboard
        float HoriMove = Input.GetAxis("Horizontal"); 
        float VertiMove = Input.GetAxis("Vertical"); 

        // Membuat vector dari input
        Vector3 moveDirection = transform.right * HoriMove + transform.forward * VertiMove;

        // Membuat movement terlihat natural
        moveDirection.Normalize();

        // Menggerakkan capsule rigidbody
        // rb.MovePosition(transform.position + moveDirection * moveSpeed * Time.deltaTime);

        // // Mengecek apakah player sedang di tanah
        // isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);

        // // Melompat jika player sedang di tanah dan tombol lompat ditekan
        // if (isGrounded && Input.GetButtonDown("Jump"))
        // {
        //     rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z); // Set vertical velocity
        // }

        // Menggerakkan capsule CharacterCollider

        float magnitude = moveDirection.magnitude;
        magnitude = Mathf.Clamp01(magnitude);
        // ChCon.MovePosition(moveDirection * magnitude * moveSpeed);

        speedJump += Physics.gravity.y * Time.deltaTime;
        
        // Melompat jika player sedang di tanah dan tombol lompat ditekan
        if (Input.GetButtonDown("Jump"))
        {
            speedJump = -0.5f;
        }

        Vector3 velo = moveDirection * magnitude;
        velo.y = speedJump;
        ChCon.Move(velo * Time.deltaTime);

        if (ChCon.isGrounded)
        {
            speedJump = -0.5f;
            isGrounded = true;
            if (Input.GetButtonDown("Jump"))
            {
                speedJump = jumpForce;
                isGrounded = false;
            }
        }

        if (moveDirection != Vector3.zero)
        {
            Quaternion ToRotating = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, ToRotating, rotationSpeed * Time.deltaTime);
        }

        // // Update posisi kamera agar mengikuti capsule
        // UpdateCameraPosition();
    }

    // void UpdateCameraPosition()
    // {
    //     // Posisi kamera mengikuti capsule dengan offset yang telah ditentukan
    //     cameraTransform.position = transform.position + cameraOffset;

    //     // Kamera menghadap ke capsule
    //     cameraTransform.LookAt(transform);
    // }
}
