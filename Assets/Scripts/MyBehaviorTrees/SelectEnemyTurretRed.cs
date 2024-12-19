using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Linq;
using System.Collections.Generic;

[TaskCategory("MyTasks")]
[TaskDescription("Select enemy turret from left to right")]

public class SelectEnemyTurretRed : Action
{
    IArmyElement m_ArmyElement;
    public SharedTransform target;
    public SharedFloat minRadius;
    public SharedFloat maxRadius;

    public override void OnAwake()
    {
        m_ArmyElement = (IArmyElement)GetComponent(typeof(IArmyElement));
    }

    public override TaskStatus OnUpdate()
    {
        if (m_ArmyElement.ArmyManager == null) return TaskStatus.Running;

        // Récupère toutes les tourelles ennemies, sans tri aléatoire (false)
        List<Turret> allTurrets = m_ArmyElement.ArmyManager.GetAllEnemiesOfType<Turret>(false);

        // Filtre les tourelles dans le rayon spécifié
        var turretsInRange = allTurrets
            .Where(t => 
            {
                float distance = Vector3.Distance(transform.position, t.transform.position);
                return distance >= minRadius.Value && distance <= maxRadius.Value;
            })
            .ToList();

        // Sélectionne la tourelle la plus à gauche par rapport à la position de la caméra
        var leftMostTurret = turretsInRange
            .OrderBy(t => Vector3.Distance(t.transform.position, Camera.main.transform.position))
            .FirstOrDefault();

        if (leftMostTurret != null)
        {
            target.Value = leftMostTurret.transform;
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
}
