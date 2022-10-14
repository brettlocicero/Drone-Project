using UnityEngine;
using System.Collections;

public class Sway : MonoBehaviour 
{
	[SerializeField] float amount = 0.02f;
	[SerializeField] float maxamount = 0.03f;
	[SerializeField] float smooth = 3f;
	[SerializeField] float walkTiltScale = 10f;

	Quaternion def;
	bool paused = false;

	void Start ()
	{
		def = transform.localRotation;
	}

	void Update ()
	{
		float factorX = (Input.GetAxis("Mouse Y")) * amount;
		float factorY = -(Input.GetAxis("Mouse X")) * amount;
		float factorZ = 0 * amount;

		if (!paused)
		{
			if (factorX > maxamount)
				factorX = maxamount;
			if (factorX < -maxamount)
				factorX = -maxamount;
			if (factorY > maxamount)
				factorY = maxamount;
			if (factorY < -maxamount)
				factorY = -maxamount;
			if (factorZ > maxamount)
				factorZ = maxamount;
			if (factorZ < -maxamount)
				factorZ = -maxamount;

			Vector3 input = new Vector3(Input.GetAxisRaw("Vertical") / 2, 0, -Input.GetAxisRaw("Horizontal")) * walkTiltScale;
			Quaternion res = Quaternion.Euler(def.x + factorX + input.x, def.y + factorY + input.y, def.z + factorZ + input.z);
			transform.localRotation = Quaternion.Slerp(transform.localRotation, res, (Time.deltaTime * smooth));
		}
	}
}