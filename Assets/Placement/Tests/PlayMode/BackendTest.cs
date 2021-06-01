using System.Collections;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.TestTools;

public class BackendTest
{
    //string _uri = "http://192.168.0.109:80/";
    string uri = "http://d283d95f7739.eu.ngrok.io/";
    //string go = "go";
    string getData = "getData/";
    string roomnumber = "e22-604-0";
    // A Test behaves as an ordinary method
    [Test]
    public void BackendTestSimplePasses()
    {
        // Use the Assert class to test conditions
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    [Timeout(100000000)]
    public IEnumerator TimeTakenForBrickCall()
    {
        List<string> timers = new List<string>();

        for(int i = 0; i < 1000; i++)
        {
            var temp = Time.realtimeSinceStartup;

            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri + getData + roomnumber))
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
                    
                    //Debug.Log(data);
                    //callback(data);
                }
            }

            var timeTaken = (Time.realtimeSinceStartup - temp).ToString("f6");
            timers.Add(timeTaken);

            Debug.Log(i);
            Debug.Log("Times used in secounds: " + timeTaken);
        }



        //Get the path of the Game data folder
        string m_Path = getPath();

        StreamWriter writer = new StreamWriter(m_Path);

        writer.WriteLine("Time");

        for(int i = 0; i < timers.Count; i++)
        {
            writer.Write(timers[i]);
            writer.Write(System.Environment.NewLine);
        }

        writer.Flush();
        writer.Close();

        //Output the Game data path to the console
        Debug.Log("dataPath : " + m_Path);
    }

    private string getPath()
    {
#if UNITY_EDITOR
        return Application.dataPath + "/TestData/" + "Saved_Times_Unity_Desktop_VM.csv";
#elif UNITY_ANDROID
            return Application.persistentDataPath+"Saved_Times_Android.csv";
#elif UNITY_IPHONE
            return Application.persistentDataPath+"/"+"Saved_Times_IOS.csv";
#else
            return Application.dataPath +"/"+"Saved_Times.csv";
#endif
    }

}
