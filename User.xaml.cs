using Soliders.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Soliders
{
    public partial class User : Window
    {
        bool startUpdate = true;

        public User()
        {
            InitializeComponent();
            ListConscripts();
            AdminOrNotAdmin();
            
        }

        public User(string iduser, int idAdmin)
        {
            InitializeComponent();
            Title = iduser;
            //Thread thread = new(ListConscripts);
            //thread.Start();
            AdminOrNotAdmin();
            using(PrContext db = new())
            {
                var listConscripts = from conscript in db.Conscripts
                                     select new
                                     {
                                         conscript.Id,
                                         Firstname = conscript.Firstname,
                                         Lastname = conscript.Lastname,
                                         Name = conscript.Name,
                                         DateOfBirth = conscript.Dateof,
                                         Category = conscript.Category,
                                         Passport = conscript.Passport,
                                         Snils = conscript.Snils,
                                         Status = conscript.Status
                                     };
                listviewUsers.ItemsSource = listConscripts.ToList();
            }
           
       
        }

        private async void ListConscripts()
        {

            while (startUpdate)
            {
                using (PrContext db = new())
                {

                    Dispatcher.Invoke(() =>
                    {
                        var listConscripts = from conscript in db.Conscripts
                                             select new
                                             {
                                                 conscript.Id,
                                                 Firstname = conscript.Firstname,
                                                 Lastname = conscript.Lastname,
                                                 Name = conscript.Name,
                                                 DateOfBirth = conscript.Dateof,
                                                 Category = conscript.Category,
                                                 Passport = conscript.Passport,
                                                 Snils = conscript.Snils,
                                                 Status = conscript.Status
                                             };
                        listviewUsers.ItemsSource = listConscripts.ToList();
                    });
                }
                await Task.Delay(2000);
            }
        }


        /*Поиск призывников*/
        private void FaceSearch(object sender, KeyEventArgs e)
        {
            string str = Search.Text.ToLower();
            using (PrContext db = new())
            {
                var listConscripts = from conscript in db.Conscripts.ToList()
                                     where conscript.Firstname.ToLower().Contains(str)
                                        || conscript.Name.ToLower().Contains(str)
                                        || conscript.Lastname.ToLower().Contains(str)
                                     select new
                                     {
                                         conscript.Id,
                                         Firstname = conscript.Firstname,
                                         Lastname = conscript.Lastname,
                                         Name = conscript.Name,
                                         DateOfBirth = conscript.Dateof,
                                         Category = conscript.Category,
                                         Passport = conscript.Passport,
                                         Snils = conscript.Snils,
                                         Status = conscript.Status
                                     };
                listviewUsers.ItemsSource = listConscripts;
            }

        }


        /*Добавить новую запись(призывника, физическое лицо)*/
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PersonData person = new();
            person.ShowDialog();
        }


        /*Подробнее о персоне*/
        private void MoreAboutThePerson(object sender, MouseButtonEventArgs e)
        {
            var str = ReturnIdThePerson(listviewUsers.SelectedItem.ToString());
      
            MorePerson more = new(Convert.ToInt32(str));
            more.ShowDialog();

            static string ReturnIdThePerson(string a)
            {
                string strId = "";
                for (int i = 0; i < a.Length; i++)
                {
                    if (char.IsDigit(a[i]))
                        strId += a[i];
                    else if (a[i] == ',')
                        break;
                }
                return strId;
            }
        }

        /*Добавить новую запись(сотудника, физическое лицо)*/
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            AddAdmin addAdmin = new();
            addAdmin.ShowDialog();
        }

        /*Список сотрудников(администрирование)*/
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ListPersonWorks listPersonWorks = new ListPersonWorks();
            listPersonWorks.ShowDialog();
        }

        private void AdminOrNotAdmin()
        {
            if(MainWindow.AdminOrNotAdmin == 1)
            {
                AddPerson.Visibility = Visibility.Visible;
                ListPerson.Visibility = Visibility.Visible;
            }
            else
            {
                AddPerson.Visibility = Visibility.Hidden;
                ListPerson.Visibility = Visibility.Hidden;
            }
                
        }




        //Выборка по статусу
        private void SelectList(object sender, EventArgs e)
        {
            if(statusProsto.Text == "Все")
            {
                startUpdate = true;
                ListConscripts();
                return;
            }

            else
            {
                startUpdate = false;

                using (PrContext db = new())
                {
                    var listConscripts = from conscript in db.Conscripts
                                         where conscript.Status == statusProsto.Text
                                         select new
                                         {
                                             conscript.Id,
                                             Firstname = conscript.Firstname,
                                             Lastname = conscript.Lastname,
                                             Name = conscript.Name,
                                             DateOfBirth = conscript.Dateof,
                                             Category = conscript.Category,
                                             Passport = conscript.Passport,
                                             Snils = conscript.Snils,
                                             Status = conscript.Status
                                         };
                    listviewUsers.ItemsSource = listConscripts.ToList();
                }
            }
           
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            using (PrContext db = new())
            {

                
                    var listConscripts = from conscript in db.Conscripts
                                         select new
                                         {
                                             conscript.Id,
                                             Firstname = conscript.Firstname,
                                             Lastname = conscript.Lastname,
                                             Name = conscript.Name,
                                             DateOfBirth = conscript.Dateof,
                                             Category = conscript.Category,
                                             Passport = conscript.Passport,
                                             Snils = conscript.Snils,
                                             Status = conscript.Status
                                         };
                    listviewUsers.ItemsSource = listConscripts.ToList();
                
            }
        }
    }
}
