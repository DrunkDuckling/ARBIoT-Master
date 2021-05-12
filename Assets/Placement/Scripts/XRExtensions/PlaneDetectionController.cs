using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace arbiot
{
    /// <summary>
    /// This script is used to toggle plane detection,
    /// and also hide or show the existing planes.
    /// </summary>
    [RequireComponent(typeof(ARPlaneManager))]
    public class PlaneDetectionController : MonoBehaviour
    {
        ARPlaneManager m_ARPlaneManager;

        void Awake()
        {
            m_ARPlaneManager = GetComponent<ARPlaneManager>();
        }

        /// <summary>
        /// Toggles plane detection and the visualization of the planes.
        /// </summary>
        public void TogglePlaneDetection(bool newValue)
        {
            if (newValue)
            {
                m_ARPlaneManager.enabled = true;
                SetAllPlanesActive(true);
            }
            else
            {
                m_ARPlaneManager.enabled = false;
                SetAllPlanesActive(false);
            }
        }

        /// <summary>
        /// Iterates over all the existing planes and activates
        /// or deactivates their <c>GameObject</c>s'.
        /// </summary>
        /// <param name="value">Each planes' GameObject is SetActive with this value.</param>
        void SetAllPlanesActive(bool value)
        {
            foreach (var plane in m_ARPlaneManager.trackables)
                plane.gameObject.SetActive(value);
        }
    }
}
