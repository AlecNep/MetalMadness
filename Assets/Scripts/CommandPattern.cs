﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommandPattern
{

    public abstract class Command : MonoBehaviour
    {
        public GameObject mPlayer;
        public Rigidbody mPlayerRb;
        public PlayerControls mPlayerControls;
        public enum mType { press = 0, hold = 1, pAndR = 2 }
        public mType mButtonType = 0;
        public bool mInUse;


        //Parent function
        public abstract void Press();
        public abstract void Release();
        public void SetButtonType(int t) //almost certainly unnecessary
        {
            if (t >= 0 && t < 3)
            {
                mButtonType = (mType)t;
            }
            else
                mButtonType = 0;
        }
        public Command()
        {
            mPlayer = GameObject.FindGameObjectWithTag("Player");
            mPlayerRb = mPlayer.GetComponent<Rigidbody>();
            mPlayerControls = mPlayer.GetComponent<PlayerControls>();
            
        }
    }

    //~~~Child classes~~~

    
    //Active gameplay commands
    public class Jump : Command
    {
        public override void Press()
        {
            
            if (mPlayerControls.IsGrounded())
            {
                if (OverCharge.mCharged)
                {
                    //Doesn't seem to make a difference; very inconsistent regardless
                    mPlayerRb.AddForce(mPlayerControls.mChargedJumpForce * (-mPlayerControls.mGravShifter.mGravNormal), ForceMode.Impulse);
                    //mPlayerRb.velocity = -mPlayerControls.mGravShifter.mGravNormal * mPlayerControls.mChargedJumpForce;
                }
                else
                {
                    //Doesn't seem to make a difference; very inconsistent regardless
                    mPlayerRb.AddForce(mPlayerControls.mJumpForce * (-mPlayerControls.mGravShifter.mGravNormal), ForceMode.Impulse);
                    //mPlayerRb.velocity = -mPlayerControls.mGravShifter.mGravNormal * mPlayerControls.mJumpForce;
                }
            }
        }

        public override void Release()
        {
            
        }

        public Jump() : base()
        {
            mButtonType = mType.press;
        }
    }

    public class Attack : Command
    {
        

        public override void Press()
        {
            //Should probably check if the weapon is valid before firing; changing it will probably make it null
            mPlayerControls.mWeapons[mPlayerControls.mWeaponIndex].Fire();
            mPlayerControls.mWeapons[mPlayerControls.mWeaponIndex + mPlayerControls.mWeaponCount].Fire();
        }

        public override void Release()
        {
            if ((int)mPlayerControls.mWeapons[mPlayerControls.mWeaponIndex].mFireType > 0)
            {
                mPlayerControls.mWeapons[mPlayerControls.mWeaponIndex].StopFiring(); //might bog things down. Come back to this
                mPlayerControls.mWeapons[mPlayerControls.mWeaponIndex + mPlayerControls.mWeaponCount].StopFiring();
            }
        }

        public Attack() : base()
        {
            //Whole thing will need to be changed once new weapons are added
            
        }
    }

    public class SwapWeapon : Command
    {
        public override void Press()
        {            
            int lTemp = mPlayerControls.mWeaponIndex;
            mPlayerControls.mWeaponIndex = mPlayerControls.mPreviousWeaponIndex;
            mPlayerControls.mPreviousWeaponIndex = lTemp;
            mPlayerControls.ClearWeapons();
        }

        public override void Release()
        {
            
        }

        public SwapWeapon() : base()
        {

        }
    }

    public class WeaponWheel : Command
    {
        GameObject mWheel;

        public override void Press()
        {
            mWheel.SetActive(true);
            mPlayerControls.ChangeControlMode(1);
        }
        
        public override void Release()
        {
            mWheel.SetActive(false);
            mPlayerControls.ChangeControlMode(0);
            WeaponSelector.Reset();
        }

        public GameObject GetWeaponWheel() //probably useless
        {
            return mWheel;
        }

        public WeaponWheel() : base()
        {
            mWheel = GameObject.Find("Weapon Wheel").gameObject;
            if (mWheel != null)
            {
                mWheel.SetActive(false);
            }
        }
    }

    public class Dash : Command
    {
        public override void Press()
        {
            if (mPlayerControls.CanDash())
            {
                mPlayerControls.mDashTimer = mPlayerControls.mDashDelay; //probably should be waaaay more secure than this
            }
        }

        public override void Release()
        {
            
        }

        public Dash() : base()
        {

        }
    }

    public class OverCharge : Command
    {
        public static bool mCharged { get; private set; }

        public override void Press()
        {
            //probably super redundant
            mCharged = true;
            mPlayerControls.mWeapons[mPlayerControls.mWeaponIndex].Overcharged(mCharged);
            mPlayerControls.mWeapons[mPlayerControls.mWeaponIndex + mPlayerControls.mWeaponCount].Overcharged(mCharged);
        }
        
        public override void Release()
        {
            //probably super redundant
            mCharged = false;
            mPlayerControls.mWeapons[mPlayerControls.mWeaponIndex].Overcharged(mCharged);
            mPlayerControls.mWeapons[mPlayerControls.mWeaponIndex + mPlayerControls.mWeaponCount].Overcharged(mCharged);
        }

        public OverCharge() : base()
        {

        }
    }

    public class OpenMap : Command
    {
        public override void Press()
        {
            
        }

        public override void Release()
        {
            
        }

        public OpenMap() : base()
        {

        }
    }

    public class Pause : Command
    {
        public override void Press()
        {
            
        }

        public override void Release()
        {
            
        }

        public Pause() : base()
        {

        }
    }

    //End active gameplay commands


    //Menu commands
    public class Select : Command
    {
        public override void Press()
        {
            
        }

        public override void Release()
        {
            
        }

        public Select() : base()
        {

        }
    }

    public class Cancel : Command
    {
        public override void Press()
        {
            
        }

        public override void Release()
        {
            
        }

        public Cancel() : base()
        {

        }
    }

    public class NextTab : Command
    {
        public override void Press()
        {
            
        }

        public override void Release()
        {
            
        }

        public NextTab() : base()
        {

        }
    }

    public class PreviousTab : Command
    {
        public override void Press()
        {
            
        }

        public override void Release()
        {
            
        }

        public PreviousTab() : base()
        {

        }
    }
    //End menu commands

    //Map commands
    public class PlaceMarker : Command
    {
        public override void Press()
        {
            
        }

        public override void Release()
        {
            
        }

        public PlaceMarker() : base()
        {

        }
    }

    public class Close : Command
    {
        public override void Press()
        {
            
        }

        public override void Release()
        {
            
        }

        public Close() : base()
        {

        }
    }

    public class ZoomIn : Command
    {
        public override void Press()
        {
            
        }
        
        public override void Release()
        {
            
        }

        public ZoomIn() : base()
        {

        }
    }

    public class ZoomOut : Command
    {
        public override void Press()
        {
            
        }
        
        public override void Release()
        {
            
        }

        public ZoomOut() : base()
        {

        }
    }
    //End map commands


    public class DoNothing : Command
    {
        public override void Press()
        {
            //Intentionally blank
            
        }
        
        public override void Release()
        {
            
        }
    }
}