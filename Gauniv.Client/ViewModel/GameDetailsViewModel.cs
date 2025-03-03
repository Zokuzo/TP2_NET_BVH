using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Gauniv.Client.Model;
using Gauniv.Client.Services;
using Gauniv.Client.WinUI;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Gauniv.Client.ViewModel
{
    [QueryProperty("SelectedGame","SelectedGame")]
    public partial class GameDetailsViewModel : ObservableObject, IDisposable
    {
        private readonly GameService _gameService;

        private Game selectedGame;

        public GameDetailsViewModel()
        {
            _gameService = new GameService();
        }

        private bool _isButtonEnabled = false;
        public bool IsButtonEnabled
        {
            get => _isButtonEnabled;
            set
            {
                _isButtonEnabled = value;
                OnPropertyChanged(nameof(IsButtonEnabled));
            }
        }
        [RelayCommand]
        public void UninstallGameDetails()
        {
            if (SelectedGame != null)
            {
                _gameService.UninstallGame(SelectedGame.Id);
                OnPropertyChanged(nameof(IsInstalled));
                OnPropertyChanged(nameof(IsStarted));
            }

        }
        [RelayCommand]
        public void InstallGameDetails()
        {
            if (SelectedGame != null )
            {
                _gameService.DownloadGame(SelectedGame.Id);
                OnPropertyChanged(nameof(IsInstalled));
                OnPropertyChanged(nameof(IsStarted));

            }
        }
        [RelayCommand]
        public void PlayGameDetails()
        {
            if (SelectedGame != null)
            {
                _gameService.StartGame(SelectedGame.Name);
                OnPropertyChanged(nameof(IsStarted));

                if (selectedGame is not null && GlobalStorage.ProcessIds.ContainsKey(selectedGame.Name))
                {
                    GlobalStorage.ProcessIds[selectedGame.Name].Exited += OnProcessExited;
                }
            }
         
        }
        [RelayCommand]
        public void StopGameDetails()
        {
            if (SelectedGame != null)
            {
                _gameService.StopGame(SelectedGame.Name);
                OnPropertyChanged(nameof(IsStarted));
                OnPropertyChanged(nameof(IsInstalled));

            }
         
        }

        public bool IsInstalled { get
            {
                if (SelectedGame == null)
                    return false;
                return !IsStarted && _gameService.IsInstalled(SelectedGame.Name);
            } 
        }
        public bool IsStarted { get
            {
                if (SelectedGame == null)
                    return false;
                return  _gameService.IsStarted(SelectedGame.Name);
            } 
        }

        public void OnProcessExited(object? sender, EventArgs e)
        {
            //StopGameDetails();
            OnPropertyChanged(nameof(IsStarted));
            OnPropertyChanged(nameof(IsInstalled));
        }

        public void Dispose()
        {
            if (GlobalStorage.ProcessIds.ContainsKey(SelectedGame.Name))
            {
                GlobalStorage.ProcessIds[selectedGame.Name].Exited -= OnProcessExited;
            }
        }
        ~GameDetailsViewModel()
        {
            if (GlobalStorage.ProcessIds.ContainsKey(SelectedGame.Name))
            {
                GlobalStorage.ProcessIds[selectedGame.Name].Exited -= OnProcessExited;
            }
        }

        public Game SelectedGame { get => selectedGame; set {
                if(selectedGame is not null && GlobalStorage.ProcessIds.ContainsKey(selectedGame.Name))
                {
                    GlobalStorage.ProcessIds[selectedGame.Name].Exited -= OnProcessExited;
                }
                if(value is not null && GlobalStorage.ProcessIds.ContainsKey(value.Name)){
                    GlobalStorage.ProcessIds[value.Name].Exited += OnProcessExited;
                }
                SetProperty(ref selectedGame, value);
                OnPropertyChanged(nameof(IsStarted));
                OnPropertyChanged(nameof(IsInstalled));
            } 
        }
    }
}
