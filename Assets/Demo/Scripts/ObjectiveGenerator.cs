using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveGenerator 
{
    public static (int, int) GerarFracaoAleatoria(int pesoMaximo)
    {
        System.Random random = new System.Random();
        
        int numerador;
        int denominador;
        float resultado;

        do
        {
            numerador = random.Next(1, pesoMaximo);       // Gera numerador aleatório
            denominador = random.Next(numerador + 1, pesoMaximo + 1); // Denominador maior que numerador
            resultado = (pesoMaximo * numerador)/denominador;
        }
        while (numerador >= denominador && resultado % 1 == 0 && resultado >= 10);  // Garante que a fração é < 1 e inteiro

        

        return SimplificarFracao(numerador, denominador);
    }

    public static (int, int) SimplificarFracao(int numerador, int denominador)
    {
        int mdc = GetMdc(numerador, denominador);
        return (numerador / mdc, denominador / mdc);
    }

    static int GetMdc(int a, int b)
    {
        while (b != 0)
        {
            int temp = b;
            b = a % b;
            a = temp;
        }

        return a;
    }

}
