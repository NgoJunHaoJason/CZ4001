using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using VRTK.Examples.Archery;

public class RightControllerListener: MonoBehaviour
{
    public GameSettings gameSettings;
    public GameObject bow;
    public GameObject arrow;

    private VRTK_ControllerEvents controllerEvents;
    private VRTK_InteractUse use;
    private VRTK_InteractGrab grab;
    private VRTK_InteractTouch touch;

    private void Start()
    {
        use = GetComponent<VRTK_InteractUse>();
        grab = GetComponent<VRTK_InteractGrab>();
        touch = GetComponent<VRTK_InteractTouch>();
    }

    private void OnEnable()
    {
        controllerEvents = GetComponent<VRTK_ControllerEvents>();
        if (controllerEvents == null)
        {
            VRTK_Logger.Error(VRTK_Logger.GetCommonMessage(VRTK_Logger.CommonMessageKeys.REQUIRED_COMPONENT_MISSING_FROM_GAMEOBJECT, "VRTK_ControllerEvents_ListenerExample", "VRTK_ControllerEvents", "the same"));
            return;
        }

        //Setup controller event listeners
        controllerEvents.TriggerPressed += DoTriggerPressed;

        controllerEvents.ButtonOnePressed += DoButtonOnePressed;
        controllerEvents.ButtonOneReleased += DoButtonOneReleased;
      
        controllerEvents.ButtonTwoPressed += DoButtonTwoPressed;
        controllerEvents.ButtonTwoReleased += DoButtonTwoReleased;

        controllerEvents.ControllerEnabled += DoControllerEnabled;
        controllerEvents.ControllerDisabled += DoControllerDisabled;
        controllerEvents.ControllerIndexChanged += DoControllerIndexChanged;

    }

    private void OnDisable()
    {
        if (controllerEvents != null)
        {
            controllerEvents.TriggerPressed -= DoTriggerPressed;

            controllerEvents.ButtonOnePressed -= DoButtonOnePressed;
            controllerEvents.ButtonOneReleased -= DoButtonOneReleased;

            controllerEvents.ButtonTwoPressed -= DoButtonTwoPressed;
            controllerEvents.ButtonTwoReleased -= DoButtonTwoReleased;

            controllerEvents.ControllerEnabled -= DoControllerEnabled;
            controllerEvents.ControllerDisabled -= DoControllerDisabled;
            controllerEvents.ControllerIndexChanged -= DoControllerIndexChanged;

        }
    }

    private void DoTriggerPressed(object sender, ControllerInteractionEventArgs e)
    {
        if (CheckHasArrow() || CheckBowHasArrow())
            return;
        else
        {
            GameObject newArrow = Instantiate(arrow);
            touch.ForceTouch(newArrow);
            grab.AttemptGrab();
            use.AttemptUse();

        }
    }

    private void DoTriggerReleased(object sender, ControllerInteractionEventArgs e)
    {
        
    }

    private void DoButtonOnePressed(object sender, ControllerInteractionEventArgs e)
    {
        gameSettings.ToggleArrowTrail();
    }

    private void DoButtonOneReleased(object sender, ControllerInteractionEventArgs e)
    {
   
    }

    private void DoButtonTwoPressed(object sender, ControllerInteractionEventArgs e)
    {

    }

    private void DoButtonTwoReleased(object sender, ControllerInteractionEventArgs e)
    {

    }

    private void DoControllerEnabled(object sender, ControllerInteractionEventArgs e)
    {
       
    }

    private void DoControllerDisabled(object sender, ControllerInteractionEventArgs e)
    {
        
    }

    private void DoControllerIndexChanged(object sender, ControllerInteractionEventArgs e)
    {
     
    }

    private bool CheckHasArrow()
    {
        return grab.GetGrabbedObject() != null;
    }

    private bool CheckBowHasArrow()
    {
        BowAim bowAim = bow.GetComponent<BowAim>();
        return bowAim.HasArrow();
    }

}
