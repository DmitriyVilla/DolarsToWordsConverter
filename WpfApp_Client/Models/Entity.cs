using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using WpfApp_Client.Views;
using System.Windows.Input;
using WpfApp_Client.Commands;
using System.Net.Http;
using System.Drawing;
using System.Windows.Media;

namespace WpfApp_Client.Models
{
    public class Entity : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        // URL des Servers
        private string url = "http://localhost:5000/api/convert";

        private RelayCommand? convertCommand;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) 
            { 
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); 
            }
        }

        private string amount = string.Empty;
        public string Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                OnPropertyChanged(nameof(Amount));
            }
        }

        private string resultHeader = string.Empty;
        public string ResultHeader
        {
            get { return resultHeader;  }
            set { resultHeader = value; OnPropertyChanged(nameof(ResultHeader)); }
        }

        private System.Windows.Media.Brush resultHeaderColor = new SolidColorBrush(Colors.Transparent);
        public System.Windows.Media.Brush ResultHeaderColor
        {
            get { return resultHeaderColor; }
            set { resultHeaderColor = value; OnPropertyChanged(nameof(ResultHeaderColor)); }
        }


        private string answer = string.Empty;
        public string Answer
        {
            get { return answer; }
            set 
            { 
                answer = value;  
                OnPropertyChanged(nameof(Answer)); 
            }
        }

        private System.Windows.Media.Brush answerTBColor = new SolidColorBrush(Colors.Black);
        public System.Windows.Media.Brush AnswerTBColor
        {
            get { return answerTBColor; }
            set { answerTBColor = value; OnPropertyChanged(nameof(AnswerTBColor)); }
        }


        private readonly HttpClient httpClient = new HttpClient();

        public ICommand ConvertCommand => convertCommand ??= new RelayCommand(async () => await ConvertAsync());

        private async Task ConvertAsync()
        {
            try
            {
                if (!string.IsNullOrEmpty(Amount))
                {
                    string amount = Amount;

                    var requestData = new StringContent(amount, Encoding.UTF8, "text/plain");

                    var response = await httpClient.PostAsync(url, requestData);

                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();

                        if (result.StartsWith("Error:"))
                        {
                            ResultHeader = "Error:";
                            ResultHeaderColor = new SolidColorBrush(Colors.Red);
                            AnswerTBColor = new SolidColorBrush(Colors.Red);
                            Answer = result.Substring(6);
                        }
                        else
                        {
                            ResultHeader = "Result:";
                            ResultHeaderColor = new SolidColorBrush(Colors.White);
                            AnswerTBColor = new SolidColorBrush(Colors.White);
                            Answer = result;
                        }
                        
                    }
                    else
                    {
                        ResultHeader = "Error";
                        ResultHeaderColor = new SolidColorBrush(Colors.Red);
                    }
                }
                else
                {
                    Answer = "The field may not be empty!";
                }
            }
            catch (Exception ex)
            {
                Answer = ex.Message;
            }
        }

    }
}
