namespace _01_GestionFlota_UrbanMove.Exceptions;
/// <summary>
/// Excepcion de negocio cuando el transporte no esta en condiciones para usarse.
/// </summary>
public class VehiculoNoAptoException : Exception
{
    public VehiculoNoAptoException() 
        : base("El vehículo no cumple con las condiciones para ser utilizado."){}
    public VehiculoNoAptoException(string mensaje):base(mensaje){}
}