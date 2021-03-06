﻿using System;

namespace Layex.ViewModels
{
    public sealed class ViewModelEventArgs : EventArgs
    {
        public ViewModelEventArgs(IViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public IViewModel ViewModel { get; private set; }

        internal static void RaiseEvent(EventHandler<ViewModelEventArgs> handler, object sender, IViewModel viewModel)
        { 
            if (handler != null)
            {
                handler(sender, new ViewModelEventArgs(viewModel));
            }
        }
    }
}
