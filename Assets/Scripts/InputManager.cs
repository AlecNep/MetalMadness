
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommandPattern
{
    public class InputManager : MonoBehaviour
    {

        //Player transform
        //public Rigidbody mPlayerRB;

        //Game mode
        public enum GameModes { Gameplay = 0, Menu = 1, Map = 2};
        public GameModes mGameMode = GameModes.Gameplay;
        //Controls on a basic Xbox 360 controller
        private Command mAButton, mBButton, mXButton, mYButton, mRBumper, mLBumper, mUp, mDown, mLeft, mRight, mRTrigger,
            mLTrigger, mRClick, mLClick, mStart, mBack;


        // Use this for initialization
        void Start()
        {
            //Default bindings
            //Will probably have to be optimized if/when more funtionality is added
            mAButton = new Jump();
            mBButton = new Dash();
            mXButton = new Attack();
            mYButton = new SwapWeapon();
            mRBumper = new WeaponWheel();
            mLBumper = new DoNothing();
            mStart = new Pause();
            mUp = new DoNothing();
            mDown = new DoNothing();
            mLeft = new DoNothing();
            mRight = new DoNothing();
            mRTrigger = new OverCharge();
            mLTrigger = new DoNothing();
            mRClick = new DoNothing();
            mLClick = new DoNothing();
            mBack = new OpenMap();
        }

        // Update is called once per frame
        void Update()
        {
            HandleInput();
        }

        //There should hopefully be a faster/neater way of doing this. For now it's a tedious mess
        public void HandleInput()
        {
            if (Input.GetButtonDown("AButton"))
            {
                mAButton.Press();
            }
            if (Input.GetButtonUp("AButton"))
            {
                mAButton.Release();
            }

            if (Input.GetButtonDown("BButton"))
            {
                mBButton.Press();
            }
            if (Input.GetButtonDown("BButton"))
            {
                mBButton.Release();
            }

            if (Input.GetButtonDown("XButton"))
            {
                mXButton.Press();
            }
            if (Input.GetButtonUp("XButton"))
            {
                mXButton.Release();
            }

            if (Input.GetButtonDown("YButton"))
            {
                mYButton.Press();
            }
            if (Input.GetButtonUp("YButton"))
            {
                mYButton.Release();
            }

            if (Input.GetButtonDown("RBumper"))
            {
                mRBumper.Press();
            }
            if (Input.GetButtonUp("RBumper"))
            {
                mRBumper.Release();
            }

            if (Input.GetButtonDown("LBumper"))
            {
                mLBumper.Press();
            }
            if (Input.GetButtonUp("LBumper"))
            {
                mLBumper.Release();
            }

            if (Input.GetButtonDown("Start"))
            {
                mStart.Press();
            }
            if (Input.GetButtonUp("Start"))
            {
                mStart.Release();
            }

            if (Input.GetButtonDown("Back"))
            {
                mBack.Press();
            }
            if (Input.GetButtonUp("Back"))
            {
                mBack.Release();
            }

            //Clicked analog sticks
            if (Input.GetButtonDown("LClick"))
            {
                mLClick.Press();
            }
            if (Input.GetButtonUp("LClick"))
            {
                mLClick.Release();
            }

            if (Input.GetButtonDown("RClick"))
            {
                mRClick.Press();
            }
            if (Input.GetButtonUp("RClick"))
            {
                mRClick.Release();
            }

            //D-pad
            //Not sure how to "Holdify" these
            if (Input.GetAxis("DPadX") > 0)
            {
                mRight.Press();
            }
            if (Input.GetAxis("DPadX") < 0)
            {
                mLeft.Press();
            }
            if (Input.GetAxis("DPadY") > 0)
            {
                mUp.Press();
            }
            if (Input.GetAxis("DPadY") < 0)
            {
                mDown.Press();
            }

            //Triggers
            //Not sure how to "holdify" these either
            if (Input.GetAxis("RTrigger") >= 0.6f)
            {
                mRTrigger.mInUse = true;
                mRTrigger.Press();
            }
            if (Input.GetAxis("RTrigger") < 0.6f && mRTrigger.mInUse)
            {
                //Only call this section if JUST released, hence the mInUse
                mRTrigger.mInUse = false;
                mRTrigger.Release();
            }

            if (Input.GetButtonDown("LTrigger"))
            {
                mLTrigger.Press();
            }
            if (Input.GetButtonUp("LTrigger"))
            {
                mLTrigger.Release();
            }
        }
    }
}