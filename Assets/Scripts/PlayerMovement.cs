using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private BoxCollider2D coll;
    [SerializeField] private LayerMask jumpableGround;
    private float dirx;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 7f;
    private enum MovementState {  idle, running, jumping, falling };
    private MovementState state = MovementState.idle;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        dirx = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirx*moveSpeed, rb.velocity.y);

        if(Input.GetButton("Jump") && IsGrounded()){

            rb.velocity = new Vector2(0, jumpForce);
        }

        UpdateAnimationState();

    }
    private void UpdateAnimationState()
    {

        MovementState state;
        if (dirx > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if(dirx < 0f){
            state = MovementState.running;
            sprite.flipX = true;
        }
        else{
            state = MovementState.idle;
        }

        if(rb.velocity.y > .1f){
            state = MovementState.jumping;
        }
        else if(rb.velocity.y < -.1f){
            state = MovementState.falling;
        }
        anim.SetInteger("state",(int)state);
    }
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround );
    }
}
