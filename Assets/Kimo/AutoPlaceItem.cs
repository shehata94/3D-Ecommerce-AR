using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;


[RequireComponent(typeof(ARRaycastManager))]
public class AutoPlaceItem : MonoBehaviour
{

 public Material planeMaterial;
    public GameObject GameObjectToPlace;

    public ItemPlacerConnection ItemPlacedController;

    private ARRaycastManager _arRaycastManager;

    private GameObject spawnedObject;


    static List<ARRaycastHit> s_Hits =new List<ARRaycastHit>();
    public float speed = 3f;
    public bool isPlacing = false;

    public Vector3 LastPlacementPos;


    private void Awake()
    {

        
        _arRaycastManager = GetComponent<ARRaycastManager>();


    }
public void SetPlaneOn(bool isOn){

        Color color = planeMaterial.color;


        if(isOn==true){
            color.a = 0.3f;

        }else{

            color.a = 0;
            LineRenderer[] allLines = GetComponentsInChildren<LineRenderer>();
            for (int i = 0; i < allLines.Length; i++)
            {
                Destroy(allLines[i]);
            }

        }

        planeMaterial.color = color;

    }
  bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }
          touchPosition = default;
        return false;

    }

    public void GameCode(Vector3 newPos){
        if (ItemPlacedController != null)
        {
            if (ItemPlacedController.hasItemBeenPlaced == false)
            {
                isPlacing = true;
                LastPlacementPos = newPos;
                ItemPlacedController.GetGameObjectToPlace().SetActive(true);
                ItemPlacedController.GetGameObjectToPlace().transform.parent = null;
                //GameObjectToPlace.transform.position = newPos;
                ItemPlacedController.GetGameObjectToPlace().transform.position = Vector3.Lerp(ItemPlacedController.GetGameObjectToPlace().transform.position, newPos, Time.deltaTime * speed);
               
               
               ItemPlacedController.GetGameObjectToPlace().transform.LookAt(new Vector3(Camera.main.transform.position.x,newPos.y,Camera.main.transform.position.z) );
                if (!ItemPlacedController.GetGameObjectToPlace().activeSelf)
                {
                    ItemPlacedController.GetGameObjectToPlace().SetActive(true);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if (!TryGetTouchPosition(out Vector2 touchPosition))
        //     return;

         if (ItemPlacedController != null)
        {
      

        if(_arRaycastManager.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f,0)), s_Hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = s_Hits[0].pose;
            GameCode(hitPose.position);
            // if( ItemPlacedController.GetGameObjectToPlace() == null)
            // {
                
            //    // spawnedObject = Instantiate(GameObjectToPlace, hitPose.position,hitPose.rotation);
            //     //ItemPlacedController.GetGameObjectToPlace() = spawnedObject;
            //     //SetPlaneOn(true);

            // }
            // else
            // {
                // isPlacing = true;
                // ItemPlacedController.GetGameObjectToPlace().SetActive(true);
                //  ItemPlacedController.GetGameObjectToPlace().transform.parent = null;
                // ItemPlacedController.GetGameObjectToPlace().transform.position =  Vector3.Lerp( ItemPlacedController.GetGameObjectToPlace().transform.position, hitPose.position, Time.deltaTime * speed);
                //  ItemPlacedController.GetGameObjectToPlace().transform.rotation = hitPose.rotation;
                // SetPlaneOn(false);
                // if (! ItemPlacedController.GetGameObjectToPlace().activeSelf)
                // {
                //      ItemPlacedController.GetGameObjectToPlace().SetActive(true);
                // }
            // }
    
    
            //   ItemPlacedController.GetGameObjectToPlace().transform.rotation = hitPose.rotation;

        
        }


        if(isPlacing == false && ItemPlacedController.hasItemBeenPlaced == false){

            //  HideItem( ItemPlacedController.GetGameObjectToPlace());
            HideItem();

            // spawnedObject.SetActive(false);
            // spawnedObject.transform.parent = Camera.main.transform;
            // spawnedObject.transform.localPosition = Vector3.zero;

        }else{

                CheckTouchType();

            }
       
        isPlacing = false;
     
     }
    }

    public void CheckTouchType(){


        if(EventSystem.current.currentSelectedGameObject !=null)
        {

            return;
        }


            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (_arRaycastManager.Raycast(touch.position, s_Hits, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = s_Hits[0].pose;
                    TapHasOccured();
                }
            }


        


    }


    public void TapHasOccured(){
        if(ItemPlacedController.hasItemBeenPlaced == false){
               ItemPlacedController.hasItemBeenPlaced = true;
              
               ItemPlacedController.GetGameObjectToPlace().transform.position = LastPlacementPos;
               SetPlaneOn(false);
        }
    }

    //  public void SetNewGameObjectToPlace(ItemPlacerConnection ItemPlacedController){
    //     HideItem(this.ItemPlacedController.GetGameObjectToPlace());
    //    // spawnedObject = newItem;
    //    this.ItemPlacedController = ItemPlacedController;

    // }

    // public void HideItem(GameObject itemToHide){

    //     itemToHide.SetActive(false);
    //     itemToHide.transform.parent = Camera.main.transform;
    //     itemToHide.transform.localPosition = Vector3.zero;
    // }

     
      public void SetNewGameObjectToPlace(ItemPlacerConnection ItemPlacedController){

        ShouldWeHideIt();
        //GameObjectToPlace = newItem;
        this.ItemPlacedController = ItemPlacedController;
        SetPlaneOn(true);

    }

    public void ShouldWeHideIt(){
        if (ItemPlacedController != null)
        {
            if (ItemPlacedController.hasItemBeenPlaced == false)
            {
                HideItem();
            }
        }

    }

    public void HideItem(){
        if (ItemPlacedController != null)
        {
            ItemPlacedController.GetGameObjectToPlace().SetActive(false);
            ItemPlacedController.GetGameObjectToPlace().transform.parent = Camera.main.transform;
            ItemPlacedController.GetGameObjectToPlace().transform.localPosition = Vector3.zero;
        }
    }

    public void RemoveItemToPlace(){
        ItemPlacedController = null;
        SetPlaneOn(false);

    }
}
