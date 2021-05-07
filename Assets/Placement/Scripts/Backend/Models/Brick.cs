using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace arbiot
{
    public class Brick
    {
        public List<BrickData> sensors; 
    }

    [Serializable]
    public struct BrickData
    {
        public string sensortype;
        public string room;
        public string uuid;
    }
}
