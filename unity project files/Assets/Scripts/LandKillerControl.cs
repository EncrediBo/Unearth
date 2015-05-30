using UnityEngine;
using System.Collections;

public class LandKillerControl : Animal {

    // Use this for initialization
    protected override void Start()
    {
        type = 5;
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {

        if (myState == State.Idle || myStateDuration >= 90)
        {
            ChangeState(State.Hunting);
        }

        base.Update();


    }
}
