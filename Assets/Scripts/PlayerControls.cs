using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    private Rigidbody2D rbd;

    [Header("Wilk")]
    private float horizontalmovement = 0f;
    [SerializeField] private float speedlmovement;
    [Range(0,0.3f)][SerializeField] private float smoothnessmovement;
    private Vector3 speed = Vector3.zero;
    private bool lookright = true;


    [Header("Jump")]
    [SerializeField] private float forcejump;
    [SerializeField] private LayerMask whatissoil;
    [SerializeField] private Transform floorcontroller;
    [SerializeField] private Vector3 boxdimensions;
    [SerializeField] private bool onground;
    private bool jump = false;

    [Header("Animation")]
    private Animator animator;


    private void Start()
    {
        rbd = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        horizontalmovement = Input.GetAxisRaw("Horizontal") * speedlmovement;

        animator.SetFloat("Horizontal", Mathf.Abs(horizontalmovement));

        animator.SetFloat("Speedy", rbd.velocity.y);

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }


    }

    private void FixedUpdate()
    {
        movement(horizontalmovement * Time.fixedDeltaTime, jump);

        onground = Physics2D.OverlapBox(floorcontroller.position, boxdimensions, 0f, whatissoil);
        jump= false;

        animator.SetBool("Onground", onground);
    }

    private void movement(float movement , bool jump)
    {
        Vector3 objectspeed = new Vector2(movement, rbd.velocity.y);
        rbd.velocity = Vector3.SmoothDamp(rbd.velocity, objectspeed, ref speed, smoothnessmovement); 

        if (movement > 0 && lookright)
        {
            spin();
        }
        else if (movement < 0 && lookright)
        {
            spin();
        }

        if (onground && jump)
        {
            onground = false;
            rbd.AddForce(new Vector2(0f , forcejump));
        }
    }

    private void spin()
    {
        lookright = !lookright;
        Vector3 scale = transform.localScale;
        scale.x *= 1;
        transform.localScale = scale; 
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(floorcontroller.position, boxdimensions);
    }
}
