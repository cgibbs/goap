using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BerryPicker : Labourer
{
	/**
	 * Our only goal will ever be to chop trees.
	 * The PickBerryAction will be able to fulfill this goal.
	 */
	public override HashSet<KeyValuePair<string,object>> createGoalState () {
		HashSet<KeyValuePair<string,object>> goal = new HashSet<KeyValuePair<string,object>> ();

		goal.Add(new KeyValuePair<string, object>("collectBerries", true ));
		return goal;
	}

}
