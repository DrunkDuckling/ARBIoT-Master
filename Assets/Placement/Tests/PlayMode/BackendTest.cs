using System.Collections;
using System.Collections.Generic;
using System.IO;
using arbiot;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.TestTools;

public class BackendTest
{
    string _uri = "http://192.168.0.109:80/";
    string uri = "http://localhost:5000/";
    string go = "go";
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
    public IEnumerator BackendTestWithEnumeratorPasses()
    {
        var gameObject = new GameObject();
        var backend = gameObject.AddComponent<FlaskBackend>();
        string test;

        for(int i = 0; i < 10; i++)
        {
            backend.GetRoomData("e22-604-0", TestBackend);
            Debug.Log(i);
        }
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;

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
        string m_Path = Application.dataPath + "/TestData/" + "Saved_timers_Brick.csv";

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


    private void TestBackend(string brickData)
    {
        Debug.Log("Got the data");
    }
}
