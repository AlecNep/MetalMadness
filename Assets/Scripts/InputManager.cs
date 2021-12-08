
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

            //Commands that should only be registered during gameplay
            Jump jump = new Jump();
            Dash dash = new Dash();
            Attack attack = new Attack();
            WeaponWheel ww = new WeaponWheel();
            SwapWeapon sw = new SwapWeapon();
            OverCharge oc = new OverCharge();
            OpenMap om = new OpenMap();

            GameplayCommand gJump = new GameplayCommand(jump);
            GameplayCommand gDash = new GameplayCommand(dash);
            GameplayCommand gAttack = new GameplayCommand(attack);
            GameplayCommand gWw = new GameplayCommand(ww);
            GameplayCommand gSw = new GameplayCommand(sw);
            GameplayCommand gOc = new GameplayCommand(oc);
            GameplayCommand gOm = new GameplayCommand(om);


            //mAButton = new Jump();
            //mBButton = new Dash();
            mAButton = gJump;
            mBButton = gDash;
            mXButton = gAttack;
            mYButton = gSw;
            mRBumper = gWw;
            mLBumper = new DoNothing();
            mStart = new Pause();
            mUp = new DoNothing();
            mDown = new DoNothing();
            mLeft = new DoNothing();
            mRight = new DoNothing();
            mRTrigger = gOc;
            mLTrigger = new DoNothing();
            mRClick = new Interact();
            mLClick = new DoNothing();
            mBack = gOm;
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

            if (Input.GetAxis("LTrigger") >= 0.6f)
            {
                mLTrigger.mInUse = true;
                mLTrigger.Press();
            }
            if (Input.GetAxis("LTrigger") < 0.6f && mLTrigger.mInUse)
            {
                //Only call this section if JUST released, hence the mInUse
                mLTrigger.mInUse = false;
                mLTrigger.Release();
            }
        }
    }
}