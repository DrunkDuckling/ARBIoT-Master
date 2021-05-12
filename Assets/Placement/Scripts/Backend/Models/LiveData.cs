using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace arbiot
{
    public class LiveData
    {
        public List<SensorData> livedata;
    }

    [Serializable]
    public struct SensorData
    {
        public string topic;
        public string uuid;
        public long time;
        public float value;
    }
}
