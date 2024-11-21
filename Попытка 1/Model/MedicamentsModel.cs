using DAL.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Попытка_1.Model
{
    public class MedicamentsModel : INotifyPropertyChanged
    {
        public int ID { get; set; }
        public int RecordID { get; set; }

        private string medicine;
        public string Medicine
        {
            get { return medicine; }
            set
            {
                medicine = value;
                OnPropertyChanged("Medicine");
            }
        }

        public MedicamentsModel()
        {

        }

        public MedicamentsModel(Medicaments medicament)
        {
            ID = medicament.ID;
            RecordID = medicament.RecordID;
            Medicine = medicament.Medicine;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
