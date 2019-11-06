using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommandPattern
{
    public class InputManager : MonoBehaviour
    {

        //Player transform
        public Rigidbody mPlayerRB;
        //Controls on a basic Xbox 360 controller
        private Command mAButton, mBButton, mXButton, mYButton, mRBumper, mLBumper, mUp, mDown, mLeft, mRight; //mRTrigger, mLTrigger; //not sure if/how well these will work here since they're triggers


        // Use this for initialization
        void Start()
        {
            //Default bindings
            //mAButton = 
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}