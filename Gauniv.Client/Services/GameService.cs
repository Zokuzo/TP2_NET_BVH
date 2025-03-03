using Gauniv.Client.Model;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gauniv.Client.Services;
using Newtonsoft.Json;
namespace Gauniv.Client.Services
{
    public class GameService
    {
        public static GameService Instance { get; } = new GameService();
        private HttpClient _httpClient;
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
        public Game GetGameById(int id)
        {
            return Games.FirstOrDefault(g => g.Id == id);
        }
        public void DownloadGame(int id)
        {
            Game game = GetGameById(id);
            if (game == null)
            {
                Console.WriteLine($"Erreur : Jeu '{game.Name}' introuvable.");
                return;
            }

            string folderPath = Path.Combine(@"C:\Games", game.Name);
            Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, game.Name + ".txt");
            File.WriteAllBytes(filePath, game.Payload); 

            Console.WriteLine($"L'installation de'{game.Name}' a débuté !");
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
public void UninstallGame(int id)
        {
            Game game = GetGameById(id);
            if (game == null)
            {
                Console.WriteLine($"Erreur : Jeu '{game.Name}' introuvable.");
                return;
            }
            string folderPath = Path.Combine(@"C:\Games", game.Name);
            if (Directory.Exists(folderPath))
                {
                Directory.Delete(folderPath, recursive: true);
                Console.WriteLine($"La désinstallation de '{game.Name}'a commencé");
            }
        }
        public async Task<List<Game>> GetGamesAsync()
        {
            string url = "/api/1.0.0/GamesApi";
            HttpResponseMessage response = await NetworkService.Instance.httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                List<Game> gamesList = JsonConvert.DeserializeObject<List<Game>>(json);
                return gamesList;
            }
            else
            {
                // Gérer l'erreur ici
                throw new Exception("Erreur lors de la récupération des jeux.");
            }
        }
        public async Task<string> GetDownloadLinkAsync(int id)
        {
            var response = await NetworkService.Instance.GetAsync($"/api/1.0.0/GamesApi/LinkToDownloadPayload/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                Console.WriteLine("Erreur lors de la récupération du lien de téléchargement : " + response.StatusCode);
                return null;
            }
        }
        public async Task<bool> SavePayloadLocallyAsync(int id)
        {
            var response = await NetworkService.Instance.GetAsync($"/api/1.0.0/GamesApi/SavePayloadLocally/{id}");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Payload sauvegardé localement !");
                return true;
            }
            else
            {
                Console.WriteLine("Erreur lors de la sauvegarde locale : " + response.StatusCode);
                return false;
            }
        }
        public async Task<string> GetCategoriesAsync()
        {
            var response = await NetworkService.Instance.GetAsync("/api/1.0.0/CategoriesApi");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                Console.WriteLine("Erreur lors de la récupération des catégories : " + response.StatusCode);
                return null;
            }
        }
        public async Task<string> GetCategoryByIdAsync(int id)
        {
            var response = await NetworkService.Instance.GetAsync($"/api/1.0.0/CategoriesApi/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                Console.WriteLine("Erreur lors de la récupération de la catégorie : " + response.StatusCode);
                return null;
            }
        }
        public async Task<string> GetGamePurchasesAsync()
        {
            var response = await NetworkService.Instance.GetAsync("/api/GamePurchaseByUser");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                Console.WriteLine("Erreur lors de la récupération des achats : " + response.StatusCode);
                return null;
            }
        }

    }
}
