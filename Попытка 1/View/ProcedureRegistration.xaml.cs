﻿using System;
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
    /// Логика взаимодействия для ProcedureRegistration.xaml
    /// </summary>
    public partial class ProcedureRegistration : Window
    {
        public ProcedureRegistration()
        {
            InitializeComponent();
            DataContext = new ProcedureRegistrationViewModel(this);
        }
    }
}
