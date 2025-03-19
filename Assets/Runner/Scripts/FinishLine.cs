using System.Collections;
using System.Collections.Generic;
using HyperCasual.Core;
using UnityEngine;

namespace HyperCasual.Runner
{
    /// <summary>
    /// Ends the game on collision with the player, triggering a win state.
    /// This component requires a Collider component and is designed to work in both Play and Edit modes.
    /// </summary>
    [ExecuteInEditMode]
    [RequireComponent(typeof(Collider))]
    public class FinishLine : Spawnable
    {
        const string k_PlayerTag = "Player";
        
        /// <summary>
        /// Handles collision with player to trigger win state
        /// </summary>
        /// <param name="col">The collider that entered the trigger zone</param>
        void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag(k_PlayerTag))
            {
                GameManager.Instance.Win();
            }
        }
    }
}