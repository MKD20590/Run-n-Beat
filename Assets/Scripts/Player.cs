using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //[SerializeField] private PhysicMaterial slippery;
    [SerializeField] private Rigidbody rig;
    [SerializeField] private Animator shield;
    [SerializeField] private Animator body;
    public GameManager gm;
    //public Animation walk;

    [SerializeField] private GameObject ground;
    [SerializeField] private Material player_material;
    //private Material faces;
    //public Collider hurt_coll;
    RaycastHit hit;
    float height;


    [SerializeField] private Collider main_coll;
    [SerializeField] private TrailRenderer trail;
    [SerializeField] private ParticleSystem jumps;
    [SerializeField] private ParticleSystem dashes;
    [SerializeField] private ParticleSystem hurt;
    public int hp = 3;
    int jumped = 0;

    public int totalDoubleJ = 0;

    float jump = 7f;
    float speed = 4f;
    float dash = 16f;

    bool moved = true;
    bool isGrounded = true;
    bool dashed = false;
    bool hurted = false;
    Vector3 move;
    
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
        move.x = Input.GetAxis("Horizontal");
        move.z = Input.GetAxis("Vertical");


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
        else if(height<=0.55 && hurted)
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
        if (move != Vector3.zero && !dashed && moved && !gm.died && !gm.win && !gm.isPaused)
        {
            rig.velocity = new Vector3(move.x * speed, rig.velocity.y, move.z * speed);
            body.SetBool("iswalking", true);
        }
        else if(move == Vector3.zero)
        {
            body.SetBool("iswalking", false);
        }

        //jumping
        if(!gm.isPaused && Input.GetKeyDown(KeyCode.Space) && jumped < 2 && !gm.died && !gm.win)
        {
            jumped++;
            if(jumped==1 && isGrounded)
            {
                //Debug.Log("kikuk");
                body.SetTrigger("jump1");
                body.ResetTrigger("jump2");
            }
            else if(jumped==2)
            {
                totalDoubleJ++;
                body.ResetTrigger("jump1");
                body.SetTrigger("jump2");
            }
            rig.velocity = new Vector3(move.x * speed, jump, move.z * speed);
            jumps.Play();
        }

        //dashing
        if(!gm.isPaused && !gm.died && !gm.win && !hurted && move != Vector3.zero && Input.GetKeyDown(KeyCode.LeftShift) || !gm.isPaused && Input.GetKeyDown(KeyCode.RightShift) && !dashed && !hurted && !gm.died && !gm.win)
        {
            trail.emitting = true;
            moved = false;
            dashes.Play();
            dashed = true;
            StartCoroutine(Game());
        }

        //player rotation
        if(!gm.isPaused && move != Vector3.zero && !gm.died && !gm.win)
        {
            Vector3 rotation = move * speed;
            rotation.y = 0f;
            rotation.Normalize();
            Quaternion rotate = Quaternion.LookRotation(rotation, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotate, 600f * Time.deltaTime);
        }

        //winning
        if(gm.win)
        {
            move = Vector3.zero;
        }
    }
    IEnumerator Game()
    {
        //dashing
        if (dashed)
        {
            body.SetTrigger("dash");
            body.ResetTrigger("jump1");
            body.ResetTrigger("jump2");
            rig.velocity = new Vector3(move.x * dash, rig.velocity.y, move.z * dash);
            //main_coll.material = slippery;
            yield return new WaitForSeconds(0.15f);
            rig.velocity = new Vector3(move.x * speed, rig.velocity.y, move.z * speed);
            yield return new WaitForSeconds(0.35f);
            //main_coll.material = null;
            trail.emitting = false;
            moved = true;
            dashed = false;
        }

        //hurt
        if(hurted)
        {
            main_coll.enabled = false;
            yield return new WaitForSeconds(0.3f);
            player_material.SetColor("_EmissionColor", Color.white * 2.5f);
            moved = true;

            yield return new WaitForSeconds(2f);
            hurted = false;
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
        if (collision.gameObject.tag == "obstacle" && !hurted && !gm.win && !gm.died && !gm.isPaused)
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

            moved = false;

            //knockback effect
            Vector3 laser = this.transform.position - collision.transform.position;
            laser.y = 2.5f;
            rig.AddForce(laser.normalized * 3.5f, ForceMode.Impulse);
            hurted = true;
            hurt.Play();
            StartCoroutine(Game());
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //is grounded, reset jumped
        if (isGrounded && collision.gameObject.tag == "ground" && !gm.win && !gm.died && !gm.isPaused)
        {
            if (body.GetCurrentAnimatorStateInfo(0).IsName("onAir"))
                jumped = 0;
        }

        //spike collision
        if (collision.gameObject.tag == "obstacle" && !hurted && !gm.win && !gm.died && !gm.isPaused)
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

            moved = false;

            //knockback effect
            Vector3 spike = this.transform.position - collision.transform.position;
            spike.y = 1.5f;
            rig.AddForce(spike * 3f, ForceMode.Impulse);
            hurted = true;
            hurt.Play();
            StartCoroutine(Game());
        }

        //laser collision
        /*else if (collision.gameObject.tag == "laser" && !hurted && !gm.win && !gm.died && !gm.load)
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
            body.SetBool("hurt", true);

            moved = false;

            //knockback effect
            Vector3 laser = this.transform.position - collision.transform.position;
            laser.y = 2.5f;
            rig.AddForce(laser.normalized * 3.5f, ForceMode.Impulse);
            hurted = true;
            hurt.Play();
            StartCoroutine(game());
        }*/ 
    }
}
