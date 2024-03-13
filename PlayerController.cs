using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
 // Rigidbody of the player.
 private Rigidbody rb; 

 // Movement along X and Y axes.
 private float movementX;
 private float movementY;

 // Speed at which the player moves.
 public float speed = 0; 
 
 // Jump force of the player.
 private float jumpForce = 7.0f;
 //counter
 public int count;
 //texto dinamico
 public TextMeshProUGUI countText;

 // Start is called before the first frame update.
 void Start()
    {
        SetCountText();
 // Get and store the Rigidbody component attached to the player.
        rb = GetComponent<Rigidbody>();
        count = 0;
    }
 
 // This function is called when a move input is detected.
 void OnMove(InputValue movementValue)
    {
 // Convert the input value into a Vector2 for movement.
        Vector2 movementVector = movementValue.Get<Vector2>();

 // Store the X and Y components of the movement.
        movementX = movementVector.x; 
        movementY = movementVector.y; 
    }
 //Change de counter on the screen
 void SetCountText() 
    {
        countText.text =  "Count: " + count.ToString();
    }
 
// Update is called once per frame.
void Update()
{
    // Check if the space bar is pressed.
    if (Input.GetKeyDown(KeyCode.Space))
    {
        // Apply an upward force to the Rigidbody to make the player jump.
        rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
    }
}

 // FixedUpdate is called once per fixed frame-rate frame.
 private void FixedUpdate() 
    {
 // Create a 3D movement vector using the X and Y inputs.
        Vector3 movement = new Vector3 (movementX, 0.0f, movementY);

 // Apply force to the Rigidbody to move the player.
        rb.AddForce(movement * speed); 
    } 
    void OnTriggerEnter(Collider other) 
    {
        // Check if the object the player collided with has the "PickUp" tag.
        if (other.gameObject.CompareTag("PickUp")) 
        {
            // Deactivate the collided object (making it disappear).
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }
    }
}