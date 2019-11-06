using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommandPattern
{

    public abstract class Command
    {
        public GameObject mPlayer;
        //Parent function
        public abstract void Execute();
    }

    //~~~Child classes~~~

    
    //Active gameplay commands
    public class Jump : Command
    {
        public override void Execute()
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