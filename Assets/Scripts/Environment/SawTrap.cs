using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawTrap : MonoBehaviour
{

    Transform mBlade;
    float mCutoff;
    [SerializeField]
    float mBladeSpeed;
    [SerializeField]
    float mMovementSpeed;
    [SerializeField]
    float mMovementDistance;

    // Start is called before the first frame update
    void Start()
    {
        mBlade = transform.Find("SawBlade");
    }

    // Update is called once per frame
    void Update()
    {
        mBlade.Rotate(0, 0, mBladeSpeed * Time.deltaTime);
    }
}
