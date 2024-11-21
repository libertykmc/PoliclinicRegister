using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Попытка_1.Model
{
    public class ChangingModel : INotifyPropertyChanged
    {
        public List<DoctorModel> Doctors { get; set; }
        
        private DoctorModel selectedDoctor;
        public DoctorModel SelectedDoctor
        {
            get { return selectedDoctor; }
            set
            {
                selectedDoctor = value;
                OnPropertyChanged("SelectedDoctor");
            }
        }
        public string DayOfWeek { get; set; }
        public int Shift { get; set; }
        public ScheduleModel Schedule { get; set; }

        public ChangingModel(ScheduleModel schedule, List<DoctorModel> doctors)
        {
            Doctors = new List<DoctorModel>();
            DBAccess db = new DBAccess();
            Schedule = schedule;
            Shift = db.GetShift(schedule.ShiftID).number;
            DayOfWeek = db.GetDay(schedule.dayofWeek);
            foreach (DoctorModel doctor in doctors)
            {
                if (doctor.Shifts[doctor.Days.IndexOf(schedule.dayofWeek)].Contains(schedule.ShiftID.ToString()) == false)
                {
                    Doctors.Add(doctor);
                }
            }
            
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
