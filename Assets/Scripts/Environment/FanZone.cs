using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanZone : MonoBehaviour
{
    private enum directions { south = 0, east = 1, north = 2, west = 3}
    [SerializeField]
    directions direction;
    [SerializeField]
    float power;

    private Vector3[] vectors;


    // Start is called before the first frame update
    void Start()
    {
        vectors = new Vector3[4] { Vector3.down, Vector3.right, Vector3.up, Vector3.left };
    }

    private void OnTriggerStay(Collider other)
    {
        other.attachedRigidbody.AddForce(power * vectors[(int)direction]);
    }
}
