using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody playerRigidbody;
    [SerializeField] Animator playerAnimator;

    [SerializeField] InteractZone interactZone;
    [SerializeField] float walkSpeed;

    enum Direction { Up, Down, Left, Right }

    float xMovement;
    float yMovement;
    bool isMoving;
    Direction direction;
    bool movementEnabled = true;

    UnityEvent<GameObject> _OnInteraction = new UnityEvent<GameObject>();

    public void Update()
    {
        if (movementEnabled)
        {
            GetMovementInput();

            //UpdateAnimations();

            if (Input.GetKeyDown(KeyCode.E))
            {
                TryInteract();
            }
        }
    }

    public void FixedUpdate()
    {
        if (movementEnabled)
        {
            DoMovement();
        }
        else
        {
            playerRigidbody.velocity = new Vector3(0, playerRigidbody.velocity.y, 0);
        }
    }

    // Take player movement input
    private void GetMovementInput()
    {
        xMovement = Input.GetAxisRaw("Horizontal");
        yMovement = Input.GetAxisRaw("Vertical");

        if ((xMovement != 0f || yMovement != 0f))
        {
            isMoving = true;

            if (xMovement > 0f)
                direction = Direction.Right;
            else if (xMovement < 0f)
                direction = Direction.Left;
            else if (yMovement > 0f)
                direction = Direction.Up;
            else if (yMovement < 0f)
                direction = Direction.Down;
        }
        else
        {
            isMoving = false;
        }
    }

    // Executes player movement
    private void DoMovement()
    {
        float currentVertical = playerRigidbody.velocity.y;
        playerRigidbody.velocity = isMoving ? new Vector3(xMovement, currentVertical, yMovement).normalized * walkSpeed : new Vector3(0f, currentVertical, 0f);

        // Rotate interact zone
        if (direction == Direction.Up)
            interactZone.transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (direction == Direction.Right)
            interactZone.transform.rotation = Quaternion.Euler(0, 90, 0);
        else if (direction == Direction.Down)
            interactZone.transform.rotation = Quaternion.Euler(0, 180, 0);
        else if (direction == Direction.Left)
            interactZone.transform.rotation = Quaternion.Euler(0, 270, 0);
    }

    // Tries to interact with an objects within range of the interactZone, returns null if there are none
    private GameObject TryInteract()
    {
        foreach(Collider result in interactZone.Colliders)
        {
            if (result != null)
            {
                GameObject interactedObject = result.gameObject;
        
                if (interactedObject.GetComponent<Interactable>() != null)
                {
                    OnInteraction.Invoke(interactedObject);
                    return interactedObject;
                }
            }
        }

        return null;
    }

    // Trigger animations based on player movement
    private void UpdateAnimations()
    {
        if(isMoving)
        {
            playerAnimator.SetBool("isMoving", true);
            playerAnimator.SetFloat("xMovement", xMovement);
            playerAnimator.SetFloat("yMovement", yMovement);
        }
        else
        {
            playerAnimator.SetBool("isMoving", false);
        }
    }


    // Enable or disable the players movement
    public void EnableMovement(bool enabled)
    {
        this.movementEnabled = enabled;
    }

    public UnityEvent<GameObject> OnInteraction
    {
        get { return _OnInteraction; }
    }
}
