using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Попытка_1.Model;
using Попытка_1.View;

namespace Попытка_1.ViewModel
{
    public class DoctorMainViewModel : INotifyPropertyChanged
    {
        public string Priem { get; set; }
        private int doctorId;
        public string FIO { get; set; }
        private DBAccess dbAccess;
        private Window thisWindow;

        public DoctorMainViewModel(int id, Window thisWindow)
        {
            dbAccess = new DBAccess();
            this.thisWindow = thisWindow;
            doctorId = id;
            DoctorModel doctor = dbAccess.GetDoctor(id);
            string[] fio = doctor.FullName.Split(' ');
            FIO = fio[0] + ' ';
            if (fio.Length > 2)
            {
                FIO += fio[1][0] + ". ";
            }
            if (fio.Length > 2)
            {
                FIO += fio[2][0] + ".";
            }
            Priem = "Прием (" + dbAccess.GetDoctorSeesCount(id, DateTime.Now) + ")";
            commands();
        }

        private void commands()
        {
            Card = new Command(obj =>
            {
                Window window = new PatientCard(doctorId);
                window.WindowState = thisWindow.WindowState;
                window.Top = thisWindow.Top;
                window.Left = thisWindow.Left;
                window.Height = thisWindow.Height;
                window.Width = thisWindow.Width;
                window.Show();
                thisWindow.Close();
            });
            Exit = new Command(obj =>
            {
                MainWindow window = new MainWindow();
                window.Show();
                thisWindow.Close();
            });
            ShowSees = new Command(obj =>
            {
                DoctorSees window = new DoctorSees(doctorId);
                window.WindowState = thisWindow.WindowState;
                window.Top = thisWindow.Top;
                window.Left = thisWindow.Left;
                window.Height = thisWindow.Height;
                window.Width = thisWindow.Width;
                window.Show();
                thisWindow.Close();
            });
        }

        public Command Exit { get; set; }
        public Command ShowSees { get; set; }
        public Command Card { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
