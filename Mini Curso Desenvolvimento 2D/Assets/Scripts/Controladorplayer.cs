using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controladorplayer : MonoBehaviour
{
    [SerializeField]
    private float speed, jumpPower, radiusCircle, airTime;
    [SerializeField]
    private Transform feetPosition, mao;
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private GameObject prefHitbox;

    private Rigidbody2D r2;
    private Animator anim;
    private float moveInput, timer;
    private bool pressJump, possJump, startTime, isGrounded, inAtaque = false;

    private void Awake(){
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        r2 = GetComponent<Rigidbody2D>();
        timer = airTime;
    }

    private void Update(){
          
        //Animator
        anim.SetInteger("MovimentoX", (int)moveInput);
        anim.SetFloat("MovimentoY", r2.velocity.y);
        anim.SetBool("isGrounded", isGrounded);

        //Ataque
        if (Input.GetKeyDown(KeyCode.J) && !inAtaque && isGrounded){
            anim.SetTrigger("Ataque");
            inAtaque = true;
        }

        //Primeira parte do pulo
        if (Input.GetButtonDown("Jump") && !inAtaque)
        {
            pressJump = true;
        }
        if (pressJump && Input.GetButtonUp("Jump"))
        {
            possJump = true;
        }
        if (startTime)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                possJump = true;
            }
        }

    }
    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(feetPosition.position, radiusCircle, groundLayer);

        // movimentacao
        moveInput = Input.GetAxisRaw("Horizontal");

        if (inAtaque){
            moveInput = 0;
            r2.velocity = new Vector2(0, 0);
        }

        r2.velocity = new Vector2(moveInput * speed, r2.velocity.y);

        //Girar
        if (moveInput > 0){
            transform.eulerAngles = new Vector3(0, 0, 0);
        }else if (moveInput < 0){
            transform.eulerAngles = new Vector3(0, 180, 0);
        }


        //Segunda parte do pulo
        if (pressJump && isGrounded)
        {
            startJump();
        }
        else if (possJump)
        {
            stopJump();
        }

        void startJump(){
            r2.gravityScale = 3;
            r2.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
            pressJump = false;
        }
        void stopJump()
        {
            r2.gravityScale = 5;
            possJump = false;
            timer = airTime;
        }
    }

    public void fimAtaque(){
        inAtaque = false;
    }
    public void hitboxAtaque(){
        GameObject hitboxtemp = Instantiate(prefHitbox, mao.position, transform.localRotation);
        Destroy(hitboxtemp, 0.3f);
    }
}
