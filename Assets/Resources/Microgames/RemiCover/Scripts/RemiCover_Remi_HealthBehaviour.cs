﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemiCover_Remi_HealthBehaviour : MonoBehaviour {

    [SerializeField]
    private float HP = 1;                           // Remilia's Health Points.
    public float burnSpeed;                         // How much will HP decrease when Remilia's collider is exposed to sunlight?

    [SerializeField]
    private int collidersOutside = 0;               // How many colliders are outside of Umbrella's shadow?

    private bool continueUpdate = true;

    private GameObject remiliaSprite = null;
    public ParticleSystem smokeParticles;
    private ParticleSystem smokeInstance;

    // Use this for initialization
    void Start() {
        HP = 1;
        remiliaSprite = transform.Find("RemiSprite").gameObject;
        smokeInstance = (ParticleSystem)Instantiate(smokeParticles, remiliaSprite.transform.position, smokeParticles.transform.rotation);
        var emission = smokeInstance.emission;
        collidersOutside = 0;
    }


    // Update is called once per frame
    void Update() {
        if (continueUpdate)
        {
            updateHP();
            if (HP <= 0) GameOver();
        }

        manageEmission();


    }

    private void manageEmission()
    {
        var emission = smokeInstance.emission;
       
        smokeInstance.transform.position = remiliaSprite.transform.position;
        smokeInstance.startSize = ((1 - HP) * 50) / 25;

        emission.rateOverTime = ((1 - HP) * 1000) / 10;
       
    }


    // Decrease HP value if some colliders are outside of Umbrella's Shadow
    private void updateHP()
    {
        this.HP -= burnSpeed * Time.deltaTime * collidersOutside;
    }


    // Game is over
    private void GameOver()
    {
        continueUpdate = false;
        gameObject.SendMessage("characterHasDied");
        MicrogameController.instance.setVictory(false, true);
        changeSpriteColor(Color.red);                                   // ONLY FOR DEBUGING
    }


    void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "UmbrellaShadow")
        {
            collidersOutside += 1;
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "UmbrellaShadow" && collidersOutside != 0)
        {
            collidersOutside -= 1;
        }
    }



    private void changeSpriteColor(Color color)
    {
        remiliaSprite.GetComponent<SpriteRenderer>().color = color;
    }


}