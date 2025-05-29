using System;
using UnityEditor;
public static class GameEvents
{
    public static event Action CanDisplayLampBar;
    public static event Action CannotDisplayLampBar;
    public static event Action LampStateChanging;
    public static event Action BarIsNull;

    public static void RaiseCanDisplayLampBar() => CanDisplayLampBar?.Invoke();
    public static void RaiseCannotDisplayLampBar() => CannotDisplayLampBar?.Invoke();
    public static void RaiseLampStateChanging() => LampStateChanging?.Invoke();
    public static void RaiseBarIsNull() => BarIsNull?.Invoke();
}
