using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scorewindow : MonoBehaviour{



private Text scoretext;

private void Awake() {
    scoretext = transform.Find("scoretext").GetComponent<Text>();
    }

private void Update(){
    scoretext.text = Level.GetInstance().GetpipesPassedCount().ToString();
}


}
