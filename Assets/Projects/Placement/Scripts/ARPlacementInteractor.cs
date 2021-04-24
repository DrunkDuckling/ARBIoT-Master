using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.Interaction.Toolkit.AR
{
    /// <summary>
    /// <see cref="UnityEvent"/> that is invoked when an object is placed.
    /// </summary>
    [Serializable, Obsolete("ARObjectPlacedEvent has been deprecated. Use ARObjectPlacementEvent instead.")]
    public class ARObjectPlacedEvent : UnityEvent<ARPlacementInteractor, GameObject>
    {
    }

    /// <summary>
    /// <see cref="UnityEvent"/> that is invoked when an object is placed.
    /// </summary>
    [Serializable]
    public class ARObjtPlacementEvent : UnityEvent<ARObjectPlacementEventDataArgs>
    {
    }

    /// <summary>
    /// Event data associated with the event when an object is placed.
    /// </summary>
    public class ARObjectPlacementEventDataArgs
    {
        /// <summary>
        /// The Interactable that placed the object.
        /// </summary>
        public ARPlacementInteractor placementInteractor { get; set; }

        /// <summary>
        /// The object that was placed.
        /// </summary>
        public GameObject placementObject { get; set; }
    }


    /// <summary>
    /// Controls the placement of Andy objects via a tap gesture.
    /// </summary>
    [HelpURL(XRHelpURLConstants.k_ARPlacementInteractable)]
    public class ARPlacementInteractor : ARBaseGestureInteractable
    {
        [SerializeField]
        [Tooltip("A GameObject to place when a raycast from a user touch hits a plane.")]
        GameObject m_PlacementPrefab;


        /// <summary>
        /// A <see cref="GameObject"/> to place when a raycast from a user touch hits a plane.
        /// </summary>
        public GameObject placementPrefab
        {
            get => m_PlacementPrefab;
            set => m_PlacementPrefab = value;
        }

        [SerializeField]
        ARObjtPlacementEvent m_ObjectPlaced = new ARObjtPlacementEvent();

        /// <summary>
        /// Gets or sets the event that is called when this Interactable places a new <see cref="GameObject"/> in the world.
        /// </summary>
        public ARObjtPlacementEvent objectPlaced
        {
            get => m_ObjectPlaced;
            set => m_ObjectPlaced = value;
        }

#pragma warning disable 618
        [SerializeField]
        ARObjtPlacementEvent m_OnObjectPlaced = new ARObjtPlacementEvent();

        /// <summary>
        /// Gets or sets the event that is called when this Interactable places a new <see cref="GameObject"/> in the world.
        /// </summary>
        [Obsolete("onObjectPlaced has been deprecated. Use objectPlaced with updated signature instead.")]
        public ARObjtPlacementEvent onObjectPlaced
        {
            get => m_OnObjectPlaced;
            set => m_OnObjectPlaced = value;
        }
#pragma warning restore 618

        readonly ARObjectPlacementEventDataArgs m_ObjectPlacementEventDataArgs = new ARObjectPlacementEventDataArgs();

        static readonly List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
        static GameObject s_TrackablesObject;

        /// <inheritdoc />
        protected override bool CanStartManipulationForGesture(TapGesture gesture)
        {
            // Allow for test planes
            if (gesture.TargetObject == null || gesture.TargetObject.layer == 9) // TODO Placement gesture layer check should be configurable
                return true;

            return false;
        }

        /// <inheritdoc />
        protected override void OnEndManipulation(TapGesture gesture)
        {
            base.OnEndManipulation(gesture);

            if (gesture.WasCancelled)
                return;

            // If gesture is targeting an existing object we are done.
            // Allow for test planes
            if (gesture.TargetObject != null && gesture.TargetObject.layer != 9) // TODO Placement gesture layer check should be configurable
                return;

            // Raycast against the location the player touched to search for planes.
            if (GestureTransformationUtility.Raycast(gesture.startPosition, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                var hit = s_Hits[0];

                // Use hit pose and camera pose to check if hittest is from the
                // back of the plane, if it is, no need to create the anchor.
                if (Vector3.Dot(Camera.main.transform.position - hit.pose.position,
                        hit.pose.rotation * Vector3.up) < 0)
                    return;

                // Instantiate placement prefab at the hit pose.
                var placementObject = Instantiate(placementPrefab, hit.pose.position, hit.pose.rotation);

                // Create anchor to track reference point and set it as the parent of placementObject.
                // TODO This should update with a reference point for better tracking.
                var anchorObject = new GameObject("PlacementAnchor");
                anchorObject.transform.position = hit.pose.position;
                anchorObject.transform.rotation = hit.pose.rotation;
                placementObject.transform.parent = anchorObject.transform;

                // Find trackables object in scene and use that as parent
                if (s_TrackablesObject == null)
                    s_TrackablesObject = GameObject.Find("Trackables");
                if (s_TrackablesObject != null)
                    anchorObject.transform.parent = s_TrackablesObject.transform;

                m_ObjectPlacementEventDataArgs.placementInteractor = this;
                m_ObjectPlacementEventDataArgs.placementObject = placementObject;
                m_ObjectPlaced?.Invoke(m_ObjectPlacementEventDataArgs);
                m_OnObjectPlaced?.Invoke(m_ObjectPlacementEventDataArgs);
            }
        }
    }
}