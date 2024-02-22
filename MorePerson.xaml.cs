using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.XMP.Impl;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Org.BouncyCastle.Asn1.X500;
using Soliders.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Soliders
{
   
    public partial class MorePerson : Window
    {
        int mestniyId = 0;
        public MorePerson(int idMore)
        {
            InitializeComponent();
            mestniyId = idMore;
            PullUpPersona();
        }


        private void PullUpPersona()
        {
            using (PrContext db = new())
            {
                //Запрос
                var getMyPerson = db.Conscripts.Where(u => u.Id == mestniyId).ToList();

                family.Text = getMyPerson.First().Firstname;
                name.Text = getMyPerson.First().Name;
                lastname.Text = getMyPerson.First().Lastname;
                datapic.Text = getMyPerson.First().Dateof;
                adressPropiska.Text = getMyPerson.First().Address;
                adressFact.Text = getMyPerson.First().AddressNext;
                familyStatus.Text = getMyPerson.First().FamilyStatus;
                category.Text = getMyPerson.First().Category;
                children.Text = getMyPerson.First().Children.ToString();
                socialStatus.Text = getMyPerson.First().SocialStatus;
                snils.Text = getMyPerson.First().Snils;
                statusProsto.Text = getMyPerson.First().Status;
                serial.Text = getMyPerson.First().Passport.Substring(0, 4);
                number.Text = getMyPerson.First().Passport.Substring(4);
            }
        }

        //Изменить информацию о призывнике/физическом лице
        private void Button_Click(object sender, RoutedEventArgs e)
        {
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
                if (string.IsNullOrEmpty(item))
                {
                    CheckIsNull = true;
                }
            }

            if (CheckIsNull)
            {
                MessageBox.Show("Вы пропустили одно или несколько полей", "Внимательно");
            }

            else
            {
                number.Text = number.Text.Replace(" ", "");
                if (StrTrue(family.Text)
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
                            var MyPerson = db.Conscripts.Where(u => u.Id == mestniyId).FirstOrDefault();

                            MyPerson.Firstname = family.Text;
                            MyPerson.Lastname = lastname.Text;
                            MyPerson.Name = name.Text;
                            MyPerson.Dateof = datapic.Text;
                            MyPerson.Address = adressPropiska.Text;
                            MyPerson.AddressNext = adressFact.Text;
                            MyPerson.FamilyStatus = familyStatus.Text;
                            MyPerson.Category = category.Text;
                            MyPerson.Children = Convert.ToInt64(children.Text);
                            MyPerson.SocialStatus = socialStatus.Text;
                            MyPerson.Snils = snils.Text;
                            MyPerson.Passport = serial.Text + " " + number.Text;
                            MyPerson.Status = statusProsto.Text;

                            db.SaveChanges();

                        }
                        MessageBox.Show("Данные изменены");
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

        //Сформировать повестку
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string str = ""; 

            using(PrContext db = new())
            {
                var sluzhil = db.Conscripts.Where(u => u.Id == mestniyId).FirstOrDefault();
                str = sluzhil.Status;
            }

            if (str == "Служит")
                MessageBox.Show("Данный боец уже служит");

            else if (str == "Служил")
                MessageBox.Show("Данный боец уже служил");

            else if(str == "Не служил")   
            {
                Thread thread = new(FormingTheAgenda);
                thread.Start();
            }
        }

        //Формирование повестки
        private void FormingTheAgenda()
        {
           
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "PDF document (*.pdf)|*.pdf";

                if (dialog.ShowDialog() == true)
                {
                    string fileName = dialog.FileName;
                    PdfDocument pdfDoc = new(new PdfWriter(fileName));
                    Document doc = new(pdfDoc, iText.Kernel.Geom.PageSize.A4);
                    PdfFont f2 = PdfFontFactory.CreateFont(Arial2H(), "Identity-H");
                Cell cell = new();
               Dispatcher.Invoke(() =>
                {
                    
                    cell.Add(new Paragraph($"Призывнику: {family.Text} {name.Text} {lastname.Text}")).SetFont(f2);

                    Cell cell1 = new Cell().Add(new Paragraph($"Проживающему по адресу: {adressPropiska.Text}")).SetFont(f2);

                    Cell cell2 = new Cell().Add(new Paragraph($"")).SetFont(f2);

                    Cell cell3 = new Cell().Add(new Paragraph($"ПОВЕСТКА")).SetFont(f2).SetFontSize(28).SetPaddingLeft(100);
                    Cell cell4 = new Cell().Add(new Paragraph($"На основании Закона РФ")).SetFont(f2).SetFontSize(24).SetPaddingLeft(100);
                    Cell cell5 = new Cell().Add(new Paragraph($"\"О воинской обязанности и военной службе\"")).SetFont(f2).SetFontSize(24).SetPaddingLeft(100);
                    Cell cell6 = new Cell().Add(new Paragraph($"")).SetFont(f2);

                    Random rnd = new();
                    var startDate = new DateTime(2022, 1, 1);
                    var newDate = startDate.AddDays(rnd.Next(366));

                    Cell cell7 = new Cell().Add(new Paragraph($"Приказываю \"{newDate.ToString().Substring(0, 10)}\" года")).SetFont(f2);

                    Cell cell8 = new Cell().Add(new Paragraph($"к 09:00 часам прибыть на призывной пункт при Ногинском ОГВК по адресу: г. Ногинск, ул. Воздушных Десантников, д. 26 в кабинет № 10 для прохождения военной службы ")).SetFont(f2);

                    Cell cell9 = new Cell().Add(new Paragraph($"М.П. Ногинский горвоенком")).SetFont(f2);

                    Cell cell10 = new Cell().Add(new Paragraph($"Искоростинский")).SetFont(f2);

                   


                    doc.Add(cell);
                    doc.Add(cell1);
                    doc.Add(cell2);
                    doc.Add(cell3);
                    doc.Add(cell4);
                    doc.Add(cell5);
                    doc.Add(cell6);
                    doc.Add(cell7);
                    doc.Add(cell8);
                    doc.Add(cell9);
                    doc.Add(cell10);
                    doc.Close();
                });

                    var proc = new Process();
                    proc.StartInfo.FileName = fileName;
                    proc.StartInfo.UseShellExecute = true;
                    proc.Start();
                }
           
        }

        //Относительный путь
        static private string Arial2H()
        {
            var x = Directory.GetCurrentDirectory();
            var y = Directory.GetParent(x).FullName;
            var c = Directory.GetParent(y).FullName;
            var r = Directory.GetParent(c).FullName + @"\DA\arial.ttf";
            return r;
        }


        private Paragraph CenteredParagraph(Text text, float width)
        {
            var tabStops = new List<TabStop> { new TabStop(width / 2, TabAlignment.CENTER) };
            var output = new Paragraph().AddTabStops(tabStops);
            output.Add(new Tab())
                    .Add(text);
            return output;
        }

    }
}
