using UnityEngine;
using System.Collections.Generic;

namespace Storytelling
{
    /// <summary>
    /// Base class for triggers that pass Vector3 data to their actions
    /// </summary>
    public abstract class VectorTriggerBehaviour : TriggerBehaviour
    {
        protected List<VectorActionBehaviour> vectorActions;
        
        protected virtual void Awake()
        {
            // Convert standard actions to vector actions
            vectorActions = new List<VectorActionBehaviour>();
            foreach (ActionBehaviour action in actions)
            {
                if (action is VectorActionBehaviour vectorAction)
                {
                    vectorActions.Add(vectorAction);
                }
            }
        }

        /// <summary>
        /// Performs all vector actions with the provided vector data
        /// </summary>
        protected void PerformAllVectorActions(Vector3 vector)
        {
            if (logWhenTriggered)
            {
                if (initiator == null)
                {
                    Debug.Log($"{name} vector trigger was triggered with vector: {vector}");
                }
                else
                {
                    Debug.Log($"{name} vector trigger was triggered by {initiator.name} with vector: {vector}");
                }
            }

            foreach (VectorActionBehaviour action in vectorActions)
            {
                action.Act(vector);
            }
        }
    }

    /// <summary>
    /// Base class for actions that receive Vector3 data
    /// </summary>
    public abstract class VectorActionBehaviour : ActionBehaviour
    {
        public virtual void Act(Vector3 vector)
        {
            if (logWhenPerformed)
            {
                Debug.Log($"{name} vector action was performed with vector: {vector}");
            }
        }

        // Override the original Act method to prevent direct calls
        public override void Act()
        {
            Debug.LogWarning($"{name} is a VectorActionBehaviour and requires a Vector3 parameter");
        }
    }

    /// <summary>
    /// Trigger that continuously passes the player's position
    /// </summary>
    public class PositionVectorTrigger : VectorTriggerBehaviour
    {
        [SerializeField] private Transform playerTransform;
        private Vector3 initialPosition;
        
        protected override void Awake()
        {
            base.Awake();
            
            if (playerTransform == null)
            {
                // Try to find player if not assigned
                playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
                
                if (playerTransform == null)
                {
                    Debug.LogError($"{name}: No player transform assigned and couldn't find object with Player tag");
                    enabled = false;
                    return;
                }
            }
            
            initialPosition = playerTransform.position;
        }

        private void Update()
        {
            CheckTriggered();
        }

        public override void CheckTriggered()
        {
            if (playerTransform != null)
            {
                // Calculate position relative to initial position
                Vector3 relativePosition = playerTransform.position - initialPosition;
                PerformAllVectorActions(relativePosition);
            }
        }
    }
}
