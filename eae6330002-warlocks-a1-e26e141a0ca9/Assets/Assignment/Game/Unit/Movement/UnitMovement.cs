using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Unit))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class UnitMovement : MonoBehaviour {

    [SerializeField]
    private Animator animator = null;

    [SerializeField]
    private Rigidbody rb = null;

    [SerializeField]
    private Vector3? moveTarget;
    public bool HasReachedDestination { get { return !moveTarget.HasValue; } }

    [SerializeField] private FloatStat moveSpeed = null;
    public FloatStat MoveSpeed { get { return moveSpeed; } }

    [SerializeField] private BoolStat pushable = null;
    public BoolStat Pushable { get { return pushable; }}

    [SerializeField]
    private float moveTargetTolerance = 0.1f;

    [SerializeField]
    private float turnSpeed = 360f;

    [SerializeField]
    private float maxMoveAngle = 45f;

    [SerializeField]
    private Map map = null;

    [SerializeField]
    private string forwardAnimation = "Forward";

    [SerializeField]
    private string idleAnimation = "Idle";

    [SerializeField]
    private float animationForwardSpeed = 1f;

    [SerializeField]
    private Unit lastForceApplier = null;
    public Unit LastForceApplier { get { return lastForceApplier; } set { lastForceApplier = value; } }

    protected virtual void Update() {
        UpdateRotation();
        UpdateTranslation();
        UpdateMapPosition();
    }

    // this method makes sure the unit is on the ground, not hovering in the air or below the map
    private void UpdateMapPosition() {
        Vector3 mapPos, mapNormal;
        map.GetMapPointFromWorldPoint(rb.position, out mapPos, out mapNormal);
        rb.MovePosition(mapPos);
        Vector3 right = rb.rotation * Vector3.right;
        Vector3 up = mapNormal;
        Vector3 forward = Vector3.Cross(right, up);
        rb.MoveRotation(Quaternion.LookRotation(forward, up));
    }

    private void UpdateRotation() {
        if (moveTarget.HasValue) {
            Vector3 delta = (moveTarget.Value - rb.position);
            Vector3 curDir = rb.rotation * Vector3.forward;
            Vector3 targetDir = delta.normalized;
            float maxTurnPerFrame = turnSpeed * Time.deltaTime;
            float angleLeft = Vector3.Angle(curDir, targetDir);
            float turnPerFrame = Mathf.Min(angleLeft, maxTurnPerFrame);
            Quaternion rotation = Quaternion.RotateTowards(
                rb.rotation,
                Quaternion.LookRotation(targetDir, Vector3.up),
                turnPerFrame
            );
            rb.MoveRotation(rotation);
        }
    }

    private void UpdateTranslation() {
        if (moveTarget.HasValue) {
            Vector3 curDir = rb.rotation * Vector3.forward;
            Vector3 curPos = rb.position;
            Vector2 delta = new Vector2(moveTarget.Value.x - curPos.x, moveTarget.Value.z - curPos.z);
            float dist = delta.magnitude;
            if (dist > moveTargetTolerance) {
                float actualMoveSpeed = Mathf.Clamp(dist * turnSpeed * Mathf.Deg2Rad, 0, moveSpeed.Current);
                Vector3 move = curDir * actualMoveSpeed * Time.deltaTime;
                Vector3 newPos = curPos + move;
                Vector3 mapPos, mapNormal;
                map.GetMapPointFromWorldPoint(newPos, out mapPos, out mapNormal);
                float terrainAngle = Vector3.Angle(Vector3.up, mapNormal);
                if (terrainAngle <= maxMoveAngle) {
                    rb.MovePosition(mapPos);
                    animator.SetFloat(forwardAnimation, animationForwardSpeed);
                }
            } else {
                animator.SetFloat(forwardAnimation, 0);
                moveTarget = null;
                OnDestinationReached();
            }
        }
    }

    protected virtual void OnDestinationReached() {

    }

    public void MoveTo(Vector3 targetPosition) {
        Stop(); // TODO: post event
        moveTarget = targetPosition;
    }

    public void Stop() {
        moveTarget = null;
        animator.SetFloat(forwardAnimation, 0);
    }

    public void Reset(Transform spawnPoint) {
        rb.MovePosition(spawnPoint.position);
        rb.MoveRotation(spawnPoint.rotation);
        rb.velocity = Vector3.zero;
        rb.Sleep();
        animator.SetTrigger(idleAnimation);
        lastForceApplier = null;
    }

    public void AddForce(Unit applier, Vector3 force, ForceMode forceMode = ForceMode.Force) {
        if (pushable.Current) {
            rb.AddForce(force, forceMode);
            lastForceApplier = applier;
        }
    }

}
