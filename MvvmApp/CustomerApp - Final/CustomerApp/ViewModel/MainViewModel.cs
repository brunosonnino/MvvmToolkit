using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CustomerLib;
using System;
using System.Collections.Generic;
using System.Windows.Data;
using System.Windows.Input;

namespace CustomerApp.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {

        ICustomerRepository _customerRepository;

        [ObservableProperty]
        private Customer _selectedCustomer;

        [ObservableProperty]
        private string _searchText;

        public IEnumerable<Customer> Customers => _customerRepository.Customers;

        
        public MainViewModel(ICustomerRepository customerRepository)
        {
            if (customerRepository == null)
                throw new ArgumentNullException("customerRepository");
            _customerRepository = customerRepository;
        }

        [RelayCommand]
        private void Add()
        {
            var customer = new Customer();
            _customerRepository.Add(customer);
            SelectedCustomer = customer;
            OnPropertyChanged("Customers");
        }

        [RelayCommand]
        private void Remove()
        {
            if (SelectedCustomer != null)
                _customerRepository.Remove(SelectedCustomer);
            SelectedCustomer = null;
            OnPropertyChanged("Customers");
        }

        [RelayCommand]
        private void Save()
        {
            _customerRepository.Commit();
        }

        [RelayCommand]
        private void Search()
        {
            var coll = CollectionViewSource.GetDefaultView(Customers);
            if (!string.IsNullOrWhiteSpace(SearchText))
                coll.Filter = c => ((Customer)c).Country.ToLower().Contains(SearchText.ToLower());
            else
                coll.Filter = null;
        }   
    }
}
