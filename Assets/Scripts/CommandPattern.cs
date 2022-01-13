using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace CommandPattern //Might not need this
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

    public class GameplayCommand : Command
    {
        private Action pressCommand;
        private Action releaseCommand;

        public override void Press()
        {
            if (pressCommand == null)
            {
                throw new NotImplementedException();
            }
            else
            {
                if (!PauseMenu.isPaused && (int)GameManager.currentGameMode < 2) //having both conditions is probably redundant
                {
                    pressCommand();
                }
                /*else
                {
                    Debug.Log("Trying to call a GameplayCommand.Press() method while paused");
                }*/
            }       
        }

        public override void Release()
        {
            if (releaseCommand == null)
            {
                throw new NotImplementedException();
            }
            else
            {
                if (!PauseMenu.isPaused && (int)GameManager.currentGameMode < 2) //having both conditions is probably redundant
                {
                    releaseCommand();
                }
                /*else
                {
                    Debug.Log("Trying to call a GameplayCommand.Release() method while paused");
                }*/
            }
        }

        public GameplayCommand()
        {
            pressCommand = null;
            releaseCommand = null;
            Debug.LogWarning(name + " is a GameplayCommand but has no methods to implement!");
        }

        public GameplayCommand(Action press, Action release)
        {
            pressCommand = press;
            releaseCommand = release;
        }

        public GameplayCommand(Command c)
        {
            pressCommand = c.Press;
            releaseCommand = c.Release;
        }
    }

    public class MenuCommand : Command
    {
        private Action pressCommand;
        private Action releaseCommand;

        public override void Press()
        {
            if (pressCommand == null)
            {
                throw new NotImplementedException();
            }
            else
            {
                if ((int)GameManager.currentGameMode == 2)
                {
                    pressCommand();
                }
                else
                {
                    Debug.Log("Trying to call a MenuCommand.Press() method while not paused. How did you manage to pull that off???");
                }
            }
        }

        public override void Release()
        {
            if (releaseCommand == null)
            {
                throw new NotImplementedException();
            }
            else
            {
                if ((int)GameManager.currentGameMode == 2)
                {
                    releaseCommand();
                }
                else
                {
                    Debug.Log("Trying to call a MenuCommand.Release() method while not paused. How did you manage to pull that off???");
                }
            }
        }
    }

    //Active gameplay commands
    public class Jump : Command
    {
        private Vector3 relativeDirections;
        public override void Press()
        {
            mPlayerControls.Jump();
            /*if (mPlayerControls.isGrounded)
            {
                relativeDirections = mPlayer.transform.InverseTransformDirection(mPlayerRb.velocity);
                if (OverCharge.mCharged)
                {
                    relativeDirections.y = mPlayerControls.mChargedJumpForce;
                }
                else
                {
                    relativeDirections.y = mPlayerControls.mJumpForce;
                    
                }
                mPlayerRb.velocity = mPlayer.transform.TransformDirection(relativeDirections);
            }*/
        }

        public override void Release()
        {
            mPlayerControls.JumpSlowdown();
            /*relativeDirections = mPlayer.transform.InverseTransformDirection(mPlayerRb.velocity);
            if (relativeDirections.y > 0)
            {
                relativeDirections.y *= mPlayerControls.mCutJumpHeight;
                mPlayerRb.velocity = mPlayer.transform.TransformDirection(relativeDirections);
            }*/
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
        public override void Press()
        {
            GameManager.Instance.weaponWheel.gameObject.SetActive(true);
            GameManager.SetGameMode(1);
        }
        
        public override void Release()
        {
            GameManager.Instance.weaponWheel.gameObject.SetActive(false);
            GameManager.SetGameMode(0);
            WeaponSelector.Reset();
        }

        public WeaponWheel() : base()
        {
            GameManager.Instance.weaponWheel.gameObject.SetActive(false);
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
            mPlayerControls.isCharged = true;
        }
        
        public override void Release()
        {
            mPlayerControls.isCharged = false;
        }

        private void SetCharge(bool charged)
        {
            mPlayerControls.mWeapons[mPlayerControls.mWeaponIndex].Overcharged(charged);
            mPlayerControls.mWeapons[mPlayerControls.mWeaponIndex + mPlayerControls.mWeaponCount].Overcharged(charged);
        }

        public OverCharge() : base()
        {

        }
    }

    public class Interact : Command
    {
        public override void Press()
        {
            if (mPlayerControls.interactableObject != null)
            {
                mPlayerControls.interactableObject.Interact();
            }
        }

        public override void Release()
        {

        }

        public Interact() : base()
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
            GameManager.Instance.pauseMenu.PauseGame();
            if (GameManager.Instance.weaponWheel.gameObject.activeSelf)
            {
                GameManager.Instance.weaponWheel.gameObject.SetActive(false);
                WeaponSelector.Reset();
            }
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

    public class MenuMoveRight : Command
    {
        public override void Press()
        {
            throw new NotImplementedException();
        }

        public override void Release()
        {
            throw new NotImplementedException();
        }
    }

    public class MenuMoveLeft : Command
    {
        public override void Press()
        {
            throw new NotImplementedException();
        }

        public override void Release()
        {
            throw new NotImplementedException();
        }
    }

    public class MenuMoveUp : Command
    {
        public override void Press()
        {
            throw new NotImplementedException();
        }

        public override void Release()
        {
            throw new NotImplementedException();
        }
    }

    public class MenuMoveDown : Command
    {
        public override void Press()
        {
            throw new NotImplementedException();
        }

        public override void Release()
        {
            throw new NotImplementedException();
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