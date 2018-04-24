﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoungKid : AEnemy
{

	// Need a field for Buff

	public YoungKid ()
	{
		Name = "YoungKid";
		MaxHP = 100f;
		Speed = 4f;
		CurrentState = State.IDLE;
		closeRange = 2f;
		Skills = new Dictionary<string, List<ASkill>> {
			{ "close", new List<ASkill>{ new Stab ()  } } 
		};
	}

	protected override void Die ()
	{
		base.Die ();
		if (Time.time >= deadAnimDuration) {
			Destroy (this.gameObject);
		}
	}
}
