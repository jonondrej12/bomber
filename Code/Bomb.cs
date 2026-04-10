using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;    

public class Bomb : NetworkBehaviour
{
    private float explosionTime = 3;
    public int bombPower = 1;
    public LayerMask ignoreLayers;
    private Collider2D coll;
    private Animator bombAn;
    public GameObject explosionEffect;


    void Start() {
        coll = gameObject.GetComponent<Collider2D>();
    }

   void Update()
    {
        explosionTime -= Time.deltaTime;
        if (explosionTime <= 0)
        {
            Explode();
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag == "P1" || other.gameObject.tag == "P2" || other.gameObject.tag == "P3" || other.gameObject.tag == "P4"){
            coll.isTrigger = false;
        }
    }

    void Explode()
    {
        //vytvori vybuch na miste kde byla bomba
        SpawnExplosion(transform.position);
        //doleva, doprava, nahoru, dolu jsou vystreleny paprsky o prislusne delce podle sily bomby, ktere narazi pouze do zdi
        RaycastHit2D leftRay = Physics2D.Raycast(transform.position, Vector2.left, bombPower * 1.92f, ~ignoreLayers);
        RaycastHit2D rightRay = Physics2D.Raycast(transform.position, Vector2.right, bombPower * 1.92f, ~ignoreLayers);
        RaycastHit2D upRay = Physics2D.Raycast(transform.position, Vector2.up, bombPower * 1.92f, ~ignoreLayers);
        RaycastHit2D downRay = Physics2D.Raycast(transform.position, Vector2.down, bombPower * 1.92f, ~ignoreLayers);  
        //pokud je sila bomby jen 1, tak se zkontroluje ze paprsky do niceho nenarazily a pote se tam objevi vybuch
        if(bombPower == 1){                  
            if(leftRay.collider == null){
                SpawnExplosion(new Vector2(transform.position.x - 1.92f, transform.position.y));
            }
            if(rightRay.collider == null){
                SpawnExplosion(new Vector2(transform.position.x + 1.92f, transform.position.y));        
            }   
            if(upRay.collider == null){
                SpawnExplosion(new Vector2(transform.position.x, transform.position.y + 1.92f));                 
            }   
            if(downRay.collider == null){
                SpawnExplosion(new Vector2(transform.position.x, transform.position.y - 1.92f));
            }
        }
        else{

            //pokud je sila bomby vyssi nez jedna a zaroven levy paprsek kontrolujici zdi zadnou zed nenasel, tak se vlevo objevi vybuchy 
            if(leftRay.collider == null){
                float c = 0;
                for(int i = 0; i < bombPower; i++){
                    SpawnExplosion(new Vector2(transform.position.x - 1.92f - c, transform.position.y));                   
                    c += 1.92f;
                }
            }
            //pokud levy paprsek narazil do zdi, tak se zmeri vzdalenost bomby od zdi a pokud je mezi bombou a zdi alespon jedno policko tak se na kazdem volnem poli az do zdi objevi vybuch
            else{
                float distanceToWall = Vector2.Distance(transform.position, leftRay.collider.gameObject.transform.position);
                if(distanceToWall > 2){
                    int d = (int)(distanceToWall / 1.92f) - 1;
                    int s = 1;
                    while(s <= d){
                        SpawnExplosion(new Vector2(transform.position.x + 1.92f*s, transform.position.y));
                        s += 1;
                    }
                }
            }

            //pravy paprsek a vybuchy vpravo od bomby
            if(rightRay.collider == null){
                float c = 0;
                for(int i = 0; i < bombPower; i++){
                    SpawnExplosion(new Vector2(transform.position.x + 1.92f + c, transform.position.y));
                    c += 1.92f;
                }
            }
            else{
                float distanceToWall = Vector2.Distance(transform.position, rightRay.collider.gameObject.transform.position);
                if(distanceToWall > 2){
                    int d = (int)(distanceToWall / 1.92f) - 1;
                    int s = 1;
                    while(s <= d){
                        SpawnExplosion(new Vector2(transform.position.x + 1.92f*s, transform.position.y));                          
                        s += 1;
                    }
                }
            }

            //horni paprsek a vybuchy nahore od bomby
            if(upRay.collider == null){
                float c = 0;
                for(int i = 0; i < bombPower; i++){
                    SpawnExplosion(new Vector2(transform.position.x, transform.position.y + 1.92f + c));                     
                    c += 1.92f;
                }
            }
            else{
                float distanceToWall = Vector2.Distance(transform.position, upRay.collider.gameObject.transform.position);
                if(distanceToWall > 2){
                    int d = (int)(distanceToWall / 1.92f) - 1;
                    int s = 1;
                    while(s <= d){
                        SpawnExplosion(new Vector2(transform.position.x, transform.position.y + 1.92f*s));                        
                        s += 1;
                    }
                }
            }

            //dolni paprsek a vybuchy dole od bomby
            if(downRay.collider == null){
                float c = 0;
                for(int i = 0; i < bombPower; i++){
                    SpawnExplosion(new Vector2(transform.position.x, transform.position.y + 1.92f - c));                   
                    c += 1.92f;
                }

            }
            else{
                float distanceToWall = Vector2.Distance(transform.position, downRay.collider.gameObject.transform.position);
                if(distanceToWall > 2){
                    int d = (int)(distanceToWall / 1.92f) - 1;
                    int s = 1;
                    while(s <= d){
                        SpawnExplosion(new Vector2(transform.position.x, transform.position.y - 1.92f*s));                      
                        s += 1;
                    }
                }
            }
        }

        //pokud paprsky narazi do zdi ktere jsou znicitelne, tak je znici
        if(downRay.collider != null && downRay.collider.tag == "weakWall"){
            bombAn = downRay.collider.gameObject.GetComponent<Animator>();
            downRay.collider.enabled = false;
            bombAn.SetBool("isDestroyed", true);
        }
        if(rightRay.collider != null && rightRay.collider.tag == "weakWall"){
            bombAn = rightRay.collider.gameObject.GetComponent<Animator>();
            rightRay.collider.enabled = false;
            bombAn.SetBool("isDestroyed", true);
        }
        if(leftRay.collider != null && leftRay.collider.tag == "weakWall"){
            bombAn = leftRay.collider.gameObject.GetComponent<Animator>();
            leftRay.collider.enabled = false;
            bombAn.SetBool("isDestroyed", true);
        }
        if(upRay.collider != null && upRay.collider.tag == "weakWall"){
            bombAn = upRay.collider.gameObject.GetComponent<Animator>();
            upRay.collider.enabled = false;
            bombAn.SetBool("isDestroyed", true);
        }

        //nakonec bomba vybuchne a hracovi ketry ji polozil snizi pocitadlo polozenych bomb
        if(MainMenu.online){
            if (IsServer)
            {
                if(gameObject.tag == "B1"){
                    GameObject.FindGameObjectWithTag("P1").GetComponent<Player>().onlineBombsPlaced.Value -= 1;
                }
                else if(gameObject.tag == "B2"){
                    GameObject.FindGameObjectWithTag("P2").GetComponent<Player>().onlineBombsPlaced.Value -= 1;
                }
                else if (gameObject.tag == "B3")
                {
                    GameObject.FindGameObjectWithTag("P3").GetComponent<Player>().onlineBombsPlaced.Value -= 1;
                }
                else if (gameObject.tag == "B4")
                {
                    GameObject.FindGameObjectWithTag("P4").GetComponent<Player>().onlineBombsPlaced.Value -= 1;
                }
                gameObject.GetComponent<NetworkObject>().Despawn(true);
            }                      
        }
        else if(!MainMenu.online){
            Destroy(gameObject);
            if (gameObject.tag == "B1")
            {
                GameObject.FindGameObjectWithTag("P1").GetComponent<Player>().bombsPlaced -= 1;
            }
            else if (gameObject.tag == "B2")
            {
                GameObject.FindGameObjectWithTag("P2").GetComponent<Player>().bombsPlaced -= 1;
            }
        }
    }

    void SpawnExplosion(Vector2 position){
        GameObject explosion = Instantiate(explosionEffect, position, Quaternion.identity); 
        if(MainMenu.online && IsServer){
            explosion.GetComponent<NetworkObject>().Spawn();
        }
    }


}

