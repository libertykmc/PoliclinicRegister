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
    /// Логика взаимодействия для DoctorMain.xaml
    /// </summary>
    public partial class DoctorMain : Window
    {
        public DoctorMain(int doctorID)
        {
            InitializeComponent();
            DataContext = new DoctorMainViewModel(doctorID, this);
        }
    }
}
