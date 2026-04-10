using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public bool active = false;
    public ParticleSystem particles;
    public Animator an;

    void Start(){
        particles.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "P1" && active){
            variables.levelNumber+=1;
            GameObject.FindGameObjectWithTag("UI").GetComponent<UISetter>().setScore(300, 1);
            SceneManager.LoadScene("Game"); 
        }
    }

    void Update(){
        if(GameObject.FindGameObjectsWithTag("Enemy").Length == 0 && !active){
            activatePortal();
        }
    }

    void activatePortal(){
        active = true;
        particles.gameObject.SetActive(true);
        particles.Play();
        an.SetBool("active", true);
    }
}
