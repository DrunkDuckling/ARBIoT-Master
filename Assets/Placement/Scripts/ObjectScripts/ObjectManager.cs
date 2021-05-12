using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace arbiot
{
    public class ObjectManager : MonoBehaviour
    {
        [SerializeField] private GameObject _flaskBackendObject; 
        [SerializeField] private GameObject _gameObjectPrefab;

        // Setting the placement bool from "ARPlacementInteracterableCustom"
        public ARPlacementInteracterableCustom arptoggle;

        // Testing Button
        [SerializeField] private Button _button;

        // Drop Down menu, that helps innitialize the sensor objects
        public TMP_Dropdown _DropRoom, _DropUuid;
        private string _uuid;
        private string _sensorText;
        // Get or Set the uuid value given from dropdownmenu (uuid)
        public string Uuid {get{return _uuid;} set{_uuid = value;}}
        public string SensorText {get{return _sensorText; } set{ _sensorText = value;}}

        //Get/Set the GameObject that needs to be instanciated
        private GameObject _iniSensorGO;
        public GameObject IniSensorGO { get { return _iniSensorGO; } set { _iniSensorGO = value; } }

        [SerializeField] private Text _txt_sensor_uuid;


        /// <summary>
        /// Setting menu that contains options for different features.
        /// </summary>
        [SerializeField]
        private GameObject _objectMenuUi = null;
        private List<GameObject> _gameObjects;
        public GameObject[] _arrayGameObjects;

        // Used for Backend Data
        private IFB _backend;
        private Brick _brick;
        private LiveData _livedata;

        // Start is called before the first frame update
        void Start()
        {
            /// <summary>
            /// Fetch the "disable_placement_via_settings"(bool)  from "ARPlacementInteracterableCustom" so we can use it
            /// </summary>
            GameObject g = GameObject.FindGameObjectWithTag ("ARplaceObject");
            //assigns the script component "ARplaceObject" to the public variable of type "ARplaceObject" named arptoggle.
            arptoggle = g.GetComponent<ARPlacementInteracterableCustom> ();
            // Disable placement if object setting is started. 
            arptoggle.Disable_placement_via_settings(true);
            
            // Innitialize backend
            _backend = _flaskBackendObject.GetComponent<IFB>();
            _gameObjects = new List<GameObject>();

            // Make sure that the uuid dropdown is empty at startup
            _DropUuid.options.Clear();
            _DropUuid.interactable = false;

            //_button.onClick.AddListener(OnButtonClick);
            StartCoroutine(UpdatePrefabArray());
            
        }

        // Method for the Room drop down menu.
        public void DropDownMenuRoom(int val)
        {
            _DropUuid.interactable = false;
            _backend.GetRoomData(_DropRoom.options[val].text, FillDDList);
        }

        // Method for the uuid drop down menu.
        public void DropDownMenuUuid(int val)
        {
            // currently holds Sensor type atm. Not uuid
            //Uuid = _DropUuid.options[val].text;
            string sensor = _DropUuid.options[val].text;
            if(_brick != null)
            {
                foreach(BrickData bs in _brick.sensors)
                {
                    if(bs.sensortype == sensor)
                    {
                        // Set textfield to sensor uuid and store it in get/set method
                        _txt_sensor_uuid.text = bs.uuid;
                        Uuid = bs.uuid;

                        // Get the gameobject currently being instanciated and give it a title and name (uuid)
                        IniSensorGO.name = bs.uuid;
                        TextMeshPro[] testText = IniSensorGO.GetComponentsInChildren<TextMeshPro>();
                        foreach (TextMeshPro tx in testText)
                        {
                            if (tx.text.Contains("Sensor"))
                            {
                                // Gets the specific textfield that is created when prefab is placed.
                                print("Got sensor text object");
                                tx.text = sensor;
                            }
                        }
                    }
                }
            }
            Debug.Log("uuid from dropdownUuid: " + Uuid);
        }
        // Used to fill dropdown menu and show it
        private void FillDDList(string brickData)
        {
            // Clear Drop down menu
            _DropUuid.options.Clear();
            // Make data into an object
            _brick = JsonUtility.FromJson<Brick>(brickData);
            // Create new list for dropdown
            List<string> items = new List<string>();
            // Place data into new list.
            if(_brick != null)
            {
                foreach(BrickData bs in _brick.sensors)
                {
                    // Add the type of sensor to a list
                    items.Add(bs.sensortype);
                }
                // Add new options to dropdown
                if (items != null)
                {
                    foreach (var item in items)
                    {
                        _DropUuid.options.Add(new TMP_Dropdown.OptionData() { text = item });
                    }
                    _DropUuid.interactable = true;
                }
            }            
        }

        IEnumerator UpdatePrefabArray()
        {
            while(true)
            {
                // suspend execution for 10 seconds
                yield return new WaitForSeconds(10);
                _backend.GetLiveData(GetLiveDatas);

                // Find GameObjects and place them in list to be used. 
                if (_arrayGameObjects == null || _arrayGameObjects.Length < GameObject.FindGameObjectsWithTag("SensorObject").Length)
                {
                    _arrayGameObjects = GameObject.FindGameObjectsWithTag("SensorObject");
                    
                }
                // Go get live data and place it into the instanciated gameobjects. 
                if (_livedata != null && _arrayGameObjects != null)
                {
                    // Itterate the GameObjects found (Prefabs) and give them value
                    foreach (GameObject go in _arrayGameObjects)
                    {
                        print(go.name);
                        foreach (SensorData ld in _livedata.livedata)
                        {
                            if (go.name == ld.uuid)
                            {
                                // Get the textfields from the GameObjects and Give them new values.
                                TextMeshPro[] testText = go.GetComponentsInChildren<TextMeshPro>();
                                foreach (TextMeshPro tx in testText)
                                {
                                    if (tx.text.Contains("uuid"))
                                    {
                                        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(ld.time);
                                        print("Got the Subtitle!");
                                        tx.text = "uuid: " + ld.uuid + "\n" + "Value: " + ld.value + "\n" + "Last updated: " + dateTimeOffset;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        // Method that converts the json string data into objects
        private void GetLiveDatas(string data)
        {
            _livedata = JsonUtility.FromJson<LiveData>(data);
        }


        /// <summary>
        /// Callback event for object option.
        /// </summary>
        public void OnObjectMenuOpened()
        {
            _objectMenuUi.SetActive(true);

            // Stop placing objects when settings are open
            arptoggle.Disable_placement_via_settings(false);
        }

        /// <summary>
        /// Callback event for closing the object menu.
        /// </summary>
        public void OnObjectMenuClosed()
        {
            _objectMenuUi.SetActive(false);

            // Run the method PlaceToggle but with a 2 secounds delay.
            Invoke("PlaceToggle", 2);
        }

        private void PlaceToggle()
        {
            // Allow placing objects again. If other bool is the same value
            arptoggle.Disable_placement_via_settings(true);
        }
    }
}
