
using System;
using UnityEngine;

public class PickBerryAction : GoapAction
{
		private bool chopped = false;
		private BerryBushComponent targetBerryBush; // where we get the berries

		private float startTime = 0;
		public float workDuration = 2; // seconds

		public PickBerryAction () {
				addPrecondition ("hasBerries", false); // if we have berries we don't want more
				addEffect ("hasBerries", true);
		}


		public override void reset ()
		{
				chopped = false;
				targetBerryBush = null;
				startTime = 0;
		}

		public override bool isDone ()
		{
				return chopped;
		}

		public override bool requiresInRange ()
		{
				return true; // yes we need to be near a berry bush
		}

		public override bool checkProceduralPrecondition (GameObject agent)
		{
				// find the nearest berry bush that we can chop our wood at
				BerryBushComponent[] bushes = (BerryBushComponent[]) UnityEngine.GameObject.FindObjectsOfType ( typeof(BerryBushComponent) );
				BerryBushComponent closest = null;
				float closestDist = 0;

				foreach (BerryBushComponent bush in bushes) {
						if (closest == null) {
								// first one, so choose it for now
								closest = bush;
								closestDist = (bush.gameObject.transform.position - agent.transform.position).magnitude;
						} else {
								// is this one closer than the last?
								float dist = (bush.gameObject.transform.position - agent.transform.position).magnitude;
								if (dist < closestDist) {
										// we found a closer one, use it
										closest = bush;
										closestDist = dist;
								}
						}
				}
				if (closest == null) {
					return false;
				}

				targetBerryBush = closest;
				target = targetBerryBush.gameObject;

				return closest != null;
		}

		public override bool perform (GameObject agent)
		{
				if (startTime == 0)
						startTime = Time.time;

				if (Time.time - startTime > workDuration) {
						// finished chopping
						BackpackComponent backpack = (BackpackComponent)agent.GetComponent(typeof(BackpackComponent));
						backpack.numBerries += 5;
						chopped = true;
				}
				return true;
		}

}