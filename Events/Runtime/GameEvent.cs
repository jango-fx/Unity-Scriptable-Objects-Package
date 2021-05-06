// ----------------------------------------------------------------------------
// based on Ryan Hipple's Unite 2017 - Game Architecture with Scriptable Objects
// ----------------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace ArtCom.ScriptableObjects.Events
{
    [CreateAssetMenu(menuName = "ScriptableObjects/GameEvent")]
    public class GameEvent : ScriptableObject
    {
        /// <summary>
        /// The list of listeners that this event will notify if it is raised.
        /// </summary>
        private readonly List<GameEventListener> eventListeners = new List<GameEventListener>();
        public bool verbose = true; // TODO: make it work

        public void Raise()
        {
            if (verbose)
                Debug.Log("➚ [" + this.name + "]");

            for (int i = eventListeners.Count - 1; i >= 0; i--)
                eventListeners[i].OnEventRaised();
        }

        public void RegisterListener(GameEventListener listener)
        {
            if (!eventListeners.Contains(listener))
                eventListeners.Add(listener);
        }

        public void UnregisterListener(GameEventListener listener)
        {
            if (eventListeners.Contains(listener))
                eventListeners.Remove(listener);
        }
    }
}