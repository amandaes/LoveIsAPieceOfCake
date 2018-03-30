using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour {


    private Rigidbody2D rb;
    private Animator myAnimator;
    public float moveSpeed = 5f;
    private bool facingRight;
    private bool attack;

    [SerializeField]
    private Transform[] groundPoints;

    [SerializeField]
    private float groundRadius;
    [SerializeField]
    private LayerMask whatIsGround;

    private bool isGrounded;

    private bool jump;

    [SerializeField]
    private float jumpForce;

	void Start () {
        facingRight = true;
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
	}

   void Update()
    {
        HandleInput(); //code will check the handleinput
    }

    void FixedUpdate () {
        //Moving the player

        float horizontal = Input.GetAxis("Horizontal");

        isGrounded = IsGrounded();

        PlayerMovement(horizontal);
        FlipPlayer(horizontal);
        HandleAttack(); //call the handle attack
        ResetValues();// reset all the values when done attacking to stop attack loop
	}

    private void PlayerMovement(float horizontal)
    {
        if (!this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) //getting the state of the animator at layer 0 or base layer and if tag is not attack, then move player.
        {
            rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
        }

        if (isGrounded && jump)
        {
            isGrounded = false;
            rb.AddForce(new Vector2(0, jumpForce));
        }

        //transitioning the animation from idle to roll
        myAnimator.SetFloat("speed", Mathf.Abs(horizontal));
    }

    //making the player attack animation
    private void HandleAttack()
    {
        if (attack && !this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))//make sure player can't start new attack before the first attack is done
        {
            myAnimator.SetTrigger("attack");
            rb.velocity = Vector2.zero; //set velocity to 0 while attacking so the player doesn't roll while attacking 
        }
    }

    //handles all input for attack, jumping, etc.
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            jump = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            attack = true; //attack bool becomes true
        }
    }

    //flip the player to face right or left
    private void FlipPlayer(float horizontal)
    {
        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            facingRight = !facingRight;

            Vector3 theScale = transform.localScale;
            theScale.x *= -1;//to return the scale value to -1
            transform.localScale = theScale;

        }
    }

    private void ResetValues()
    {
        attack = false;
        jump = false;
    }

    private bool IsGrounded() //check if player is touching ground
    {
        if (rb.velocity.y <= 0)
        {
            foreach (Transform point in groundPoints) //check if groundpoints is colliding with something
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);

                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                        return true; //return true if colliding with something else than player
                    }
                }
            }
        }
        return false; //ootherwise return false
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == ("Enemy"))
        {
            Destroy(col.gameObject);
            Destroy(gameObject);
            SceneManager.LoadScene(2);
        }
    }

}
