using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommandPattern
{

    public abstract class Command
    {
        //Parent function
        public abstract void Execute();
    }

    //Child classes

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

    public class DoNothing : Command
    {
        public override void Execute()
        {
            //Intentionally blank
        }
    }
}