﻿
public class Earthquake : ASkill
{
	public Earthquake ()
	{
		Name = "Earthquake";
		Damage = 50.0f;
		Duration = 2f;
		IsActivated = false;
	}

	public override void ActivateCollider (AEnemy enemy)
	{
		
	}
}
