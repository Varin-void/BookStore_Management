using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace BookStore_Final_Project
{
    public partial class SellerPanel : Window
    {
        private DbBookStore dbContext;
        private List<Purchase> pendingPurchases; 
        private ObservableCollection<Purchase> tempPurchases;
        public SellerPanel()
        {
            InitializeComponent();
            dbContext = new DbBookStore();
            LoadBooks();
            LoadPurchases();
            pendingPurchases = new List<Purchase>();
            tempPurchases = new ObservableCollection<Purchase>();
            dgTempPurchases.ItemsSource = tempPurchases;
        }

        #region WinControlRegion
        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void BtnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
                this.WindowState = WindowState.Normal;
            else
                this.WindowState = WindowState.Maximized;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        #endregion

        #region PanelButtonRegion
        private void exit_purchase_Click(object sender, RoutedEventArgs e)
        {
            MainWindow login_window = new MainWindow();
            login_window.Show();
            this.Close();
        }

        private void btnBestSelling_Click(object sender, RoutedEventArgs e)
        {
            var bestSellingBook = dbContext.Purchases
                .GroupBy(purchase => purchase.Books)
                .OrderByDescending(group => group.Sum(purchase => purchase.Quantity))
                .FirstOrDefault();

            if (bestSellingBook != null)
            {
                MessageBox.Show($"Best Selling Book: {bestSellingBook.Key.Title}\nTotal Quantity Sold: {bestSellingBook.Sum(purchase => purchase.Quantity)}", "Best Selling Book", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("No purchases found.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void show_purchase_Click(object sender, RoutedEventArgs e)
        {
            //Change the display of Datagrid
            dgPurchases.Visibility = Visibility.Collapsed;
            dgTempPurchases.Visibility = Visibility.Visible;
        }

        private void btnBestSellingAuthor_Click(object sender, RoutedEventArgs e)
        {
            // Check if there are any purchases in the database
            if (dbContext.Purchases.Any())
            {
                // Retrieve the best-selling author from the database
                var bestSellingAuthor = dbContext.Authors
                    .OrderByDescending(a => a.Books.Sum(b => b.Purchases.Sum(p => p.Quantity)))
                    .FirstOrDefault();

                if (bestSellingAuthor != null)
                {
                    MessageBox.Show($"Best Selling Author: {bestSellingAuthor.FirstName} {bestSellingAuthor.LastName}", "Best Selling Author", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("No best-selling author found.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("No purchases available.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnNewReleases_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve the newest book's publish date from the database
            var newestPublishDate = dbContext.Books
                .Max(b => b.PublishDate);

            // Retrieve the books with the newest publish date
            var newestBooks = dbContext.Books
                .Where(b => b.PublishDate == newestPublishDate)
                .ToList();

            if (newestBooks.Count > 0)
            {
                // Display the newest books
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("New Releases:");
                foreach (var book in newestBooks)
                {
                    sb.AppendLine($"Title: {book.Title}, Publish Date: {book.PublishDate}");
                }
                MessageBox.Show(sb.ToString(), "New Releases", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("No new releases found.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnPopularGenre_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve the genre of the most purchased books from the database
            var popularGenre = dbContext.Books
                .Where(b => b.Purchases.Any())
                .GroupBy(b => b.Genres)
                .OrderByDescending(g => g.Sum(b => b.Purchases.Sum(p => p.Quantity)))
                .Select(g => g.Key.Description)
                .FirstOrDefault();

            if (popularGenre != null)
            {
                MessageBox.Show($"Popular Genre: {popularGenre.ToString()}", "Popular Genre", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("No popular genre found.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        #endregion

        #region PurchaseGridControlRegion

        private void AddPurchase_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve values from input fields
            string saleDate = dpSaleDate.SelectedDate.ToString();
            int quantity;
            Book selectedBook = cmbBooks.SelectedItem as Book;

            // Validate input fields
            if (string.IsNullOrEmpty(saleDate) || selectedBook == null || !int.TryParse(txtQuantity.Text, out quantity))
            {
                MessageBox.Show("Please complete all the fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Calculate the total cost
            int totalCost = selectedBook.Price * quantity;

            // Check if the quantity is greater than the available stock
            if (quantity > selectedBook.Stock)
            {
                MessageBox.Show("Insufficient stock available.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Create a new Purchase object
            Purchase newPurchase = new Purchase
            {
                SaleDate = saleDate,
                Quantity = quantity,
                TotalCost = totalCost,
                Books = selectedBook
            };

            // Update the stock of the book
            selectedBook.Stock -= quantity;

            // Add the new purchase to the pending purchases
            pendingPurchases.Add(newPurchase);
            tempPurchases.Add(newPurchase);

            // Clear the input fields
            ClearFields();
        }

        private void CompletePurchase_Click(object sender, RoutedEventArgs e)
        {
            if (pendingPurchases.Count > 0)
            {
                // Calculate the subtotal of all the purchases
                decimal subtotal = pendingPurchases.Sum(purchase => purchase.TotalCost);

                // Show message box with purchase information and subtotal
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Confirm Purchases:");
                foreach (var purchase in pendingPurchases)
                {
                    sb.AppendLine($"Book: {purchase.Books.Title}, Quantity: {purchase.Quantity}, Books Cost: {purchase.TotalCost}$");
                }
                sb.AppendLine($"Subtotal: {subtotal}$");

                MessageBox.Show(sb.ToString(), "Purchase Information", MessageBoxButton.OK, MessageBoxImage.Information);

                // Add the pending purchases to the database
                foreach (var purchase in pendingPurchases)
                {
                    dbContext.Purchases.Add(purchase);
                }
                dbContext.SaveChanges();

                // Clear the pending purchases list
                pendingPurchases.Clear();
                tempPurchases.Clear(); // Clear tempPurchases as well

                // Refresh the data grid
                LoadPurchases();
            }
            else
            {
                MessageBox.Show("No pending purchases to complete.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ViewAllPurchases_Click(object sender, RoutedEventArgs e)
        {
            LoadPurchases();
            dgTempPurchases.Visibility = Visibility.Collapsed;
            dgPurchases.Visibility = Visibility.Visible;
        }

        private void LoadBooks()
        {
            cmbBooks.ItemsSource = dbContext.Books.ToList();
            
        }

        private void cmbBooks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Book selectedBook = cmbBooks.SelectedItem as Book;
            if (selectedBook != null)
            {
                price_book.Text = $"{selectedBook.Price}$";
            }
        }

        private void LoadPurchases()
        {
            dgPurchases.ItemsSource = dbContext.Purchases.ToList();
        }

        private void ClearFields()
        {
            dpSaleDate.SelectedDate = null;
            txtQuantity.Text = string.Empty;
            cmbBooks.SelectedIndex = -1;
        }

        #endregion



    }
}