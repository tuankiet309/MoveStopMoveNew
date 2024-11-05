//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PlayerController : MonoBehaviour
//{
//    [SerializeField] protected DetectionCircle attackCircle;
//    [SerializeField] protected Transform throwLocation;
//    [SerializeField] protected GameObject targetCircleInstance;
//    [SerializeField] protected WeaponComponent weaponComponent;
//    [SerializeField] Transform weaponHolder;

//    protected HashSet<GameObject> enemyAttackers = new HashSet<GameObject>();
//    protected GameObject targetToAttack = null;
//    protected Vector3 targetToAttackPos = Vector3.zero;
//    protected float distanceBuff = 0;
//    protected Weapon weapon;
//    protected Projectile weaponToThrow;
//    protected GameObject weaponOnHand;
//    private bool isUlti = false;

//    public void OnEnable()
//    {
        
//    }
//    public void OnDisable()
//    {
        
//    }
//    public void Start()
//    {
        
//    }
//    public void Update()
//    {

//    }
//    public void LateUpdate()
//    {
        
//    }
//    public void FixedUpdate()
//    {
          
//    }
//    protected virtual void UpdateEnemyList(GameObject target, bool isInCircle)
//    {
//        if (isInCircle && IsTargetAlive(target))
//        {
//            if (!enemyAttackers.Contains(target))
//            {
//                enemyAttackers.Add(target);
//            }
//            //onHaveTarget?.Invoke(target);
//        }
//        else
//        {
//            enemyAttackers.Remove(target);
//            if (enemyAttackers.Count == 0)
//            //onHaveTarget?.Invoke(null);
//        }
//    }
//    protected virtual bool IsTargetAlive(GameObject target)
//    {
//        LifeComponent deadController = target.GetComponent<LifeComponent>();
//        return deadController != null && !deadController.IsDead;
//    }

//}
