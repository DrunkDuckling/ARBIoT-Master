
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DepthMenu;
using arbiot;

/// <summary>
/// Setting menu for Hello AR Sample, including settings for different features.
/// </summary>
public class SettingsMenu : MonoBehaviour
{

    // Setting the placement bool from "ARPlacementInteracterableCustom"
    public ARPlacementInteracterableCustom arptoggle;

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
    /// Depth button for opening depth menue
    /// </summary>
    [SerializeField]
    private Button _depthButton = null;

    /// <summary>
    /// Depth Menue used for controlling depth settings
    /// </summary>
    [SerializeField]
    private DepthMenu.DepthMenu _depthMenu;

    //[Header("Instant Placement Settings")]

    /// <summary>
    /// Instant Placement menu for configuring instant placement options.
    /// </summary>
    //[SerializeField]
    //private GameObject _instantPlacementMenuUi = null;

    /// <summary>
    /// The button to open instant placement menu.
    /// </summary>
    //[SerializeField]
    //private Button _instantPlacementButton = null;


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
        _depthButton.gameObject.SetActive(true);
        _depthButton.onClick.AddListener(OnClickDepthMenu);


        /// <summary>
        /// Fetch the "disable_placement_via_settings"(bool)  from "ARPlacementInteracterableCustom" so we can use it
        /// </summary>
        // Finds the object the script "IGotBools" is attached to and assigns it to the gameobject called g.
        GameObject g = GameObject.FindGameObjectWithTag ("ARplaceObject");
        //assigns the script component "IGotBools" to the public variable of type "IGotBools" names boolBoy.
        arptoggle = g.GetComponent<ARPlacementInteracterableCustom> ();

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

        // Allow placing objects again. If other bool is the same value
        arptoggle.Disable_placement_via_settings(true);

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

        // Stop placing objects when settings are open
        arptoggle.Disable_placement_via_settings(false);
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

    // Update is called once per frame
    void Update()
    {

    }
}
