using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class TileDisplayController : MonoBehaviour {

	public Camera firstPersonCamera;
	private Anchor anchor;
	private DetectedPlane detectedPlane;
	private float yOffset;
//	private int score;
	private DetectedPlane selectedPlane;

    /// <summary>
    /// A model to place when a raycast from a user touch hits a plane.
    /// </summary>
    public GameObject TilePrefab;

    /// <summary>
    /// The rotation in degrees need to apply to model when the Andy model is placed.
    /// </summary>
    private const float k_ModelRotation = 180.0f;

	// Use this for initialization
	void Start () {
		foreach (Renderer r in GetComponentsInChildren<Renderer>())
	    {
	        r.enabled = false;
	    }
	}
	
	// Update is called once per frame
	void Update () {

		if (Session.Status != SessionStatus.Tracking)
		{
		    return;
		}	

		// If there is no plane, then return
		if (detectedPlane == null)
		{
		    return;
		}

		// Check for the plane being subsumed.
		// If the plane has been subsumed switch attachment to the subsuming plane.
		while (detectedPlane.SubsumedBy != null)
		{
		    detectedPlane = detectedPlane.SubsumedBy;
		}
	}

	public void SetSelectedPlane (TrackableHit hit)
	{
	    Debug.Log ("Selected plane centered at " + selectedPlane.CenterPose.position);
	    Debug.Log("With size of x " + selectedPlane.ExtentX + " and z of " + selectedPlane.ExtentZ);
//		showSearchingUI = false;

		this.selectedPlane = hit.Trackable as DetectedPlane;
		CreateAnchor(hit);
	}

	void CreateAnchor (TrackableHit hit)
	{
		var tileObject = Instantiate(TilePrefab, hit.Pose.position, hit.Pose.rotation);
//
//		// Compensate for the hitPose rotation facing away from the raycast (i.e. camera).
		tileObject.transform.Rotate(0, k_ModelRotation, 0, Space.Self);
//
//        //Disable plane detect UI
//
//		showSearchingUI = false;
////		if (DetectedPlanePrefab) DetectedPlanePrefab.SetActive(false);
//
////                    // Create an anchor to allow ARCore to track the hitpoint as understanding of the physical
////                    // world evolves.
		var anchor = hit.Trackable.CreateAnchor(hit.Pose);
		tileObject.transform.parent = anchor.transform;
////
////
////                    // Make Andy model a child of the anchor.
//        andyObject.transform.parent = anchor.transform;
	}
}
