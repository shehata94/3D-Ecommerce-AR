
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textureChange : MonoBehaviour {

  public GameObject[] boxs;
  public Material[] materialss;
 // public MeshRenderer  Rend;

  void Start (){
    //  Rend = GetComponent<MeshRenderer>();
      //Rend.enabled = true;
  }

  public void buttonPressed(int k =0){
    if(k==0){
    foreach(GameObject box in boxs){
      box.GetComponent<MeshRenderer>().material = materialss[k];
    }
    }
    if(k==1){
    foreach(GameObject box in boxs){
      box.GetComponent<MeshRenderer>().material = materialss[k];
    }
    }
    if(k==2){
    foreach(GameObject box in boxs){
      box.GetComponent<MeshRenderer>().material = materialss[k];
    }
    }
    if(k==3){
    foreach(GameObject box in boxs){
      box.GetComponent<MeshRenderer>().material = materialss[k];
    }
    }
    if(k==4){
    foreach(GameObject box in boxs){
      box.GetComponent<MeshRenderer>().material = materialss[k];
    }
    }
    
  }
}