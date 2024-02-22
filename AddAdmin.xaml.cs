using Microsoft.EntityFrameworkCore;
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
    //Добавить сотрудника
    public partial class AddAdmin : Window
    {
        public AddAdmin()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Добавить сотрудника военкомата
            //family name lastname login password adminOrNot
            //Проверка не занят ли логин
            var listObjAddAdmin = new List<string>() { family.Text, name.Text, lastname.Text, login.Text, password.Text };
            int pro = 0;
            foreach (var item in listObjAddAdmin)
            {
                if (string.IsNullOrEmpty(item))
                    pro++;
            }

            if(pro > 0)
            {
                MessageBox.Show($"Вы пропустили поле", "Внимательно");
            }

            else
            {
                listObjAddAdmin = new List<string>() { family.Text, name.Text, lastname.Text };
                pro = 0;
                foreach (var item in listObjAddAdmin)
                {
                    if (!StrTrue(item))
                        pro++;
                }

                if (pro > 0)
                    MessageBox.Show("Некорректно заполнено одно из полей");
                else
                {
                    int t = 0;
                    //Делаю запрос в БД
                    using (PrContext db = new())
                    {
                        var users = db.Works.FromSqlRaw("SELECT * FROM Works").ToList();
                        int y = 0;
                        foreach (var item in users)
                        {
                            if(login.Text == item.Login)
                            {
                                y++;
                                break;
                            }
                        }
                        if (y > 0)
                            MessageBox.Show("Пользователь с таким логином существует", "Используйте другой логин");
                        else
                        {
                            if (adminOrNot.IsChecked == true)
                                t = 1;
                            try
                            {
                                db.Database.ExecuteSqlRaw("INSERT INTO Works(firstname, name, lastname, pass, login, admin) VALUES({0}, {1}, {2}, {3}, {4}, {5})", family.Text, name.Text, lastname.Text, login.Text, password.Text, Convert.ToInt64(t));
                                MessageBox.Show("Новый пользователь успешно добавлен!");
                                Close();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Что то пошло не так");
                            }
                        }
                    }
                }
            }

            static bool StrTrue(string str)
            {
                int j = 0;
                for (int i = 0; i < str.Length; i++)
                {
                    if (!char.IsLetter(str[i]))
                        j++;
                }

                if (j == 0)
                    return true;
                else
                    return false;
            }
        }
    }
}
