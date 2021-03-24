using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System;
using System.Text;
using Unity.Collections.LowLevel.Unsafe;

/// <summary>
/// This component tests for depth functionality and enables/disables
/// a text message on the screen reporting that depth is not suppoted.
/// </summary>
public class DepthMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    [SerializeField]
    [Tooltip("The AROcclusionManager which will manage depth functionality.")]
    AROcclusionManager m_OcclusionManager;

    /// <summary>
    /// Get or set the <c>AROcclusionManager</c>.
    /// </summary>
    public AROcclusionManager occlusionManager
    {
        get { return m_OcclusionManager; }
        set { m_OcclusionManager = value; }
    }

    [SerializeField]
    Text m_DepthAvailabilityInfo;

    /// <summary>
    /// The UI Text used to display information about the availability of depth functionality.
    /// </summary>
    public Text depthAvailabilityInfo
    {
        get { return m_DepthAvailabilityInfo; }
        set { m_DepthAvailabilityInfo = value; }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Assert(m_OcclusionManager != null, "no occlusion manager");
        Debug.Assert(m_DepthAvailabilityInfo != null, "no text box");
        m_DepthAvailabilityInfo.enabled = ((m_OcclusionManager.descriptor?.supportsHumanSegmentationStencilImage == false)
                                           && (m_OcclusionManager.descriptor?.supportsHumanSegmentationDepthImage == false)
                                           && (m_OcclusionManager.descriptor?.supportsEnvironmentDepthImage == false));
    }
}
