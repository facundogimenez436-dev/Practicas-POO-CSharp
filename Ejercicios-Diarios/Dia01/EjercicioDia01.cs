namespace EjerciciosDiarios.Dia01;

public static class EjercicioDia01
{
    public static void Ejecutar()
    {
        Console.WriteLine("DIA 01: Encapsulamiento y Validación");

        CuentaBancaria cuenta = new("001-987654", "Juan Pérez", 500);

        Console.WriteLine($"Cuenta: {cuenta.NumeroCuenta} | Titular: {cuenta.Titular} | Saldo Inicial: ${cuenta.Saldo}");

        // Pruebas de operaciones
        cuenta.Depositar(200);
        cuenta.Extraer(100);
        cuenta.Extraer(800); // Intento fallido por saldo insuficiente
        cuenta.Depositar(-50); // Intento fallido por monto inválido
    }
}