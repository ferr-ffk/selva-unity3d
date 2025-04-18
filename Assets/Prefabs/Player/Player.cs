using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private MovementComponent _movementComponent;

    [SerializeField]
    private InputActionReference _movementReference;

    [SerializeField]
    private InputActionReference _lookReference;

    [SerializeField]
    private GameObject _camera;

    [SerializeField]
    private GameObject _armature;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Obt�m o vetor 2D de dire��o a partir do input
        Vector2 joystickDirection = _movementReference.action.ReadValue<Vector2>();

        // Cria um vetor 3D para movimentar apenas no x e z (sendo Y a vertical)
        Vector3 direction = new(joystickDirection.x, 0, joystickDirection.y);

        // Utiliza a rota��o que a c�mera est� apontando, e usa para que o jogador se movimente de acordo com ela
        Quaternion cameraRotation = Quaternion.Euler(0, _camera.transform.eulerAngles.y, 0);

        Quaternion desiredRotation = Quaternion.LookRotation(direction);

        // Rotaciona a arma��o do jogador para a dire��o desejada
        //_armature.transform.rotation = Quaternion.Slerp(_armature.transform.rotation, desiredRotation, Time.deltaTime * 8);

        // Move o Jogador
        _movementComponent.Move(gameObject, direction, cameraRotation);
    }
}
