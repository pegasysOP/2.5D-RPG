using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float walkSpeed;
    [SerializeField] GameObject interactZone;

    enum Direction { Up, Down, Left, Right }

    Rigidbody rb;
    Animator animator;
    VerticalSpriteLayering verticalSpriteLayering;

    float xMovement;
    float yMovement;
    bool isMoving;
    Direction direction;
    bool movementEnabled = true;

    UnityEvent<GameObject> _OnInteraction;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        verticalSpriteLayering = GetComponent<VerticalSpriteLayering>();

        _OnInteraction = new UnityEvent<GameObject>();
    }

    void Update()
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

    void FixedUpdate()
    {
        if (movementEnabled)
        {
            DoMovement();
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }

    // Take player movement input
    void GetMovementInput()
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
    void DoMovement()
    {
        float currentVertical = rb.velocity.y;
        rb.velocity = isMoving ? new Vector3(xMovement, currentVertical, yMovement).normalized * walkSpeed : new Vector3(0f, currentVertical, 0f);

        // Rotate interact zone
        if (direction == Direction.Up)
            interactZone.transform.rotation = Quaternion.Euler(0, 180, 0);
        else if (direction == Direction.Down)
            interactZone.transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (direction == Direction.Left)
            interactZone.transform.rotation = Quaternion.Euler(0, 270, 0);
        else if (direction == Direction.Right)
            interactZone.transform.rotation = Quaternion.Euler(0, 90, 0);
    }

    // Trigger animations based on player movement
    void UpdateAnimations()
    {
        if(isMoving)
        {
            animator.SetBool("isMoving", true);
            animator.SetFloat("xMovement", xMovement);
            animator.SetFloat("yMovement", yMovement);

            verticalSpriteLayering.UpdateSortingOrder();
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    // Tries to interact with an objects within range of the interactZone, returns null if there are none
    GameObject TryInteract()
    {
        Collider[] results = new Collider[5];

        // change to perhaps use a flag on a dedicated script on the interact zone now that it's 3d

        //if(interactZone.GetComponent<Collider>()(new ContactFilter(), results) > 0)
        //{
        //    foreach(Collider result in results)
        //    {
        //        if (result != null)
        //        {
        //            GameObject interactedObject = result.gameObject;
        //
        //            if (interactedObject.GetComponent<Interactable>() != null)
        //            {
        //                OnInteraction.Invoke(interactedObject);
        //                return interactedObject;
        //            }
        //        }
        //    }
        //}

        return null;
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
