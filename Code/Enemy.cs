using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Vector3 movingDirection = Vector3.zero;
    Vector3 lastDirection = Vector3.zero;
    bool directionSetted = false;
    public LayerMask ignoreLayers;
    public Animator an;
    private float destroyTime = 1.2f;
    private bool alive = true;
    public float movementSpeed;


    void Start()
    {
        PickDirection();
    }

    void FixedUpdate()
    {
        if (directionSetted && alive)
        {
            Move();
        }
        else if(alive){
            PickDirection();
        }
    }

    void Update(){
        if(!alive){
            destroyTime -= Time.deltaTime;
            if(destroyTime <= 0){
                Destroy(gameObject);
                if(movementSpeed == 4.2f){
                    GameObject.FindGameObjectWithTag("UI").GetComponent<UISetter>().setScore(500, 1);
                }
                else{
                    GameObject.FindGameObjectWithTag("UI").GetComponent<UISetter>().setScore(100, 1);
                }
            }
        }
    }

    void PickDirection()
    {
        RaycastHit2D downRay = Physics2D.Raycast(transform.position, Vector2.down, 1.92f, ~ignoreLayers);
        RaycastHit2D upRay = Physics2D.Raycast(transform.position, Vector2.up, 1.92f, ~ignoreLayers);
        RaycastHit2D leftRay = Physics2D.Raycast(transform.position, Vector2.left, 1.92f, ~ignoreLayers);
        RaycastHit2D rightRay = Physics2D.Raycast(transform.position, Vector2.right, 1.92f, ~ignoreLayers);
        //dokud není nastaven směr tak se bude vybýrat náhodný který není stejný jako ten kterým se hýbal doteď            // náhodné číslo od 1 do 4, které určí směr jakým půjde
            int randomDirection = Random.Range(1, 5);
            // direction 1 = doprava
            if (randomDirection == 1 && lastDirection != Vector3.right && rightRay.collider == null)
            {
                movingDirection = Vector3.right;
                directionSetted = true;
            }
            // direction 2 = doleva
            else if (randomDirection == 2 && lastDirection != Vector3.left && leftRay.collider == null)
            {
                movingDirection = Vector3.left;
                directionSetted = true;
            }
            // direction 3 = nahoru
            else if (randomDirection == 3 && lastDirection != Vector3.up && upRay.collider == null)
            {
                movingDirection = Vector3.up;
                directionSetted = true;
            }
            // direction 4 = dolu
            else if (randomDirection == 4 && lastDirection != Vector3.down && downRay.collider == null)
            {
                movingDirection = Vector3.down;
                directionSetted = true;
            }
    }

    void Move()
    {
        transform.position += movingDirection * Time.deltaTime * movementSpeed;
        RaycastHit2D checkingObsatcles = Physics2D.Raycast(transform.position, movingDirection, 0.92f, ~ignoreLayers);
        if (checkingObsatcles.collider != null && checkingObsatcles.collider.tag != "B1")
        {
            lastDirection = movingDirection;
            directionSetted = false;
        }
        else if(checkingObsatcles.collider != null && checkingObsatcles.collider.tag == "B1"){
            movingDirection = -movingDirection;
        }
    }

    public void Die(){
        an.SetBool("alive", false);
        alive = false;
        gameObject.GetComponent<Collider2D>().enabled = false;      
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "P1"){
            other.gameObject.GetComponent<Player>().Die();
        }
    }
}
