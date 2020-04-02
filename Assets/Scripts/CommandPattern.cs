using System.Collections;
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
        //Idea: if the button type calls for it, start a timer once the button is pressed and stop it once it's released
        //could also create a toggle for things where the duration doesn't matter
        //essentially writing button code from scratch!

        //Ex. 1: if the weapon wheel button is released before a certain threshold, only swap to the last weapon
        //e.g. after releasing: if (pressTime < threshold) {swapWeapon();} else {openWheel();}
        //Ex. 2: for weapons like the minigun or FT, turn on a variable once the fire button is pressed and turn it off when released
        //e.g. in weapon script update function: if (firing) {fire()} keep in mind timer is already taken care of in parent class


        //Parent function
        public abstract void Execute();
        public void SetButtonType(int t)
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

    public abstract class HoldableCommand : Command
    {
        public abstract void Hold();
        public abstract void Release();
    }

    
    //Active gameplay commands
    public class Jump : Command
    {
        public override void Execute()
        {
            
            if (mPlayerControls.IsGrounded())// || mPlayerControls.mCanDoubleJump)
            {
                mPlayerRb.AddForce(mPlayerControls.mJumpforce * (-mPlayerControls.mGravNormal), ForceMode.Impulse);
            }
        }
        
        public Jump() : base()
        {
            mButtonType = mType.press;
        }
    }

    public class Attack : HoldableCommand
    {
        

        public override void Execute()
        {
            //Should probably check if the weapon is valid before firing; changing it will probably make it null
            mPlayerControls.mWeapons[mPlayerControls.mWeaponIndex].Fire();
            mPlayerControls.mWeapons[mPlayerControls.mWeaponIndex + 2].Fire();
        }

        public override void Hold()
        {
            throw new System.NotImplementedException();
        }

        public override void Release()
        {
            throw new System.NotImplementedException();
        }

        public Attack() : base()
        {
            //Whole thing will need to be changed once new weapons are added
            //mCurrentWeapon = new Weapon[mPlayer.transform.childCount]; //will find the weapon even if the component or object itself is disabled
            
        }
    }

    public class SwapWeapon : Command
    {
        public override void Execute()
        {
            //IMPORTANT: this is only temporary code
            //mPlayerControls.mWeaponIndex = 2 / (mPlayerControls.mWeaponIndex + 2);
            
            int lTemp = mPlayerControls.mWeaponIndex;
            //print("Before: cur=" + mPlayerControls.mWeaponIndex + ", prev=" + mPlayerControls.mPreviousWeaponIndex + ", temp=" + lTemp);
            mPlayerControls.mWeaponIndex = mPlayerControls.mPreviousWeaponIndex;
            mPlayerControls.mPreviousWeaponIndex = lTemp;
            //print("Before: cur=" + mPlayerControls.mWeaponIndex + ", prev=" + mPlayerControls.mPreviousWeaponIndex + ", temp=" + lTemp);
            mPlayerControls.ClearWeapons();
        }

        public SwapWeapon() : base()
        {

        }
    }

    public class WeaponWheel : HoldableCommand
    {
        GameObject mWheel;
        bool mIsActive = false;
        

        public override void Execute()
        {
            if (mIsActive)
            {
                Release();
            }
            else
            {
                Hold();
            }
        }

        public override void Hold()
        {
            mWheel.SetActive(true);
            mIsActive = !mIsActive;
        }

        public override void Release()
        {
            mWheel.SetActive(false);
            mIsActive = !mIsActive;
        }

        public WeaponWheel() : base()
        {
            mButtonType = (mType)2;
            mWheel = GameObject.Find("UI").GetComponent<Transform>().Find("Weapon Wheel").gameObject;
            if (mWheel != null)
            {
                mWheel.SetActive(false);
            }
        }
    }

    public class Dash : Command
    {
        public override void Execute()
        {
            
        }

        public Dash() : base()
        {

        }
    }

    public class OverCharge : HoldableCommand
    {
        public override void Execute()
        {

        }

        public override void Hold()
        {
            throw new System.NotImplementedException();
        }

        public override void Release()
        {
            throw new System.NotImplementedException();
        }

        public OverCharge() : base()
        {

        }
    }

    public class OpenMap : Command
    {
        public override void Execute()
        {
            
        }

        public OpenMap() : base()
        {

        }
    }

    public class Pause : Command
    {
        public override void Execute()
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
        public override void Execute()
        {
            
        }

        public Select() : base()
        {

        }
    }

    public class Cancel : Command
    {
        public override void Execute()
        {
            
        }

        public Cancel() : base()
        {

        }
    }

    public class NextTab : Command
    {
        public override void Execute()
        {
            
        }

        public NextTab() : base()
        {

        }
    }

    public class PreviousTab : Command
    {
        public override void Execute()
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
        public override void Execute()
        {
            
        }

        public PlaceMarker() : base()
        {

        }
    }

    public class Close : Command
    {
        public override void Execute()
        {
            
        }

        public Close() : base()
        {

        }
    }

    public class ZoomIn : HoldableCommand
    {
        public override void Execute()
        {
            
        }

        public override void Hold()
        {
            throw new System.NotImplementedException();
        }

        public override void Release()
        {
            throw new System.NotImplementedException();
        }

        public ZoomIn() : base()
        {

        }
    }

    public class ZoomOut : HoldableCommand
    {
        public override void Execute()
        {
            
        }

        public override void Hold()
        {
            throw new System.NotImplementedException();
        }

        public override void Release()
        {
            throw new System.NotImplementedException();
        }

        public ZoomOut() : base()
        {

        }
    }
    //End map commands


    public class DoNothing : HoldableCommand
    {
        public override void Execute()
        {
            //Intentionally blank
            
        }

        public override void Hold()
        {
            throw new System.NotImplementedException();
        }

        public override void Release()
        {
            throw new System.NotImplementedException();
        }
    }
}