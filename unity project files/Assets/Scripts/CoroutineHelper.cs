using System;

public class CoroutineHelper {

    private static DateTime _start;

    public static void Start()
    {
        _start = DateTime.Now;
    }

    public static bool Exceeded(int ms)
    {
        DateTime now = DateTime.Now;
        int duration = (now - _start).Milliseconds;

        return (ms >= duration);
    }
}
