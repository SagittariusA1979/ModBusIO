using System;
using ModbusTCP;

class Program
{
    static void Main(string[] args)
    {
        // Create an instance of the Master class
        Master modbusMaster = new Master();

        // IP address and port of the Modbus slave
        string ipAddress = "192.168.0.20";
        ushort port = 502;
        bool noSyncConnection = false;

        // Connect to the Modbus slave
        modbusMaster.connect(ipAddress, port, noSyncConnection);

        // Define the register address and value to write
        ushort registerAddress = 8009;
        ushort valueToWrite = 0x000F; // Example 16-bit value where 4 LSBs are set to 1 (binary: 0000 0000 0000 1111)

        // Convert the ushort value to a byte array
        byte[] valueBytes = BitConverter.GetBytes(valueToWrite);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(valueBytes); // Modbus uses big-endian order
        }

        // Write to a single register
        modbusMaster.WriteSingleRegister(1, 1, registerAddress, valueBytes);

        // Optionally, you can verify by reading back the register value
        byte[] readValues = new byte[2];
        modbusMaster.ReadHoldingRegister(1, 1, registerAddress, 1, ref readValues);
        ushort readValue = BitConverter.ToUInt16(readValues, 0);
        if (BitConverter.IsLittleEndian)
        {
            readValue = (ushort)((readValue << 8) | (readValue >> 8)); // Convert to big-endian order
        }
        Console.WriteLine($"Value read from register {registerAddress}: {readValue:X}");

        // Disconnect
        modbusMaster.disconnect();
    }
}