using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMovement : MonoBehaviour {
    public enum Axis {//The axis the block should lock onto, if the block can only go up and down, lock it onto the Z-axis
        Z_Axis,
        X_Axis
    }

    public enum Math_Calc {
        Round,
        Ceil,
        Floor
    }

    [Tooltip("On axis is the object allow to move on")]
    public Axis movement;

    [Tooltip("Should the block lock onto whole numbers")]
    public bool lockOnToGrid = true;

    private Vector3 previousPosition;//When the user first clicks on the block
    private Vector3 initPosition;

    public float lockOffset = .5f;
    public Math_Calc calculation = Math_Calc.Floor;

    public bool IsDragging { get; private set; }
    private Vector3 change = new Vector3();
    private Vector3 location = new Vector3();

    public int moves;//how many times this block has moved
    
    void LockOnToGrid() {
        Vector3 position = transform.position;
        if (movement == Axis.X_Axis) {
            if (calculation == Math_Calc.Round) {
                position.x = Mathf.RoundToInt(position.x) + lockOffset;
            }
            if (calculation == Math_Calc.Ceil) {
                position.x = Mathf.CeilToInt(position.x) + lockOffset;
            }
            if (calculation == Math_Calc.Floor) {
                position.x = Mathf.FloorToInt(position.x) + lockOffset;
            }
        }
        if (movement == Axis.Z_Axis) {
            if (calculation == Math_Calc.Round) {
                position.z = Mathf.RoundToInt(position.z) + lockOffset;
            }
            if (calculation == Math_Calc.Ceil) {
                position.z = Mathf.CeilToInt(position.z) + lockOffset;
            }
            if (calculation == Math_Calc.Floor) {
                position.z = Mathf.FloorToInt(position.z) + lockOffset;
            }
            //position.z = Mathf.FloorToInt(position.z) + .5f;
        }
        transform.position = position;
        CheckMoves();
    }

    void CheckMoves() {
        if (movement == Axis.X_Axis) {
            if (initPosition.x != transform.position.x) {
                moves++;
            }
        }
        if (movement == Axis.Z_Axis) {
            if (initPosition.z != transform.position.z) {
                moves++;
            }
        }
    }

    void ResolveCollision() {
        if (IsDragging) {
            Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector3 origin = transform.position;
            Vector3 direction = -change.normalized;
            float distance = (location - transform.position).magnitude;
            float offset = 0;

            if (movement == Axis.X_Axis) {
                offset = GetComponent<BoxCollider>().bounds.size.x / 2f;
            }
            else if (movement == Axis.Z_Axis) {
                offset = GetComponent<BoxCollider>().bounds.size.z / 2f;
            }

            RaycastHit hit;

            distance += offset;
            Debug.DrawRay(origin, direction * distance);

            if (Physics.Raycast(origin, direction, out hit, distance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore)) {
                Vector3 hitPoint = hit.point;

                if (movement == Axis.X_Axis) {
                    float floatDirection = Vector3.SignedAngle(origin, direction, Vector3.up) <= 0 ? -1 : 1;
                    hitPoint.x += (direction.magnitude * offset * floatDirection);
                }
                else if (movement == Axis.Z_Axis) {
                    float floatDirection = Vector3.SignedAngle(origin, direction, Vector3.right) <= 0 ? 1 : -1;
                    hitPoint.z += (direction.magnitude * offset * floatDirection);
                }
                transform.position = hitPoint;
            }
            else {
                transform.position = location;
            }
            previousPosition = currentMousePosition;
        }
    }

    private void OnMouseDown() {
        initPosition = transform.position;
        previousPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseDrag() {
        IsDragging = true;
        Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (movement == Axis.X_Axis) {
            change.x = (previousPosition.x - currentMousePosition.x);
            location.x = transform.position.x - (previousPosition.x - currentMousePosition.x);
            location.y = transform.position.y;
            location.z = transform.position.z;
        }
        else if (movement == Axis.Z_Axis) {
            location.x = transform.position.x;
            location.y = transform.position.y;
            location.z = transform.position.z - (previousPosition.z - currentMousePosition.z);

            change.z = (previousPosition.z - currentMousePosition.z);
        }
        ResolveCollision();
    }

    private void OnMouseUp() {
        if (lockOnToGrid) {
            LockOnToGrid();
        }
        else {
            CheckMoves();
        }
        IsDragging = false;
    }
}
