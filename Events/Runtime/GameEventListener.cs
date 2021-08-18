// ----------------------------------------------------------------------------
// based on Ryan Hipple's Unite 2017 - Game Architecture with Scriptable Objects
// ----------------------------------------------------------------------------
using System;
using UnityEngine;
using UnityEngine.Events;
#if USE_TIMELINE_MARKER
using UnityEngine.Playables;
using UnityEngine.Timeline;
using ƒx.UnityUtils.Timeline;
#endif

namespace ƒx.UnityUtils.ScriptableObjects.Events
{
    [ExecuteInEditMode]
    public class GameEventListener : MonoBehaviour
#if USE_TIMELINE_MARKER
                                    , INotificationReceiver
#endif
    {
        //public bool executeInEditMode = false;

        [Tooltip("Event to register with.")]
        public GameEvent gameEvent;

        [Tooltip("Response to invoke when Event is raised.")]
        public UnityEvent response = new UnityEvent();
        // private bool eventAdded = false;

        // TODO: autopopulate on creation
        /*
        private void OnAwake()
        {
            if (!eventAdded)
            {
                GameObject audioSrc = GetComponent<GameObject>();
                UnityAction methodDelegate = System.Delegate.CreateDelegate(typeof(UnityAction), audioSrc, "SendMessage") as UnityAction;
                UnityEditor.Events.UnityEventTools.AddPersistentListener(response, methodDelegate);
                eventAdded = true;
            }
        }
        */
        
        private void OnEnable()
        {
            gameEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            gameEvent.UnregisterListener(this);
        }

        public void OnEventRaised()
        {
            if (gameEvent.verbose)
                Debug.Log("➘| [" + gameEvent.name + "] => " + response.GetPersistentTarget(0) + "." + response.GetPersistentMethodName(0) + "()");
            response.Invoke();
        }

#if USE_TIMELINE_MARKER
        public void OnNotify(Playable origin, INotification notification, object context)
        {
            UnityUtils.TimelineUtilities.GameEventMarker marker = notification as UnityUtils.TimelineUtilities.GameEventMarker;
            GameEvent ge = (GameEvent) marker.gameEvent;
            if (marker == null)
                return;

            // Debug.Log(ge);
            ge.Raise();
        }
#endif
    }
}