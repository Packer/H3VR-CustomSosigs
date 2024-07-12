using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomSosigUI : MonoBehaviour
{
    public static CustomSosigUI instance;

    void Awake()
    {
        instance = this;
    }
}
