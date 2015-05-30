﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SeaManControl : Animal
{
    // Use this for initialization
    protected override void Start()
    {
        type = 2;
        base.Start();
    }
    // Use this for initialization
    protected override void Update()
    {
        if (myStateDuration >= 90)
        {
            ChangeState(State.Hunting);
        }

        base.Update();
    }
}
