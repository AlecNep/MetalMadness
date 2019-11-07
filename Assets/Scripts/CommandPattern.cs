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
        //Parent function
        public abstract void Execute();
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
            if (mPlayerControls.IsGrounded())
            {
                //mRb.AddForce(mJumpforce * transform.up, ForceMode.Impulse); //original
                mPlayerRb.AddForce(mPlayerControls.mJumpforce * (-mPlayerControls.mGravNormal), ForceMode.Impulse);
            }
        }

        public Jump() : base()
        {

        }
    }

    public class Attack : Command
    {
        public override void Execute()
        {

        }
    }

    public class SwapWeapon : Command
    {
        public override void Execute()
        {

        }
    }

    public class WeaponWheel : Command
    {
        public override void Execute()
        {

        }
    }

    public class Dash : Command
    {
        public override void Execute()
        {

        }
    }

    public class OpenMap : Command
    {
        public override void Execute()
        {

        }
    }

    public class Pause : Command
    {
        public override void Execute()
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
    }

    public class Cancel : Command
    {
        public override void Execute()
        {

        }
    }

    public class NextTab : Command
    {
        public override void Execute()
        {

        }
    }

    public class PreviousTab : Command
    {
        public override void Execute()
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
    }

    public class Close : Command
    {
        public override void Execute()
        {

        }
    }

    public class ZoomIn : Command
    {
        public override void Execute()
        {

        }
    }

    public class ZoomOut : Command
    {
        public override void Execute()
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