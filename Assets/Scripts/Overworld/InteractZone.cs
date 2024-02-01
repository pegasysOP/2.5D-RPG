using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractZone : MonoBehaviour
{
    public List<Collider> Colliders { get { return colliders; } }
    List<Collider> colliders = new List<Collider>();

    private void OnTriggerEnter(Collider other)
    {
        colliders.Add(other);

        if (colliders.Count > 5)
            colliders.RemoveAt(0);
    }

    private void OnTriggerExit(Collider other)
    {
        colliders.Remove(other);
    }
}
