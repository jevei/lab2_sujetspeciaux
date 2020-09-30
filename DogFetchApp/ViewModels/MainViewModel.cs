using ApiHelper;
using DogFetchApp.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace DogFetchApp.ViewModels
{
    class MainViewModel : BaseViewModel
    {
        private ObservableCollection<string> breeds;
        private ObservableCollection<BitmapImage> images;
        private string selectedBreed;
        private int selectedIndex;
        public ObservableCollection<BitmapImage> Images
        {
            get => images;
            private set
            {
                images = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<string> Breeds
        {
            get => breeds;
            private set
            {
                breeds = value;
                OnPropertyChanged();
            }
        }
        public string SelectedBreed
        {
            get => selectedBreed;
            set
            {
                selectedBreed = value;
                OnPropertyChanged();
            }
        }
        public int SelectedIndex
        {
            get => selectedIndex;
            set
            {
                selectedIndex = value;
                OnPropertyChanged();
            }
        }
        public DelegateCommand<string> ChangeLanguageCommand { get; private set; }
        public AsyncCommand<string> TestCommand { get; private set; }
        public AsyncCommand<object> WindowLoadedCommand { get; private set; }
        public AsyncCommand<string> FetchButtonCommand { get; private set; }
        public DelegateCommand<int> NextButtonCommand { get; private set; }
        public MainViewModel()
        {
            ChangeLanguageCommand = new DelegateCommand<string>(ChangeLanguage);
            TestCommand = new AsyncCommand<string>(Test);
            FetchButtonCommand = new AsyncCommand<string>(FetchButton);
            WindowLoadedCommand = new AsyncCommand<object>(WindowLoaded);
            NextButtonCommand = new DelegateCommand<int>(NextButton);
        }

        private void NextButton(int i)
        {
            if (Images != null)
            {
                if (i++ <= Images.Count())
                {
                    SelectedIndex++;
                }
            }
        }

        private async Task FetchButton(string nb)
        {
            await LoadDogImageAsync(nb, SelectedBreed);
        }

        private async Task LoadDogImageAsync(string nb, string selectedBreed)
        {
            var images = await DogApiProcessor.LoadDogImage(nb, selectedBreed);
            Images = new ObservableCollection<BitmapImage>();
            foreach(var i in images)
            {
                Images.Add(new BitmapImage(new Uri(i)));
            }
        }

        private void ChangeLanguage(string param)
        {
            Properties.Settings.Default.Language = param;
            Properties.Settings.Default.Save();

            if (MessageBox.Show(
                    "Please restart app for the settings to take effect.\nWould you like to restart?",
                    "Warning!",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                Restart();
        }
        void Restart()
        {
            var filename = Application.ResourceAssembly.Location;
            var newFile = Path.ChangeExtension(filename, ".exe");
            Process.Start(newFile);
            Application.Current.Shutdown();
        }
        private async Task Test(string arg)
        {
            var images = await DogApiProcessor.LoadBreedList();

            MessageBox.Show(string.Join(Environment.NewLine, images));
        }
        private async Task WindowLoaded(object arg) => await LoadDogAsync();

        private async Task LoadDogAsync()
        {
            var temp = await DogApiProcessor.LoadBreedList();
            Breeds = new ObservableCollection<string>();
            foreach (var b in temp)
            {
                Breeds.Add(b);
            }
        }
    }
}
