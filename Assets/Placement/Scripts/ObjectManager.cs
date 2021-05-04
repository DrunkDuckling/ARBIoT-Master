using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace arbiot
{
    public class ObjectManager : MonoBehaviour
    {
        [SerializeField] private GameObject _flaskBackendObject; 
        [SerializeField] private GameObject _gameObjectPrefab;
        private List<GameObject> _gameObjects;
        public GameObject[] _arrayGameObjects;

        private IFB _backend;

        // Start is called before the first frame update
        void Start()
        {
            _backend = _flaskBackendObject.GetComponent<IFB>();
            _gameObjects = new List<GameObject>();

            StartCoroutine(UpdatePrefabArray());
            
        }

        // Update is called once per frame
        void Update()
        {
            

        }

        IEnumerator UpdatePrefabArray()
        {
            while(true)
            {
                // suspend execution for 5 seconds
                yield return new WaitForSeconds(5);
                if(_arrayGameObjects == null || _arrayGameObjects.Length < GameObject.FindGameObjectsWithTag("SensorObject").Length)
                    _arrayGameObjects = GameObject.FindGameObjectsWithTag("SensorObject");
                
                foreach(GameObject go in _arrayGameObjects)
                {
                    print(go.name);
                }
            }
            
        }

    }
}
