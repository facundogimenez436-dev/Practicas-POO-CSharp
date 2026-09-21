using Spectre.Console;
using _01_GestionFlota_UrbanMove.Common;
using _01_GestionFlota_UrbanMove.Exceptions;
using _01_GestionFlota_UrbanMove.Interfaces;
using _01_GestionFlota_UrbanMove.Models;
using _01_GestionFlota_UrbanMove.Services;

namespace _01_GestionFlota_UrbanMove.UI;

public static class FlotaConsole
{
    public static void RegistrarTransporte(List<Transporte> flota)
    {
        AnsiConsole.MarkupLine("[grey]Función no implementada todavía.[/]");
        Pausar();
    }

    public static void ListarFlota(List<Transporte> flota)
    {
        AnsiConsole.MarkupLine("[grey]Función no implementada todavía.[/]");
        Pausar();
    }

    public static void IniciarViajeUI(List<Transporte> flota, ProcesarAlquiler alquilerService)
    {
        AnsiConsole.MarkupLine("[grey]Función no implementada todavía.[/]");
        Pausar();
    }

    public static void FinalizarViajeUI(List<Transporte> flota, ProcesarAlquiler alquilerService)
    {
        AnsiConsole.MarkupLine("[grey]Función no implementada todavía.[/]");
        Pausar();
    }

    public static void CargarCamionUI(List<Transporte> flota)
    {
        AnsiConsole.MarkupLine("[grey]Función no implementada todavía.[/]");
        Pausar();
    }

    public static void DescargarCamionUI(List<Transporte> flota)
    {
        AnsiConsole.MarkupLine("[grey]Función no implementada todavía.[/]");
        Pausar();
    }

    public static void EliminarTransporte(List<Transporte> flota)
    {
        AnsiConsole.MarkupLine("[grey]Función no implementada todavía.[/]");
        Pausar();
    }

    public static void Pausar()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[dim]Presione cualquier tecla para continuar...[/]");
        Console.ReadKey(true);
    }
}





