using UnityEngine;
using System.Collections;

public class PlayerNavigation : MonoBehaviour {


  
    public NavMeshAgent navMeshAgent;
    public Transform target;
    public Transform target2;
    Transform targetTmp;
    public bool IsarrivedTarget = false;

	// Use this for initialization
	void Start () {
        targetTmp = target;
       
        navMeshAgent = GetComponent<NavMeshAgent>();
	}
	

    void OnTriggerEnter(Collider coll)
    {
        if (coll)
        {
           // IsarrivedTarget = !IsarrivedTarget;
            if (IsarrivedTarget)
            {
                targetTmp = target;
                IsarrivedTarget = false;

            } else 
            {
                targetTmp = target2;
                IsarrivedTarget = true;
            }
            
            Debug.Log("ARRIVATO!");
        } 
      
    }

	// Update is called once per frame
	void Update () {

        navMeshAgent.SetDestination(targetTmp.transform.position);

    
    }


}
