using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Protection.PlayReady;

namespace Gauniv.Client.Services
{
    internal partial class NetworkService : ObservableObject
    {

        public static NetworkService Instance { get; private set; } = new NetworkService();
        [ObservableProperty]
        private string token = "aaaa";
        public HttpClient httpClient;

        public NetworkService() {
            httpClient = new HttpClient();
            //Token = null;
            OnConnected?.Invoke();
        }

        public event Action OnConnected;
        public async Task<HttpResponseMessage> GetAsync(string endpoint)
        {
            try
            {
                // Ajouter le token à l'en-tête si présent
                if (!string.IsNullOrEmpty(Token))
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                }

                string url = $"https://api.example.com{endpoint}"; // Remplace par l'URL de base de ton API
                var response = await httpClient.GetAsync(url);
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de l'appel GET : " + ex.Message);
                return null;
            }
        }

    }
}
