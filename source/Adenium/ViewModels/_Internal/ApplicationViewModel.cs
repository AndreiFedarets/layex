﻿using Caliburn.Micro;

namespace Adenium.ViewModels
{
    [ViewModel("Application")]
    internal sealed class ApplicationViewModel : LayoutedItemsViewModel
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
            bool newItem = !Items.Contains(item);
            if (newItem)
            {
                item.Deactivated += OnItemDeactivated;
            }
            base.ActivateItem(item);
            if (newItem)
            {
                _windowManager.ShowWindow(item);
            }
        }

        private void OnItemDeactivated(object sender, DeactivationEventArgs e)
        {
            if (e.WasClosed)
            {
                IViewModel viewModel = (IViewModel)sender;
                viewModel.Deactivated -= OnItemDeactivated;
                Items.Remove(viewModel);
            }
        }
    }
}