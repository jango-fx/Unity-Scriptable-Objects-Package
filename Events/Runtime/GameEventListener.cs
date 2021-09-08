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
                GameObject go = GetComponent<GameObject>();
                UnityAction methodDelegate = System.Delegate.CreateDelegate(typeof(UnityAction), go, "SendMessage") as UnityAction;
                UnityEditor.Events.UnityEventTools.AddPersistentListener(response, methodDelegate);
                eventAdded = true;
            }
        }
        */

        void Reset()
        {
            UnityEditor.Events.UnityEventTools.AddVoidPersistentListener(response, null);
            EnableEditMode();
        }

        /*
        void OnValidate()
        {
            if (emitInEditor)
                EnableEditMode();
            else
                DisableEditMode();
        }
        */

        void EnableEditMode()
        {
            response.SetPersistentListenerState(0, UnityEventCallState.EditorAndRuntime);
        }

        void DisableEditMode()
        {
            response.SetPersistentListenerState(0, UnityEventCallState.RuntimeOnly);
        }

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

//! BUG: only works if attached to the same GameObject as the Timeline !?
#if USE_TIMELINE_MARKER
        public void OnNotify(Playable origin, INotification notification, object context)
        {
            GameEventMarker marker = notification as GameEventMarker;
            GameEvent ge = (GameEvent) marker.gameEvent;
            if (marker == null)
                return;

            // Debug.Log(ge);
            ge.Raise();
        }
#endif
    }
}