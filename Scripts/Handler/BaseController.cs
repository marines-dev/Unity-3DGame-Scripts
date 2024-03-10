using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    public bool switchPlay = false;
    public bool SwitchPlay
    {
        protected get { return switchPlay; }
        set
        {
            if (value == switchPlay)
                return;

            switchPlay = value;
            if (switchPlay)
            {
                //OnPlay();
            }
            else
            {
                //OnStop();
            }
        }
    }

    //protected abstract void OnPlay();
    //protected abstract void OnStop();
}
