# LumenCat92
<div align="center">

![LumenCat92.jpg](https://github.com/lumenCat92/TimeCounter/blob/main/Image/LumenCat92.jpg)
</div>

# TimeCounterManager

<div align="center">

![TimeCounterManager.jpg](https://github.com/lumenCat92/TimeCounter/blob/main/Image/TimeCounterManager.jpg)
</div>

# Language
<details>
<summary>English</summary>

# How Can Install This?

Download this to Assets Folder in your unity project.

# What is This?

its Manager that time counting and execute call back func depending on time passed.

# Where Can Use This?

General time counting, or execute call back func depending on time passed, and if u need, its also supporting, sessions checking too.

# How to Use This?

1. since this is singleton manager, u have to attached this to GameObj in scene as components.

2. when u look at the code,
```csharp
public class TimeCounterManager : MonoBehaviour
{
    public static TimeCounterManager Instance { set; get; }
    public TimeCountData SetTimeCounting(float maxTime, Action function, object sequenceKey = null, Func<object, bool> sequnceMatch = null)
    {
        return GetAndSetTimeData(maxTime, maxTime, function, sequenceKey, sequnceMatch);
    }
    public TimeCountData SetTimeCounting(float maxTime, float timeInterval, Action function, object sequenceKey = null, Func<object, bool> sequnceMatch = null)
    {
        return GetAndSetTimeData(maxTime, timeInterval, function, sequenceKey, sequnceMatch);
    }

    IEnumerator DoTimeCounting(TimeCountData data)
    {
        var nowTime = 0f;
        while (nowTime < data.MaxTime)
        {
            yield return new WaitForSeconds(data.timeInterval);
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
            nowTime += data.timeInterval;
        }

        StopTimeCounting(data);
        yield break;
    }
}
```

maxTime : max time for counting  
timeInterval : time counting interval  
function : call back func that will execute each time counting interval  
sequenceKey : if u using time count in one sessesion. this will be key  
sequnceMatch : This will decide which function to execute based on whether the sequenceKey matches the object passed as a parameter  
  
Example.  
if try to execute call-back after specific time,
```csharp
{
    void Do()
    {
        var time = 4f;
        TimeCounterManager.Instance.SetTimeCounting(time, () => { Action();});
    }
}
```

if try to execute call-back in a sessection that "if data is same".
``` csharp
{
    MouduleData data = null;
    void Do()
    {
        var maxTime = 4f;
        var timeInterval = 0.1f;
        TimeCounterManager.Instance.SetTimeCounting(maxTime, timeInterval ,() => { Action();}, data, (key) => data == key);
    }
}
```

3. If you want to manually terminate it before time runs out, you must use the TimeCountData issued when the function is executed.
``` csharp
{
    public TimeCountData SetTimeCounting(float maxTime, Action function, object sequenceKey = null, Func<object, bool> sequnceMatch = null)
    {
        return GetAndSetTimeData(maxTime, maxTime, function, sequenceKey, sequnceMatch);
    }

    public void StopTimeCounting(TimeCountData timeCountData)
    {
        if (countingList.Contains(timeCountData))
        {
            StopCoroutine(timeCountData.ProcessingTimeCounting);
            countingList.Remove(timeCountData);
        }
    }
}
```


</details>

<details>
<summary>한국어</summary>

# 어떻게 설치하죠?

직접 다운로드해서 프로젝트의 Assets에 설치합니다.

# 이게 뭐죠?

시간 카운팅과 해당 시간에 경과에 따른 콜백 함수를 실행시키는 매니져입니다.

# 어디에 쓰나요?

일반적인 시간카운팅, 또는 시간 이후에 따른 콜백 실행, 콜백시 세션 체크가 필요하다면 사용가능합니다.

# 어떻게 사용하나요?

1. 싱글톤 메니저라 씬에 있는 게임 오브젝트에 컴포넌트로 추가하셔야합니다.

2. 핵심코드를 둘러보면,
```csharp
public class TimeCounterManager : MonoBehaviour
{
    public static TimeCounterManager Instance { set; get; }
    public TimeCountData SetTimeCounting(float maxTime, Action function, object sequenceKey = null, Func<object, bool> sequnceMatch = null)
    {
        return GetAndSetTimeData(maxTime, maxTime, function, sequenceKey, sequnceMatch);
    }
    public TimeCountData SetTimeCounting(float maxTime, float timeInterval, Action function, object sequenceKey = null, Func<object, bool> sequnceMatch = null)
    {
        return GetAndSetTimeData(maxTime, timeInterval, function, sequenceKey, sequnceMatch);
    }

    IEnumerator DoTimeCounting(TimeCountData data)
    {
        var nowTime = 0f;
        while (nowTime < data.MaxTime)
        {
            yield return new WaitForSeconds(data.timeInterval);
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
            nowTime += data.timeInterval;
        }

        StopTimeCounting(data);
        yield break;
    }
}
```

maxTime : 최대 카운팅 시간  
timeInterval : 카운팅 간격  
function : 카운팅 간격마다 실행시킬 콜백 함수  
sequenceKey : 하나의 시퀀스로 사용할 경우, 시퀀스의 키가 될 항목  
sequnceMatch : sequenceKey와 sequenceMatch함수에 넘겨질 object파라메터의 일치 여부에 따라 function 동작의 유무를 결정.  
  
Example.  
특정 시간 이후, 콜백을 실행시키려 한다면,
```csharp
{
    void Do()
    {
        var time = 4f;
        TimeCounterManager.Instance.SetTimeCounting(time, () => { Action();});
    }
}
```

"data가 같을 때"라는 하나의 세션 특정 주기마다 콜백을 실행하려 한다면.
``` csharp
{
    MouduleData data = null;
    void Do()
    {
        var maxTime = 4f;
        var timeInterval = 0.1f;
        TimeCounterManager.Instance.SetTimeCounting(maxTime, timeInterval ,() => { Action();}, data, (key) => data == key);
    }
}
```

3. 시간이 다되기전에 수동으로 종료하려고 한다면, 함수 실행시 발급받는 TimeCountData를 사용해야합니다.
``` csharp
{
    public TimeCountData SetTimeCounting(float maxTime, Action function, object sequenceKey = null, Func<object, bool> sequnceMatch = null)
    {
        return GetAndSetTimeData(maxTime, maxTime, function, sequenceKey, sequnceMatch);
    }

    public void StopTimeCounting(TimeCountData timeCountData)
    {
        if (countingList.Contains(timeCountData))
        {
            StopCoroutine(timeCountData.ProcessingTimeCounting);
            countingList.Remove(timeCountData);
        }
    }
}
```