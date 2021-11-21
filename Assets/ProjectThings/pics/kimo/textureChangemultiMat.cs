
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textureChangemultiMat : MonoBehaviour {

  public GameObject box;
  public Material[] materialss;
 // public MeshRenderer  Rend;

  void Start (){
    //  Rend = GetComponent<MeshRenderer>();
      //Rend.enabled = true;
  }

  public void buttonPressed(){
    
      box.GetComponent<MeshRenderer>().materials = materialss;
    
  }
}