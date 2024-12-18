using Microsoft.Win32;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp.Pdf;
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
    public class PatientCardViewModel : INotifyPropertyChanged
    {
        public List<PatientModel> Patients { get; set; }
        private List<string> texts;
        private int patientID = -1;
        public int doctorId;
        private PatientModel сurrentPatient;
        public PatientModel CurrentPatient
        {
            get { return сurrentPatient; }
            set
            {
                сurrentPatient = value;
                if (CurrentPatient != null)
                {
                    patientID = CurrentPatient.ID;
                }
                OnPropertyChanged("CurrentPatient");
            }
        }
        private string cardText;
        public string CardText
        {
            get { return cardText; }
            set
            {
                cardText = value;
                OnPropertyChanged("CardText");
            }
        }
        private bool byDate;
        public bool ByDate
        {
            get { return byDate; }
            set
            {
                byDate = value;
                OnPropertyChanged("ByDate");
            }
        }
        private DateTime startDate;
        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
                OnPropertyChanged("StartDate");
            }
        }
        private DateTime endDate;
        public DateTime EndDate
        {
            get { return endDate; }
            set
            {
                endDate = value;
                OnPropertyChanged("EndDate");
            }
        }
        public string FIO { get; set; }
        private DBAccess dbAccess;
        private Window thisWindow;
        public bool Visibility { get; set; }
        public PatientCardViewModel(Window thisWindow)
        {
            CardText = "";
            dbAccess = new DBAccess();
            ByDate = false;
            this.thisWindow = thisWindow;
            FIO = "Регистратура";
            Visibility = true;
            CurrentPatient = null;
            Patients = dbAccess.GetPatients(false);
            CurrentPatient = null;
            EndDate = DateTime.Now.Date;
            StartDate = DateTime.Now.Date;
            Back = new Command(obj =>
            {
                Window window = new RegistrationMain();
                window.WindowState = thisWindow.WindowState;
                window.Top = thisWindow.Top;
                window.Left = thisWindow.Left;
                window.Height = thisWindow.Height;
                window.Width = thisWindow.Width;
                window.Show();
                thisWindow.Close();
            });
            commands();
        }
        public PatientCardViewModel(int id, Window thisWindow)
        {
            CardText = "";
            dbAccess = new DBAccess();
            ByDate = false;
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
            Visibility = true;
            CurrentPatient = null;
            Patients = dbAccess.GetPatients(false).Where(i => i.PlaceOfSee == doctor.PlaceOfSee).ToList();
            CurrentPatient = null;
            EndDate = DateTime.Now.Date;
            StartDate = DateTime.Now.Date;
            Back = new Command(obj =>
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
            commands();
        }

        public PatientCardViewModel(int id, Window thisWindow, int patientId)
        {
            CardText = "";
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
            patientID = patientId;
            Visibility = false;
            CurrentPatient = null;
            DoctorSees window = new DoctorSees(doctorId);
            Back = new Command(obj =>
            {
                window.WindowState = thisWindow.WindowState;
                window.Top = thisWindow.Top;
                window.Left = thisWindow.Left;
                window.Height = thisWindow.Height;
                window.Width = thisWindow.Width;
                window.Show();
                thisWindow.Close();
            });

            List<RecordPatientModel> records = dbAccess.GetRecords(patientId).OrderByDescending(i => i.Date).ToList();
            texts = new List<string>();
            foreach(RecordPatientModel r in records)
            {
                texts.Add(r.ToText());
                CardText = CardText + texts.Last() + '\n';
            }
            CurrentPatient = null;
            ByDate = false;
            commands();
        }

        private void commands()
        {
            Show = new Command(obj =>
            {
                CardText = "";
                List<RecordPatientModel> records = dbAccess.GetRecords(patientID);
                if (ByDate == true)
                {
                    records = records.Where(i => i.Date.Date >= StartDate && i.Date.Date <= EndDate).ToList();
                }
                records = records.OrderByDescending(i => i.Date).ToList();
                texts = new List<string>();
                foreach (RecordPatientModel r in records)
                {
                    texts.Add(r.ToText());
                    CardText = CardText + texts.Last() + '\n';
                }
            }, func => { return CurrentPatient != null; });
            Print = new Command(obj =>
            {
                // Создание документа
                Document document = new Document();
                Section section = document.AddSection();

                // Добавление заголовка
                Paragraph main = new Paragraph();
                section.Add(main);
                main.AddText("Медицинская карта\n");
                main.AddText(dbAccess.GetPatient(patientID).FullName + '\n');
                if (ByDate == true)
                {
                    main.AddText("С " + startDate.Date.ToString().Substring(0, 10) + " по " + endDate.Date.ToString().Substring(0, 10));
                }
                else
                {
                    main.AddText("Полная");
                }
                main.Format.Font.Size = 18;
                main.Format.Alignment = ParagraphAlignment.Center;

                // Добавление текста
                for (int i = 0; i < texts.Count(); i++)
                {
                    Paragraph paragraph = new Paragraph();
                    paragraph.Format.Font.Size = 16;
                    paragraph.AddText(texts[i]);
                    section.Add(paragraph);
                }

                // Настройка сохранения файла через SaveFileDialog
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*",
                    DefaultExt = "pdf",
                    FileName = "Карта_" + patientID.ToString(),
                    Title = "Сохранить медицинскую карту"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    string selectedPath = saveFileDialog.FileName;

                    // Рендеринг PDF-документа
                    PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(true);
                    pdfRenderer.Document = document;
                    pdfRenderer.RenderDocument();

                    // Сохранение документа в выбранный путь
                    pdfRenderer.PdfDocument.Save(selectedPath);

                    MessageBox.Show("Файл успешно сохранён: " + selectedPath, "Экспорт завершён", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }, func => { return CardText != ""; });

        }

        public Command Back { get; set; }
        public Command Show { get; set; }
        public Command Print { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
