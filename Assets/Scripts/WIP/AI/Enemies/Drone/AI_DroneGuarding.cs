using UnityEngine;
using System.Collections;

public class AI_DroneGuarding : MonoBehaviour, IAI_ImplementedStrategy {

    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;
    public bool isPathRandomized = false;
    public float fieldOfViewAngle = 110f;               // Number of degrees, centred on forward, for the enemy see.
    public bool playerInSight;							// Whether or not the player is currently sighted.

    public void Awake()
    {
        this.agent = this.GetComponent<NavMeshAgent>();
        this.agent.autoBraking = false;
    }

     public void GotoNextPointOrdered()
    {
        // Returns if no points have been set up
        if (points.Length == 0)
        {
            Debug.LogWarning("WARNING! " + this.ToString() + " Has the transform's array leght equal to zero");
            return;
        }

        // Set the agent to go to the currently selected destination.
        this.agent.destination = this.points[this.destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        this.destPoint = (this.destPoint + 1) % this.points.Length;
    }

    public void GotoNextPointRandomized()
    {
        // Returns if no points have been set up
        if (points.Length == 0)
        {
            Debug.LogWarning("WARNING! " + this.ToString() + " Has the transform's array leght equal to zero");
            return;
        }

        // Set the agent to go to the currently selected destination.
        this.agent.destination = this.points[this.destPoint].position;

        // Choose the next point in the array as the destination,
        // randomly.
        this.destPoint = Random.Range(0, this.points.Length);  
    }
    
    #region IMPLEMENTED_STRATEGY_METHOD
    public StrategyState ExecuteImplementedStrategy () {

		Debug.Log ("Drone is in <<Guarding>>");


        if (this.agent.remainingDistance < 0.5f)
        {

            if (this.isPathRandomized)
                {
                    this.GotoNextPointRandomized();
                }
            else
                {
                    this.GotoNextPointOrdered();
                }

        }


		if (Input.GetKeyDown (KeyCode.A)) {

			Debug.Log ("Drone switches from <<Guarding>> to <<Defending>>");
			return StrategyState.Defending;

		} else {

			Debug.Log ("Drone does not change strategy");
			return StrategyState.NoStrategyChanging;

		}

	}
	#endregion

}