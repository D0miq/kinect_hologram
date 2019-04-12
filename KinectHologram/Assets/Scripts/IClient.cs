using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IClient : MonoBehaviour
{
    public abstract void Send(string message);
}
