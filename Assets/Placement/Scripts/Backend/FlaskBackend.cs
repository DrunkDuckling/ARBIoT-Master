using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace arbiot
{
    public class FlaskBackend : MonoBehaviour, IFB
    {
        //[SerializeField] private string _uri = "http://localhost:5000/";
        [SerializeField] private string _uri = "http://192.168.0.109:80/";

        public void GetAddressesPointData(Action<string> callback)
        {
            Debug.Log("Get room point data");
            StartCoroutine(GetRequest(_uri + "rum/p", callback));
        }

        public void GetAddressesAreaData(Action<string> callback)
        {
            Debug.Log("Get room area data");
            StartCoroutine(GetRequest(_uri + "rum/a", callback));
        }

        public void GetRoomData(string roomId, Action<string> callback)
        {
            StartCoroutine(GetRequest(_uri + "getData/" + roomId, callback));
        }

        public void GetOK(Action<string> callback)
        {
            StartCoroutine(GetRequest(_uri, callback));
        }

        IEnumerator GetRequest(string uri, Action<string> callback)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();

                string[] pages = uri.Split('/');
                int page = pages.Length - 1;

                if (webRequest.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.Log(pages[page] + ": Error: " + webRequest.error);
                    Debug.Log(pages[page] + ": Error: " + webRequest.downloadHandler.text);
                }
                else
                {
                    string data = webRequest.downloadHandler.text;
                    if (data[0].ToString() == "[")
                    {
                        data = data.Substring(1, data.Length - 3);
                    }

                    Debug.Log(data);
                    callback(data);
                }
            }
        }


        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
