using UnityEngine;
using System.IO.Ports;

public class ArduinoController : MonoBehaviour
{
    // Connect to Arduino Serial Monitor
    private SerialPort serialPort;
    public string portName = "COM16"; // Change this depend on the Arduino's port
    public int baudRate = 9600;

    public GameObject square;

    private void Start()
    {
        // Initiating Arduino Connection
        serialPort = new SerialPort(portName, baudRate);
        if (!serialPort.IsOpen)
        {
            serialPort.Open();
            serialPort.ReadTimeout = 1000;
        }
    }

    private void Update()
    {
        // Checks if connection successful
        if (serialPort != null && serialPort.IsOpen && serialPort.BytesToRead > 0)
        {
            string data = serialPort.ReadLine();
            string[] distances = data.Split(','); // Data sent in (x1, x2) format, stored in an array

            if (distances.Length == 2)
            {
                // TryParse is to convert string to float, allow comparison.
                if (float.TryParse(distances[0], out float distance1) && float.TryParse(distances[1], out float distance2))
                {
                    if (distance1 < 20 && distance2 < 20)
                    {
                        square.GetComponent<SpriteRenderer>().color = Color.green;
                    }
                    else if (distance1 < 20)
                    {
                        square.GetComponent<SpriteRenderer>().color = Color.red;
                    }
                    else if (distance2 < 20)
                    {
                        square.GetComponent<SpriteRenderer>().color = Color.blue;
                    }
                    else
                    {
                        square.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f); // White
                    }
                }
            }
        }
    }

    // Close the Arduino Serial Port connection
    private void OnApplicationQuit()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }
}