using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;


[RequireComponent(typeof(ARRaycastManager))]
public class PlaceObjectsOnPlane : MonoBehaviour
{

    /// <Addition>
    public GameObject GameObjectToPlace;

    public ItemPlacerConnection ItemPlacedController;
    public float speed = 3f;
    public bool isPlacing = false;

    public Vector3 LastPlacementPos;

    /// </Addition>







    [SerializeField]
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]
    GameObject m_PlacedPrefab;

    /// <summary>
    /// The prefab to instantiate on touch.
    /// </summary>
    public GameObject placedPrefab
    {
        get { return m_PlacedPrefab; }
        set { m_PlacedPrefab = value; }
    }

    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    public GameObject spawnedObject { get; private set; }


    /// <summary>
    /// Invoked whenever an object is placed in on a plane.
    /// </summary>
    public static event Action onPlacedObject;

    ARRaycastManager m_RaycastManager;

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    
    [SerializeField]
    int m_MaxNumberOfObjectsToPlace = 1;

    int m_NumberOfPlacedObjects = 0;

    [SerializeField]
    bool m_CanReposition = true;

    public bool canReposition
    {
        get => m_CanReposition;
        set => m_CanReposition = value;
    }


    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
    }

    public void GameCode(Vector3 newPos)
    {
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
               

               // ItemPlacedController.GetGameObjectToPlace().transform.LookAt(new Vector3(Camera.main.transform.position.x, newPos.y, Camera.main.transform.position.z));
                if (!ItemPlacedController.GetGameObjectToPlace().activeSelf)
                {
                    ItemPlacedController.GetGameObjectToPlace().SetActive(true);
                }
            }
        }
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {


                if (ItemPlacedController != null)
                {
                    if (m_RaycastManager.Raycast(touch.position, s_Hits, TrackableType.PlaneWithinPolygon))
                    {
                        Pose hitPose = s_Hits[0].pose;
                        GameCode(hitPose.position);


                        //    if (m_NumberOfPlacedObjects < m_MaxNumberOfObjectsToPlace)
                        //{
                        //    spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation);

                        //    m_NumberOfPlacedObjects++;
                        //}
                        //else
                        //{
                         //  if (m_CanReposition)
                         //   {
                         //       spawnedObject.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);
                         //   }
                        //}
                     

                        if (onPlacedObject != null)
                        {
                            onPlacedObject();
                        }
                    }


                    if (isPlacing == false && ItemPlacedController.hasItemBeenPlaced == false)
                    {

                        //  HideItem( ItemPlacedController.GetGameObjectToPlace());
                        HideItem();

                        // spawnedObject.SetActive(false);
                        // spawnedObject.transform.parent = Camera.main.transform;
                        // spawnedObject.transform.localPosition = Vector3.zero;

                    }
                    else
                    {

                        CheckTouchType();

                    }

                    isPlacing = false;

                }
            }
        }
    }
    public void CheckTouchType()
    {


        if (EventSystem.current.currentSelectedGameObject != null)
        {

            return;
        }


        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (m_RaycastManager.Raycast(touch.position, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = s_Hits[0].pose;
                TapHasOccured();
            }
        }





    }


    public void TapHasOccured()
    {
        if (ItemPlacedController.hasItemBeenPlaced == false)
        {
            ItemPlacedController.hasItemBeenPlaced = true;

            ItemPlacedController.GetGameObjectToPlace().transform.position = LastPlacementPos;
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


    public void SetNewGameObjectToPlace(ItemPlacerConnection ItemPlacedController)
    {

        ShouldWeHideIt();
        //GameObjectToPlace = newItem;
        this.ItemPlacedController = ItemPlacedController;

    }

    public void ShouldWeHideIt()
    {
        if (ItemPlacedController != null)
        {
            if (ItemPlacedController.hasItemBeenPlaced == false)
            {
                HideItem();
            }
        }

    }

    public void HideItem()
    {
        if (ItemPlacedController != null)
        {
            ItemPlacedController.GetGameObjectToPlace().SetActive(false);
            ItemPlacedController.GetGameObjectToPlace().transform.parent = Camera.main.transform;
            ItemPlacedController.GetGameObjectToPlace().transform.localPosition = Vector3.zero;
        }
    }

    public void RemoveItemToPlace()
    {
        ItemPlacedController = null;

    }

}
