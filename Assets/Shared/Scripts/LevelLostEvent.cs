using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HyperCasual.Core;

namespace HyperCasual.Gameplay
{
    /// <summary>
    /// The event is triggered when the player loses a level.
    /// This event can be used to track level failure status and trigger appropriate game responses.
    /// </summary>
    [CreateAssetMenu(fileName = nameof(LevelLostEvent),
        menuName = "Runner/" + nameof(LevelLostEvent))]
    public class LevelLostEvent : AbstractGameEvent
    {
        /// <summary>
        /// The reason for level failure, if any
        /// </summary>
        [HideInInspector]
        public string FailureReason { get; set; } = string.Empty;

        /// <summary>
        /// Time at which the level was lost in seconds
        /// </summary>
        [HideInInspector]
        public float FailureTime { get; set; }

        public override void Reset()
        {
            FailureReason = string.Empty;
            FailureTime = 0f;
        }
    }
}