﻿using CircuitSolver.View;

namespace CircuitSolver
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
