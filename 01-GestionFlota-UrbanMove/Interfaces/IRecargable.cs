namespace _01_GestionFlota_UrbanMove.Interfaces;
/// <summary>
/// Interface para transporte electrico que necesitan controlar y recargar su bateria.
/// </summary>
public interface IRecargable
{
    /// <summary>Guarda el porcentaje actual de bateria.</summary>
    int NivelBateria { get; set; }
    /// <summary>Realiza la accion de cargar la bateria.</summary>
    void CargarBateria();
}