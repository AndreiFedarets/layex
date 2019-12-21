﻿using System;

namespace Adenium.ViewModels
{
    public sealed class ViewModelEventArgs : EventArgs
    {
        public ViewModelEventArgs(IViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public IViewModel ViewModel { get; private set; }
    }
}
