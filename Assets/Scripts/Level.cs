using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour{


    private const float CAMERA_ORTHO_SIZE = 50f;
    private const float PIPE_WIDTH = 7.8f;
    private const float PIPE_HEAD_HEIGHT = 3.75f;
    private const float PIPE_MOVE_SPEED = 5f;
    private const float PIPE_DESTROY_X_POSITION = -120f;



private List<Pipe> pipeList;

private void Awake(){
    pipeList = new List<Pipe>();
}


    private void Start() {
       CreateGapPipes(50f, 20f, 20f);
 }

 private void Update(){
     HandlePipeMovement();
 }
// för att göra så att pipsen rör sig
private void HandlePipeMovement(){
   for(int i=0; i<pipeList.Count; i++){
       Pipe pipe = pipeList[i];
        pipe.Move();
        if (pipe.GetXPosition() < PIPE_DESTROY_X_POSITION){
            //förstör pipe
            pipe.DestroySelf();
            pipeList.Remove(pipe);
            i--;
        }
    }
}

private void CreateGapPipes(float gapY, float gapSize, float xPosition){
CreatePipe(gapY - gapSize * .5f, xPosition, true);
CreatePipe(CAMERA_ORTHO_SIZE * 2f - gapY - gapSize * .5f, xPosition, false);
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

        Pipe pipe = new Pipe(pipeHead, pipeBody);
        pipeList.Add(pipe);
    }

/* för en pipe */
private class Pipe {
    private Transform pipeHeadTransform;
    private Transform pipeBodyTransform;

        public Pipe(Transform pipeHeadTransform, Transform pipeBodyTransform){
            this.pipeBodyTransform = pipeBodyTransform;
            this.pipeHeadTransform = pipeHeadTransform;
        }



public void Move(){
   pipeBodyTransform.position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime;
   pipeHeadTransform.position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime; 
}
    public float GetXPosition(){
        return pipeHeadTransform.position.x;
    }

public void DestroySelf() {
    Destroy( pipeHeadTransform.gameObject);
    Destroy( pipeBodyTransform.gameObject);
}
}

}

