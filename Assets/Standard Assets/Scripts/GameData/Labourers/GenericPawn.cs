﻿/**/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenericPawn : MonoBehaviour, IGoap
{
		public TaskMaster master;
		public BackpackComponent backpack;
		public float moveSpeed = 1;

		void Start ()
		{
				if (backpack == null)
						backpack = gameObject.AddComponent <BackpackComponent>( ) as BackpackComponent;
				if (backpack.tool == null) {
						GameObject prefab = Resources.Load<GameObject> (backpack.toolType);
						GameObject tool = Instantiate (prefab, transform.position, transform.rotation) as GameObject;
						backpack.tool = tool;
						tool.transform.parent = transform; // attach the tool
				}
		}


		void Update ()
		{

		}

		/**
	 * Key-Value data that will feed the GOAP actions and system while planning.
	 */
		public HashSet<KeyValuePair<string,object>> getWorldState () {
				HashSet<KeyValuePair<string,object>> worldData = new HashSet<KeyValuePair<string,object>> ();

				worldData.Add(new KeyValuePair<string, object>("hasOre", (backpack.numOre > 0) ));
				worldData.Add(new KeyValuePair<string, object>("hasLogs", (backpack.numLogs > 0) ));
				worldData.Add(new KeyValuePair<string, object>("hasFirewood", (backpack.numFirewood > 0) ));
				worldData.Add(new KeyValuePair<string, object>("hasTool", (backpack.tool != null) ));
				worldData.Add(new KeyValuePair<string, object>("hasBerries", (backpack.numBerries > 0) ));

				return worldData;
		}
				
		public HashSet<KeyValuePair<string,object>> createGoalState ()
		{

		}
				
		public void planFailed (HashSet<KeyValuePair<string, object>> failedGoal)
		{
				// Not handling this here since we are making sure our goals will always succeed.
				// But normally you want to make sure the world state has changed before running
				// the same goal again, or else it will just fail.
				Debug.Log("<color=orange>Plan Failed</color> " + GoapAgent.prettyPrint(failedGoal));
		}

		public void planFound (HashSet<KeyValuePair<string, object>> goal, Queue<GoapAction> actions)
		{
				// Yay we found a plan for our goal
				Debug.Log ("<color=green>Plan found</color> "+GoapAgent.prettyPrint(actions));
		}

		public void actionsFinished ()
		{
				// Everything is done, we completed our actions for this gool. Hooray!
				Debug.Log ("<color=blue>Actions completed</color>");
		}

		public void planAborted (GoapAction aborter)
		{
				// An action bailed out of the plan. State has been reset to plan again.
				// Take note of what happened and make sure if you run the same goal again
				// that it can succeed.
				Debug.Log ("<color=red>Plan Aborted</color> "+GoapAgent.prettyPrint(aborter));
		}

		public bool moveAgent(GoapAction nextAction) {
				// move towards the NextAction's target
				float step = moveSpeed * Time.deltaTime;
				gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, nextAction.target.transform.position, step);

				if (gameObject.transform.position.Equals(nextAction.target.transform.position) ) {
						// we are at the target location, we are done
						nextAction.setInRange(true);
						return true;
				} else
						return false;
		}
}

