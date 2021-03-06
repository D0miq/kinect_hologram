﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Ball : MonoBehaviour
{
    [SerializeField]
    private float startForce;

    [SerializeField]
    private AudioClip tableBounce;

    [SerializeField]
    private AudioClip paddleBounce;

    private int tableBounceCounter = 0;
    private GameObject activePlayer;
    private AudioSource audioSource;
    private Rigidbody controller;
    private Collider _collider;
    private Vector3 velocityBeforeImpact;
    private bool started = false;
    private PaddleMovement paddle;

    private void Awake() {
        this.audioSource = GetComponent<AudioSource>();
        this.controller = GetComponent<Rigidbody>();
        this._collider = GetComponent<Collider>();
    }

    private void FixedUpdate() {
        this.velocityBeforeImpact = controller.velocity;

        if(Input.GetKeyDown(KeyCode.S) && !started) {     
            this.started = true;
            this.controller.useGravity = true;      
            this.controller.AddForce(Vector3.back * startForce, ForceMode.Impulse);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Paddle"))
        {
            this.StartCoroutine(StopCollision(collision));
        }
    }

    private void OnCollisionEnter(Collision other) {
        float relativeVelocity = other.relativeVelocity.sqrMagnitude;

        switch(other.gameObject.tag) {
            case "Table":
                this.TableBounce(other, relativeVelocity);
                break;
            case "Paddle":
                this.PaddleBounce(other, relativeVelocity);
                break;
            case "Out":
                if(this.tableBounceCounter == 0) {
                    
                } else if(this.tableBounceCounter == 1) {
                    
                }
                break;
            default: 
                break;
        }
    }

    private void TableBounce(Collision collision, float relativeVelocity) {
        this.tableBounceCounter++;

        this.audioSource.clip = this.tableBounce;
        this.audioSource.volume = relativeVelocity / 10;
        this.audioSource.Play();

        if(this.tableBounceCounter >= 2) {

        }
    }

    private void PaddleBounce(Collision collision, float relativeVelocity) {
        this.activePlayer = collision.gameObject;
        this.tableBounceCounter = 0;

        this.audioSource.clip = this.paddleBounce;
        this.audioSource.volume = relativeVelocity / 10;
        this.audioSource.Play();

        var paddleMovement = collision.gameObject.GetComponent<PaddleMovement>();
        //this.controller.AddForce(paddleMovement.Force - /* Síla míčku */, ForceMode.Impulse);  
    }

    private IEnumerator StopCollision(Collision collision)
    {
        Physics.IgnoreCollision(this._collider, collision.collider, true);
        yield return new WaitForSeconds(0.1f);
        Physics.IgnoreCollision(this._collider, collision.collider, false);
    }
}
