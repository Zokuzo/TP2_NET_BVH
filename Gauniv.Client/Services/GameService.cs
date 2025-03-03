using Gauniv.Client.Model;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gauniv.Client.Services
{
    public class GameService
    {
        private static List<Game> games = new()
            {
            new Game { Id= 1, Name = "Cyberpunk 2077", Description = "RPG", Payload=new byte[1024]},
            new Game { Id=2, Name = "Elden Ring", Description = "Action RPG", Payload=new byte [1024] },
            new Game { Id=3, Name = "The Witcher 3", Description = "RPG" , Payload=new byte [1024]}
        };

        public static List<Game> Games { get => games; set => games = value; }

        public List<Game> GetAllGames()
        {
            return Games;
        }
        public Game GetGameByName(string name)
        {
            return Games.FirstOrDefault(g => g.Name == name);
        }
        public void DownloadGame(string gameTitle)
        {
            Game game = GetGameByName(gameTitle);
            if (game == null)
            {
                Console.WriteLine($"Erreur : Jeu '{gameTitle}' introuvable.");
                return;
            }

            string folderPath = Path.Combine(@"C:\Games", gameTitle);
            Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, gameTitle + ".txt");
            File.WriteAllBytes(filePath, game.Payload); 

            Console.WriteLine($"L'installation de'{gameTitle}' a débuté !");
        }
  
        public void StartGame(string gameTitle)
        {
            GlobalStorage.ProcessIds[gameTitle] = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = $@"C:\Games\{gameTitle}\{gameTitle}.txt",
                    UseShellExecute = true
                },
                EnableRaisingEvents = true
            };
            GlobalStorage.ProcessIds[gameTitle].Start();
        }
        public void StopGame(string gameTitle)
        {
            if(GlobalStorage.ProcessIds.ContainsKey(gameTitle))
            {

                //Process[] processes = Process.GetProcessesByName(gameTitle);
                Process process = Process.GetProcessById(GlobalStorage.ProcessIds[gameTitle].Id);
                process.Kill();
            }
        }
        public bool IsStarted(string gameTitle)
        {
            if (!GlobalStorage.ProcessIds.ContainsKey(gameTitle))
                return false;

            try{
                Process process = Process.GetProcessById(GlobalStorage.ProcessIds[gameTitle].Id);
                Debug.WriteLine($"{gameTitle} est en cours d'execution : {process}.");
                return process != null;
            }
            catch(Exception e)
            {
                return false;
            }
}
        public bool IsInstalled(string gameTitle)
        {
            string folderPath = Path.Combine(@"C:\Games", gameTitle);
            return Directory.Exists(folderPath);
        }
public void UninstallGame(string gameTitle)
        {
            Game game = GetGameByName(gameTitle);
            if (game == null)
            {
                Console.WriteLine($"Erreur : Jeu '{gameTitle}' introuvable.");
                return;
            }
            string folderPath = Path.Combine(@"C:\Games", gameTitle);
            if (Directory.Exists(folderPath))
                {
                Directory.Delete(folderPath, recursive: true);
                Console.WriteLine($"La désinstallation de '{gameTitle}'a commencé");
            }
        }

    }
}
