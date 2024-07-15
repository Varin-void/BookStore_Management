using System;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Windows;
using System.Windows.Controls;

namespace BookStore_Final_Project
{
    /// <summary>
    /// Interaction logic for AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : Window
    {
        DbBookStore db = new DbBookStore();
        public AdminPanel()
        {
            InitializeComponent();
        }


        #region BasicWinControlRegion
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

        private void show_user_Click(object sender, RoutedEventArgs e)
        {
            ClearUserFields();
            user_grid.Visibility = Visibility.Visible;
            author_grid.Visibility = Visibility.Collapsed;
            book_grid.Visibility = Visibility.Collapsed;
            Background_image.Visibility = Visibility.Collapsed;
            promotion_grid.Visibility = Visibility.Collapsed;
            transaction_grid.Visibility = Visibility.Collapsed;
        }

        private void show_author_Click(object sender, RoutedEventArgs e)
        {
            user_grid.Visibility = Visibility.Collapsed;
            author_grid.Visibility = Visibility.Visible;
            book_grid.Visibility = Visibility.Collapsed;
            Background_image.Visibility = Visibility.Collapsed;
            promotion_grid.Visibility = Visibility.Collapsed;
            transaction_grid.Visibility = Visibility.Collapsed;
        }

        private void show_book_Click(object sender, RoutedEventArgs e)
        {
            ClearBookFields();
            user_grid.Visibility = Visibility.Collapsed;
            author_grid.Visibility = Visibility.Collapsed;
            book_grid.Visibility = Visibility.Visible;
            Background_image.Visibility = Visibility.Collapsed;
            promotion_grid.Visibility = Visibility.Collapsed;
            transaction_grid.Visibility = Visibility.Collapsed;
        }

        private void show_Discount_Click(object sender, RoutedEventArgs e)
        {
            user_grid.Visibility = Visibility.Collapsed;
            author_grid.Visibility = Visibility.Collapsed;
            book_grid.Visibility = Visibility.Collapsed;
            Background_image.Visibility = Visibility.Collapsed;
            promotion_grid.Visibility = Visibility.Visible;
            transaction_grid.Visibility = Visibility.Collapsed;
        }

        private void show_Transaction_Click(object sender, RoutedEventArgs e)
        {
            user_grid.Visibility = Visibility.Collapsed;
            author_grid.Visibility = Visibility.Collapsed;
            book_grid.Visibility = Visibility.Collapsed;
            Background_image.Visibility = Visibility.Collapsed;
            promotion_grid.Visibility = Visibility.Collapsed;
            transaction_grid.Visibility = Visibility.Visible;
        }

        private void exit_panel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow login_window = new MainWindow();
            login_window.Show();
            this.Close();
        }

        #endregion


        #region LoadDataRegion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        public void LoadData()
        {
            //Clear combo box in-case there is duplicate objects
            ClearComboBox();

            //User data load
            var users = db.Users.ToList();
            userDatagrid.ItemsSource = users;

            //Author data load
            var author = db.Authors.ToList();
            authorDatagrid.ItemsSource = author;

            foreach (var item in author)
            {
                string tempAuthor = item.FirstName + " " + item.LastName;
                if (!author_list.Items.Contains(tempAuthor))
                    author_list.Items.Add(tempAuthor);
            }

            //Publisher data load
            var publisher = db.Publishers.ToList();
            foreach (var item in publisher)
            {
                if (!publisher_list.Items.Contains(item.PublisherName))
                    publisher_list.Items.Add(item.PublisherName);
            }

            //Genres data load
            var genre = db.Genres.ToList();
            foreach (var item in genre)
            {
                if (!genre_list.Items.Contains(item.Description))
                    genre_list.Items.Add(item.Description);
            }

            //Books data load
            var book = db.Books.ToList();
            bookDatagrid.ItemsSource = book;
            foreach (var item in book)
            {
                if (!book_list.Items.Contains(item.Title))
                    book_list.Items.Add(item.Title);
            }

            //Discount data load
            var promotions = db.Discounts.Include("Books").ToList();
            promotionDatagrid.ItemsSource = promotions;

            //Transaction data load
            var purchases = db.Purchases.Include("Books").ToList();
            transactionDatagrid.ItemsSource = purchases;

        }

        public void ClearComboBox()
        {
            author_list.Items.Clear();
            publisher_list.Items.Clear();
            genre_list.Items.Clear();
            book_list.Items.Clear();
        }
        #endregion


        #region BookGridRegion
        private void add_book_Click(object sender, RoutedEventArgs e)
        {
            // Check if any required field is empty
            if (string.IsNullOrWhiteSpace(title_txt.Text) ||
                string.IsNullOrWhiteSpace(page_txt.Text) ||
                string.IsNullOrWhiteSpace(prime_txt.Text) ||
                string.IsNullOrWhiteSpace(price_txt.Text) ||
                string.IsNullOrWhiteSpace(stock_txt.Text) ||
                author_list.SelectedItem == null ||
                publisher_list.SelectedItem == null ||
                genre_list.SelectedItem == null ||
                pick_date.SelectedDate == null)
            {
                MessageBox.Show("Please fill in all the required fields.");
                return;
            }
            // Retrieve the values from the input fields
            string title = title_txt.Text;
            int pages = int.Parse(page_txt.Text);
            DateTime publishDate = pick_date.SelectedDate.Value;
            int primeCost = int.Parse(prime_txt.Text);
            int price = int.Parse(price_txt.Text);
            bool sequel = sequel_list.SelectedItem.ToString() == "Yes";
            int stock = int.Parse(stock_txt.Text);
            Author author = db.Authors.FirstOrDefault(a => (a.FirstName + " " + a.LastName) == author_list.SelectedItem.ToString());
            Publisher publisher = db.Publishers.FirstOrDefault(p => p.PublisherName == publisher_list.SelectedItem.ToString());
            Genre genre = db.Genres.FirstOrDefault(g => g.Description == genre_list.SelectedItem.ToString());

            // Create a new Book instance
            Book newBook = new Book
            {
                Title = title,
                Pages = pages,
                PublishDate = publishDate,
                PrimePrice = primeCost,
                Price = price,
                Sequel = sequel,
                Stock = stock,
                Publisher = publisher,
                Genres = genre
            };

            // Add the author to the book's Authors collection
            newBook.Authors.Add(author);

            // Add the new book to the database
            db.Books.Add(newBook);
            db.SaveChanges();

            // Refresh the data
            LoadData();

            //Clear Book Fields
            ClearBookFields();

            // Display a success message or perform any other desired actions
            MessageBox.Show("Book added successfully!");
        }

        private void edit_book_Click(object sender, RoutedEventArgs e)
        {
            // Check if any required field is empty
            if (string.IsNullOrWhiteSpace(title_txt.Text) ||
                string.IsNullOrWhiteSpace(page_txt.Text) ||
                string.IsNullOrWhiteSpace(prime_txt.Text) ||
                string.IsNullOrWhiteSpace(price_txt.Text) ||
                string.IsNullOrWhiteSpace(stock_txt.Text) ||
                author_list.SelectedItem == null ||
                publisher_list.SelectedItem == null ||
                genre_list.SelectedItem == null ||
                pick_date.SelectedDate == null)
            {
                MessageBox.Show("Please fill in all the required fields.");
                return;
            }
            // Get the selected book from the data grid
            Book selectedBook = bookDatagrid.SelectedItem as Book;
            if (selectedBook == null)
            {
                MessageBox.Show("Please select a book to edit.");
                return;
            }

            // Retrieve the updated values from the input fields
            string title = title_txt.Text;
            int pages = int.Parse(page_txt.Text);
            DateTime publishDate = pick_date.SelectedDate.Value;
            int primeCost = int.Parse(prime_txt.Text);
            int price = int.Parse(price_txt.Text);
            bool sequel = sequel_list.SelectedItem.ToString() == "Yes";
            int stock = int.Parse(stock_txt.Text);
            Author author = db.Authors.FirstOrDefault(a => (a.FirstName + " " + a.LastName) == author_list.SelectedItem.ToString());
            Publisher publisher = db.Publishers.FirstOrDefault(p => p.PublisherName == publisher_list.SelectedItem.ToString());
            Genre genre = db.Genres.FirstOrDefault(g => g.Description == genre_list.SelectedItem.ToString());

            // Update the selected book with the new values
            selectedBook.Title = title;
            selectedBook.Pages = pages;
            selectedBook.PublishDate = publishDate;
            selectedBook.PrimePrice = primeCost;
            selectedBook.Price = price;
            selectedBook.Sequel = sequel;
            selectedBook.Stock = stock;
            selectedBook.Publisher = publisher;
            selectedBook.Genres = genre;
            selectedBook.Authors.Clear();
            selectedBook.Authors.Add(author);

            // Save the changes to the database
            db.SaveChanges();

            // Refresh the data
            LoadData();

            ClearBookFields();

            // Display a success message or perform any other desired actions
            MessageBox.Show("Book updated successfully!");
        }

        private void delete_book_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected book from the data grid
            Book selectedBook = bookDatagrid.SelectedItem as Book;
            if (selectedBook == null)
            {
                MessageBox.Show("Please select a book to delete.");
                return;
            }

            // Confirm the deletion with the user
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this book?", "Confirm Deletion", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    // Remove the selected book from the database
                    db.Books.Remove(selectedBook);
                    db.SaveChanges();

                    // Refresh the data
                    LoadData();

                    // Display a success message or perform any other desired actions
                    MessageBox.Show("Book deleted successfully!");
                }
                catch (Exception ex)
                {
                    // Handle any exceptions that occur during deletion
                    MessageBox.Show("An error occurred while deleting the book: " + ex.Message);
                }
            }
        }

        private void bookDatagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get the selected book from the data grid
            Book selectedBook = bookDatagrid.SelectedItem as Book;
            if (selectedBook != null)
            {
                // Populate the fields with the selected book's data
                title_txt.Text = selectedBook.Title;
                page_txt.Text = selectedBook.Pages.ToString();
                pick_date.SelectedDate = selectedBook.PublishDate;
                prime_txt.Text = selectedBook.PrimePrice.ToString();
                price_txt.Text = selectedBook.Price.ToString();
                sequel_list.SelectedItem = selectedBook.Sequel ? "Yes" : "No";
                stock_txt.Text = selectedBook.Stock.ToString();

                // Set the selected author in the ComboBox
                Author selectedAuthor = selectedBook.Authors.FirstOrDefault();
                if (selectedAuthor != null)
                {
                    string authorFullName = $"{selectedAuthor.FirstName}" + " " + $"{selectedAuthor.LastName}";
                    author_list.SelectedItem = authorFullName;
                }

                // Set the selected publisher in the ComboBox
                publisher_list.SelectedItem = selectedBook.Publisher?.PublisherName;

                // Set the selected genre in the ComboBox
                genre_list.SelectedItem = selectedBook.Genres?.Description;
            }
        }

        private void clear_all_Click(object sender, RoutedEventArgs e)
        {
            ClearBookFields();
        }

        private void ClearBookFields()
        {
            title_txt.Text = "";
            page_txt.Text = "";
            prime_txt.Text = "";
            price_txt.Text = "";
            stock_txt.Text = "";
            author_list.SelectedItem = null;
            publisher_list.SelectedItem = null;
            genre_list.SelectedItem = null;
            pick_date.SelectedDate = null;
            sequel_list.SelectedItem = null;
        }
        #endregion


        #region UserGridRegion

        private void add_user_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(user_txt.Text) ||
              string.IsNullOrWhiteSpace(password_txt.Text) ||
              role_list.SelectedItem == null)
            {
                MessageBox.Show("Please fill in all the required fields.");
                return;
            }

            // Retrieve the values from the input fields
            string user_name = user_txt.Text;
            string pass_word = password_txt.Text;
            string role = (role_list.SelectedItem as ComboBoxItem)?.Content.ToString();

            // Create a new User instance
            User newUser = new User
            {
                userName = user_name,
                passWord = pass_word,
                role = role
            };

            // Add the new User to the database
            db.Users.Add(newUser);
            db.SaveChanges();

            // Refresh the data
            LoadData();

            ClearUserFields();

            // Display a success message or perform any other desired actions
            MessageBox.Show("User added successfully!");
        }

        private void edit_user_Click(object sender, RoutedEventArgs e)
        {
            // Check if any required field is empty
            if (string.IsNullOrWhiteSpace(user_txt.Text) ||
                string.IsNullOrWhiteSpace(password_txt.Text) ||
                role_list.SelectedItem == null)
            {
                MessageBox.Show("Please fill in all the required fields.");
                return;
            }
            // Get the selected User from the data grid
            User selectedUser = userDatagrid.SelectedItem as User;
            if (selectedUser == null)
            {
                MessageBox.Show("Please select a User to edit.");
                return;
            }

            // Retrieve the values from the input fields
            string user_name = user_txt.Text;
            string pass_word = password_txt.Text;
            string roles = (role_list.SelectedItem as ComboBoxItem)?.Content.ToString();


            // Update the selected book with the new values
            selectedUser.userName = user_name;
            selectedUser.passWord = pass_word;
            selectedUser.role = roles;

            // Save Change to Database
            db.SaveChanges();

            // Refresh the data
            LoadData();

            ClearUserFields();

            // Display a success message or perform any other desired actions
            MessageBox.Show("User Edited successfully!");
        }

        private void delete_user_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected User from the data grid
            User selectedUser = userDatagrid.SelectedItem as User;
            if (selectedUser == null)
            {
                MessageBox.Show("Please select a User to edit.");
                return;
            }

            // Confirm the deletion with the user
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this User?", "Confirm Deletion", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    // Remove the selected book from the database
                    db.Users.Remove(selectedUser);
                    db.SaveChanges();

                    // Refresh the data
                    LoadData();

                    ClearUserFields();

                    // Display a success message or perform any other desired actions
                    MessageBox.Show("User deleted successfully!");
                }
                catch (Exception ex)
                {
                    // Handle any exceptions that occur during deletion
                    MessageBox.Show("An error occurred while deleting the User: " + ex.Message);
                }
            }
        }

        private void userDatagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get the selected User from the data grid
            User selectedUser = userDatagrid.SelectedItem as User;
            if (selectedUser != null)
            {
                // Populate the fields with the selected User's data
                user_txt.Text = selectedUser.userName;
                password_txt.Text = selectedUser.passWord;

                // Set the selected role in the ComboBox
                ComboBoxItem selectedRoleItem = role_list.Items.OfType<ComboBoxItem>()
                    .FirstOrDefault(item => item.Content.ToString() == selectedUser.role);
                if (selectedRoleItem != null)
                {
                    role_list.SelectedItem = selectedRoleItem;
                }
            }
        }

        private void ClearUserFields()
        {
            user_txt.Text = "";
            password_txt.Text = "";
            role_list.SelectedIndex = -1;
        }

        #endregion


        #region PromotionGridRegion
        private void add_promo_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve input values from the UI controls
            string description = description_txt.Text;
            string percentageText = percentage_txt.Text;
            DateTime? startDate = start_date.SelectedDate;
            DateTime? endDate = end_date.SelectedDate;

            if (string.IsNullOrWhiteSpace(description) ||
                string.IsNullOrWhiteSpace(percentageText) ||
                !startDate.HasValue ||
                !endDate.HasValue)
            {
                MessageBox.Show("Please fill in all the fields.");
                return;
            }

            if (!int.TryParse(percentageText, out int percentage) || percentage < 0 || percentage > 100)
            {
                MessageBox.Show("Invalid percentage value. Percentage must be a non-negative integer between 0 and 100.");
                return;
            }

            // Create a new Discount object
            Discount discount = new Discount
            {
                Description = description,
                Percentage = percentage,
                StartDate = startDate.Value,
                EndDate = endDate.Value
            };

            // Retrieve the selected book from the ComboBox
            Book book = db.Books.FirstOrDefault(b => b.Title == book_list.SelectedItem.ToString());

            if (book != null)
            {
                // Associate the discount with the selected book
                discount.Books.Add(book);

                // Add the discount to the database context
                db.Discounts.Add(discount);
                db.SaveChanges();

                // Refresh the DataGrid
                LoadData();
            }
            else
            {
                // Handle the case when no book is selected
                MessageBox.Show("Please select a book.");
            }
        }

        private void delete_promo_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve the selected discount from the DataGrid
            Discount selectedDiscount = promotionDatagrid.SelectedItem as Discount;

            if (selectedDiscount != null)
            {
                // Remove the discount from the database
                db.Discounts.Remove(selectedDiscount);
                db.SaveChanges();

                // Refresh the DataGrid
                LoadData();

                // Show a success message
                MessageBox.Show("Discount deleted successfully.");
            }
            else
            {
                // No discount is selected
                MessageBox.Show("Please select a discount from the list.");
            }
        }

        private void view_promo_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve the selected discount from the DataGrid
            Discount selectedDiscount = promotionDatagrid.SelectedItem as Discount;

            if (selectedDiscount != null)
            {
                // Retrieve the book associated with the discount
                Book associatedBook = selectedDiscount.Books.FirstOrDefault();

                // Display the discount information
                MessageBox.Show($"Discount Description: {selectedDiscount.Description}\n" +
                                $"Start Date: {selectedDiscount.StartDate}\n" +
                                $"End Date: {selectedDiscount.EndDate}\n" +
                                $"Percentage: {selectedDiscount.Percentage}\n" +
                                $"Book Title: {associatedBook?.Title}");
            }
            else
            {
                // No discount is selected
                MessageBox.Show("Please select a discount from the list.");

            }

        }

        #endregion

        
    }
}