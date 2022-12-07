using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController cController;

    public float moveSpeed = 6;

    public float turnSmoothTime = .1f;
    float turnSmoothVelocity;

    public float playerGravity;
    
    public float constantGravity;
    public float maxGravity;

    private float currentGravity;
    private Vector3 gravityDirection;
    private Vector3 gravityMovement;

    private void Awake() {
        gravityDirection = Vector3.down;
    }

    #region Gravity

    private bool IsGrounded() {
        return cController.isGrounded;
    }

    private void CalculateGravity() {
        if(IsGrounded()) {
            currentGravity = constantGravity;
        }
        else {
            if (playerGravity > maxGravity) {
                currentGravity -= playerGravity * Time.deltaTime;
            }
        }

        gravityMovement = gravityDirection * -currentGravity * Time.deltaTime;
    }


    #endregion

    #region Update
    void Update()
    {
        CalculateGravity();

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector3(horizontal, 0, vertical).normalized;

        if(dir.magnitude >= .1f)
        {
            // rotate player to face direction they are moving
            float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            cController.Move((dir.normalized * moveSpeed * Time.deltaTime) + (gravityMovement * Time.deltaTime));
        }
    }
    #endregion
    
    
    /*
    [SerializeField] private float moveSpeed = 1;
    [SerializeField] private Rigidbody rb;

    // Update is called once per frame
    void Update()
    {
        var dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        rb.AddForce(dir * moveSpeed);
    }
    */
}
