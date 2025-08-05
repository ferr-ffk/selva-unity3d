using System;
using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class JumpComponent : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField, Tooltip("A velocidade de pulo, sendo pulo o movimento em direção Y+. Deve ser definida.")]
    private float _velocidadePulo;

    [System.NonSerialized]
    public bool grounded;

    [SerializeField, Tooltip("A altura do Personagem")]
    private float _characterHeight;

    [SerializeField, Tooltip("O atrito do jogador no chão")]
    private float _groundDrag = 1f;

    private Rigidbody _rigidbody;

    [System.NonSerialized]
    public Vector3 velocidadeAtual = Vector3.zero;

    private void Start()
    {
        // Congela a rotação do objeto para que ele não caia rs
        _rigidbody = GetComponent<Rigidbody>();
        if (_rigidbody == null)
        {
            Debug.LogError("Rigidbody não encontrado!");
            return;
        }
        else
        {
            _rigidbody.freezeRotation = true;
        }
    }
    private void Update()
    {
        // Atualiza se o jogador está atualmente no chão com base em um Raycast
        grounded = Physics.Raycast(transform.position, Vector3.down, _characterHeight * 0.5f + 0.3f, LayerMask.GetMask("Ground"));

        if (grounded)
        {
            // Adiciona atrito ao jogador
            _rigidbody.linearDamping = _groundDrag;
        }
        else
        {
            // Remove o atrito do jogador
            _rigidbody.linearDamping = 0;
        }
    }

    /// <summary>
    /// Usa a velocidade de pulo do componente para movimentar o objeto na direção Y+.
    /// </summary>
    /// <param name="obj">O GameObject a ser movimentado</param>
    public void Jump(GameObject obj)
    {
        if (obj == null)
        {
            Debug.LogError("Objeto não pode ser nulo!");
            return;
        }

        if (_rigidbody == null)
        {
            Debug.LogError("Rigidbody não encontrado!");
            return;
        }

        // Reseta a velocidade em Y
        _rigidbody.linearVelocity = new Vector3(_rigidbody.linearVelocity.x, 0f, _rigidbody.linearVelocity.z);

        // Adiciona uma força para cima, com base na velocidade de pulo, ForceMode.Impulse para que seja aplicada instantaneamente
        _rigidbody.AddForce(Vector3.up * _velocidadePulo, ForceMode.Impulse);
    }
}
