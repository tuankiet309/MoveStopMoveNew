using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyLog : MonoBehaviour
{
    public static void LogCaller(
      [System.Runtime.CompilerServices.CallerLineNumber] int line = 0
    , [System.Runtime.CompilerServices.CallerMemberName] string memberName = ""
    , [System.Runtime.CompilerServices.CallerFilePath] string filePath = ""
)
    {
        // Can replace UnityEngine.Debug.Log with any logging API you want
        UnityEngine.Debug.Log($"{line} :: {memberName} :: {filePath}");
    }
}
