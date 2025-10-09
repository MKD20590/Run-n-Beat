using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody rig;
    [SerializeField] private Animator shield;
    [SerializeField] private Animator body;
    public GameManager gm;
    //public Animation walk;

    [SerializeField] private GameObject ground;
    [SerializeField] private Material player_material;
    RaycastHit hit;
    float height;


    [SerializeField] private Collider main_coll;
    [SerializeField] private TrailRenderer trail;
    [SerializeField] private ParticleSystem jumps;
    [SerializeField] private ParticleSystem dashes;
    [SerializeField] private ParticleSystem hurt;
    public int hp = 3;
    int jumped = 0;

    public int totalDoubleJumps = 0;

    float jump = 7f;
    float speed = 4f;
    float dash = 16f;

    [SerializeField] private bool isMoving = true;
    [SerializeField] private bool isGrounded = true;
    [SerializeField] private bool isDashing = false;
    [SerializeField] private bool isHurt = false;
    Vector3 movement;
    
    // Start is called before the first frame update
    void Start()
    {
        hp = 3;

        main_coll = GetComponent<CapsuleCollider>();
        //main_coll.material = null;
        player_material.EnableKeyword("_EMISSION");
        player_material.SetColor("_EmissionColor", Color.white * 2.5f);
        
        trail.emitting = false;
        rig = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(hp);
        //movement input
        movement.x = Input.GetAxis("Horizontal");
        movement.z = Input.GetAxis("Vertical");


        //raycast for grounding
        Ray ray = new Ray(transform.position, Vector3.down);
        //Debug.DrawRay(transform.position, Vector3.down, Color.red);
        if(Physics.Raycast(ray,out hit))
        {
            height = hit.distance - 0.5f;
        }
        if(height<=0.37f)
        {
            isGrounded = true;
            body.ResetTrigger("dash");
            body.ResetTrigger("jump1");
            body.ResetTrigger("jump2");
        }
        else if(height<=0.55 && isHurt)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        //Debug.Log(height);
        //Debug.Log(jumped);
        body.SetBool("isGrounded", isGrounded);
        
        //movement
        if (movement != Vector3.zero && !isDashing && isMoving && !gm.isDied && !gm.win && !gm.isPaused)
        {
            rig.velocity = new Vector3(movement.x * speed, rig.velocity.y, movement.z * speed);
            body.SetBool("iswalking", true);
        }
        else if(movement == Vector3.zero)
        {
            body.SetBool("iswalking", false);
        }

        //jumping
        if(!gm.isPaused && Input.GetKeyDown(KeyCode.Space) && jumped < 2 && !gm.isDied && !gm.win)
        {
            jumped++;
            if(jumped == 1 && isGrounded)
            {
                body.SetTrigger("jump1");
                body.ResetTrigger("jump2");
            }
            else if(jumped==2)
            {
                totalDoubleJumps++;
                body.ResetTrigger("jump1");
                body.SetTrigger("jump2");
            }
            rig.velocity = new Vector3(movement.x * speed, jump, movement.z * speed);
            jumps.Play();
        }

        //dashing
        if(!gm.isPaused && !gm.isDied && !gm.win && !isHurt && movement != Vector3.zero && Input.GetKeyDown(KeyCode.LeftShift)
            ||
            !gm.isPaused && !gm.isDied && !gm.win && !isHurt && movement != Vector3.zero && Input.GetKeyDown(KeyCode.RightShift))
        {
            trail.emitting = true;
            isMoving = false;
            dashes.Play();
            isDashing = true;
            StartCoroutine(DashOrHurt());
        }

        //player rotation
        if(!gm.isPaused && movement != Vector3.zero && !gm.isDied && !gm.win)
        {
            Vector3 rotation = movement * speed;
            rotation.y = 0f;
            rotation.Normalize();
            Quaternion rotate = Quaternion.LookRotation(rotation, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotate, 600f * Time.deltaTime);
        }

        //winning
        if(gm.win)
        {
            movement = Vector3.zero;
        }
    }
    IEnumerator DashOrHurt()
    {
        //dashing
        if (isDashing)
        {
            body.SetTrigger("dash");
            body.ResetTrigger("jump1");
            body.ResetTrigger("jump2");
            rig.velocity = new Vector3(movement.x * dash, rig.velocity.y, movement.z * dash);
            yield return new WaitForSeconds(0.15f);
            rig.velocity = new Vector3(movement.x * speed, rig.velocity.y, movement.z * speed);
            yield return new WaitForSeconds(0.35f);
            trail.emitting = false;
            isMoving = true;
            isDashing = false;
        }

        //hurt
        if(isHurt)
        {
            main_coll.enabled = false;
            yield return new WaitForSeconds(0.3f);
            player_material.SetColor("_EmissionColor", Color.white * 2.5f);
            isMoving = true;

            yield return new WaitForSeconds(2f);
            isHurt = false;
            if(hp>0)
            {
                main_coll.enabled = true;
                body.SetBool("hurt", false);
                shield.ResetTrigger("hurt");
                shield.SetTrigger("continue");
            }
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        //laser collision
        if (collision.gameObject.tag == "obstacle" && !isHurt && !gm.win && !gm.isDied && !gm.isPaused)
        {
            hp--;
            player_material.SetColor("_EmissionColor", Color.red * 2.5f);
            if (hp > 0)
            {
                shield.ResetTrigger("continue");
                shield.SetTrigger("hurt");
            }
            else
            {
                body.SetTrigger("died");
            }
            body.SetBool("hurt", true);

            isMoving = false;

            //knockback effect
            Vector3 laser = this.transform.position - collision.transform.position;
            laser.y = 2.5f;
            rig.AddForce(laser.normalized * 3.5f, ForceMode.Impulse);
            isHurt = true;
            hurt.Play();
            StartCoroutine(DashOrHurt());
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //is grounded, reset jumped
        if (isGrounded && collision.gameObject.tag == "ground" && !gm.win && !gm.isDied && !gm.isPaused)
        {
            if (body.GetCurrentAnimatorStateInfo(0).IsName("onAir"))
                jumped = 0;
        }

        //spike collision
        if (collision.gameObject.tag == "obstacle" && !isHurt && !gm.win && !gm.isDied && !gm.isPaused)
        {
            hp--;
            player_material.SetColor("_EmissionColor", Color.red * 2.5f);
            if(hp>0)
            {
                shield.ResetTrigger("continue");
                shield.SetTrigger("hurt");
            }
            else
            {
                body.SetTrigger("died");
            }

            body.SetBool("hurt",true);

            isMoving = false;

            //knockback effect
            Vector3 spike = this.transform.position - collision.transform.position;
            spike.y = 1.5f;
            rig.AddForce(spike * 3f, ForceMode.Impulse);
            isHurt = true;
            hurt.Play();
            StartCoroutine(DashOrHurt());
        }
    }
}
