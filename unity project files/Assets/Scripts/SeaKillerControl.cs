using UnityEngine;
using System.Collections;

public class SeaKillerControl : Animal {

	// Use this for initialization
	protected override void Start () {
        type = 6;
        base.Start();
	}
	
	// Update is called once per frame
	protected override void Update () {

        if (myState == State.Idle || myStateDuration >= 90)
        {
            ChangeState(State.Hunting);
        }

        base.Update();


	}
}
