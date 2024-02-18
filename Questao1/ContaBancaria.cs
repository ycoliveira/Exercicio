using System;
using System.Globalization;

namespace Questao1
{
    public class ContaBancaria {

        private readonly int numeroConta;
        private string titularConta;
        private double saldo;

        private const double TAXA_SAQUE = 3.50;
        private const double SALDO_ZERO = 0.0;

        protected ContaBancaria() { }

        public ContaBancaria(int numeroConta, string titularConta, double depositoInicial = SALDO_ZERO)
        {
            this.numeroConta = numeroConta;
            SetTitularConta(titularConta);
            Saldo = depositoInicial;
        }
        public int NumeroConta => numeroConta;

        public string TitularConta
        {
            get { return titularConta; }
            private set { titularConta = value; }
        }

        public double Saldo
        {
            get { return saldo; }
            private set { saldo = value; }
        }

        public void SetTitularConta(string titularConta)
        {
            if (string.IsNullOrWhiteSpace(titularConta))
            {
                throw new ArgumentException("O titular da conta não pode estar vazio.", nameof(titularConta));
            }

            TitularConta = titularConta;
        }

        public void Deposito(double valor)
        {
            if (valor <= 0)
            {
                Console.WriteLine("O valor do depósito deve ser maior que zero.");
                return;
            }

            Saldo += valor;
            Console.WriteLine($"Depósito de ${valor:F2} realizado com sucesso.");
        }

        public void Saque(double valor)
        {
            if (valor <= 0)
            {
                Console.WriteLine("OValor do saque deve ser maior que zero.");
                return;
            }

            if(valor > Saldo)
            {
                Console.WriteLine("Seu saldo é insuficiente.");
                return;
            }

            Saldo -= valor;
            Saldo -= TAXA_SAQUE;
        }

        public override string ToString()
        {
            return $"Conta {numeroConta}, Titular: {TitularConta}, Saldo: ${Saldo:F2}";
        }

    }
}
