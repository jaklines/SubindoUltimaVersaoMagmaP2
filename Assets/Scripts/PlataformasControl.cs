using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enumera��o para os tipos de movimento das plataformas
public enum TipoMovimento { Horizontal, Vertical };
public class PlataformaMovimento : MonoBehaviour
{
    public TipoMovimento tipoMovimento;
    private Vector3 startPosition;
    public float velocidade = 1f; 
    public float distancia = 4f;

    void Start()
    {
        startPosition = transform.position; // Armazena a posi��o inicial da plataforma
    }

    void Update()
    {
        switch (tipoMovimento) // Verifica o tipo de movimento da plataforma
        {
            case TipoMovimento.Horizontal:
                MovimentoHorizontal(); // Chama o movimento horizontal
                break;
            case TipoMovimento.Vertical:
                MovimentoVertical(); // Chama o movimento vertical
                break;
        }
    }

    void MovimentoHorizontal()
    {
        // Calcula o movimento horizontal da plataforma usando a fun��o PingPong para criar um movimento de ida e volta
        float movimento = Mathf.PingPong(Time.time * velocidade, distancia * 2) - distancia;
        transform.position = startPosition + Vector3.right * movimento;
    }

    void MovimentoVertical()
    {
        float movimento = Mathf.PingPong(Time.time * velocidade, distancia * 2) - distancia;
        transform.position = startPosition + Vector3.up * movimento;
    }
}
