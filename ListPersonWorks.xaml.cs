using Soliders.Models;
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
using System.Windows.Shapes;

namespace Soliders
{
    public partial class ListPersonWorks : Window
    {
        private static int idPerson = 1;
        public ListPersonWorks()
        {
            InitializeComponent();
            ListWorks();
        }

        /*Изменить данные*/
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using(PrContext db = new())
            {
                var getMyRabs = db.Works.Where(u => u.Id == idPerson).FirstOrDefault();
                if (getMyRabs != null)
                {
                    getMyRabs.Firstname = family.Text;
                    getMyRabs.Name = name.Text;
                    getMyRabs.Lastname = lastname.Text;
                    getMyRabs.Login = login.Text;
                    getMyRabs.Pass = password.Text;
                    if ((bool)administratorRights.IsChecked)
                        getMyRabs.Admin = 1;
                    else getMyRabs.Admin = 0;
                    if ((bool)block.IsChecked)
                        getMyRabs.Block = 1;
                    else getMyRabs.Block = 0;
                    db.SaveChanges();
                    MessageBox.Show("Обновлено");
                }
                else
                    MessageBox.Show("Что то пошло не так, мдаа......");
            }
        }

        //Метод для заполнения списка пользователей
        private void ListWorks()
        {
            using (PrContext db = new())
            {
                var listWorks = from p in db.Works
                                select new
                                {
                                    p.Id,
                                    name = p.Name,
                                    lastname = p.Lastname,
                                    firstname = p.Firstname,
                                    login = p.Login,
                                    pass = p.Pass
                                };
                listviewUsers.ItemsSource = listWorks.ToList();


                var getMyWorks = db.Works.FirstOrDefault();

                family.Text = getMyWorks.Firstname;
                name.Text = getMyWorks.Name;
                lastname.Text = getMyWorks.Lastname;
                login.Text = getMyWorks.Login;
                password.Text = getMyWorks.Pass;
                if (getMyWorks.Admin == 1)
                    administratorRights.IsChecked = true;
                else administratorRights.IsChecked = false;
                if (getMyWorks.Block == 1)
                    block.IsChecked = true;
                else block.IsChecked = false;
            }
        }


        //Выбор пользователя при двойном клике
        private void ChoiceOfOnePerson(object sender, MouseButtonEventArgs e)
        {
            var strId = ReturnIdPerson(listviewUsers.SelectedItem.ToString());
            
            using (PrContext db = new())
            {
                var getMyWorks = db.Works.Where(u => u.Id == strId).FirstOrDefault();
                idPerson = (int)strId;
                family.Text = getMyWorks.Firstname;
                name.Text = getMyWorks.Name;
                lastname.Text = getMyWorks.Lastname;
                login.Text = getMyWorks.Login;
                password.Text = getMyWorks.Pass;
                if (getMyWorks.Admin == 1)
                    administratorRights.IsChecked = true;
                else administratorRights.IsChecked = false;
                if (getMyWorks.Block == 1)
                    block.IsChecked = true;
                else block.IsChecked = false;
            }

            static long ReturnIdPerson(string str)
            {
                string a = ""; 

                for (int i = 0; i < str.Length; i++)
                {
                    if (char.IsDigit(str[i]))
                        a += str[i];
                    else if (str[i] == ',')
                        break;
                }

                return Convert.ToInt64(a);
            }
        }
    }
}
