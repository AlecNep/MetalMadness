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
        public Weapon [] mCurrentWeapon; //almost certainly will need to be changed
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

    public class Attack : Command
    {
        public override void Execute()
        {
            //Should probably check if the weapon is valid before firing; changing it will probably make it null
            mCurrentWeapon[0].Fire();
            mCurrentWeapon[1].Fire();
        }

        public Attack() : base()
        {
            mCurrentWeapon = new Weapon[2]; //will find the weapon even if the component or object itself is disabled
            mCurrentWeapon = mPlayer.GetComponentsInChildren<Weapon>(); //will almost certainly need to be changed too
        }
    }

    public class SwapWeapon : Command
    {
        public override void Execute()
        {
            
        }

        public SwapWeapon() : base()
        {

        }
    }

    public class WeaponWheel : Command
    {
        GameObject mWheel;
        

        public override void Execute()
        {
            
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

    public class ZoomIn : Command
    {
        public override void Execute()
        {
            
        }

        public ZoomIn() : base()
        {

        }
    }

    public class ZoomOut : Command
    {
        public override void Execute()
        {
            
        }

        public ZoomOut() : base()
        {

        }
    }
    //End map commands


    public class DoNothing : Command
    {
        public override void Execute()
        {
            //Intentionally blank
            
        }
    }
}