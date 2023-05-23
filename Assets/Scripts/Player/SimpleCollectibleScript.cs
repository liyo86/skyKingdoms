using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class SimpleCollectibleScript : MonoBehaviour {

	public enum CollectibleTypes {NoType, GemBlue, GemPurple, GemRed, Type4, Type5};

	public CollectibleTypes CollectibleType; // this gameObject's type

	public bool rotate; // do you want it to rotate?

	public float rotationSpeed;

	public AudioClip collectSound;

	public GameObject collectEffect;

	// Update is called once per frame
	void Update () {

		if (rotate)
			transform.Rotate (Vector3.up * rotationSpeed * Time.deltaTime, Space.World);

	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" || other.tag == "Dragon") {
			Collect ();
		} else if (other.tag == "Enemy")
		{
			MyGameManager.Instance.GameOver();
		}
	}

	public void Collect()
	{
		if(collectSound)
			AudioSource.PlayClipAtPoint(collectSound, transform.position);
		if(collectEffect)
			Instantiate(collectEffect, transform.position, Quaternion.identity);

		MyGameManager.Instance.CollectGem(this.CollectibleType);

		Destroy (gameObject);
	}
}
