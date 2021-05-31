using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

namespace arbiot
{
    public class Backend : MonoBehaviour
    {

        [SerializeField]
        private string _uri = "http://localhost:5000/";

        //[SerializeField] 
        //private string _uri = "http://192.168.0.109:80/";

        private string _newUri;

        [SerializeField]
        private TMP_Dropdown _Dropdown;


        // Start is called before the first frame update
        void Start()
        {
        
        }

        public void SetUriDD(string val)
        {
            _newUri = val;
        }

        public void SetUriDD(int val)
        {
            _newUri = _Dropdown.options[val].text;
        }

        public void ClickOK()
        {
            _uri = _newUri;
        }

        public void ClickTest()
        {
            _uri = _newUri;
            StartCoroutine(TestConnectionSpeed(_uri));
        }

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

        public void GetLiveData(Action<string> callback)
        {
            StartCoroutine(GetRequest(_uri + "go", callback));
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

                    //Debug.Log(data);
                    callback(data);
                }
            }
        }


        public IEnumerator TestConnectionSpeed(string uri)
        {
            List<string> timers = new List<string>();

            for (int i = 0; i < 1000; i++)
            {
                var temp = Time.realtimeSinceStartup;

                using (UnityWebRequest webRequest = UnityWebRequest.Get(uri + "getData/" + "e22-604-0"))
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

                        if (data != null)
                        {
                            //Debug.Log("Got Data");
                        }
                        else Debug.Log("Data Was null:");
                    }
                }

                var timeTaken = (Time.realtimeSinceStartup - temp).ToString("f6");
                timers.Add(timeTaken);

                Debug.Log(i);
                Debug.Log("Times used in secounds: " + timeTaken);
            }



            //Get the path of the Game data folder
            string m_Path = Application.persistentDataPath + "Saved_Inventory.csv";

            StreamWriter writer = new StreamWriter(m_Path);

            writer.WriteLine("Time");

            for (int i = 0; i < timers.Count; i++)
            {
                writer.Write(timers[i]);
                writer.Write(System.Environment.NewLine);
            }

            writer.Flush();
            writer.Close();

            //Output the Game data path to the console.
            Debug.Log("dataPath : " + m_Path);
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }


    }
}
