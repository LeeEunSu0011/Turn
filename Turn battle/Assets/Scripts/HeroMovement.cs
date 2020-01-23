using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMovement : MonoBehaviour
{
    float moveSpeed = 10f;

    Vector3 curPos, lastPos;

    public Animator anim;

    void Start()
    {
        if(GameManager.instance.nextSpawnPoint != "")
        {
            GameObject spawnPoint = GameObject.Find(GameManager.instance.nextSpawnPoint);
            transform.position = spawnPoint.transform.position;

            GameManager.instance.nextSpawnPoint = "";
        }
        else if(GameManager.instance.lastHeroPosition != Vector3.zero)
        {
            transform.position = GameManager.instance.lastHeroPosition;
            GameManager.instance.lastHeroPosition = Vector3.zero;
        }
    }

    void FixedUpdate()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveX, 0, moveZ);
        GetComponent<Rigidbody>().velocity = movement * moveSpeed;

        curPos = transform.position;
        if(curPos == lastPos)
        {
            GameManager.instance.isWalking = false;
        }
        else
        {
            GameManager.instance.isWalking = true;
        }

        lastPos = curPos;

        //HeroAnim
        anim.SetBool("isWalking", GameManager.instance.isWalking);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "teleporter")
        {
            CollisionHandler col = other.gameObject.GetComponent<CollisionHandler>();
            GameManager.instance.nextSpawnPoint = col.spawnPointName;
            GameManager.instance.sceneToLoad = col.sceneToLoad;
            GameManager.instance.LoadNextScene();
        }
            
        if(other.tag == "EncounterZone")
        {
            RegionData region = other.GetComponent<RegionData>();
            GameManager.instance.curRegions = region;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(other.tag == "EncounterZone")
        {
            GameManager.instance.canGetEncounter = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "EncounterZone")
        {
            GameManager.instance.canGetEncounter = false;
        }
    }
}
