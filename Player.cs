using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
    //basic movment
    public float moveSpeed;
    public float sprintSpeed;
    private float moveSpeedHelp;
    public float jumpHeight;
    //double jump
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask WhatIsGround;
    private bool grounded;
    private bool doublejump; 
    //other
    private Animator anim;
    static Vector2 start;
    //wall jump
    public bool wallSliding;
    public Transform wallCheckPoint;
    public float wallCheckRadius;
    public bool wallCheck;
    public LayerMask wallLayerMask;
    public bool facingRight;
    public bool walldeb;
    //Death Screen
    public Death death;
    public SpriteRenderer rend;
    private Rigidbody2D rb2d;
    public bool canMove=true;
    // Respawn Checkpoint
    public CheckPoint checkPoint;




    void Start()
    {
        anim = GetComponent<Animator>();
        
        start.x = this.transform.position.x;
        start.y = this.transform.position.y;
        moveSpeedHelp = moveSpeed;
        //gm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        //Health
        //curHealth = maxHealth;
    }

    
        
      


    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, WhatIsGround);
    }
	
	//Movment is gold in this part, simple but I worked hard to master it :)
	void Update ()
    {
        if (grounded)
            doublejump = false;
        anim.SetBool("Grounded", grounded);

        //////////////////////////////////////////////////////////////////////////////////////////////

        if (Input.GetKeyDown(KeyCode.W) && grounded && !wallSliding &&canMove == true)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.W) && !grounded && !doublejump && !wallSliding)
        {
            Jump();
            doublejump = true;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////
        
        if((Input.GetKey(KeyCode.D))&& canMove==true)
        {
            
            facingRight = true;
            
            GetComponent<Rigidbody2D>().velocity = new Vector2(moveSpeed, GetComponent<Rigidbody2D>().velocity.y);
       
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveSpeed = sprintSpeed;
        }
        if (((Input.GetKeyUp(KeyCode.LeftShift))==true))
        {                               
                    moveSpeed = moveSpeedHelp;            
        }

        ///////////////////////////////////////////////////////////////////////////////////

        if ((Input.GetKey(KeyCode.A))&& canMove == true)
        {
            facingRight = false;
            
            
            GetComponent<Rigidbody2D>().velocity = new Vector2(-moveSpeed, GetComponent<Rigidbody2D>().velocity.y);           
        }
        ///////////////////////////////////////////////////////////////////////////////////

        if (Input.GetKeyUp(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        //////////////////////////////////////////////////////////////////////////////////
        //Stuff to smooth move camera i took it from some Tuturial xD
        anim.SetFloat("Speed", Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x));
        if (GetComponent<Rigidbody2D>().velocity.x > 0)
            transform.localScale = new Vector3(1f, 1f, 1f);
        else if(GetComponent<Rigidbody2D>().velocity.x < 0)
            transform.localScale = new Vector3(-1f, 1f, 1f);

       

        if(this.transform.position.y < -7)
        {
            this.transform.position = new Vector2(start.x, start.y);
        }

        ////////////////////////////////////////////////////////////////
       
        //Wall jump checking
        
            wallCheck = Physics2D.OverlapCircle(wallCheckPoint.position, wallCheckRadius, wallLayerMask);


            if (facingRight || !facingRight)
            {
                if(wallCheck)
                {                
                        HandleWallsSliding();
                }
                
            }
       
        ////////////////////////////////////////////////////////////////////////////////////////////////////
       



    }

    public void HandleWallsSliding()
    {
        walldeb = false;
       if (facingRight && (Input.GetKeyUp(KeyCode.W)))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-5, jumpHeight);
            transform.localScale = new Vector3(1f, 1f, 1f);
            facingRight = false;
            walldeb = true;
        }
        if (!facingRight && (Input.GetKeyUp(KeyCode.W)) && walldeb==false)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(5, jumpHeight);
            transform.localScale = new Vector3(-1f, 1f, 1f);
            facingRight = true;
        }
        walldeb = true;
    }

    void Jump()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, jumpHeight);
    }
   public void OnCollisionEnter2D(Collision2D coll)
    {
       if (coll.gameObject.tag == "Obstacle")
       {
            canMove = false;
            death.DeathScreenDo();
        }

        if (coll.gameObject.tag == "SlimeWall")
        {
            HandleWallsSliding();
        }
    }
    

   // void OnTriggerEnter2D(Collider2D col)
   // {
   //     if (col.CompareTag("Coin"))
   //     {
   //         Destroy(col.gameObject);
   //         gm.points += 5;
   //     }
       

  //  }
    public void Die()
    {
        rend = GetComponent<SpriteRenderer>();
        rend.enabled = false;
        canMove = false;
        
    }
    public void UnDie()
    {
        rend = GetComponent<SpriteRenderer>();
        rend.enabled = true;
        canMove = true;
    }

   // public void Damage(int dmg)
    //{
    //    curHealth -= dmg;
   // }
    

}
