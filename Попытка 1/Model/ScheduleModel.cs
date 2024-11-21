using DAL.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Попытка_1.Model
{
    public class ScheduleModel : INotifyPropertyChanged
    {
        public int ID { get; set; }

        public int DoctorID { get; set; }

        private int room;
        public int Room
        {
            get { return room; }
            set
            {
                room = value;
                OnPropertyChanged("Room");
            }
        }
        private string day;
        public string Day
        {
            get { return day; }
            set
            {
                day = value;
                OnPropertyChanged("Day");
            }
        }
        private string shift;
        public string Shift
        {
            get { return shift; }
            set
            {
                shift = value;
                OnPropertyChanged("Shift");
            }
        }
        public int dayofWeek { get; set; }
        public string Changing { get; set; }
        public int? ZamID { get; set; }
        public int ShiftID { get; set; }
        public ScheduleModel()
        {
            
        }
        public ScheduleModel(Schedule schedule)
        {
            DBAccess DB = new DBAccess();
            ID = schedule.ID;
            DoctorID = schedule.DoctorID;
            Room = schedule.Room;
            dayofWeek = schedule.dayofWeek;
            Day = DB.GetDay(dayofWeek);
            ShiftID = schedule.ShiftID;
            Shift = DB.GetShift(ShiftID).number.ToString();
            ZamID = schedule.ZamID;
            Changing = "";
            var doctor = DB.GetDoctor(DoctorID);
            if (doctor.ZamEnd != null && doctor.ZamStart != null && DateTime.Now >= doctor.ZamStart && DateTime.Now <= doctor.ZamEnd)
            {
                Changing += "\nС " + doctor.ZamStart.ToString().Substring(0, 10) + "\n" + "До " + doctor.ZamEnd.ToString().Substring(0, 10);
                if (ZamID != null)
                {
                    Changing += "\nЗамена на" + "\n" + DB.GetDoctor(ZamID.Value).FullName;
                }
                else
                {
                    Changing += "\nПрием отменен";
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
