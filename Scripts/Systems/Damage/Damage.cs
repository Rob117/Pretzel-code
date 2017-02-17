using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Team;

public class Damage {
    public int amount;
    public DamageTypes type;
    public bool criticalHit;

    public Damage(int amount, DamageTypes type, bool isCrit) {
        this.amount = amount;
        this.type = type;
        this.criticalHit = isCrit;
    }
}

public class DamagePackage {
    public List<Damage> allDamage = new List<Damage>();
    public Teams sourceTeam;
    public GameObject sourceObject;
    public bool canDealFriendlyFire = false;

    public DamagePackage(Teams source, GameObject sourceObject, bool friendlyFire = false) {
        sourceTeam = source;
        this.sourceObject = sourceObject;
        this.canDealFriendlyFire = friendlyFire;
    }

    public void AddDamage(Damage damage) {
        allDamage.Add(damage);
    }
}

/// <summary>
/// The type of damage that an attack can be of.
/// </summary>
public enum DamageTypes {normal, fire, lightning, ice};