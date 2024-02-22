using iText.Kernel.XMP.Impl;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Soliders
{
    public partial class PersonData : Window
    {
        private int idWorkPersonData = 0;
        public PersonData()
        {
            InitializeComponent();
        }

        public PersonData(int idPerson)
        {
            InitializeComponent();
            idWorkPersonData = idPerson;
        }


        //Добавить призывника в базу
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            /*
             Список объектов XAML
            family
            name
            lastname
            datapic
            adressPropiska
            adressFact
            familyStatus
            category
            children
            socialStatus
            snils
            statusProsto
            serial
            number
             */

            //Проверка на пустые поля
            var listObjPersonData = new List<string>() 
            {
                family.Text, 
                name.Text, 
                lastname.Text, 
                datapic.Text, 
                adressPropiska.Text, 
                adressFact.Text, 
                familyStatus.Text, 
                category.Text, 
                children.Text, 
                socialStatus.Text,
                snils.Text,
                statusProsto.Text,
                serial.Text,
                number.Text
            };

            bool CheckIsNull = false;

            foreach (var item in listObjPersonData)
            {
                if(string.IsNullOrEmpty(item))
                {
                    CheckIsNull = true; 
                }
            }

            if(CheckIsNull)
            {
                MessageBox.Show("Вы пропустили одно или несколько полей", "Внимательно");
            }

            else
            {
                if(StrTrue(family.Text) 
                    && StrTrue(name.Text) 
                    && StrTrue(lastname.Text) 
                    && StrTrue(socialStatus.Text) 
                    && IntTrue(children.Text) 
                    && IntTrue(serial.Text) 
                    && IntTrue(number.Text))
                {
                    try
                    {
                        //Добавляю в БД запись о призывнике
                        using (PrContext db = new())
                        {
                           
                            Conscript conscript = new()
                            {
                                Firstname = family.Text,
                                Lastname = lastname.Text,
                                Name = name.Text,
                                Dateof = datapic.Text,
                                Address = adressPropiska.Text,
                                AddressNext = adressFact.Text,
                                FamilyStatus = familyStatus.Text,
                                Category = category.Text,
                                Children = Convert.ToInt64(children.Text),
                                SocialStatus = socialStatus.Text,
                                Snils = snils.Text,
                                Passport = serial.Text + " " + number.Text,
                                Status = statusProsto.Text
                            };
                            db.Conscripts.Add(conscript);
                            db.SaveChanges();
                            db.Database.ExecuteSqlRaw("INSERT INTO Commission(works_fk, conscript_fk) VALUES({0}, {1})", Convert.ToInt64(MainWindow.IdWorks), Convert.ToInt64(db.Conscripts.Count()));
                        }
                        MessageBox.Show("Призывник добавлен!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                  
                }

                else
                    MessageBox.Show("Некорректно заполнено одно из полей");
            }


            static bool IntTrue(string str)
            {
                int j = 0;
                for (int i = 0; i < str.Length; i++)
                {
                    if (!char.IsDigit(str[i]))
                        j++;
                }

                if (j == 0)
                    return true;
                else
                    return false;
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
