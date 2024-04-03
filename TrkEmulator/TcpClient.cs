using System.Net.Sockets;
using System.Text;

namespace TrkEmulator;

public sealed class TcpClientEmulator
{
    private readonly TcpClient _client;
    private NetworkStream? _networkStream;
    private bool _isConnected;

    /// <summary>
    /// Определяет, подключен ли клиент к серверу
    /// </summary>
    public bool IsConnected
    {
        get => _isConnected;
        set => _isConnected = value;
    }
    
    public delegate void NewMessageReceivedEventHandler(string message);
    
    public event NewMessageReceivedEventHandler? NewMessageReceived;

    private void OnNewMessageReceived(string message)
    {
        NewMessageReceived?.Invoke(message);
    }
    
    public TcpClientEmulator()
    {
        _client = new TcpClient();
    }
    
    /// <summary>
    /// Запускает клиента, который сразу же начинает обращаться к серверу
    /// </summary>
    public void Start()
    {
        try
        {
            _client.Connect("127.0.0.1", 60000);
            _networkStream = _client.GetStream();
            _isConnected = _client.Connected;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    /// <summary>
    /// Останавливает работу клиента, закрывая соединение
    /// </summary>
    public void Stop()
    {
        try
        {
            _client.Close();
            _isConnected = _client.Connected;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    /// <summary>
    /// Отправляет на сервер сообщение
    /// </summary>
    /// <param name="message">Отправляемое сообщение</param>
    public void SendMessage(string message)
    {
        try
        {
            if (_networkStream != null)
            {
                byte[] sendingData = Encoding.UTF8.GetBytes(message);
                _networkStream.Write(sendingData);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    /// <summary>
    /// Получает сообщение с сервера
    /// </summary>
    /// <returns>Сообщение в виде <see cref="string"/></returns>
    public string RecieveMessage()
    {
        try
        {
            if (_networkStream != null)
            {
                var responseData = new byte[512];
                var response = new StringBuilder();
                int bytes;  // количество полученных байтов
                do
                {
                    // получаем данные
                    bytes = _networkStream.Read(responseData);
                    // преобразуем в строку и добавляем ее в StringBuilder
                    response.Append(Encoding.UTF8.GetString(responseData, 0, bytes));
                }
                while (_client.Available > 0); // пока данные есть в потоке 

                return response.ToString();
            }

            return "ERROR";
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return "ERROR";
        }
    }
}