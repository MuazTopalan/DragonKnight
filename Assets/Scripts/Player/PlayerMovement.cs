using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header ("Movement parameters")]
    [SerializeField] private float speed; // SerializeField just means that we can directly change the  variable from the Inspector tab later on
    [SerializeField] private float jumpPower;

    [Header("Coyote time")]
    [SerializeField] private float coyoteTime;  //time between leaving the edge and being able to perform the jump
    private float coyoteCounter;    //how much time has been passed since leaving the edge 

    [Header("Multiple jumps")]
    [SerializeField] private int extraJumps;
    private int jumpCounter;

    [Header("Wall jumping")]
    [SerializeField] private float wallJumpX;
    [SerializeField] private float wallJumpY;

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    [Header("SFX")]
    [SerializeField] private AudioClip jumpSound;

    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float horizontalInput;

    private void Awake( )
    {
        // These lines grab references for rigidbody2d and animator from the object
        body = GetComponent<Rigidbody2D>(); 
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        // Flipping player like a papaer according to the side inputs *left-right*
        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        //Set animator paramters
        anim.SetBool("Running", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        //jumping
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        //adjustable jump height
        if (Input.GetKeyUp(KeyCode.Space) && body.velocity.y > 0)
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);

        if (onWall())
        {
            body.gravityScale = 0;
            body.velocity = Vector2.zero;
        }
        else
        {
            body.gravityScale = 5;
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if(isGrounded())
            {
                coyoteCounter = coyoteTime; //reset coyote counter when on the ground
                jumpCounter = extraJumps;
            }
            else
                coyoteCounter -= Time.deltaTime;    // start decrasing untill 0, this way we can still jump
        }
    }

    private void Jump()
    {
        if (coyoteCounter < 0 && !onWall() && jumpCounter <= 0) return; //if coyote counter is 0 or less and not on wall dont do anything

        SoundManager.instance.PlaySound(jumpSound);

        if (onWall())
            WallJump();
        else
        {
            if (isGrounded())
                body.velocity = new Vector2(body.velocity.x, jumpPower);
            else
            {
                //if not on the ground and coyoteCTR is bigger than 0 do normal jump
                if (coyoteCounter > 0)
                    body.velocity = new Vector2(body.velocity.x, jumpPower);
                else
                {
                    if(jumpCounter > 0)
                    {
                        body.velocity = new Vector2(body.velocity.x, jumpPower);
                        jumpCounter --;
                    }
                }
            }

            //reset the ctr
            coyoteCounter = 0;
        }
    }
    private void WallJump()
    {
        body.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpX, wallJumpY));
        wallJumpCooldown = 0;
    }
    private bool isGrounded ()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down , 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded() && !onWall();
    }
}
       
