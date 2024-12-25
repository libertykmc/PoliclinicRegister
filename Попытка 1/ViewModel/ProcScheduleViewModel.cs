using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Попытка_1.Model;
using iText.Kernel.Pdf; // Для создания PDF-файлов
using iText.Layout; // Для работы с содержимым PDF
using iText.Layout.Element; // Для добавления элементов в PDF
using Microsoft.Win32; // Для SaveFileDialog





namespace Попытка_1.ViewModel
{
    public class ProcScheduleViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ScheduleProcedureModel> schedules;
        public ObservableCollection<ScheduleProcedureModel> Schedules
        {
            get { return schedules; }
            set
            {
                schedules = value;
                OnPropertyChanged("Schedules");
            }
        }
        public List<TypeofProcModel> Procedures { get; set; }
        public List<DaysModel> Days { get; set; }
        public ScheduleProcedureModel selectedSchedule;
        public ScheduleProcedureModel SelectedSchedule
        {
            get { return selectedSchedule; }
            set
            {
                selectedSchedule = value;
                if (value != null)
                {
                     SelectedDay = Days.Where(i => i.ID == selectedSchedule.DayID).FirstOrDefault();
                    SelectedProcedure = Procedures.Where(i => i.ID == selectedSchedule.ProcedureID).FirstOrDefault();
                    Room = selectedSchedule.Room.ToString();
                    Count = selectedSchedule.Count.ToString();
                }
                else
                {
                    SelectedDay = null;
                    SelectedProcedure = null;
                    Room = "";
                    Count = "";
                }
                OnPropertyChanged("SelectedSchedule");
            }
        }
        public DaysModel currentDay;
        public DaysModel CurrentDay
        {
            get { return currentDay; }
            set
            {
                currentDay = value;
                setSchedule();
                OnPropertyChanged("CurrentDay");
            }
        }

        public DaysModel selectedDay;
        public DaysModel SelectedDay
        {
            get { return selectedDay; }
            set
            {
                selectedDay = value;
                OnPropertyChanged("SelectedDay");
            }
        }

        public TypeofProcModel selectedProcedure;
        public TypeofProcModel SelectedProcedure
        {
            get { return selectedProcedure; }
            set
            {
                selectedProcedure = value;
                OnPropertyChanged("SelectedProcedure");
            }
        }

        private string room;
        public string Room
        {
            get { return room; }
            set
            {
                room = value;
                OnPropertyChanged("Room");
            }
        }

        private string count;
        public string Count
        {
            get { return count; }
            set
            {
                count = value;
                OnPropertyChanged("Count");
            }
        }

        private DBAccess dbAccess;
        private Window thisWindow;

        public ProcScheduleViewModel(Window thisWindow)
        {
            dbAccess = new DBAccess();
            Procedures = dbAccess.GetTypeProc();
            Days = dbAccess.GetDays();
            CurrentDay = Days.First();
            this.thisWindow = thisWindow;
            setSchedule();
            commands();
        }

        private void setSchedule()
        {
            SelectedSchedule = null;
            Schedules = new ObservableCollection<ScheduleProcedureModel>(dbAccess.GetScheduleProcedures().Where(i => i.DayID == currentDay.ID).ToList());
        }
        private void commands()
        {
            Exit = new Command(obj =>
            {
                RegistrationMain window = new RegistrationMain();
                window.WindowState = thisWindow.WindowState;
                window.Top = thisWindow.Top;
                window.Left = thisWindow.Left;
                window.Height = thisWindow.Height;
                window.Width = thisWindow.Width;
                window.Show();
                thisWindow.Close();
            });
            Create = new Command(obj =>
            {
                ScheduleProcedureModel sc = new ScheduleProcedureModel();
                sc.Room = int.Parse(room);
                sc.Count = int.Parse(count);
                sc.ProcedureID = selectedProcedure.ID;
                sc.Procedure = selectedProcedure.Type;
                sc.DayID = selectedDay.ID;
                sc.ID = dbAccess.AddScheduleProcedure(sc);
                if (sc.DayID == currentDay.ID)
                {
                    Schedules.Add(sc);
                }
            }, obj =>
            {
                return
                room != "" && int.TryParse(room, out int a) &&
                count != "" && int.TryParse(count, out int b) &&
                selectedDay != null && selectedProcedure != null &&
                schedules.Where(i => i.DayID == selectedDay.ID && i.ProcedureID == selectedProcedure.ID && int.Parse(room) == i.Room).FirstOrDefault() == null;
            });

            Update = new Command(obj =>
            {
                selectedSchedule.Room = int.Parse(room);
                selectedSchedule.Count = int.Parse(count);
                dbAccess.UpdateScheduleProcedure(selectedSchedule);
            }, obj =>
            {
                return
                room != "" && int.TryParse(room, out int a) &&
                count != "" && int.TryParse(count, out int b) &&
                selectedDay != null && selectedDay.ID == SelectedSchedule?.DayID &&
                selectedProcedure != null && selectedProcedure.ID == SelectedSchedule?.ProcedureID &&
                schedules.Where(i => i.DayID == selectedDay.ID && i.ProcedureID == selectedProcedure.ID && int.Parse(room) == i.Room).FirstOrDefault() == null;
            });

            Delete = new Command(obj =>
            {
                selectedSchedule.Closed = true;
                dbAccess.UpdateScheduleProcedure(selectedSchedule);
                setSchedule();
            }, obj =>
            {
                return
                selectedSchedule != null;
            });

            Print = new Command(obj =>
            {
                // Установка контекста лицензии EPPlus
                OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                // Получаем расписание процедур
                var schedules = dbAccess.GetScheduleProcedures();
                ExcelPackage excel = new ExcelPackage();
                var workSheet = excel.Workbook.Worksheets.Add("Расписание процедурных");
                workSheet.DefaultRowHeight = 32;
                workSheet.DefaultColWidth = 14;
                int y = 1;

                // Заполнение данных в Excel
                foreach (TypeofProcModel type in Procedures)
                {
                    var buffer = schedules.Where(i => i.ProcedureID == type.ID).OrderBy(i => i.DayID).ToList();
                    if (buffer.Count > 0)
                    {
                        workSheet.Cells[y, 1].Value = type.Type;
                        for (int x = 2; x <= buffer.Count + 1; x++)
                        {
                            workSheet.Cells[y, x].Value = dbAccess.GetDay(buffer[x - 2].DayID) + "\r\n" + buffer[x - 2].Room;
                        }
                        y++;
                    }
                }

                // Вывод диалога сохранения файла
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*",
                    DefaultExt = "xlsx",
                    FileName = "расписание_процедур_" + DateTime.Now.ToString("yyyyMMdd_HHmmss"),
                    Title = "Сохранить как Excel"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    string selectedPath = saveFileDialog.FileName;

                    // Сохранение Excel-файла в выбранное место
                    File.WriteAllBytes(selectedPath, excel.GetAsByteArray());

                    MessageBox.Show("Файл успешно сохранён");
                }
            });


        }
        public Command Exit { get; set; }
        public Command Create { get; set; }
        public Command Update { get; set; }
        public Command Delete { get; set; }
        public Command Print { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
