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
using Windows.ApplicationModel.VoiceCommands;
using Gauniv.Client.Model;
using Gauniv.WebServer.Dtos;
using Gauniv.WebServer.Api;

namespace Gauniv.Client.ViewModel
{
    public partial class IndexViewModel : ObservableObject
    {
        private readonly GameService _gameService;
        public ObservableCollection<string> Games { get; set; } = new ObservableCollection<string>();

        public AsyncRelayCommand LoadGamesCommand { get; }

        public IndexViewModel()
        {
            
        }

    
    }
}
