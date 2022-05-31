using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;
public class Level : MonoBehaviour{


    private const float CAMERA_ORTHO_SIZE = 50f;
    private const float PIPE_WIDTH = 7.8f;
    private const float PIPE_HEAD_HEIGHT = 3.75f;
    private const float PIPE_MOVE_SPEED = 30f;
    private const float PIPE_DESTROY_X_POSITION = -100f;
    private const float PIPE_SPAWN_X_POSITION = +100f;
    private const float BIRD_X_POSITION = 0f;


private static Level instance;

public static Level GetInstance(){
    return instance;
}

private List<Pipe> pipeList;
private float pipeSpawnTimer;
private int pipesPassedCount;
private float pipesSpawned;
private float pipeSpawnTimerMax;
private float gapSize;
private State state;


public enum Difficulty{
    Easy,
    Medium,
    Hard,
    Impossible,
}

private enum State {
    Playing,
    BirdDead,
}

private void Awake(){
    instance = this;
    pipeList = new List<Pipe>();
    pipeSpawnTimerMax = 1f;
    SetDifficulty(Difficulty.Easy);
    state = State.Playing;
}


    private void Start() {
     //  CreateGapPipes(50f, 20f, 20f);
     bird2.GetInstance().OnDied += bird2_OnDied;

 }
private void bird2_OnDied(object sender, System.EventArgs e){
  //  CMDebug.TextPopupMouse("död");
  state = State.BirdDead;

  FunctionTimer.Create(()=>{
      UnityEngine.SceneManagement.SceneManager.LoadScene("spelscen");
  }, 1f);
}

 private void Update(){
     if(state == State.Playing){
     HandlePipeMovement();
     HandlePipeSpawning();
     }
 }

private void HandlePipeSpawning(){
    pipeSpawnTimer -= Time.deltaTime;
    if (pipeSpawnTimer < 0){
        //spawna nya pipes
        pipeSpawnTimer += pipeSpawnTimerMax;

        float heightEdgeLimit = 10f;
        float minHeight = gapSize * .5f;
        float totalHeight = CAMERA_ORTHO_SIZE * 2f;
        float maxHeight = totalHeight - gapSize * .5f - heightEdgeLimit;
        float height = Random.Range(minHeight, maxHeight);
       CreateGapPipes(height, gapSize, PIPE_SPAWN_X_POSITION);

    }
}


// för att göra så att pipsen rör sig
private void HandlePipeMovement(){
   for(int i=0; i<pipeList.Count; i++){
       Pipe pipe = pipeList[i];
       bool isToTheRightOfBird = pipe.GetXPosition() > BIRD_X_POSITION;
        pipe.Move();
        if (isToTheRightOfBird && pipe.GetXPosition() <= BIRD_X_POSITION && pipe.IsBottom()){
            pipesPassedCount++;
        }
        if (pipe.GetXPosition() < PIPE_DESTROY_X_POSITION){
            //förstör pipe
            pipe.DestroySelf();
            pipeList.Remove(pipe);
            i--;
        }
    }
}
    private void SetDifficulty(Difficulty difficulty){
        switch(difficulty){
            case Difficulty.Easy:
            gapSize = 50f;
            pipeSpawnTimerMax = 1.2f;
            break;
            case Difficulty.Medium:
            gapSize = 40f;
             pipeSpawnTimerMax = 1.1f;
            break;
              case Difficulty.Hard:
            gapSize = 30f;
             pipeSpawnTimerMax = 1f;
            break;
              case Difficulty.Impossible:
            gapSize = 24f;
             pipeSpawnTimerMax = .9f;
            break;
      
         


        }
    }

    private Difficulty GetDifficulty(){
        if (pipesSpawned >= 30) return Difficulty.Impossible;
         if (pipesSpawned >= 20) return Difficulty.Hard;
          if (pipesSpawned >= 10) return Difficulty.Medium;
          return Difficulty.Easy;
         
    }



private void CreateGapPipes(float gapY, float gapSize, float xPosition){
CreatePipe(gapY - gapSize * .5f, xPosition, true);
CreatePipe(CAMERA_ORTHO_SIZE * 2f - gapY - gapSize * .5f, xPosition, false);
pipesSpawned++;
SetDifficulty(GetDifficulty());
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

        Pipe pipe = new Pipe(pipeHead, pipeBody, createBottom);
        pipeList.Add(pipe);
    

}
    public float GetPipesSpawned(){
        return pipesSpawned;
    }

    public int GetpipesPassedCount(){
        return pipesPassedCount;
    }

/* för en pipe */
private class Pipe {
    private Transform pipeHeadTransform;
    private Transform pipeBodyTransform;
    private bool isBottom;
        public Pipe(Transform pipeHeadTransform, Transform pipeBodyTransform, bool isBottom){
            this.pipeBodyTransform = pipeBodyTransform;
            this.pipeHeadTransform = pipeHeadTransform;
            this.isBottom = isBottom;
        }



public void Move(){
   pipeBodyTransform.position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime;
   pipeHeadTransform.position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime; 
}
    public float GetXPosition(){
        return pipeHeadTransform.position.x;
    }

public bool IsBottom(){
    return isBottom;
}
public void DestroySelf() {
    Destroy( pipeHeadTransform.gameObject);
    Destroy( pipeBodyTransform.gameObject);
}
}

}

