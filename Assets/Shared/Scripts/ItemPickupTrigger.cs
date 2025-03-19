using UnityEngine;
using HyperCasual.Core;
using HyperCasual.Runner;

namespace HyperCasual.Gameplay
{
    /// <summary>
    /// Fires an ItemPickedEvent when the player enters the trigger collider attached to this game object.
    /// </summary>
    public class ItemPickupTrigger : Spawnable
    {
        [SerializeField, Tooltip("Tag used to identify the player")]
        private string m_PlayerTag = "Player";
        
        [SerializeField, Tooltip("Event to raise when item is picked up")]
        private ItemPickedEvent m_Event;
        
        [SerializeField, Tooltip("Number of items stored in this pickup")]
        private int m_Count = 1;

        private void OnValidate()
        {
            if (m_Count < 1)
                m_Count = 1;
        }
        
        private void OnTriggerEnter(Collider col)
        {
            if (m_Event == null || !col.CompareTag(m_PlayerTag)) 
                return;
            
            m_Event.Count = m_Count;
            m_Event.Raise();
            gameObject.SetActive(false);
        }
    }
}