using System;
using System.IO;
using System.Collections.Generic;

class TicketDistributor
{
    static string filePath = "fnumero.txt";
    static Dictionary<string, int> lastNumbers = new Dictionary<string, int>
    {
        { "V", 0 }, // Versement
        { "R", 0 }, // Retrait
        { "I", 0 }  // Informations
    };

    static void Main()
    {
        LoadLastNumbers();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Bienvenue au distributeur de tickets de la banque.");
            Console.WriteLine("1 - Versement");
            Console.WriteLine("2 - Retrait");
            Console.WriteLine("3 - Informations");
            Console.WriteLine("4 - Quitter");
            Console.Write("Sélectionnez une option : ");

            string choice = Console.ReadLine();
            if (choice == "4") break;

            string type = choice switch
            {
                "1" => "V",
                "2" => "R",
                "3" => "I",
                _ => null
            };

            if (type == null)
            {
                Console.WriteLine("Option invalide. Appuyez sur une touche pour recommencer...");
                Console.ReadKey();
                continue;
            }

            Console.Write("Entrez votre nom : ");
            string nom = Console.ReadLine();
            Console.Write("Entrez votre prénom : ");
            string prenom = Console.ReadLine();
            Console.Write("Entrez votre numéro de compte : ");
            string compte = Console.ReadLine();

            lastNumbers[type]++;
            string ticket = $"{type}-{lastNumbers[type]}";

            Console.WriteLine($"Votre numéro est {ticket}");
            Console.WriteLine($"Il y a {lastNumbers[type] - 1} personnes avant vous.");

            SaveLastNumbers();
            SaveClientInfo(ticket, nom, prenom, compte);

            Console.WriteLine("Appuyez sur une touche pour continuer...");
            Console.ReadKey();
        }
    }

    static void LoadLastNumbers()
    {
        if (!File.Exists(filePath)) return;

        string[] lines = File.ReadAllLines(filePath);
        foreach (var line in lines)
        {
            var parts = line.Split(':');
            if (parts.Length == 2 && lastNumbers.ContainsKey(parts[0]))
            {
                lastNumbers[parts[0]] = int.Parse(parts[1]);
            }
        }
    }

    static void SaveLastNumbers()
    {
        List<string> lines = new List<string>();
        foreach (var entry in lastNumbers)
        {
            lines.Add($"{entry.Key}:{entry.Value}");
        }
        File.WriteAllLines(filePath, lines);
    }

    static void SaveClientInfo(string ticket, string nom, string prenom, string compte)
    {
        using (StreamWriter sw = File.AppendText("clients.txt"))
        {
            sw.WriteLine($"{ticket} - {nom} {prenom} - Compte: {compte}");
        }
    }
}
