﻿using Caliburn.Micro;
using System;

namespace Layex.ViewModels
{
    [ViewModel("")]
    public sealed class ApplicationViewModel : ItemsViewModel
    {
        private readonly IWindowManager _windowManager;

        public ApplicationViewModel(IWindowManager windowManager, IDependencyContainer dependencyContainer)
        {
            _windowManager = windowManager;
            ((IRequireDependencyContainer)this).Configure(dependencyContainer);
        }

        public void Initialize()
        {
            OnInitialize();
        }

        public override void ActivateItem(IViewModel item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            bool newItem = !Items.Contains(item);
            if (newItem)
            {
                item.Deactivated += OnItemDeactivated;
            }
            base.ActivateItem(item);
            if (newItem)
            {
                if (item is IDialogViewModel)
                {
                    _windowManager.ShowDialog(item);
                }
                else
                {
                    _windowManager.ShowWindow(item);
                }
            }
        }

        private void OnItemDeactivated(object sender, DeactivationEventArgs e)
        {
            if (e.WasClosed)
            {
                IViewModel viewModel = (IViewModel)sender;
                viewModel.Deactivated -= OnItemDeactivated;
                Items.Remove(viewModel);
                IDisposable disposable = viewModel as IDisposable;
                disposable?.Dispose();
            }
        }
    }
}
