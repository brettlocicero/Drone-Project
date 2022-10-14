using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CursorManager
{
    public static void LockCursor () 
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public static void UnlockCursor () 
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}