
using System;
using UnityEngine;

public class DropOffBerriesAction: GoapAction
{
		private bool droppedOffBerries = false;
		private SupplyPileComponent targetSupplyPile; // where we drop off the Berries

		public DropOffBerriesAction () {
				addPrecondition ("hasBerries", true); // can't drop off Berries if we don't already have some
				addEffect ("hasBerries", false); // we now have no Berries
				addEffect ("collectBerries", true); // we collected Berries
		}


		public override void reset ()
		{
				droppedOffBerries = false;
				targetSupplyPile = null;
		}

		public override bool isDone ()
		{
				return droppedOffBerries;
		}

		public override bool requiresInRange ()
		{
				return true; // yes we need to be near a supply pile so we can drop off the Berries
		}

		public override bool checkProceduralPrecondition (GameObject agent)
		{
				// find the nearest supply pile
				SupplyPileComponent[] supplyPiles = (SupplyPileComponent[]) UnityEngine.GameObject.FindObjectsOfType ( typeof(SupplyPileComponent) );
				SupplyPileComponent closest = null;
				float closestDist = 0;

				foreach (SupplyPileComponent supply in supplyPiles) {
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
				if (closest == null)
						return false;

				targetSupplyPile = closest;
				target = targetSupplyPile.gameObject;

				return closest != null;
		}

		public override bool perform (GameObject agent)
		{
				BackpackComponent backpack = (BackpackComponent)agent.GetComponent(typeof(BackpackComponent));
				targetSupplyPile.numBerries += backpack.numBerries;
				droppedOffBerries = true;
				backpack.numBerries = 0;

				return true;
		}
}