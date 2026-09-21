namespace _01_GestionFlota_UrbanMove.Interfaces;
/// <summary>
/// Interface para los transporte que cobran una tarifa.
/// </summary>
public interface IAlquilable
{
    /// <summary>
    /// Calcula el precio del viaje.
    /// </summary>
    /// <param name="minutos"></param>
    /// <returns></returns>
    decimal CalcularCosto(int minutos);
}