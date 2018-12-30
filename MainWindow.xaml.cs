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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Quobject.SocketIoClientDotNet.Client;
using Newtonsoft.Json;
using callin.Models;

namespace callin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Message> messages = new List<Message>();
        private Socket socket;

        public MainWindow()
        {
            InitializeComponent();
            socket = IO.Socket("http://localhost:3000");
            socket.On("chat message", (data) => {
                Message incoming = JsonConvert.DeserializeObject<Message>((string)data);
                Dispatcher.Invoke(() => {
                    updateChatWindow(incoming); // Update UI
                });                
            });
        }

        public async void BtnSend_click(object sender, RoutedEventArgs e)
        {
            // Make sure name and message fields not empty
            if(String.IsNullOrWhiteSpace(this.NameTextBox.Text))
            {
                MessageBox.Show("Please ensure name text field is not empty.");
                return;
            }
            else if (String.IsNullOrWhiteSpace(this.textBox.Text))
            {
                return;
            }

            Message messageToSend = new Message(this.NameTextBox.Text, this.textBox.Text);
            emitMessage(messageToSend); // Emit message
            this.textBox.Clear(); // Clear entered text
        }

        private void updateChatWindow(Message message)
        {
            if (this.messages.Count >= 100) // Maintain strict limit of 100 messages
            {
                int range = this.messages.Count - 100;
                this.MessageStack.Children.RemoveRange(0, range);
                this.messages.RemoveRange(0, range);
            }
            this.MessageStack.Children.Add(new TextBlock
            {
                Text = message.UserId + ": " + message.Text,
                TextWrapping = TextWrapping.WrapWithOverflow,
                Margin = new Thickness(0, 0, 0, 5),
                FontSize = 15
            });
            this.MessageStackScroll.ScrollToBottom();
        }

        private void emitMessage(Message message)
        {
            socket.Emit("message", JsonConvert.SerializeObject(message));
        }
    }
}
