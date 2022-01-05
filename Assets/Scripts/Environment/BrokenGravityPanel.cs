using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenGravityPanel : MonoBehaviour
{
    [SerializeField]
    private int direction;

    private void OnValidate()
    {
        if (direction < 0 || direction > 3)
        {
            direction = 0;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    protected void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out GravityShifter shiftable))
        {
            shiftable.ForceGravity(direction);
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out GravityShifter shiftable))
        {
            shiftable.Lock(false);
        }
    }
}
