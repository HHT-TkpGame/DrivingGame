using UnityEngine;

public class CourseShaderMover : MonoBehaviour
{
    [SerializeField] Material groundMat;
	// Update is called once per frame

	bool isRainy;
	public void Start()
	{
		isRainy = true;
	}
	void Update()
    {
		if (isRainy)
		{
			groundMat.SetVector("PlayerPos", transform.position);
		}
    }
}
