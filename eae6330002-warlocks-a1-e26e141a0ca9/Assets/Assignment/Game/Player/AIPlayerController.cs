using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIPlayerController : PlayerController {

    // coming in a future assignment

    [SerializeField]
    private float islandAreaReduction = 0.9f;

    protected override void Start() {
        base.Start();
        StartCoroutine(MoveAround());
    }

    IEnumerator MoveAround() {
        Unit unit = Units[0] as Unit;

        while (true) {

            if(unit == null || unit.Health.IsDead) {
                yield return null;
                continue;
            }

            Unit target = game.Players.Find(p => p.IsLocal).GetComponent<PlayerController>().Units[0] as Unit;
            if(target == null) {
                yield return null;
                continue;
            }

            Ability ability = unit.Abilities[0];

            Vector3 targetPoint = target.transform.position;
            Vector3 targetDelta = targetPoint - unit.transform.position;
            float dist = targetDelta.magnitude;
            Vector3 targetDir = targetDelta * (1f/dist);


            if (dist > 10f) {
                Vector3 moveToPoint = (target.transform.position + unit.transform.position) * 0.5f;
                unit.Controller.MoveTo(moveToPoint);
                yield return new WaitUntil(() => unit.Movement.HasReachedDestination);

                targetPoint = target.transform.position;
                targetDelta = targetPoint - unit.transform.position;
                dist = targetDelta.magnitude;
                targetDir = targetDelta * (1f / dist);
            }

            string msg = null;
            if (!ability.CanCast(out msg)) {
                yield return null;
                continue;
            }

            PointTargeting pointTargeting = ability.GetComponent<PointTargeting>();
            if(pointTargeting != null) {
                pointTargeting.Target = targetPoint;
            }

            ability.Cast();

            //if (unit != null && 
            //    unit.Health.IsAlive &&
            //    unit.Controller.HasReachedDestination) {
            //    Vector3 target = game.Map.Island.GetRandomPosition();
            //    target.x *= islandAreaReduction;
            //    target.z *= islandAreaReduction;
            //    unit.Controller.MoveTo(target);
            //}

            yield return new WaitForSeconds(5f);
        }
    }

}
