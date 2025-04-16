using System;
using UnityEngine;

public class ComponenteMovimento : MonoBehaviour
{

    [SerializeField, Tooltip("A velocidade desejada do objeto. Deve ser definida.")]
    private float _velocidadeDesejada;

    /// <summary>
    /// A velocidade alvo do jogador, que pode ser alterada em tempo de execução.
    /// </summary>
    public float TargetVelocity { get => _velocidadeDesejada; set => _velocidadeDesejada = value; }

    private float _initialVelocity;

    /// <summary>
    /// A velocidade inicial do objeto. É definida automaticamente.
    /// </summary>
    public float InitialVelocity { get => _initialVelocity; private set => _initialVelocity = value; }

    [SerializeField, Tooltip("O multiplicador de velocidade ao correr. Por padrão 1,5.")]
    private float _runningMultiplier = 1.5f;

    /// <summary>
    /// O multiplicador de velocidade ao correr. Por padrão 1,5.
    /// </summary>
    public float RunningMultiplier { get => _runningMultiplier; set => _runningMultiplier = value; }

    [SerializeField, Tooltip("A aceleração do objeto, por padrão 1.")]
    private float _aceleracao = 1.0f;

    [NonSerialized]
    public float timeWalking;

    private Rigidbody _rigidbody;
    private CharacterController _characterController;

    [NonSerialized]
    public Vector3 velocidadeAtual = Vector3.zero;

    private bool isJogador;

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
        isJogador = TryGetComponent<CharacterController>(out _characterController);

        // Define a velocidade inicial como a velocidade desejada do objeto
        _initialVelocity = _velocidadeDesejada;
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

            velocidadeAtual = Vector3.zero;

            return;
        }

        // Atualiza o tempo que o jogador está andando
        timeWalking += Time.deltaTime;

        // Transforma o vetor de movimento com base na direção dele (se esta existir), para que o movimento esteja sempre alinhado com a câmera
        Vector3 direcaoMovimento = forward * direcao;

        // Calcula a direção desejada de movimento
        Vector3 velocidadeAlvo = direcaoMovimento * _velocidadeDesejada;

        // Calcula o valor da aceleração para ser utilizada no Lerp
        float valorAceleracao = 1 - Mathf.Exp(-_aceleracao * Time.deltaTime);

        // Calcula o Lerp, que é basicamente uma forma de deixar mais suave o movimento com aceleração
        Vector3 direcaoLerp = Vector3.Lerp(velocidadeAtual, velocidadeAlvo, valorAceleracao);

        // Movimenta o objeto
        // Se possui o componente de CharacterController, usa ele para o movimento, caso contrário usa o rigid body
        if (isJogador)
        {
            _characterController.Move(direcaoLerp * Time.deltaTime);
        } else
        {
            // Adiciona a força ao objeto, com base na aceleração do jogador; o multiplicadorAr é para que o jogador se movimente mais devagar no ar, e define como um ForceMode.Force para que seja aplicada constantemente
            _rigidbody.AddForce(direcaoLerp.normalized * _velocidadeDesejada * 10f, ForceMode.Force);
        }

        // Atualiza a variável local para o valor mais recente da velocidade do objeto
        velocidadeAtual = velocidadeAlvo;
    }
}
