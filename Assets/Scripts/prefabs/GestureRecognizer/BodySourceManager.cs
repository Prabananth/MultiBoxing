using UnityEngine;
using System.Collections;
using Windows.Kinect;
using System;

public class BodySourceManager : MonoBehaviour
{
    private KinectSensor _Sensor;
    private BodyFrameReader _Reader;
    private Body[] _Data = null;
    private Windows.Kinect.Vector4 floorClipPlane;
    private static TimeSpan LastFrameTime;
    private static int FrameCount;

    public Body[] GetData()
    {
        return _Data;
    }

    public Windows.Kinect.Vector4 GetFloorClipPlane()
    {
        return floorClipPlane;
    }

    public static TimeSpan GetKinectDataTime()
    {
        return LastFrameTime;
    }

    public static int GetFrameCount()
    {
        return FrameCount;
    }



    void Start()
    {
        _Sensor = KinectSensor.GetDefault();

        if (_Sensor != null)
        {
            _Reader = _Sensor.BodyFrameSource.OpenReader();

            if (!_Sensor.IsOpen)
            {
                _Sensor.Open();
            }
        }
        FrameCount = 0;
        LastFrameTime = new TimeSpan(0, 0, 0);
    }

    void Update()
    {
        if (_Reader != null)
        {
            var frame = _Reader.AcquireLatestFrame();
            if (frame != null)
            {
                if (_Data == null)
                {
                    _Data = new Body[_Sensor.BodyFrameSource.BodyCount];
                }
                floorClipPlane = frame.FloorClipPlane;
                frame.GetAndRefreshBodyData(_Data);

                if (LastFrameTime != frame.RelativeTime)
                {
                    LastFrameTime = frame.RelativeTime;
                    FrameCount++;
                }

                frame.Dispose();
                frame = null;
            }
        }
    }

    void OnApplicationQuit()
    {
        if (_Reader != null)
        {
            _Reader.Dispose();
            _Reader = null;
        }

        if (_Sensor != null)
        {
            if (_Sensor.IsOpen)
            {
                _Sensor.Close();
            }

            _Sensor = null;
        }
    }
}
