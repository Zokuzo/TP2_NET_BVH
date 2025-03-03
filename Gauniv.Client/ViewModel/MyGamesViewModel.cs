using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Gauniv.Client.Model;
using Gauniv.Client.Pages;
using Gauniv.Client.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gauniv.Client.ViewModel
{
    public partial class MyGamesViewModel: ObservableObject
    {
        private readonly GameService _gameService;

        [ObservableProperty]
        private ObservableCollection<Game> games = new ObservableCollection<Game>();

        public MyGamesViewModel()
        {
            _gameService = new GameService();
            foreach(var g in _gameService.GetAllGames())
            {
                games.Add(g);
            }
        }


        [RelayCommand]
        public void GoToDetails(Game selectedGame)
        {
            NavigationService.Instance.Navigate<GameDetails>(new() { ["SelectedGame"] = selectedGame });
        }
    }
}
