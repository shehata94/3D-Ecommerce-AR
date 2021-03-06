using System.Collections.Generic;
using DilmerGames.Managers;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class ARPlacementInteractableSingle : ARBaseGestureInteractable
{
    [SerializeField]
    [Tooltip("A GameObject to place when a raycast from a user touch hits a plane.")]
    private GameObject[] placementPrefab;

    [SerializeField]
    [Tooltip("Callback event executed after object is placed.")]
    private ARObjectPlacedEvent onObjectPlaced;

    private GameObject placementObject;
    
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private static GameObject trackablesObject;

    public int n=0;


    public void Selectone(){

        n=0;
    }
    public void Selecttwo(){

         n=1;
    }

    public GameObject PlacementObject 
    { 
        get 
        {
            if(placementObject == null)
            {
                placementObject = Instantiate(placementPrefab[n], Vector3.zero, Quaternion.identity);
            }
            return placementObject; 
        } 
    }

    protected override bool CanStartManipulationForGesture(TapGesture gesture)
    {
        // Allow for test planes
        if (gesture.TargetObject == null || gesture.TargetObject.layer == 9)
            return true;

        return false;
    }

    public void DestroyPlacement()
    {
        Destroy(placementObject);
        placementObject = null;
    }
    
    protected override void OnEndManipulation(TapGesture gesture)
    {
        if (gesture.WasCancelled)
            return;

        // If gesture is targeting an existing object we are done.
        // Allow for test planes
        if (gesture.TargetObject != null && gesture.TargetObject.layer != 9)
            return;
        
        // app state -> plane detection state set
        if (ApplicationState.Instance.CurrentState != State.PlaneDetection)
        {
            if(placementObject != null) DestroyPlacement();
            return;
        }

        // Raycast against the location the player touched to search for planes.
        if (GestureTransformationUtility.Raycast(gesture.StartPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            var hit = hits[0];
            
            // Use hit pose and camera pose to check if hittest is from the
            // back of the plane, if it is, no need to create the anchor.
            if (Vector3.Dot(Camera.main.transform.position - hit.pose.position, hit.pose.rotation * Vector3.up) < 0)
                return;
            
            if(placementObject == null)
            {
                UIPaneManager.Hide("ExitPlacementButton");
                UIPaneManager.Show("ClosePlacementButton");
                
                placementObject = Instantiate(placementPrefab[n], hit.pose.position, hit.pose.rotation);

                // Create anchor to track reference point and set it as the parent of placementObject.
                // TODO: this should update with a reference point for better tracking.
                var anchorObject = new GameObject("PlacementAnchor");
                anchorObject.transform.position = hit.pose.position;
                anchorObject.transform.rotation = hit.pose.rotation;
                placementObject.transform.parent = anchorObject.transform;

                // Find trackables object in scene and use that as parent
                if (trackablesObject == null)
                    trackablesObject = GameObject.Find("Trackables");
                if (trackablesObject != null)
                    anchorObject.transform.parent = trackablesObject.transform;

                onObjectPlaced?.Invoke(this, placementObject);
            }
        }
    }
}