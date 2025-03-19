using System.Collections;
using System.Collections.Generic;
using HyperCasual.Core;
using UnityEngine;

namespace HyperCasual.Runner
{
    /// <summary>
    /// Represents an obstacle that ends the game when the player collides with it.
    /// This component requires a Collider component and works in both Play and Edit modes.
    /// The obstacle triggers the game's lose state on collision with the player.
    /// </summary>
    [ExecuteInEditMode]
    [RequireComponent(typeof(Collider))]
    public class Obstacle : Spawnable
    {
        /// <summary>
        /// Tag used to identify the player object
        /// </summary>
        private const string k_PlayerTag = "Player";
        
        /// <summary>
        /// Handles collision with the player to trigger lose state
        /// </summary>
        /// <param name="col">The collider that entered the trigger zone</param>
        private void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag(k_PlayerTag))
            {
                GameManager.Instance.Lose();
            }
        }
    }
}