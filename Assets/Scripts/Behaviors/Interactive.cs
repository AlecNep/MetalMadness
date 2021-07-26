using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Interactive : MonoBehaviour
{
    Collider bounds;

    private void Awake()
    {
        bounds = GetComponent<Collider>();
        bounds.isTrigger = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
