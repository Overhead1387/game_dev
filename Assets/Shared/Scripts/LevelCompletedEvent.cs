using System.Collections;
using System.Collections.Generic;
using HyperCasual.Core;
using UnityEngine;

namespace HyperCasual.Gameplay
{
    /// <summary>
    /// The event is triggered when the player completes a level.
    /// This event can be used to track level completion status and trigger level progression.
    /// </summary>
    [CreateAssetMenu(fileName = nameof(LevelCompletedEvent),
        menuName = "Runner/" + nameof(LevelCompletedEvent))]
    public class LevelCompletedEvent : AbstractGameEvent
    {
        /// <summary>
        /// Time taken to complete the level in seconds
        /// </summary>
        [HideInInspector]
        public float CompletionTime { get; set; }

        /// <summary>
        /// Resets the completion time when the event is reset
        /// </summary>
        public override void Reset()
        {
            CompletionTime = 0f;
        }
    }
}