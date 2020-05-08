using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Trigger : UnityEvent<Collider> {}

public class FinishCollider : MonoBehaviour
{

    public Trigger triggerEnter;
    
    public Trigger triggerExit;

    void Start() {
        if (triggerEnter == null) {
            triggerEnter = new Trigger();
        }
        if (triggerExit == null) {
            triggerExit = new Trigger();
        }

    }

    private void OnTriggerEnter(Collider collider) {
        print("Hello");
        triggerEnter.Invoke(collider);
    }

    private void OnTriggerExit(Collider collider) {
        triggerExit.Invoke(collider);
    }
}
