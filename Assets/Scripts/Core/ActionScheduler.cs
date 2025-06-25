using UnityEngine;

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        IAction currentAction;

        // Returns true if there is an action currently scheduled
        // This is used to check if the AI or player is currently performing an action
        public void StartAction(IAction action)
        {
            if (currentAction == action) return;
            if (currentAction != null)
            {
                currentAction.Cancel();
            }
            currentAction = action;
        }

        // Cancels the current action
        // This is used to stop any ongoing action, such as moving or attacking
        public void CancelCurrentAction()
        {
            StartAction(null);
        }
    }
}