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
using Попытка_1.ViewModel;

namespace Попытка_1.View
{
    /// <summary>
    /// Логика взаимодействия для PatientCard.xaml
    /// </summary>
    public partial class PatientCard : Window
    {
        public PatientCard(int id)
        {
            InitializeComponent();
            DataContext = new PatientCardViewModel(id, this);
        }

        public PatientCard()
        {
            InitializeComponent();
            DataContext = new PatientCardViewModel(this);
        }

        public PatientCard(int id, int patientId)
        {
            InitializeComponent();
            DataContext = new PatientCardViewModel(id, this, patientId);
        }
    }
}
