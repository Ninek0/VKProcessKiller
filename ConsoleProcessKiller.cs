using System.Diagnostics;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ProcessKiller;

public class ConsoleProcessKiller
{
    private bool _exit = false;
    private Process[] _processesList = [];
    private string? _command;

    public void Start()
    {
        while (!_exit)
        {
            //Console.Clear();
            PrintProcesses();
            AskAction();
            Console.ReadLine();
        }
    }
    
    void PrintProcesses()
    {
        try
        {
            _processesList = Process.GetProcesses();
            foreach (var process in _processesList)
            {
                Console.WriteLine("Process ID: {0}\tProcess name: {1}", process.Id, process.ProcessName);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    void AskAction()
    {
        Console.Write("Command: ");
        try
        {
            _command = Console.ReadLine();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        if (_command != null)
        {
            if (_command == "help" || _command == "HELP")
            {
                PrintHelp();
                return;
            }

            if (_command.StartsWith("kill", true, new CultureInfo("en-US")))
            {
                string[] words = _command.Split(' ');
                if (words.Length > 1)
                {
                    for (int i = 1; i < words.Length; i++)
                    {
                        if (int.TryParse(words[i], out int processID))
                        {
                            Console.WriteLine(processID);
                            KillProcess(processID);
                        }
                        else
                        {
                            Console.WriteLine("Unknown process ID");
                        }
                    }
                }
                return;
            }

            if (_command == "exit" || _command == "EXIT")
            {
                _exit = true;
            }
            
        }
        Console.WriteLine("Unknown command");
    }
    void KillProcess(int processId)
    {
        Process? toKill = null;
        try
        {
            toKill = Process.GetProcessById(processId);
        }
        catch (Exception e)
        {
            Console.WriteLine("Process with this ID: {0} is not running. ID may be expired", processId);
            Console.WriteLine(e);
            return;
        }

        try
        {
            toKill!.Kill();
        }
        catch (Exception e)
        {
            Console.WriteLine("Cannot kill process");
            Console.WriteLine(e);
            return;
        }
        Console.WriteLine("Process killed");
    }
    void PrintHelp()
    {
        Console.WriteLine("kill - is for kill process");
        Console.WriteLine("\tadd id of processes that u want to kill");
        Console.WriteLine("\texample: kill 1 2 3 4");
        Console.WriteLine();
        Console.WriteLine("exit - is for close this weird app");
    }
}