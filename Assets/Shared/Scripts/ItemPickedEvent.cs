using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HyperCasual.Core;

namespace HyperCasual.Gameplay
{
    /// <summary>
    /// The event is triggered when the player picks up an item (like coin and keys).
    /// Tracks the count of items picked up in a single event.
    /// </summary>
    [CreateAssetMenu(fileName = nameof(ItemPickedEvent),
        menuName = "Runner/" + nameof(ItemPickedEvent))]
    public class ItemPickedEvent : AbstractGameEvent
    {
        /// <summary>
        /// The number of items picked up. A value of -1 indicates no items were picked up.
        /// </summary>
        [HideInInspector]
        public int Count { get; set; } = -1;
        
        public override void Reset()
        {
            Count = -1;
        }
    }    
}
