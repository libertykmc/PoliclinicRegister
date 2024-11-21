using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Попытка_1.Model;
using Попытка_1.View;

namespace Попытка_1.ViewModel
{
    public class DoctorSeesViewModel : INotifyPropertyChanged
    {
        private int doctorId;
        public string FIO { get; set; }
        private DBAccess dbAccess;
        private Window thisWindow;

        public ObservableCollection<DoctorSeeModel> Sees { get; set; }

        private DoctorSeeModel selectedSee;
        public DoctorSeeModel SelectedSee
        {
            get { return selectedSee; }
            set
            {
                selectedSee = value;
                OnPropertyChanged("SelectedSee");
            }
        }

        public DoctorSeesViewModel(int id, Window thisWindow)
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
            Sees = new ObservableCollection<DoctorSeeModel>(dbAccess.GetDoctorSees(doctorId, DateTime.Now));
            SelectedSee = null;
            commands();
        }

        private void commands()
        {
            Exit = new Command(obj =>
            {
                Window window = new DoctorMain(doctorId);
                window.WindowState = thisWindow.WindowState;
                window.Top = thisWindow.Top;
                window.Left = thisWindow.Left;
                window.Height = thisWindow.Height;
                window.Width = thisWindow.Width;
                window.Show();
                thisWindow.Close();
            });
            NotVisited = new Command(obj =>
            {
                SelectedSee.Visited = false;
                dbAccess.UpdateDoctorSee(SelectedSee);
                Sees.Remove(SelectedSee);
            }, obj => {
                return SelectedSee != null;
            });
            Write = new Command(obj =>
            {
                Window window = new DoctorRecordln(doctorId, SelectedSee.PatientID, SelectedSee.ID);
                window.WindowState = thisWindow.WindowState;
                window.Top = thisWindow.Top;
                window.Left = thisWindow.Left;
                window.Height = thisWindow.Height;
                window.Width = thisWindow.Width;
                window.Show();
                thisWindow.Close();
            }, obj => {
                return SelectedSee != null;
            });
            Card = new Command(obj =>
            {
                Window window = new PatientCard(doctorId, SelectedSee.PatientID);
                window.WindowState = thisWindow.WindowState;
                window.Top = thisWindow.Top;
                window.Left = thisWindow.Left;
                window.Height = thisWindow.Height;
                window.Width = thisWindow.Width;
                window.Show();
                thisWindow.Close();
            }, obj => {
                return SelectedSee != null;
            });
        }

        public Command NotVisited { get; set; }
        public Command Exit { get; set; }
        public Command Write { get; set; }
        public Command Card { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
