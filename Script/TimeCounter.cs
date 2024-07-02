using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace LumenCat92.TimeCounter
{
    public class TimeCounterManager : MonoBehaviour
    {
        public static TimeCounterManager Instance { set; get; }
        List<TimeCountData> countingList = new List<TimeCountData>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
                print("there has two TimeCounter Exist");
            }
        }
        public TimeCountData SetTimeCounting(float maxTime, Action function, object sequenceKey = null, Func<object, bool> sequnceMatch = null)
        {
            return GetAndSetTimeData(maxTime, maxTime, function, sequenceKey, sequnceMatch);
        }
        public TimeCountData SetTimeCounting(float maxTime, float timeInterval, Action function, object sequenceKey = null, Func<object, bool> sequnceMatch = null)
        {
            return GetAndSetTimeData(maxTime, timeInterval, function, sequenceKey, sequnceMatch);
        }
        TimeCountData GetAndSetTimeData(float maxTime, float timeInterval, Action function, object sequenceKey, Func<object, bool> sequnceMatch)
        {
            var timeData = new TimeCountData(maxTime, timeInterval, function, sequenceKey, sequnceMatch);
            countingList.Add(timeData);
            var processingTimeCounting = StartCoroutine(DoTimeCounting(timeData));
            timeData.ProcessingTimeCounting = processingTimeCounting;

            return timeData;
        }
        IEnumerator DoTimeCounting(TimeCountData data)
        {
            var nowTime = 0f;
            while (nowTime < data.MaxTime)
            {
                yield return new WaitForSeconds(data.TimeInterval);
                if (data.sequenceKey != null)
                {
                    if (data.sequenceMatch(data.sequenceKey))
                    {
                        data.RequestFunction.Invoke();
                    }
                }
                else
                {
                    data.RequestFunction.Invoke();
                }
                nowTime += data.TimeInterval;
            }

            StopTimeCounting(data);
            yield break;
        }

        public void StopTimeCounting(TimeCountData timeCountData)
        {
            if (countingList.Contains(timeCountData))
            {
                StopCoroutine(timeCountData.ProcessingTimeCounting);
                countingList.Remove(timeCountData);
            }
        }

        public class TimeCountData
        {
            public float MaxTime { private set; get; }
            public float TimeInterval { private set; get; }
            public Action RequestFunction { private set; get; }
            public Coroutine ProcessingTimeCounting { set; get; }
            public object sequenceKey = null;
            public Func<object, bool> sequenceMatch = null;

            public TimeCountData(float maxTime, float timeInterval, Action requestFunc, object sequenceKey = null, Func<object, bool> sequenceMatfch = null)
            {
                this.MaxTime = maxTime;
                this.TimeInterval = timeInterval;
                this.RequestFunction = requestFunc;
                this.sequenceKey = sequenceKey;
                this.sequenceMatch = sequenceMatfch;
            }
        }
    }
}