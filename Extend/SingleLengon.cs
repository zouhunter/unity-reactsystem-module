using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;


public class SingleLengon<T> where T : new() {
    private static T instance = default(T);
    private static object lockHelper = new object();
    public static bool mManualReset = false;

    protected SingleLengon() { }
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                lock (lockHelper)
                {
                    if (instance == null)
                    {
                        instance = new T();
                    }
                }
            }
            return instance;
        }
    }
}
