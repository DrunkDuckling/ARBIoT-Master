
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Setting menu for Hello AR Sample, including settings for different features.
/// </summary>
public class SettingsMenu : MonoBehaviour
{

    [Header("Common Settings")]

    /// <summary>
    /// Scene object that contains all setting menu UI elements.
    /// </summary>
    [SerializeField]
    private GameObject _menuWindow = null;

    /// <summary>
    /// Setting menu that contains options for different features.
    /// </summary>
    [SerializeField]
    private GameObject _settingMenuUi = null;

    /// <summary>
    /// Setting button for opening menu windows.
    /// </summary>
    [SerializeField]
    private Button _settingButton = null;

    [Header("Depth Settings")]

    /// <summary>
    /// Depth menu that contains options for depth features.
    /// </summary>
    [SerializeField]
    private GameObject _depthMenuUi = null;

    /// <summary>
    /// Toggle button for the depth settings. Hides depth button if off
    /// </summary>
    [SerializeField]
    private Toggle _toogleDepth = null;

    /// <summary>
    /// Depth button for opening depth menue
    /// </summary>
    [SerializeField]
    private Button _depthButton = null;

    /// <summary>
    /// Depth Menue used for controlling depth settings
    /// </summary>
    [SerializeField]
    private DepthMenu _depthMenu = null;

    [Header("Instant Placement Settings")]

    /// <summary>
    /// Instant Placement menu for configuring instant placement options.
    /// </summary>
    [SerializeField]
    private GameObject _instantPlacementMenuUi = null;

    /// <summary>
    /// The button to open instant placement menu.
    /// </summary>
    [SerializeField]
    private Button _instantPlacementButton = null;


    /// <summary>
    /// Unity's Start() method. is called before the first frame update
    /// </summary>
    void Start()
    {
        /// <summary>
        /// Setting the menue options. 
        /// </summary>
        _menuWindow.SetActive(false);
        _settingMenuUi.SetActive(false);
        _settingButton.onClick.AddListener(OnMenuButtonClick);

        /// <summary>
        /// Setting the Depth menue options.
        /// </summary>
        _depthButton.gameObject.SetActive(false);
        _depthButton.onClick.AddListener(OnClickDepthMenu);

       // _instantPlacementMenuUi.SetActive(false);
       // _instantPlacementButton.onClick.AddListener(OnClickInstantPlacementMenu);
    }

    /// <summary>
    /// Unity's OnDestroy() method.
    /// </summary>
    public void OnDestroy()
    {
        _settingButton.onClick.RemoveListener(OnMenuButtonClick);
        _depthButton.onClick.RemoveListener(OnClickDepthMenu);
        // _instantPlacementButton.onClick.RemoveListener(OnClickInstantPlacementMenu);
    }

    /// <summary>
    /// Callback event for closing the setting menu.
    /// </summary>
    public void OnMenuClosed()
    {
        _menuWindow.SetActive(false);
        _settingMenuUi.SetActive(false);
        _depthMenuUi.SetActive(false);
        //_instantPlacementMenuUi.SetActive(false);
        //      _planeDiscoveryGuide.EnablePlaneDiscoveryGuide(true);
    }

    /// <summary>
    /// Callback event for option button.
    /// </summary>
    private void OnMenuButtonClick()
    {
        print("Click Menu");
        _menuWindow.SetActive(true);
        _settingMenuUi.SetActive(true);
//#if ARCORE_FEATURE_INSTANT_PLACEMENT // Both Dpeth and IP are enabled.
//        _settingMenuUi.SetActive(true);
//#else // Only Depth is enabled.
//        _depthMenuUi.SetActive(true);
        //_depthMenu.OnMenuButtonClicked();
 //   #endif
            //_planeDiscoveryGuide.EnablePlaneDiscoveryGuide(false);
    }

    /// <summary>
    /// Callback event for opening instant placement menu.
    /// </summary>
    private void OnClickInstantPlacementMenu()
    {
        _settingMenuUi.SetActive(false);
       // _instantPlacementMenuUi.SetActive(true);
    }

    /// <summary>
    /// Callback event for opening the depth menu
    /// </summary>
    private void OnClickDepthMenu()
    {
        _settingMenuUi.SetActive(false);
        _depthMenuUi.SetActive(true);
    }

    /// <summary>
    /// Method used for the depth toggle button 
    /// </summary>
    public void Toggle_Depth(bool newValue)
    {
        _depthButton.gameObject.SetActive(newValue);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
