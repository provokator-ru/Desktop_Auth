using Desktop_Auth.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace Desktop_Auth.Pages
{
    /// <summary>
    /// Логика взаимодействия для AutarizationPage.xaml
    /// </summary>
    public partial class AutarizationPage : Page
    {
        private string accessCode; ///Поле для хранения сгенерированного кода доступа
        public AutarizationPage()
        {
            InitializeComponent();
        }

        private void Button_EnterClick(object sender, RoutedEventArgs e)///Действия для кнопки Вход
        {
            string employeeNumber = TxtbNumber.Text;
            string employeePassword = TxtbPassword.Text;

            var EnterUser = ApplicationData.db.Autorizations.FirstOrDefault(u => u.Number == TxtbNumber.Text && u.Password == TxtbPassword.Text);


            if (EnterUser != null && EnterUser.Password == employeePassword)
            {
                // Генерируем код доступа
                accessCode = GenerateAccessCode();

                // Открываем модальное окно с сгенерированным кодом доступа
                MessageBox.Show($"Ваш код доступа: {accessCode}", "Код доступа", MessageBoxButton.OK, MessageBoxImage.Information);

                TxtbCode.IsEnabled = true;
                TxtbCode.Focus();
            }
            else
            {
                MessageBox.Show("Неправильный номер или пароль сотрудника!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Button_CancelClick(object sender, RoutedEventArgs e)///Действия для кнопки отмена-очищает поля ввода
        {
            TxtbNumber.Text = "";
            TxtbPassword.Text = "";
            TxtbCode.Text = "";
            MessageBox.Show("Вы отменили ввод!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Button_ResetClick(object sender, RoutedEventArgs e)///Кнопка обновление кода
        {
            MessageBox.Show("Обновление кода!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private bool CheckEmployeeNumber(string employeeNumber)
        {
            var currentUser = ApplicationData.db.Autorizations.FirstOrDefault(u => u.Number == employeeNumber);
            return currentUser != null;
        }

        private void TxtbNumber_Key(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string Number = TxtbNumber.Text;

                // Проверяем наличие номера сотрудника в базе данных
                if (CheckEmployeeNumber(Number))
                {
                    TxtbPassword.IsEnabled = true;
                    TxtbPassword.Focus(); // Устанавливаем фокус на поле ввода пароля
                }
                else
                {
                    MessageBox.Show("Введённый номер сотрудника не найден!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void TxtbCode_Key(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string enteredCode = TxtbCode.Text;

                if (enteredCode == accessCode)
                {
                    // Переходим на другую страницу после правильного ввода кода доступа
                    NavigationService.Navigate(new PageUser());
                }
                else
                {
                    MessageBox.Show("Неправильный код доступа!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private string GenerateAccessCode()
        {
            // Генерируем код доступа (8 символов, латиница, верхний и нижний регистр, спецсимвол, цифра)
            string characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()";
            Random random = new Random();
            StringBuilder accessCodeBuilder = new StringBuilder();

            for (int i = 0; i < 8; i++)
            {
                int index = random.Next(0, characters.Length);
                accessCodeBuilder.Append(characters[index]);
            }

            return accessCodeBuilder.ToString();
        }
    }
}
