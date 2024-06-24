// Install-Package NModbus4

using System;
using System.Net.Sockets;
using NModbus;
using NModbus.Device;

#region Version 0.1
// class Program
// {
//     static void Main(string[] args)
//     {
//         string ipAddress = "192.168.0.100"; // Replace with your device's IP address
//         int port = 502; // Modbus TCP port
//         byte slaveId = 1; // Modbus slave ID

//         try
//         {
//             using (TcpClient client = new TcpClient(ipAddress, port))
//             using (IModbusMaster master = new ModbusFactory().CreateMaster(client))
//             {
//                 // Read digital inputs (DI8 ETH BK)
//                 ushort startAddressInputs = 8000; // Starting address of the inputs
//                 ushort numInputs = 8; // Number of digital inputs
//                 bool[] inputs = master.ReadInputs(slaveId, startAddressInputs, numInputs);
//                 Console.WriteLine("Digital Inputs:");
//                 for (int i = 0; i < inputs.Length; i++)
//                 {
//                     Console.WriteLine($"Input {i + 1}: {(inputs[i] ? "ON" : "OFF")}");
//                 }

//                 // Write to digital outputs (DO4 ETH BK)
//                 ushort startAddressOutputs = 8009; // Starting address of the outputs
//                 ushort numOutputs = 4; // Number of digital outputs
//                 bool[] outputsToWrite = new bool[numOutputs];
//                 outputsToWrite[0] = true; // Set first output to ON
//                 outputsToWrite[1] = false; // Set second output to OFF
//                 outputsToWrite[2] = true; // Set third output to ON
//                 outputsToWrite[3] = false; // Set fourth output to OFF
//                 master.WriteMultipleCoils(slaveId, startAddressOutputs, outputsToWrite);
//                 Console.WriteLine("Digital Outputs written successfully.");

//                 // Verify outputs
//                 bool[] readOutputs = master.ReadCoils(slaveId, startAddressOutputs, numOutputs);
//                 Console.WriteLine("Digital Outputs:");
//                 for (int i = 0; i < readOutputs.Length; i++)
//                 {
//                     Console.WriteLine($"Output {i + 1}: {(readOutputs[i] ? "ON" : "OFF")}");
//                 }
//             }
//         }
//         catch (Exception ex)
//         {
//             Console.WriteLine($"An error occurred: {ex.Message}");
//         }
//     }
// }
#endregion

#region Version 0.2
class Program
{
    static void Main(string[] args)
    {
        string ipAddress = "192.168.0.100"; // Replace with your device's IP address
        int port = 502; // Modbus TCP port

        try
        {
            using (TcpClient client = new TcpClient(ipAddress, port))
            using (IModbusMaster master = new ModbusFactory().CreateMaster(client))
            {
                byte slaveId = 1; // Modbus slave ID

                // Read Holding Registers (FC3)
                ushort startAddressHolding = 8000; // Starting address of the holding registers
                ushort numHoldingRegisters = 4; // Number of holding registers to read
                ushort[] holdingRegisters = master.ReadHoldingRegisters(slaveId, startAddressHolding, numHoldingRegisters);
                Console.WriteLine("Holding Registers:");
                for (int i = 0; i < holdingRegisters.Length; i++)
                {
                    Console.WriteLine($"Register {startAddressHolding + i}: {holdingRegisters[i]}");
                }

                // Read Input Registers (FC4)
                ushort startAddressInputRegisters = 8000; // Starting address of the input registers
                ushort numInputRegisters = 4; // Number of input registers to read
                ushort[] inputRegisters = master.ReadInputRegisters(slaveId, startAddressInputRegisters, numInputRegisters);
                Console.WriteLine("Input Registers:");
                for (int i = 0; i < inputRegisters.Length; i++)
                {
                    Console.WriteLine($"Register {startAddressInputRegisters + i}: {inputRegisters[i]}");
                }

                // Write Single Holding Register (FC6)
                ushort writeAddress = 8009; // Address to write the single holding register
                ushort valueToWrite = 1234; // Value to write to the register
                master.WriteSingleRegister(slaveId, writeAddress, valueToWrite);
                Console.WriteLine($"Wrote {valueToWrite} to register {writeAddress}");

                // Write Multiple Holding Registers (FC16)
                ushort startAddressWriteMultiple = 8009; // Starting address for writing multiple holding registers
                ushort[] valuesToWrite = new ushort[] { 1, 2, 3, 4 }; // Values to write to the registers
                master.WriteMultipleRegisters(slaveId, startAddressWriteMultiple, valuesToWrite);
                Console.WriteLine("Wrote multiple values to holding registers.");

                // Verify written values (reading back using FC3)
                ushort[] writtenRegisters = master.ReadHoldingRegisters(slaveId, startAddressWriteMultiple, (ushort)valuesToWrite.Length);
                Console.WriteLine("Written Holding Registers:");
                for (int i = 0; i < writtenRegisters.Length; i++)
                {
                    Console.WriteLine($"Register {startAddressWriteMultiple + i}: {writtenRegisters[i]}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
#endregion


