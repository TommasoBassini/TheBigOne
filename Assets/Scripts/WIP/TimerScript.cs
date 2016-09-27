using UnityEngine;
using System.Collections;

public delegate void TimedDelegatedMethod (TimerScript timerScriptReference, float changeAmount);

public abstract class TimerScript : MonoBehaviour {
	/*
	#region TIMER_DELEGATES
	public TimedDelegatedMethod[] DelegatedMethod = new TimedDelegatedMethod[] {

		delegate (TimerScript timerScriptReference, float changeAmount) {

			if (timerScriptReference is OxygenScript) {

				(timerScriptReference as OxygenScript).OxygenAmount -= changeAmount;

			} else if (timerScriptReference is BatteryScript) {

				(timerScriptReference as BatteryScript).BatteryEnergyAmount -= changeAmount;

			} else Debug.LogError ("ERRORE RICONOSCIMENTO TIPO SCRIPT, DELEGATO 0, TIMER");
			
		},

		delegate (TimerScript timerScriptReference, float changeAmount) {

			if (timerScriptReference is OxygenScript) {

				(timerScriptReference as OxygenScript).OxygenAmount += changeAmount;

			} else if (timerScriptReference is BatteryScript) {

				(timerScriptReference as BatteryScript).BatteryEnergyAmount += changeAmount;

			} else Debug.LogError ("ERRORE RICONOSCIMENTO TIPO SCRIPT, DELEGATO 1, TIMER");
			
		}

	};
	#endregion
	*/

	#region TIMER_COROUTINES
	public IEnumerator CO_TimerCoroutine (float characteristicChangeSpeed, float characteristicChangeAmount, TimedDelegatedMethod DelegatedMethod) {

		while (true) {

			yield return new WaitForSeconds (characteristicChangeSpeed);
			DelegatedMethod (this, characteristicChangeAmount);

		}

	}

	public IEnumerator CO_TimerCoroutine (int characteristicSteps, float characteristicChangeSpeed, float characteristicChangeAmount, TimedDelegatedMethod DelegatedMethod) {

		do {

			yield return new WaitForSeconds (characteristicChangeSpeed);
			DelegatedMethod (this, characteristicChangeAmount);

		} while (--characteristicSteps > 0);

	}
	#endregion

}