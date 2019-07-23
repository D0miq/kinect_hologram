using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    private static string ipAddress;

    public static string IpAddress
    {
        get => ipAddress;
        set => ipAddress = value;
    }
}
