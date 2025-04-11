using UnityEngine;

public class Teste : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        const float V = 1000f;


        if (Input.GetKeyDown(KeyCode.Space)) {
            GetComponent<Rigidbody>().AddForce(Vector3.forward * V * Time.deltaTime, ForceMode.Impulse);
            GetComponent<Rigidbody>().AddForce(Vector3.up * V * Time.deltaTime, ForceMode.Impulse);
        }

        Debug.DrawLine(Vector3.zero, Vector3.forward * V, Color.red);
        Debug.DrawLine(Vector3.zero, Vector3.up * V, Color.red);
    }
}
