using System;
using UnityEngine;
using UnityEngine.UIElements;

public class MovementComponent : MonoBehaviour
{

    [SerializeField, Tooltip("The maximum velocity the object can achieve.")]
    private float _targetVelocity;

    [SerializeField, Tooltip("Acceleration for given object. Defaults to 1")]
    private float _acceleration = 1.0f;

    [SerializeField, Tooltip("The layer mask belonging to the ground.")]
    private LayerMask _groundLayer;

    [SerializeField, Tooltip("The character's height.")]
    private float _height = 1f;

    [NonSerialized]
    public float timeWalking;

    private Rigidbody _rigidbody;
    private CharacterController _characterController;

    [NonSerialized]
    public Vector3 currentVelocity = Vector3.zero;

    private bool isPlayer;

    private void Start()
    {
        // Congela a rotação do objeto para que ele não caia rs
        _rigidbody = GetComponent<Rigidbody>();
        if (_rigidbody == null)
        {
            Debug.LogError("Rigidbody não encontrado!");
            return;
        } else
        {
            _rigidbody.freezeRotation = true;
        }

        // Pega o CharacterController do objeto, caso exista
        isPlayer = TryGetComponent<CharacterController>(out _characterController);
    }

    /// <summary>
    /// Movimenta o objeto passado em uma direção dada.
    /// </summary>
    /// <param name="objeto">O GameObject a ser movimentado</param>
    /// <param name="direcao">O Vector3 representando a direção</param>
    /// <param name="forward">O vetor representando a frente do movimento, útil caso o componente seja do jogador, e sua rotação muda constantemente por conta da câmera</param>
    public void Move(GameObject objeto, Vector3 direcao, Quaternion forward = default)
    {
        if (objeto == null)
        {
            Debug.LogError("Objeto não pode ser nulo!");
            return;
        }

        if (direcao == Vector3.zero)
        {
            // Reseta o tempo de andar
            timeWalking = 0;

            currentVelocity = Vector3.zero;

            return;
        }

        // Atualiza o tempo que o jogador está andando
        timeWalking += Time.deltaTime;

        // Transforma o vetor de movimento com base na direção dele (se esta existir), para que o movimento esteja sempre alinhado com a câmera
        Vector3 direcaoMovimento = forward * direcao;

        // Calcula a direção desejada de movimento
        Vector3 velocidadeAlvo = direcaoMovimento * _targetVelocity;

        // Calcula o valor da aceleração para ser utilizada no Lerp
        float valorAceleracao = 1 - Mathf.Exp(-_acceleration * Time.deltaTime);

        // Calcula o Lerp, que é basicamente uma forma de deixar mais suave o movimento com aceleração
        Vector3 direcaoLerp = Vector3.Lerp(currentVelocity, velocidadeAlvo, valorAceleracao);

        // Movimenta o objeto
        // Se possui o componente de CharacterController, usa ele para o movimento, caso contrário usa o rigid body
        if (isPlayer)
        {
            _characterController.Move(direcaoLerp * Time.deltaTime);
        } else
        {
            // Adiciona a força ao objeto, com base na aceleração do jogador; o multiplicadorAr é para que o jogador se movimente mais devagar no ar, e define como um ForceMode.Force para que seja aplicada constantemente
            _rigidbody.AddForce(direcaoLerp.normalized * _targetVelocity * 10f, ForceMode.Force);
        }

        // Atualiza a variável local para o valor mais recente da velocidade do objeto
        currentVelocity = velocidadeAlvo;
    }

    /// <summary>
    /// Checks if the player is grounded. Needs player height and layer Mask.
    /// </summary>
    /// <returns>True if grounded.</returns>
    public bool Grounded()
    {
        return Physics.Raycast(gameObject.transform.position, Vector3.down, _height * 0.5f + 0.3f, _groundLayer);
    }
}
