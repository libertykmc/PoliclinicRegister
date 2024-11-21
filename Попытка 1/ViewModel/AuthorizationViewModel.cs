using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Попытка_1.Model;
using Попытка_1.View;

namespace Попытка_1.ViewModel
{
    public class AuthorizationViewModel : INotifyPropertyChanged
    {
        private string login;
        public string Login
        {
            get { return login; }
            set
            {
                login = value;
                OnPropertyChanged("Login");
            }
        }

        private int wrong;
        public int Wrong
        {
            get { return wrong; }
            set
            {
                wrong = value;
                OnPropertyChanged("Wrong");
            }
        }

        private DBAccess db;
        private MainWindow window;
        public AuthorizationViewModel(MainWindow autorization)
        {
            window = autorization;
            db = new DBAccess();
            wrong = 0;
            setEnterCommand();
        }

        private void setEnterCommand()
        {
            Enter = new Command(obj =>
            {
                PasswordBox password = obj as PasswordBox;
                if (password != null)
                {
                    UserModel user = db.GetUserByLogin(login);
                    if (user != null)
                    {
                        if (user.Password == password.Password)
                        {
                            if(user.UserType == 1)
                            {
                                DoctorMain reg = new DoctorMain(user.DoctorID.Value);
                                reg.Show();
                                window.Close();
                            }
                            if (user.UserType == 0)
                            {
                                RegistrationMain reg = new RegistrationMain();
                                reg.Show();
                                window.Close();
                            }
                            if (user.UserType == 2)
                            {
                                Procedural reg = new Procedural();
                                reg.Show();
                                window.Close();
                            }
                        }
                        else
                        {
                            password.Password = "";
                            Wrong = 2;
                        }
                    }
                    else
                    {
                        Wrong = 1;
                    }
                }
            },
                  obj => { return login != ""; });
        }

        public Command Enter { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
