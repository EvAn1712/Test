using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneSupreme : ArmyElement, IShoot
{
    [SerializeField] GameObject m_MissilePrefab;
    [SerializeField] Transform[] m_MissileSpawnPos;
    NavMeshAgent m_NavMeshAgent;

    Transform m_Transform;

    public float speed = 5.0f;
    public float rotationSpeed = 200.0f;
    public float shootCooldown = 1.0f;
    private float nextShootTime = 0f;
    public int rafale = 3;

    private void Awake()
    {
        m_Transform = transform;
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 forwardMovement = m_Transform.forward * moveVertical * speed * Time.deltaTime;
        m_Transform.Translate(forwardMovement, Space.World);

        // Rotation gauche/droite
        float moveHorizontal = Input.GetAxis("Horizontal");
        m_Transform.Rotate(Vector3.up, moveHorizontal * rotationSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= nextShootTime)
        {
            DroneSupreme drone = GetComponent<DroneSupreme>();
            if (drone != null)
            {
                if (rafale > 0)
                {
                    drone.Shoot();
                    rafale--;

                    if (rafale == 0)
                    {
                        nextShootTime = Time.time + shootCooldown;
                        rafale = 3;
                    }
                    else
                    {
                        nextShootTime = Time.time; // Pas de cooldown entre les tirs de la rafale
                    }
                }
            }
        }
    }

    public void Shoot()
    {
        for (int i = 0; i < m_MissileSpawnPos.Length; i++)
        {
            Transform missileSpawnPos = m_MissileSpawnPos[i];
            GameObject newMissileGO = Instantiate(m_MissilePrefab, missileSpawnPos.position, Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized));
            newMissileGO.tag = gameObject.tag;
            Missile missile = newMissileGO.GetComponent<Missile>();
            missile.SetStartSpeed(m_NavMeshAgent.speed);
        }
    }

    public void Die()
    {
        ArmyManager.ArmyElementHasBeenKilled(gameObject);
        Destroy(gameObject);

    }
}
