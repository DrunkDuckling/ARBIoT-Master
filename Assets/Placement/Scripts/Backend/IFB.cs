using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace arbiot
{
    public interface IFB
    {
        void GetAddressesPointData(Action<string> callback);
        void GetAddressesAreaData(Action<string> callback);
        void GetLiveData(Action<string> callback);
        void GetRoomData(string roomid, Action<string> callback);
    }
}
