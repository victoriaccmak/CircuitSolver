//Author: Victoria Mak
//File Name: NewCircViewModel.cs
//Project Name: CircuitSolver
//Creation Date: May 20, 2024
//Modified Date: 
//Description: A cross platform app that will solve linear circuits

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CircuitSolver.Model;
using CircuitSolver.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitSolver.ViewModel
{
    public partial class NewCircViewModel : ObservableObject
    {
        [ObservableProperty]
        public string text;

        [ObservableProperty]
        public bool dcSelected;
        [ObservableProperty]
        public bool acSelected;
        [ObservableProperty]
        public bool firstOrderSelected;

        private byte type;
        object selection;

        [ObservableProperty]
        string verification;

        public object Selection
        {
            get => selection;
            set
            {
                try
                {
                    selection = value;
                    OnPropertyChanged(nameof(selection));

                    if (selection != null)
                    {
                        type = Byte.Parse(selection.ToString());
                        Verification = "You have selected " + type;
                    }
                    else
                    {
                        type = 3;
                        Verification = "You have not selected a type yet.";
                    }
                }
                catch (Exception ex) 
                {
                    Verification = ex.Message;
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public NewCircViewModel()
        {
            dcSelected = false;
            acSelected = false;
            firstOrderSelected = false;

            type = 3;
            text = "";
            verification = "You have not selected a type yet.";
        }


        //public void Ac_CheckedChanged(object sender, CheckedChangedEventArgs e)
        //{
        //    if (e.Value)
        //    {
        //        type = 1;

        //        if (DcSelected)
        //        {
        //            DcSelected = false;
        //        }
        //        else if (FirstOrderSelected)
        //        {
        //            FirstOrderSelected = false;
        //        }
        //    }
        //}
        //public void DcCheckedChanged(object sender, CheckedChangedEventArgs e)
        //{
        //    if (e.Value)
        //    {
        //        type = 2;

        //        if (AcSelected)
        //        {
        //            AcSelected = false;
        //        }
        //        else if (FirstOrderSelected)
        //        {
        //            FirstOrderSelected = false;
        //        }
        //    }
        //}
        //public void FirstOrderCheckedChanged(object sender, CheckedChangedEventArgs e)
        //{
        //    if (e.Value)
        //    {
        //        type = 3;

        //        if (DcSelected)
        //        {
        //            DcSelected = false;
        //        }
        //        else if (AcSelected)
        //        {
        //            AcSelected = false;
        //        }
        //    }
        //}

        //async void OnTypesRadioButtonCheckedChanged(object sender, CheckedChangedEventArgs e)
        //{
        //    try
        //    {
        //        RadioButton button = sender as RadioButton;

        //        if (button != null)
        //        {
        //            type = (byte)button.Value;
        //        }
        //        else
        //        {
        //            type = 4;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        //    }
        //}

        [RelayCommand]
        async Task Create()
        {
            try
            {
                if (type == 3)
                {
                    await Shell.Current.DisplayAlert("Invalid", "You must select a type", "OK");
                    return;
                }

                if (Text.Trim().Length == 0)
                {
                    await Shell.Current.DisplayAlert("Invalid", "You must enter a circuit name", "OK");
                    return;
                }

                Circuit newCirc = new Circuit(Text, DateTime.Now, type);
                newCirc.AddNode("A");

                await Shell.Current.GoToAsync($"{nameof(CircPage)}", new Dictionary<string, object> { { "Circuit", newCirc } });
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
