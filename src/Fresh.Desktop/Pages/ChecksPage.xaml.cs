﻿using CG.Web.MegaApiClient;
using Fresh.Desktop.Windows;
using Fresh.Service.Director;
using Fresh.Service.Services.PageServices;
using Fresh.Service.ViewModels;
using Fresh.Service.ViewModels.ViewDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Input;

namespace Fresh.Desktop.Pages
{
    /// <summary>
    /// Interaction logic for ChecksPage.xaml
    /// </summary>
    public partial class ChecksPage : Page
    {
        public ChecksPage()
        {
            InitializeComponent();
            DataContext = this;
            Click();

        }
        public static List<CheckDetailsView> checkDetailsView = new();

        private void comboBoxCashiers_Loaded(object sender, RoutedEventArgs e)
        {

        }
        public async void Click()
        {
            CheckPage check = new CheckPage();
            List<ChecksView> ChecksPages = await check.GetChecksViews();
            DirectorRegisterService directorRegisterService = new();
            var users = await directorRegisterService.GetAllAsync();
            if (usersNameCombo.Text == null)
            {
                ProductsDgUi.ItemsSource = (await check.GetChecksViews()).OrderByDescending(x => DateTime.Parse(x.Date));
                return;
            }
            var view = new List<string>();
            foreach(var user in users)
                view.Add(user.FullName);
            usersNameCombo.ItemsSource = view;
            if (usersNameCombo.Text.Length == 0 && datePicker.Text.Length > 0)
            {
                var dateTime = DateTime.Parse(datePicker.Text);
                ProductsDgUi.ItemsSource = ChecksPages.OrderByDescending(x => DateTime.Parse(x.Date))
                    .Where(x => DateTime.Parse(x.Date).Date == dateTime.Date);
            }
            else if (usersNameCombo.Text.Length > 0 && datePicker.Text.Length == 0)
                ProductsDgUi.ItemsSource = ChecksPages.OrderByDescending(x => DateTime.Parse(x.Date))
                    .Where(x => x.Caisher == usersNameCombo.Text);
            else if (usersNameCombo.Text.Length > 0 && datePicker.Text.Length > 0)
                ProductsDgUi.ItemsSource = ChecksPages.OrderByDescending(x => DateTime.Parse(x.Date))
                    .Where(x => x.Caisher == usersNameCombo.Text && DateTime.Parse(x.Date).Date == DateTime.Parse(datePicker.Text).Date);
            else
                ProductsDgUi.ItemsSource = (await check.GetChecksViews()).OrderByDescending(x => DateTime.Parse(x.Date));
        }
        private void ProductsDgUi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ButtonAutomationPeer peer = new ButtonAutomationPeer(hiddenHelper);
            IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
            invokeProv.Invoke();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ButtonAutomationPeer peer = new ButtonAutomationPeer(hiddenHelper);
            IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
            invokeProv.Invoke();
        }

        private void hiddenHelper_Click(object sender, RoutedEventArgs e)
        {
            Click();
        }

        private async void RowDouble_Clicked(object sender, MouseButtonEventArgs e)
        {
            ChecksView checksView = (ChecksView)ProductsDgUi.SelectedItem;
            CheckPage checkPage = new();
            checkDetailsView = await checkPage.GetCheckDetailsById(checksView.Id);
            ChecksDescription checksDescription = new ChecksDescription();
            checksDescription.ShowDialog();
        }

        private void btnSaveToCloud_Click(object sender, RoutedEventArgs e)
        {
            MegaApiClient client = new MegaApiClient();
            try
            {
                client.Login("saparbaevazulaykho18@gmail.com", "GoodLuck18041388");
                IEnumerable<INode> nodes = client.GetNodes();

                INode root = nodes.Single(x => x.Type == NodeType.Root);
                INode myFolder = client.CreateFolder($"{DateTime.Now.ToString("MM/yyyy")}", root);

                INode myFile = client.UploadFile(@"../../../../../database/fresh-market.db", myFolder);
                Uri downloadLinkImage = client.GetDownloadLink(myFile);

                MessageBox.Show("Added successfully!");
                client.Logout();

            }
            catch
            {
                MessageBox.Show("Something went wrong. Check your internet connection.");
            }
        }
    }
}
