using Spectre.Console;
using _01_GestionFlota_UrbanMove.Common;
using _01_GestionFlota_UrbanMove.Exceptions;
using _01_GestionFlota_UrbanMove.Interfaces;
using _01_GestionFlota_UrbanMove.Models;
using _01_GestionFlota_UrbanMove.Services;
using System.Collections;

namespace _01_GestionFlota_UrbanMove.UI;
/// <summary>
/// Constiene los metodos que implementa la interfaz de la consola.
/// </summary>
public static class FlotaConsole
{
    public static void RegistrarTransporte(List<Transporte> flota)
    {
        var opcion = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("== Seleccione el [bold blue]tipo de vehiculo[/] a registrar ==")
            .PageSize(10)
            .AddChoices(new[] {
            "1. Bicicleta Normal",
            "2. Monopatin Electrico",
            "3. Camion Logistica"
        }));
        AnsiConsole.MarkupLine("\n[bold blue]Datos Generales:[/]");
        AnsiConsole.WriteLine();
        try
        {
            string id = AnsiConsole.Prompt(
                new TextPrompt<string>("Ingrese el [bold yellow]ID/Patente[/]")
                .Validate(input =>
                {
                    if (flota.Any(v => v.Id.Equals(input, StringComparison.OrdinalIgnoreCase)))
                    {
                        return ValidationResult.Error($"[bold red]El ID '{input}' ya existe.Ingresa uno diferente.[/]");
                    }
                    return ValidationResult.Success();
                })
            );
            string marca = AnsiConsole.Ask<string>("Ingrese la [bold yellow]Marca[/]");
            string modelo = AnsiConsole.Ask<string>("Ingrese el [bold yellow]Modelo[/]");

            Transporte? nuevoTransporte = null;
            switch (opcion)
            {
                case "1. Bicicleta Normal":
                    nuevoTransporte = new BicicletaNormal(id, marca, modelo);
                    break;
                case "2. Monopatin Electrico":
                    nuevoTransporte = new MonopatinElectrico(id, marca, modelo);
                    break;
                case "3. Camion Logistica":
                    nuevoTransporte = new CamionLogistica(id, marca, modelo);
                    break;
            }
            if (nuevoTransporte != null)
            {
                flota.Add(nuevoTransporte);
                AnsiConsole.MarkupLine("\n[bold yellow]Vehiculo Registrado![/]");
                Thread.Sleep(2000);
                AnsiConsole.Clear();

                AnsiConsole.Write(
                    new FigletText("UrbanMove")
                        .Color(Color.Blue));
                AnsiConsole.Write(new Rule("[yellow]Sistema de Gestión de Flota & Logística[/]"));
                AnsiConsole.WriteLine();

                var panel = new Panel(nuevoTransporte!.ToString())
                {
                    Header = new PanelHeader("[bold green] Resumen del vehiculo registrado.[/]"),
                    Border = BoxBorder.Rounded,
                    Padding = new Padding(1, 0, 1, 0),
                    Expand = true
                };
                AnsiConsole.Write(panel);
            }

        }
        catch (ArgumentOutOfRangeException ex)
        {
            AnsiConsole.MarkupLine($"[bold red]Error de rango en los datos:[/] {ex.Message}");
        }
        catch (VehiculoNoAptoException ex)
        {
            AnsiConsole.MarkupLine($"[bold red]Error vehiculo no apto:[/] {ex.Message}");
        }
        catch (BateriaInsuficienteException ex)
        {
            AnsiConsole.MarkupLine($"[bold red]Error bateria insuficiente:[/] {ex.Message}");
        }
        catch (ArgumentException ex)
        {
            AnsiConsole.MarkupLine($"[bold red]Error de validacion:[/] {ex.Message}");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[bold red]Error inesperado:[/] {ex.Message}");
        }
        
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





