using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Попытка_1.Model;
using Попытка_1.View;

namespace Попытка_1.ViewModel
{
    public class MainViewModel
    {
        private DBAccess DBAccess;
        public MainViewModel(Window window)
        {
            DBAccess = new DBAccess();

            RegistrationProc = new Command(obj =>
            {
                ProcedureRegistration window1 = new ProcedureRegistration();
                window1.WindowState = window.WindowState;
                window1.Top = window.Top;
                window1.Left = window.Left;
                window1.Height = window.Height;
                window1.Width = window.Width;
                window1.Show();
                window.Close();
            });
            Registration = new Command(obj =>
            {
                Registration window1 = new Registration();
                window1.WindowState = window.WindowState;
                window1.Top = window.Top;
                window1.Left = window.Left;
                window1.Height = window.Height;
                window1.Width = window.Width;
                window1.Show();
                window.Close();
            });
            Test = new Command(obj =>
            {
                Changing window1 = new Changing();
                window1.WindowState = window.WindowState;
                window1.Top = window.Top;
                window1.Left = window.Left;
                window1.Height = window.Height;
                window1.Width = window.Width;
                window1.Show();
                window.Close();
            });
            Card = new Command(obj =>
            {
                PatientCard window1 = new PatientCard();
                window1.WindowState = window.WindowState;
                window1.Top = window.Top;
                window1.Left = window.Left;
                window1.Height = window.Height;
                window1.Width = window.Width;
                window1.Show();
                window.Close();
            });
            Patients = new Command(obj =>
            {
                Patients window1 = new Patients();
                window1.WindowState = window.WindowState;
                window1.Top = window.Top;
                window1.Left = window.Left;
                window1.Height = window.Height;
                window1.Width = window.Width;
                window1.Show();
                window.Close();
            });
            Doctors = new Command(obj =>
            {
                Doctors window1 = new Doctors();
                window1.WindowState = window.WindowState;
                window1.Top = window.Top;
                window1.Left = window.Left;
                window1.Height = window.Height;
                window1.Width = window.Width;
                window1.Show();
                window.Close();
            });
            Nurces = new Command(obj =>
            {
                Nurces window1 = new Nurces();
                window1.WindowState = window.WindowState;
                window1.Top = window.Top;
                window1.Left = window.Left;
                window1.Height = window.Height;
                window1.Width = window.Width;
                window1.Show();
                window.Close();
            });
            ProcSchedule = new Command(obj =>
            {
                ProcSchedule window1 = new ProcSchedule();
                window1.WindowState = window.WindowState;
                window1.Top = window.Top;
                window1.Left = window.Left;
                window1.Height = window.Height;
                window1.Width = window.Width;
                window1.Show();
                window.Close();
            });
            Schedule = new Command(obj =>
            {
                Schedule window1 = new Schedule();
                window1.WindowState = window.WindowState;
                window1.Top = window.Top;
                window1.Left = window.Left;
                window1.Height = window.Height;
                window1.Width = window.Width;
                window1.Show();
                window.Close();
            });
            Exit = new Command(obj =>
            {
                MainWindow window1 = new MainWindow();
                window1.Show();
                window.Close();
            });

            ExportPdf = new Command(obj =>
            {

                var Report = DBAccess.getStatistic();
                
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Лист 1");

                    worksheet.Cell(1, 1).Value = "Диагноз";
                    worksheet.Cell(1, 2).Value = "Количество";

                    int rowIdx = 2;
                    foreach (var column in Report)
                    {
                        worksheet.Cell(rowIdx, 1).Value = column.Diagnosis;
                        worksheet.Cell(rowIdx, 2).Value = column.Count;
                        rowIdx++;
                    }

                    var dialog = new SaveFileDialog()
                    {
                        FileName = "Статистика",
                        DefaultExt = ".xlsx",
                        Filter = "Excel Files|*.xlsx"
                    };

                    //  if (dialog.ShowDialog() == true)
                    //{
                    //  workbook.SaveAs(dialog.FileName);
                    //}
                    if (dialog.ShowDialog() == true)
                    {
                        try
                        {
                            workbook.SaveAs(dialog.FileName);
                            MessageBox.Show("Статистика успешно экспортирована", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Произошла ошибка при сохранении файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Экспорт статистики был отменен.", "Отмена", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

            });
        }
        public Command RegistrationProc { get; set; }
        public Command Registration { get; set; }
        public Command Test { get; set; }
        public Command Exit { get; set; }
        public Command Card { get; set; }
        public Command Patients { get; set; }
        public Command Doctors { get; set; }
        public Command Nurces { get; set; }
        public Command ProcSchedule { get; set; }
        public Command Schedule { get; set; }

        public Command ExportPdf { get; set; }
    }
}
