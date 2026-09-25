using Spectre.Console;
using _01_GestionFlota_UrbanMove.Common;
using _01_GestionFlota_UrbanMove.Exceptions;
using _01_GestionFlota_UrbanMove.Interfaces;
using _01_GestionFlota_UrbanMove.Models;
using _01_GestionFlota_UrbanMove.Services;
using _01_GestionFlota_UrbanMove.UI;

namespace _01_GestionFlota_UrbanMove;

public class Program
{
    static void Main(string[] args)
    {
        // Lista global para registrar la flota.
        List<Transporte> flota = new();
        //
        ProcesarAlquiler alquilerService = new();

        bool salir = false;

        while (!salir)
        {
            Console.Clear();

            // Título principal con estilo
            AnsiConsole.Write(
                new FigletText("UrbanMove")
                    .Color(Color.Blue));

            AnsiConsole.Write(new Rule("[yellow]Sistema de Gestión de Flota & Logística[/]"));
            AnsiConsole.WriteLine();

            // Menú interactivo navegable con las flechas del teclado
            var opcion = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold white]Seleccione una acción con las flechas del teclado:[/]")
                    .PageSize(10)
                    .AddChoices(new[] {
                        "1. Registrar nuevo transporte",
                        "2. Listar flota de vehículos",
                        "3. Iniciar viaje / alquiler",
                        "4. Finalizar viaje / calcular costo",
                        "5. Cargar vehículo en camión",
                        "6. Descargar vehículo de camión",
                        "7. Editar transporte de la flota",
                        "8. Eliminar transporte de la flota",
                        "0. Salir"
                    }));

            switch (opcion)
            {
                case "1. Registrar nuevo transporte":
                    FlotaConsole.RegistrarTransporte(flota);
                    break;
                case "2. Listar flota de vehículos":
                    FlotaConsole.ListarFlota(flota);
                    break;
                case "3. Iniciar viaje / alquiler":
                    FlotaConsole.IniciarViajeUI(flota, alquilerService);
                    break;
                case "4. Finalizar viaje / calcular costo":
                    FlotaConsole.FinalizarViajeUI(flota, alquilerService);
                    break;
                case "5. Cargar vehículo en camión":
                    FlotaConsole.CargarCamionUI(flota);
                    break;
                case "6. Descargar vehículo de camión":
                    FlotaConsole.DescargarCamionUI(flota);
                    break;
                case "7. Editar transporte de la flota":
                    FlotaConsole.EditarTransporte(flota);
                    break;
                case "8. Eliminar transporte de la flota":
                    FlotaConsole.EliminarTransporte(flota);
                    break;
                case "0. Salir":
                    salir = true;
                    AnsiConsole.MarkupLine("\n[bold yellow]¡Gracias por usar UrbanMove![/]");
                    break;
            }
        }
    }
}