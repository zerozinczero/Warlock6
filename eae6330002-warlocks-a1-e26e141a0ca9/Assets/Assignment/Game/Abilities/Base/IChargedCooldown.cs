using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChargedCooldown {

	int Charges { get; }
	int MaxCharges { get; }

}
