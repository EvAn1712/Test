using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ArmyElement : MonoBehaviour, IArmyElement
{
	public ArmyManager ArmyManager { get; set; }
	[SerializeField] Health m_Health;
	public float Health { get => m_Health.Value; }

	public void Die()
	{
		ArmyManager.ArmyElementHasBeenKilled(gameObject);
		Destroy(gameObject);
	}

	public void AddHealth(float healthToAdd)
{
    m_Health.Value += healthToAdd;
    Debug.Log($"{gameObject.name} a maintenant {m_Health.Value} points de vie.");
}
}
