namespace _01_GestionFlota_UrbanMove.Exceptions;
/// <summary>
/// Excepcion de negocio cuando la bateria no cumple con el porcentaje minimo requerido.
/// </summary>
public class BateriaInsuficienteException : Exception
{
    public BateriaInsuficienteException()
        : base("La batería del vehículo no es suficiente para realizar la operación."){}
    public BateriaInsuficienteException(string mensaje):base(mensaje){}
}