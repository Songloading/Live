﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Aniki - The first boss.
/// </summary>
public class Aniki : AEnemy
{
	private bool IsFrontHit1;
	public GameObject EarthquakeAlert;
	public GameObject YunshiAlert;
	private Collider LeftSword;
	private Collider RightSword;
    public GameObject Storm;
	//earthquake red circle
	public Image bossHP;


	public Aniki ()
	{
		Name = "Aniki";
		MaxHP = 1000f;
		CurrentHP = 1000f;
		Speed = 5f;
		CurrentState = State.IDLE;
		closeRange = 8f;
		deadAnimDuration = 1f;
		IsFrontHit1 = true;
		Buff = new EmptyBuff ();

		Skills = new Dictionary<string, List<ASkill>> { {
				"close",
				new List<ASkill> {
					new FrontHit (),
					new FrontHit (),
					new FrontHit (),
					new Earthquake ()
				}
			}
			, {
				"far",
				new List<ASkill> {
					new Yunshi (),
					new EmptySkill (),
					new EmptySkill (),
					new EmptySkill (),
					new EmptySkill ()
				}
			}
		}; 
	}

	protected override void Awake ()
	{
		base.Awake ();
		IsAnimator = false;
		RightSword = GameObject.Find ("Weapon_Blade_Primary").GetComponent<BoxCollider> ();
		LeftSword = GameObject.Find ("Weapon_Blade_Secondary").GetComponent<BoxCollider> ();
//		YunshiAlert = GameObject.Find ("YunshiAlert");
	}

	protected override void Update ()
	{
		base.Update ();
		bossHP.fillAmount = CurrentHP / MaxHP;
		//update bossHP in HUD

	}

	public override void Attack ()
	{
		if (!CurrentSkill.Name.Equals ("Yunshi")) {
			gameObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		}
		//CanDealDamage = true;
		if (CurrentSkill.Name.Equals ("FrontHit")) {
			if (IsFrontHit1) {
				CurrentSkill.ActivateCollider (true, RightSword);
				CurrentSkill.ActivateCollider (false, LeftSword);
				Animation.Play ("FrontHit1");
				CanDealDamage = true;
			} else {
				CurrentSkill.ActivateCollider (true, LeftSword);
				CurrentSkill.ActivateCollider (false, RightSword);
				Animation.Play ("FrontHit2");
				CanDealDamage = true;
			}
			IsFrontHit1 = !IsFrontHit1;
		} else if (CurrentSkill.Name.Equals ("Earthquake") || CurrentSkill.Name.Equals ("Yunshi")) {
			Animation.Play ("Earthquake");
		}
	}

	public override void DecideState ()
	{
		base.DecideState ();
		if (!(CurrentState == State.ATTACK || CurrentState == State.IDLE)) {
			CurrentSkill.ActivateCollider (false, RightSword);
			CurrentSkill.ActivateCollider (false, LeftSword);
		}
		/// Only for activate collider of Earthquake
		if (CurrentSkill.Name.Equals ("Earthquake")) {
			EarthquakeAlert.SetActive (true);//activate the alert
                                             // Mathf.Abs (Time.time - AttackEndTime) <= 0.04f 保证collider在离AttackEndTime左右0.04秒的时间范围内被激活
            if (0.52f <= AttackEndTime - Time.time && AttackEndTime - Time.time <= 0.54f)
            {
                Instantiate(Storm, transform.position, transform.rotation);
            }
                Collider c = gameObject.GetComponent<SphereCollider> ();
			if (0.01f <= AttackEndTime - Time.time && AttackEndTime - Time.time <= 0.06f) {
				CurrentSkill.ActivateCollider (true, c);
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
				CanDealDamage = true;
			} else {
				CurrentSkill.ActivateCollider (false, c);
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
                CanDealDamage = false;
			}
		} else {
			EarthquakeAlert.SetActive (false);
		}

		if (CurrentSkill.Name.Equals ("Yunshi")) {
			YunshiAlert.SetActive (true);//activate the alert
			// Mathf.Abs (Time.time - AttackEndTime) <= 0.04f 保证collider在离AttackEndTime左右0.04秒的时间范围内被激活
			Collider c = YunshiAlert.GetComponent<SphereCollider> ();
            if (0.52f <= AttackEndTime - Time.time && AttackEndTime - Time.time <= 0.54f)
            {
                Instantiate(Storm, transform.position, transform.rotation);
            }
            if (0.01f <= AttackEndTime - Time.time && AttackEndTime - Time.time <= 0.06f) {
				CurrentSkill.ActivateCollider (true, c);
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                CanDealDamage = true;
			} else {
				CurrentSkill.ActivateCollider (false, c);
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
                CanDealDamage = false;
			}
		} else {
			YunshiAlert.SetActive (false);
		}
	}

	public override void Die ()
	{
        EnemyMove(0f);
		if (!IsDead) {
			Animation.Play ("Die");
			IsDead = true;
			deadAnimDuration += Time.time; //update deadAniDuration to deadAnimEndTime
		}

		if (Time.time >= deadAnimDuration) {
            CurrentHP = -1f;
		}
	}
}
