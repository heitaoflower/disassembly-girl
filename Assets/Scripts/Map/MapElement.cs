using UnityEngine;
using System;

public class MapElement : MonoBehaviour {

    public Action OnVisible = null;

    public Action OnInVisible = null;

    public Action OnUpdate = null;
    
    void OnBecameVisible()
    {
        if (OnVisible != null)
        {
            OnVisible();
        }
    }

    void OnBecameInvisible()
    {
        if (OnInVisible != null)
        {
            OnInVisible();
        }
    }

    void Update()
    {
        if (OnUpdate != null)
        {
            OnUpdate();
        }
    }
}
