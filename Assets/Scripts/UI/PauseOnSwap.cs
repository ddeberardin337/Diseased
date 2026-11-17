using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseOnSwap : MonoBehaviour
{
    public static PauseOnSwap instance;

    void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
    }
}
