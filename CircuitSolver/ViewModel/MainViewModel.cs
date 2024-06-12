//Author: Victoria Mak
//File Name: MainViewModel.cs
//Project Name: CircuitSolver
//Creation Date: May 20, 2024
//Modified Date: 
//Description: A cross platform app that will solve linear circuits

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using AndroidX.Fragment.App.StrictMode;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CircuitSolver.Model;
using CircuitSolver.View;

namespace CircuitSolver.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        string debugString;

        [ObservableProperty]
        public ObservableCollection<Circuit> circuits;
        [ObservableProperty]
        public ObservableCollection<Circuit> dispCircuits = new ObservableCollection<Circuit>();

        [ObservableProperty]
        public bool filterOn;

        [ObservableProperty]
        public bool searchSelected;
        [ObservableProperty]
        public bool filterSelected;
        [ObservableProperty]
        public bool sortSelected;

        //[ObservableProperty]
        //public bool dcChecked;
        //[ObservableProperty]
        //public bool acChecked;
        //[ObservableProperty]
        //public bool firstOrderChecked;
        //[ObservableProperty]
        //public bool faveChecked;

        [ObservableProperty]
        bool[] filterChecks;

        [ObservableProperty]
        string searchNameText;
        [ObservableProperty]
        DateTime searchDate;

        [ObservableProperty]
        DateTime maxDate;

        object sortTypeSelection;
        object sortOrderSelection;

        [ObservableProperty]
        string searchNameLabel;
        [ObservableProperty]
        string searchDateLabel;
        [ObservableProperty]
        string filterLabel;

        [ObservableProperty]
        bool nameSearched;
        [ObservableProperty]
        bool dateSearched;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel()
        {
            DebugString = "ewer";
            circuits = new ObservableCollection<Circuit>
            {
                new Circuit("hi1", new DateTime(new DateOnly(2024, 2, 22), new TimeOnly(7, 30)), 1),
                new Circuit("ai1", new DateTime(new DateOnly(2024, 3, 22), new TimeOnly(6, 30)), 0),
                new Circuit("ei1", new DateTime(new DateOnly(2024, 3, 22), new TimeOnly(7, 30)), 2)
            };

            dispCircuits = circuits;

            searchSelected = false;
            filterSelected = false;
            sortSelected = false;

            //firstOrderChecked = false;
            //dcChecked = false;
            //acChecked = false;
            //faveChecked = false;

            filterChecks = new bool[] { false, false, false, false };

            maxDate = DateTime.Now;

            sortTypeSelection = "1";
            sortOrderSelection = "1";

            searchNameText = "";
            searchDate = DateTime.Today;
            filterLabel = "";

            dispCircuits.OrderByDescending(a => a.DateTime);
        }

        public object SortTypeSelection
        {
            get => sortTypeSelection;
            set
            {
                sortTypeSelection = value;
                DebugString = DebugString + (string)sortTypeSelection;
                OnPropertyChanged(nameof(sortTypeSelection));
            }
        }

        public object SortOrderSelection
        {
            get => sortOrderSelection;
            set
            {
                sortOrderSelection = value;
                DebugString = DebugString + (string)sortOrderSelection;
                OnPropertyChanged(nameof(sortOrderSelection));
            }
        }

        [RelayCommand]
        public void Delete(Circuit c)
        {
            if (Circuits.Contains(c))
            {
                Circuits.Remove(c);
                Debug.WriteLine("Removed");
            }

            Debug.WriteLine("Not Removed");
        }

        [RelayCommand]
        public void Favorite(Circuit c)
        {
            c.IsFave = !c.IsFave;
        }

        [RelayCommand]
        public void SearchForName()
        {
            if (SearchNameText.Trim().Length == 0)
            {
                return;
            }

            Circuit[] temp = new Circuit[DispCircuits.Count];
            DispCircuits.CopyTo(temp, 0);
            DispCircuits = new ObservableCollection<Circuit>();

            foreach (Circuit c in temp)
            {
                if (c.Name.Contains(SearchNameText.Trim()))
                {
                    DispCircuits.Add(c);
                }
            }

            SearchNameLabel = "Keyword: \"" + SearchNameText + "\"";
            NameSearched = true;
            SearchSelected = false;
        }

        [RelayCommand]
        public void SearchForDate()
        {
            Circuit[] temp = new Circuit[DispCircuits.Count];
            DispCircuits.CopyTo(temp, 0);
            DispCircuits = new ObservableCollection<Circuit>();

            foreach (Circuit c in temp)
            {
                if (c.DateTime.Equals(SearchDate))
                {
                    DispCircuits.Add(c);
                }
            }

            SearchDateLabel = "Created on: " + SearchDate.ToString() + "\"";
            DateSearched = true;
            SearchSelected = false;
        }

        [RelayCommand]
        public void ClearNameSearch()
        {
            DispCircuits = Circuits;
            NameSearched = false;
            SearchNameText = "";
            SearchNameLabel = "";

            if (DateSearched)
            {
                SearchForDate();
            }

            if (FilterOn)
            {
                Filter();
            }

            Sort();
        }

        [RelayCommand]
        public void ClearDateSearch()
        {
            DispCircuits = Circuits;
            DateSearched = false;
            SearchDate = DateTime.Today;
            SearchDateLabel = "";

            if (NameSearched)
            {
                SearchForName();
            }

            if (FilterOn)
            {
                Filter();
            }

            Sort();
        }

        [RelayCommand]
        public void Filter()
        {
            FilterSelected = false;

            if (!FilterChecks.Contains(true))
            {
                FilterOn = false;
                return;
            }

            FilterOn = true;
            Circuit[] temp = new Circuit[DispCircuits.Count];
            DispCircuits.CopyTo(temp, 0);
            DispCircuits = new ObservableCollection<Circuit>();
            FilterLabel = "Filtering by: ";

            for (int i = 0; i < 4; i++)
            {
                if (FilterChecks[i])
                {
                    FilterLabel += Circuit.TYPES[i] + ", ";
                    foreach (Circuit c in temp)
                    {
                        if (!DispCircuits.Contains(c))
                        {
                            if (c.Type == i)
                            {
                                DispCircuits.Add(c);
                            }
                        }
                    }
                }
            }
        }

        [RelayCommand]
        public void ClearFilter()
        {
            FilterSelected = false;

            DispCircuits = Circuits;
            
            if (NameSearched)
            {
                SearchForName();
            }

            if (DateSearched)
            {
                SearchForDate();
            }

            Sort();
            FilterOn = false;
            FilterChecks[0] = false;
            FilterChecks[1] = false;
            FilterChecks[2] = false;
            FilterChecks[3] = false;
        }

        [RelayCommand]
        public async Task Sort()
        {
            try
            {
                SortSelected = false;

                if (sortTypeSelection == null)
                {
                    sortTypeSelection = "1";
                }

                if (sortOrderSelection == null)
                {
                    sortOrderSelection = "1";
                }

                List<Circuit> ordered = DispCircuits.ToList();

                if ((string)sortTypeSelection == "0")
                {
                    if ((string)sortOrderSelection == "0")
                    {
                        DebugString = "0";

                        ordered = ordered.OrderBy(c => c.Name).ToList();
                        DispCircuits = new ObservableCollection<Circuit>();
                        DebugString = "ordered" + ordered.Count;
                    }
                    else
                    {
                        DebugString = "1";

                        ordered = ordered.OrderByDescending(c => c.Name).ToList();
                        DispCircuits = new ObservableCollection<Circuit>();

                        DebugString = "ordered" + ordered.Count;
                    }
                }
                else
                {
                    if ((string)sortOrderSelection == "0")
                    {
                        DebugString = "2";

                        ordered = ordered.OrderBy(c => c.DateTime).ToList();
                        DispCircuits = new ObservableCollection<Circuit>();
                    }
                    else
                    {
                        DebugString = "3";

                        ordered = ordered.OrderByDescending(c => c.DateTime).ToList();
                        DispCircuits = new ObservableCollection<Circuit>();
                    }
                }

                foreach (Circuit c in ordered)
                {
                    DispCircuits.Add(c);
                }

                DebugString += " , " + DispCircuits.Count;
                SortSelected = false;
            }
            catch (Exception e)
            {
                await Shell.Current.DisplayAlert("Error", e.Message, "OK");
            }
        }

        [RelayCommand]
        public void FilterClicked()
        {
            FilterSelected = !FilterSelected;
        }

        [RelayCommand]
        public void SearchClicked()
        {
            SearchSelected = !SearchSelected;
        }

        [RelayCommand]
        public void SortClicked()
        {
            SortSelected = !SortSelected;
        }

        [RelayCommand]
        public async Task Test(Circuit c)
        {
            c.IsFave = true;
            await Shell.Current.DisplayAlert("1", "2", "3");
        }

        [RelayCommand]
        async Task GoToCircuit(Circuit circuit)
        {
            if (circuit == null)
            {
                return;
            }

            await Shell.Current.GoToAsync($"{nameof(CircPage)}", 
                new Dictionary<string, object>
                {
                    {"Circuit", circuit }
                });

            //await Shell.Current.GoToAsync($"{nameof(CircPage)}");
        }

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
