
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
        private Command mAButton, mBButton, mXButton, mYButton, mRBumper, mLBumper, mUp, mDown, mLeft, mRight, mRTrigger, mLTrigger, mRClick, mLClick, mStart, mBack;


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
            mLBumper = new OpenMap();
            mStart = new Pause();

            mUp = new DoNothing();
            mDown = new DoNothing();
            mLeft = new DoNothing();
            mRight = new DoNothing();
            mRTrigger = new DoNothing();
            mLTrigger = new DoNothing();
            mRClick = new DoNothing();
            mLClick = new DoNothing();
            mBack = new DoNothing();
        }

        // Update is called once per frame
        void Update()
        {
            HandleInput();
        }

        public void HandleInput()
        {
            //Maybe put this somewhere else
            //The buttons stay the same, it's their context that changes
            /*switch ((int)mGameMode)
            {
                case 0: //Active gameplay
                    
                    break;
                case 1: //Menu
                    break;
                case 2: //Map
                    break;
            }*/
            if (Input.GetButtonDown("AButton"))
            {
                mAButton.Execute();
            }
            if (Input.GetButtonDown("BButton"))
            {
                mBButton.Execute();
            }
            if (Input.GetButtonDown("XButton"))
            {
                mXButton.Execute();
            }
            if (Input.GetButtonDown("YButton"))
            {
                mYButton.Execute();
            }
            if (Input.GetButtonDown("RBumper"))
            {
                mRBumper.Execute();
            }
            if (Input.GetButtonDown("LBumper"))
            {
                mLBumper.Execute();
            }
            if (Input.GetButtonDown("Start"))
            {
                mStart.Execute();
            }
            if (Input.GetButtonDown("Back"))
            {
                mBack.Execute();
            }

            //Clicked analog sticks
            if (Input.GetButtonDown("LClick"))
            {
                mLClick.Execute();
            }
            if (Input.GetButtonDown("RClick"))
            {
                mRClick.Execute();
            }

            //D-pad
            if (Input.GetAxis("DPadX") > 0)
            {
                mRight.Execute();
            }
            if (Input.GetAxis("DPadX") < 0)
            {
                mLeft.Execute();
            }
            if (Input.GetAxis("DPadY") > 0)
            {
                mUp.Execute();
            }
            if (Input.GetAxis("DPadY") < 0)
            {
                mDown.Execute();
            }

            //Triggers
            if (Input.GetAxis("RTrigger") > 0.3f)
            {
                mRTrigger.Execute();
            }
            if (Input.GetAxis("LTrigger") > 0.3f)
            {
                mLTrigger.Execute();
            }
        }
    }
}