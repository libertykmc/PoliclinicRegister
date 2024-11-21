using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Попытка_1.Model;

namespace Попытка_1.ViewModel
{
    public class ChangingViewModel : INotifyPropertyChanged
    {
        private bool first;
        private DBAccess dbAccess;
        private Window window;
        public ObservableCollection<ChangingModel> Changings { get; set; }
        private ObservableCollection<DoctorModel> doctors; 
        public ObservableCollection<DoctorModel> Doctors
        {
            get { return doctors; }
            set
            {
                doctors = value;
                OnPropertyChanged("Doctors");
            }
        }
        private bool changing { get; set; }
        public bool Changing
        {
            get { return changing; }
            set
            {
                changing = value;
                setDoctors();
                OnPropertyChanged("Changing");
            }
        }
        private DateTime endDate { get; set; }
        public DateTime EndDate
        {
            get { return endDate; }
            set
            {
                endDate = value;
                if (!first)
                {
                    setZamScheludes();
                }
                OnPropertyChanged("EndDate");
            }
        }

        private DateTime startDate { get; set; }
        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
                if (!first)
                {
                    setZamScheludes();
                }
                OnPropertyChanged("StartDate");
            }
        }

        private DoctorModel selectedDoctor;
        public DoctorModel SelectedDoctor
        {
            get { return selectedDoctor; }
            set
            {
                selectedDoctor = value;
                if (SelectedDoctor != null)
                {
                    first = true;
                    setZamScheludes();
                }
                OnPropertyChanged("SelectedDoctor");
            }
        }
        public Command Exit { get; set; }
        public Command Calc { get; set; }
        public Command Cancel { get; set; }
        public Command Submit { get; set; }

        private void setZamScheludes()
        {
            Changings.Clear();
            if (first && selectedDoctor.ZamEnd != null && selectedDoctor.ZamEnd >= DateTime.Now.Date)
            {
                EndDate = selectedDoctor.ZamEnd.Value;
                StartDate = selectedDoctor.ZamStart.Value;
            }
            List<DoctorModel> doctorModels = Doctors.Where(i => i.SpecializationID == SelectedDoctor.SpecializationID).ToList();
            List<DaysModel> days = dbAccess.GetDays();
            foreach (DoctorModel d in doctorModels)
            {
                d.Workload = 0;
                foreach (DaysModel day in days)
                {
                    d.Shifts.Add(dbAccess.GetDoctorShiftsForDay(d.ID, day.ID, SelectedDoctor.ID, startDate.Date, endDate.Date));
                    d.Days.Add(day.ID);
                    d.Workload += (d.Shifts.Last().Length - d.Shifts.Last().Replace(" ", "").Length) * 3;
                }
            }
            List<ScheduleModel> schedules = dbAccess.GetDoctorSchedule(SelectedDoctor.ID);
            foreach(ScheduleModel schedule in schedules)
            {
                Changings.Add(new ChangingModel(schedule, doctorModels));
            }
            if (first && selectedDoctor.ZamEnd != null && selectedDoctor.ZamEnd >= DateTime.Now.Date)
            {
                foreach (ChangingModel changing in Changings)
                {
                    if (changing.Schedule.ZamID != null)
                    {
                        changing.SelectedDoctor = changing.Doctors.Where(i => i.ID == changing.Schedule.ZamID.Value).FirstOrDefault();
                    }
                }
            }
            first = false;
        }

        private void submit()
        {
            if (SelectedDoctor.ZamEnd == null || SelectedDoctor.ZamEnd < endDate.Date)
            {
                SelectedDoctor.ZamEnd = endDate.Date;
            }
            else
            {
                SelectedDoctor.ZamEnd = endDate.Date;
                List<DoctorSeeModel> sees = dbAccess.GetDoctorSees(SelectedDoctor.ID).Where(i => i.DateTime.Date > SelectedDoctor.ZamEnd && i.Closed == false).ToList();
                foreach (DoctorSeeModel s in sees)
                {
                    s.ZamID = null;
                    dbAccess.UpdateDoctorSee(s);
                }
            }
            if (SelectedDoctor.ZamStart == null || SelectedDoctor.ZamStart > startDate.Date)
            {
                SelectedDoctor.ZamStart = startDate.Date;
            }
            else
            {
                SelectedDoctor.ZamStart = startDate.Date;
                List<DoctorSeeModel> sees = dbAccess.GetDoctorSees(SelectedDoctor.ID).Where(i => i.DateTime.Date < SelectedDoctor.ZamStart && i.Closed == false).ToList();
                foreach (DoctorSeeModel s in sees)
                {
                    s.ZamID = null;
                    dbAccess.UpdateDoctorSee(s);
                }
            }
            dbAccess.UpdateDoctor(SelectedDoctor);
            foreach (ChangingModel changing in Changings)
            {
                List<DoctorSeeModel> sees = dbAccess.GetDoctorSees(SelectedDoctor.ID).Where(i => changing.Schedule.dayofWeek == dbAccess.GetDayWeekId(i.DateTime.DayOfWeek.ToString()).Value && i.DateTime.Date >= SelectedDoctor.ZamStart.Value.Date && i.DateTime.Date <= SelectedDoctor.ZamEnd.Value.Date).ToList();
                if (changing.SelectedDoctor != null) // производим замену
                {
                    changing.Schedule.ZamID = changing.SelectedDoctor.ID;
                    dbAccess.UpdateSchedule(changing.Schedule);
                    //List<DoctorSeeModel> sees = dbAccess.GetDoctorSees(SelectedDoctor.ID).Where(i => i.DateTime.Date >= SelectedDoctor.ZamStart.Value.Date && i.DateTime.Date <= SelectedDoctor.ZamEnd.Value.Date).ToList();
                    foreach (DoctorSeeModel s in sees)
                    {
                        s.ZamID = changing.Schedule.ZamID;
                        dbAccess.UpdateDoctorSee(s);
                    }
                }
                else // замена невозможна, отменяем записи
                {
                    changing.Schedule.ZamID = null;
                    dbAccess.UpdateSchedule(changing.Schedule);
                    foreach (DoctorSeeModel s in sees)
                    {
                        s.Closed = true;
                        dbAccess.UpdateDoctorSee(s);
                    }
                }
            }
        }

        private void calculate()
        {
            foreach (ChangingModel schedule in Changings)
            {
                foreach (DoctorModel doctor in schedule.Doctors)
                {
                    doctor.CurrentWorkload += 3;
                    if (doctor.Shifts[doctor.Days.IndexOf(schedule.Schedule.dayofWeek)] == "")
                    {
                        doctor.CurrentWorkload += 3;
                    }
                    else
                    {
                        List<string> buffer = new List<string>(doctor.Shifts[doctor.Days.IndexOf(schedule.Schedule.dayofWeek)].Remove(0, 1).Split(' '));
                        bool f = false;
                        foreach(string s in buffer)
                        {
                            if (Math.Abs(Int32.Parse(s) - schedule.Shift) == 1)
                            {
                                f = true;
                                break;
                            }
                        }
                        if (!f)
                        {
                            doctor.CurrentWorkload += 2;
                        }
                    }
                }
                schedule.Doctors.Sort((e1, e2) =>
                {
                    return (e1.Workload + e1.CurrentWorkload).CompareTo(e2.Workload + e2.CurrentWorkload);
                }
                );
                if (schedule.Doctors.Count > 0)
                {
                    schedule.SelectedDoctor = schedule.Doctors.First();
                }
                else
                {
                    schedule.SelectedDoctor = null;
                }
            }
        }

        private void setDoctors()
        {
            Doctors.Clear();
            //if (changing)
            //{
            //    Doctors =new ObservableCollection<DoctorModel>(dbAccess.getChangingDoctors());
            //}
            //else
            //{
            //    Doctors = new ObservableCollection<DoctorModel>(dbAccess.getReadyDoctors());
            //}
            Doctors = new ObservableCollection<DoctorModel>(dbAccess.GetDoctors());
            SelectedDoctor = null;
        }

        public ChangingViewModel(Window window)
        {
            Changings = new ObservableCollection<ChangingModel>();
            dbAccess = new DBAccess();
            Doctors = new ObservableCollection<DoctorModel>();
            setDoctors();
            changing = false;
            endDate = DateTime.Now;
            startDate = DateTime.Now;
            this.window = window;
            Calc = new Command(obj =>
            {
                calculate();
            },
            obj => { 
                return selectedDoctor != null && endDate.Date >= DateTime.Now.Date && startDate.Date <= endDate.Date; ; 
            });
            Submit = new Command(obj =>
            {
                submit();
            },
            obj => { return selectedDoctor != null && endDate.Date >= DateTime.Now.Date && startDate.Date <= endDate.Date; });
            Exit = new Command(obj =>
            {
                Window main = new RegistrationMain();
                main.WindowState = window.WindowState;
                main.Top = window.Top;
                main.Left = window.Left;
                main.Height = window.Height;
                main.Width = window.Width;
                main.Show();
                window.Close();
            });
            Cancel = new Command(obj =>
            {
                List<DoctorSeeModel> sees = dbAccess.GetDoctorSees(SelectedDoctor.ID).Where(i => i.DateTime.Date >= SelectedDoctor.ZamStart.Value.Date && i.DateTime.Date <= SelectedDoctor.ZamEnd.Value.Date).ToList();
                foreach (DoctorSeeModel s in sees)
                {
                    s.ZamID = null;
                    dbAccess.UpdateDoctorSee(s);
                }
                selectedDoctor.ZamEnd = null;
                selectedDoctor.ZamStart = null;
                dbAccess.UpdateDoctor(selectedDoctor);
            }, obj => { return selectedDoctor != null && selectedDoctor.ZamEnd != null && selectedDoctor.ZamStart != null; });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
