using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit.AR;


namespace arbiot
{
    /// <summary>
    /// <see cref="UnityEvent"/> that is invoked when an object is placed.
    /// </summary>
    [Serializable]
    public class ARObjectPlacementEvent : UnityEvent<ARObjectPlacementEventArgs>
    {
    }

    /// <summary>
    /// Event data associated with the event when an object is placed.
    /// </summary>
    public class ARObjectPlacementEventArgs
    {
        /// <summary>
        /// The Interactable that placed the object.
        /// </summary>
        public ARPlacementInteractable placementInteractable { get; set; }

        /// <summary>
        /// The object that was placed.
        /// </summary>
        public GameObject placementObject { get; set; }
    }

    public class ARPlacementInteracterableCustom : ARBaseGestureInteractable
    {
        [SerializeField]
        [Tooltip("A GameObject to place when a raycast from a user touch hits a plane.")]
        private GameObject placementPrefab;

        [SerializeField]
        [Tooltip("Callback event executed after object is placed.")]
        private ARObjectPlacementEventCustom m_ObjectPlaced = new ARObjectPlacementEventCustom();

        // Used to determine if we want to place an object on planes. 
        private bool toggle_Placement;
        private bool disable_placement_via_settings;

        /// <summary>
        /// Gets or sets the event that is called when this Interactable places a new <see cref="GameObject"/> in the world.
        /// </summary>
        public ARObjectPlacementEventCustom objectPlaced
        {
            get => m_ObjectPlaced;
            set => m_ObjectPlaced = value;
        }

        [SerializeField]
        [Tooltip("The LayerMask that is used during an additional raycast when a user touch does not hit any AR trackable planes.")]
        LayerMask m_FallbackLayerMask;

        /// <summary>
        /// The <see cref="LayerMask"/> that is used during an additional raycast
        /// when a user touch does not hit any AR trackable planes.
        /// </summary>
        public LayerMask fallbackLayerMask
        {
            get => m_FallbackLayerMask;
            set => m_FallbackLayerMask = value;
        }

        private GameObject placementObject;

        private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

        private static GameObject trackablesObject;

        /// <summary>
        /// Gets the pose for the object to be placed from a raycast hit triggered by a <see cref="TapGesture"/>.
        /// </summary>
        /// <param name="gesture">The tap gesture that triggers the raycast.</param>
        /// <param name="pose">When this method returns, contains the pose of the placement object based on the raycast hit.</param>
        /// <returns>Returns <see langword="true"/> if there is a valid raycast hit that hit the front of a plane.
        /// Otherwise, returns <see langword="false"/>.</returns>
        protected virtual bool TryGetPlacementPose(TapGesture gesture, out Pose pose)
        {
            // Raycast against the location the player touched to search for planes.
            if (GestureTransformationUtility.Raycast(gesture.startPosition, hits, arSessionOrigin, TrackableType.PlaneWithinPolygon, m_FallbackLayerMask))
            {
                pose = hits[0].pose;

                // Use hit pose and camera pose to check if hit test is from the
                // back of the plane, if it is, no need to create the anchor.
                // ReSharper disable once LocalVariableHidesMember -- hide deprecated camera property
                var camera = arSessionOrigin != null ? arSessionOrigin.camera : Camera.main;
                if (camera == null)
                    return false;

                return Vector3.Dot(camera.transform.position - pose.position, pose.rotation * Vector3.up) >= 0f;
            }

            pose = default;
            return false;
        }

        /// <summary>
        /// Instantiates the placement object and positions it at the desired pose.
        /// </summary>
        /// <param name="pose">The pose at which the placement object will be instantiated.</param>
        /// <returns>Returns the instantiated placement object at the input pose.</returns>
        /// <seealso cref="placementPrefab"/>
        protected virtual GameObject PlaceObject(Pose pose)
        {
            // Instantiate at plane position click on screen and current pose rotation.
            // This is where the object is created and placed ingame. 
            var placementObject = Instantiate(placementPrefab, pose.position, pose.rotation);

            // Create anchor to track reference point and set it as the parent of placementObject.
            var anchor = new GameObject("PlacementAnchor").transform;
            anchor.position = pose.position;
            anchor.rotation = pose.rotation;
            placementObject.transform.parent = anchor;
            
            // Just for testing, so i know that they are different. 
            placementObject.name = "" + Time.time;

            // Use Trackables object in scene to use as parent
            if (arSessionOrigin != null && arSessionOrigin.trackablesParent != null)
                anchor.parent = arSessionOrigin.trackablesParent;

            //print("4: PlaceObject");
            return placementObject;
        }

        /// <summary>
        /// This method is called after an object has been placed.
        /// </summary>
        /// <param name="args">Event data containing a reference to the instantiated placement object.</param>
        protected virtual void OnObjectPlaced(ARPlacementInteracterableCustom arp, GameObject gameObject)
        {
            //print("3: OnObjectPlaced");
            objectPlaced?.Invoke(arp, gameObject);
        }

        protected override bool CanStartManipulationForGesture(TapGesture gesture)
        {
            //print("2: CanStartManipulationForGesture");
            return gesture.targetObject == null;
        }

        protected override void OnEndManipulation(TapGesture gesture)
        {
            base.OnEndManipulation(gesture);
            //print("1: OnEndManipulation");

            if (gesture.isCanceled)
                return;

            if (arSessionOrigin == null)
                return;
            // If we do not want to place an object == toggle if off.
            if(!toggle_Placement || !disable_placement_via_settings)
                return;
                
            if(TryGetPlacementPose(gesture, out var pose))
            {
                var placementObject = PlaceObject(pose);

                OnObjectPlaced(this, placementObject);
            }
        }

        public void Toggle_Object_Placement(bool newValue)
        {
            toggle_Placement = newValue;
        }

        public void Disable_placement_via_settings(bool newValue)
        {
            Debug.Log("Disable_placement_via_settings: " + newValue);
            disable_placement_via_settings = newValue;
        }
    }
}
    
