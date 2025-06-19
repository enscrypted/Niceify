using System;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Niceify.Views;

namespace Niceify.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        private string _selectedTone;
        private string _customPrompt;
        private bool _sendEnter;
        private MainWindow _mainWindow;

        public ObservableCollection<string> Tones { get; } = new()
        {
            "Nice",
            "Professional",
            "Friendly",
            "Assertive",
            "Casual",
            "Formal",
            "Encouraging",
            "Empathetic",
            "Neutral",
            "Excited",
            "Melancholy",
            "Jubilant",
            "Anxious",
            "Irritated",
            "Romantic",
            "Sarcastic",
            "Passive-Aggressive",
            "Mean",
            "Aggressive",
            "Frightened",
            "Terrified",
            "Mortified",
            "Overconfident",
            "Unhinged",
            "Dramatic",
            "Extra",
        };

        public string SelectedTone
        {
            get => _selectedTone;
            set => this.RaiseAndSetIfChanged(ref _selectedTone, value);
        }

        public string CustomPrompt
        {
            get => _customPrompt;
            set => this.RaiseAndSetIfChanged(ref _customPrompt, value);
        }

        public bool SendEnter
        {
            get => _sendEnter;
            set => this.RaiseAndSetIfChanged(ref _sendEnter, value);
        }

        public ReactiveCommand<Unit, Unit> SaveCommand { get; }

        public MainWindowViewModel()
        {
            SaveCommand = ReactiveCommand.CreateFromTask(SaveAsync);

            SelectedTone = Tones.FirstOrDefault() ?? "Professional";
            CustomPrompt = "";
            SendEnter = true;
        }
        
        private Task WriteToFileAsync(string json)
        {
            return Task.Run(() =>
            {
                // This happens on a background thread
                var path = "appSettings.json";
                File.WriteAllText(path, json);
            });
        }
        
        private async Task SaveAsync()
        {
            try
            {
                var settings = new
                {
                    Theme = "Light",
                    Tones = Tones.ToArray(),
                    SelectedTone = SelectedTone,
                    CustomPrompt = CustomPrompt,
                    SendAfterPaste = SendEnter
                };

                var json = System.Text.Json.JsonSerializer.Serialize(settings, new System.Text.Json.JsonSerializerOptions
                {
                    WriteIndented = true
                });

                await WriteToFileAsync(json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in SaveCommand: {ex.Message}");
            }
        }

    }
}
