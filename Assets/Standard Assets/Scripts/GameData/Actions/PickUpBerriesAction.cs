
using System;
using UnityEngine;

public class PickUpBerriesAction : GoapAction
{
		private bool hasBerries = false;
		private SupplyPileComponent targetSupplyPile; // where we get the Berries from

		public PickUpBerriesAction () {
				addPrecondition ("hasBerries", false); // don't get a Berries if we already have one
				addEffect ("hasBerries", true); // we now have a Berries
		}


		public override void reset ()
		{
				hasBerries = false;
				targetSupplyPile = null;
		}

		public override bool isDone ()
		{
				return hasBerries;
		}

		public override bool requiresInRange ()
		{
				return true; // yes we need to be near a supply pile so we can pick up the Berries
		}

		public override bool checkProceduralPrecondition (GameObject agent)
		{
				// find the nearest supply pile that has spare Berries
				SupplyPileComponent[] supplyPiles = (SupplyPileComponent[]) UnityEngine.GameObject.FindObjectsOfType ( typeof(SupplyPileComponent) );
				SupplyPileComponent closest = null;
				float closestDist = 0;

				foreach (SupplyPileComponent supply in supplyPiles) {
						if (supply.numBerries > 0) {
								if (closest == null) {
										// first one, so choose it for now
										closest = supply;
										closestDist = (supply.gameObject.transform.position - agent.transform.position).magnitude;
								} else {
										// is this one closer than the last?
										float dist = (supply.gameObject.transform.position - agent.transform.position).magnitude;
										if (dist < closestDist) {
												// we found a closer one, use it
												closest = supply;
												closestDist = dist;
										}
								}
						}
				}
				if (closest == null)
						return false;

				targetSupplyPile = closest;
				target = targetSupplyPile.gameObject;

				return closest != null;
		}

		public override bool perform (GameObject agent)
		{
				if (targetSupplyPile.numBerries > 0) {
						targetSupplyPile.numBerries -= 1;
						hasBerries = true;
						//TODO play effect, change actor icon
						BackpackComponent backpack = (BackpackComponent)agent.GetComponent(typeof(BackpackComponent));
						backpack.numBerries = 1;

						return true;
				} else {
						// we got there but there was no Berries available! Someone got there first. Cannot perform action
						return false;
				}
		}
}
