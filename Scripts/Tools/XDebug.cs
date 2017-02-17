using UnityEngine;

public class XDebug {
    public static void Log(string str, params object[] args) {
        Debug.Log(string.Format(str, args));
    }
}