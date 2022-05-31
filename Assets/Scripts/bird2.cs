using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;


public class bird2 : MonoBehaviour{
    private const float JUMP_AMOUNT = 70f;

    private static bird2 instance;

    public static bird2 GetInstance(){
        return instance;
    }
    
    public event EventHandler OnDied;

    private Rigidbody2D birdRigidbody2D;

    private void Awake(){
        instance = this;
        birdRigidbody2D = GetComponent<Rigidbody2D>();
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
        Jump();
        }
    }
    private void Jump() {
        birdRigidbody2D.velocity = Vector2.up * JUMP_AMOUNT;
    }
    private void OnTriggerEnter2D(Collider2D collider){
        birdRigidbody2D.bodyType = RigidbodyType2D.Static;
        if (OnDied != null) OnDied(this , EventArgs.Empty);
    }
}
