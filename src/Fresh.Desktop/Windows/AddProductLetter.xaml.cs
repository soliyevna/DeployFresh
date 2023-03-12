﻿using Fresh.Domain.Entities;
using Fresh.Service.Attributes;
using Fresh.Service.Director;
using Fresh.Service.Services.Empolyee;
using Fresh.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Fresh.Desktop.Windows
{
    public partial class AddProductLetter : Window
    {
        private IList<VievModelProductLetter> vievModelProductLetters = new List<VievModelProductLetter>();
        private ObservableCollection<VievModelProductLetter> cassaData = new ObservableCollection<VievModelProductLetter>();
        private IList<Product> products = new List<Product>();
        private IList<ConsignmentLetterView> letter = new List<ConsignmentLetterView>();

        ObservableCollection<string> strings = new ObservableCollection<string>();
        public AddProductLetter()
        {
            InitializeComponent();
            Category_ComboBox();
        }

        private async void Category_ComboBox()
        {
            DirectorProductService directorCategoryService = new DirectorProductService();
            var resault = directorCategoryService.GetAllAsync();
            foreach (var item in await resault)
            {
                strings.Add(item.Name);

            }
            txtProduct.ItemsSource = strings;
        }

        private void GridRefresh()
        {
            DataGridCassaLetter.ItemsSource = cassaData;
        }

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
           
            if (txtProduct.Text.Length > 0 && txtPrice.Text.Length > 0 && txtTotal.Text.Length > 0 && txtKgL.Text.Length > 0 &&
                double.Parse(txtTotal.Text) > 0 && double.Parse(txtPrice.Text) > 0)
            {
                VievModelProductLetter vievModelProductLetter = new VievModelProductLetter();
                vievModelProductLetter.Name = txtProduct.Text;
                vievModelProductLetter.KgL = txtKgL.Text;
                vievModelProductLetter.Total = txtTotal.Text;
                vievModelProductLetter.Price = txtPrice.Text;
                vievModelProductLetter.TotalPrice = double.Parse(txtTotal.Text.ToString()) * double.Parse(txtPrice.Text.ToString());
                vievModelProductLetters.Add(vievModelProductLetter);

                cassaData.Add(new VievModelProductLetter { Name = txtProduct.Text, KgL = txtKgL.Text, Total = txtTotal.Text, Price = txtPrice.Text });
                GridRefresh();

                txtProduct.Text = null;
                txtKgL.Text = null;
                txtTotal.Text = null;
                txtPrice.Text = null;
            }
            else
            {
                MessageBox.Show("Ma'luot to'liq kiritilmagan");
            }
        }


        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var res = (VievModelProductLetter)DataGridCassaLetter.SelectedItem;
            cassaData.Remove(res);
            txtProduct.Text = null;
            txtKgL.Text = null;
            txtTotal.Text = null;
            txtPrice.Text = null;
            GridRefresh();
        }

        private async void Close_Click(object sender, RoutedEventArgs e)
        {
            vievModelProductLetters.Clear();
            txtProduct.Text = null;
            txtKgL.Text = null;
            txtTotal.Text = null;
            txtPrice.Text = null;
            cassaData.Clear();
            Cassa cassa = new Cassa();
            cassa.Show();
            this.Close();
        }

        private async void Accept_Click(object sender, RoutedEventArgs e)
        {

            if (cassaData.Count > 0)
            {
                string checkDescription = "";
                double price = 0;
                Product product = new Product();
                ConsignmentLetterView consignmentLetterView = new ConsignmentLetterView();
                foreach (var view in vievModelProductLetters)
                {
                    double price2 = double.Parse(view.Price) * double.Parse(view.Total);
                    checkDescription += $"{view.Name}        {view.Total} {view.KgL}      {price2}\n";
                    product.Value = float.Parse(view.Total);
                    product.Name = view.Name;
                    product.Price = float.Parse(view.Price);
                    products.Add(product);
                    price += price2;
                }

                Fresh.Domain.Entities.ProductLetter check = new Fresh.Domain.Entities.ProductLetter();
                check.ProductDescription = checkDescription;
                check.Date = DateTime.Now.ToString();
                check.UserId = GlobalVariable.Id;
                check.Price = (float)price;
               
                checkDescription += $"\nTime: {check.Date}\n\n Total Money: {price}\n\n Vendor: {GlobalVariable.Name}";

                MessageBox.Show($"{checkDescription}");
                EmpolyeeProductLetterService empolyeeProductLetterService = new EmpolyeeProductLetterService();
                var res =  await empolyeeProductLetterService.CreateAsync(check);
               
                vievModelProductLetters.Clear();
                txtProduct.Text = null;
                txtKgL.Text = null;
                txtTotal.Text = null;
                txtPrice.Text = null;
                cassaData.Clear();
                GridRefresh();
            }
        }

        private async void NotAccept_Click(object sender, RoutedEventArgs e)
        {
            cassaData.Clear();
            vievModelProductLetters.Clear();
            txtProduct.Text = null;
            txtKgL.Text = null;
            txtTotal.Text = null;
            txtPrice.Text = null;
            GridRefresh();
        }

        private async void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private async void NewProduct_Click(object sender, RoutedEventArgs e)
        {
            AddProducts addProducts = new AddProducts();
            addProducts.Show();
        }

        private async void Refresh_Click(object sender, RoutedEventArgs e)
        {
            Category_ComboBox();
        }
    }
}
