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
        public TMP_Dropdown _DropRoom, _DropUuid;
        private string _uuid;
        // Get or Set the uuid value given from dropdownmenu (uuid)
        public string Uuid {get{return _uuid;} set{_uuid = value;}}
        /// <summary>
        /// Setting menu that contains options for different features.
        /// </summary>
        [SerializeField]
        private GameObject _objectMenuUi = null;
        private List<GameObject> _gameObjects;
        public GameObject[] _arrayGameObjects;
        private IFB _backend;
        private Brick _brick;

        // Start is called before the first frame update
        void Start()
        {
            /// <summary>
            /// Fetch the "disable_placement_via_settings"(bool)  from "ARPlacementInteracterableCustom" so we can use it
            /// </summary>
            // Finds the object the script "IGotBools" is attached to and assigns it to the gameobject called g.
            GameObject g = GameObject.FindGameObjectWithTag ("ARplaceObject");
            //assigns the script component "IGotBools" to the public variable of type "IGotBools" names boolBoy.
            arptoggle = g.GetComponent<ARPlacementInteracterableCustom> ();

            // Innitialize backend
            _backend = _flaskBackendObject.GetComponent<IFB>();
            _gameObjects = new List<GameObject>();

            // Make sure that the uuid dropdown is empty at startup
            _DropUuid.options.Clear();
            _DropUuid.interactable = false;

            _button.onClick.AddListener(OnButtonClick);
            StartCoroutine(UpdatePrefabArray());
            
        }

        // Update is called once per frame
        void Update()
        {
            

        }

        private void OnButtonClick()
        {
            _backend.GetRoomData("e22-604-0", SortBrickData);

        }

        public void DropDownMenuRoom(int val)
        {
            _DropUuid.interactable = false;
            _backend.GetRoomData(_DropRoom.options[val].text, FillDDList);
        }
        public void DropDownMenuUuid(int val)
        {
            Uuid = _DropUuid.options[val].text;
            Debug.Log("uuid from dropdownUuid: " + Uuid);
        }
        // Used to fill and 
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
                    items.Add(bs.uuid);
                }
            }
            // Add new options to dropdown
            if(items != null){
                foreach(var item in items)
                {
                    _DropUuid.options.Add(new TMP_Dropdown.OptionData() { text = item } );
                }
            }
            _DropUuid.interactable = true;
        }

        
        private void SortBrickData(string brickData)
        {
            _brick = JsonUtility.FromJson<Brick>(brickData);
            if(_brick != null)
            {
                foreach(BrickData bs in _brick.sensors)
                {
                    Debug.Log(bs.sensortype);
                    Debug.Log(bs.room);
                    Debug.Log(bs.uuid);
                }
            }
            //Debug.Log(_brick.sensors[0]);
        }

        IEnumerator UpdatePrefabArray()
        {
            while(true)
            {
                // suspend execution for 5 seconds
                yield return new WaitForSeconds(10);
                if(_arrayGameObjects == null || _arrayGameObjects.Length < GameObject.FindGameObjectsWithTag("SensorObject").Length)
                    _arrayGameObjects = GameObject.FindGameObjectsWithTag("SensorObject");
                
                foreach(GameObject go in _arrayGameObjects)
                {
                    print(go.name);
                }
            }
            
        }

        /// <summary>
        /// Callback event for object option.
        /// </summary>
        private void OnObjectMenuOpened()
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

            // Allow placing objects again. If other bool is the same value
            //arptoggle.Disable_placement_via_settings(true);

            //_instantPlacementMenuUi.SetActive(false);
            //      _planeDiscoveryGuide.EnablePlaneDiscoveryGuide(true);
        }
    }
}
