using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour{


    private const float CAMERA_ORTHO_SIZE = 50f;
    private const float PIPE_WIDTH = 7.8f;
    private const float PIPE_HEAD_HEIGHT = 3.75f;

    private void Start() {
        CreatePipe(40f, 20f, true);
        CreatePipe(40f, 20f, false);    
 }

    private void CreatePipe(float height, float xPosition, bool createBottom){
        
        // lägger till pipehead både uppe och nere
        Transform pipeHead = Instantiate(GameAssets.GetInstance().pfPipeHead);
        float pipeHeadYPosition;
        if (createBottom){
            pipeHeadYPosition = -CAMERA_ORTHO_SIZE + height - PIPE_HEAD_HEIGHT * .5f;
        } else {
         pipeHeadYPosition = +CAMERA_ORTHO_SIZE - height + PIPE_HEAD_HEIGHT * .5f;
        }
        pipeHead.position = new Vector3(xPosition, pipeHeadYPosition);
        
        //lägger till pipebody både uppe och nere
        Transform pipeBody = Instantiate(GameAssets.GetInstance().pfPipeBody);
                float pipeBodyYPosition;
        if (createBottom){
            pipeBodyYPosition = -CAMERA_ORTHO_SIZE;
        } 
        else {
         pipeBodyYPosition = +CAMERA_ORTHO_SIZE;
        pipeBody.localScale = new Vector3(1, -1, -1);
        }
        pipeBody.position = new Vector3(xPosition, pipeBodyYPosition);
        

        SpriteRenderer pipeBodySpriteRenderer = pipeBody.GetComponent<SpriteRenderer>();
        pipeBodySpriteRenderer.size = new Vector2(PIPE_WIDTH, height);
  
        BoxCollider2D pipeBodyBoxCollider = pipeBody.GetComponent<BoxCollider2D>();
        pipeBodyBoxCollider.size = new Vector2(PIPE_WIDTH, height);
        pipeBodyBoxCollider.offset = new Vector2(0f, height * .5f);
    }
}

