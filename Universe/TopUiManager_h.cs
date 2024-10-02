using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class TopUiManager_h : MonoBehaviour
{
    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();
    public GameObject notReady;
    
    public void OnMinimizeWindow()
    {
        ShowWindow(GetActiveWindow(), 2);
    }

    public void AppOff()
    {
        Application.Quit();
    }

    public void NotReady()
    {
        notReady.SetActive(true);
    }
    public void NotReadyOut()
    {
        notReady.SetActive(false);
    }

}
