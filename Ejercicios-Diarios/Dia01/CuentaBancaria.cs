namespace EjerciciosDiarios.Dia01;

public class CuentaBancaria
{
    public string NumeroCuenta { get; }
    public string Titular { get; set; }
    public decimal Saldo { get; private set; }

    public CuentaBancaria(string numeroCuenta, string titular, decimal saldoInicial = 0)
    {
        if (saldoInicial < 0)
            throw new ArgumentException("El saldo inicial no puede ser negativo.");

        NumeroCuenta = numeroCuenta;
        Titular = titular;
        Saldo = saldoInicial;
    }

    public void Depositar(decimal monto)
    {
        if (monto <= 0)
        {
            Console.WriteLine("El monto a depositar debe ser mayor a cero.");
            return;
        }

        Saldo += monto;
        Console.WriteLine($"Depósito exitoso de ${monto}. Nuevo saldo: ${Saldo}");
    }

    public bool Extraer(decimal monto)
    {
        if (monto <= 0)
        {
            Console.WriteLine("El monto a extraer debe ser mayor a cero.");
            return false;
        }

        if (monto > Saldo)
        {
            Console.WriteLine($"Fondos insuficientes. Intento de extracción: ${monto} | Saldo actual: ${Saldo}");
            return false;
        }

        Saldo -= monto;
        Console.WriteLine($"Extracción exitosa de ${monto}. Nuevo saldo: ${Saldo}");
        return true;
    }
}