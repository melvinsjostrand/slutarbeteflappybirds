using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class gamehandler : MonoBehaviour {

    private void Start() {
        Debug.Log("gamehandler.Start");
       

       //test popup så att jag vet att codemonkey debug funkar
       //int count = 0;
      // FunctionPeriodic.Create(() => {
           //CMDebug.TextPopupMouse("Ding! " + count);
         //  count++;
    //   }, .300f);
 //   }


//för att skapa ett nytt object (pipes som man ska åka mellan)
    GameObject gameObject = new GameObject("Pipe", typeof(SpriteRenderer));
  gameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.GetInstance().pipeHeadSprite;
 }
}
