using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

using MCore.Comp.Communications;

namespace MCore.Comp.ScanSystem
{
    public class SLLaserDeskTcp
    {
        private SLLaserDesk _slLaserDesk = null;
        private Byte STX = 0x02;
        private Byte ETX = 0x03;

        TcpClient _client = null;


        public bool IsConnected
        {
            get
            {
                return _client.Connected;
            }
        }

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public SLLaserDeskTcp()
        {
        }

        ///// <summary>
        ///// Manual creation constructor
        ///// </summary>
        ///// <param name="name"></param>
        //public SLLaserDeskTcp(string name) 
        //    : base (name)
        //{
        //}
        
        #endregion Constructors


        public SLLaserDesk Bind
        {
            get { return _slLaserDesk; }
            set
            {
                _slLaserDesk = value;
                _client = new TcpClient();
            }
        }

        public void ConnectToLaserDesk(string ipAddress,int port)
        {
            try
            {
                if (!_client.Connected)
                {
                    _client = new TcpClient();
                }
                _client.Connect(ipAddress, port);
                _client.ReceiveTimeout = 200;
            }
            catch(Exception ex)
            {

            }
           
        }

        public void DisconnectFromLaserDesk()
        {
            try
            {
                _client.Close();
            }
            catch(Exception ex)
            {

            }
        }





        public void SendTCPCommand(List<Byte> command,bool updateLastSendMsg)
        {
            Byte[] commandBytes = command.ToArray();
            NetworkStream ns = _client.GetStream();
            ns.Flush();
            ns.Write(commandBytes, 0, commandBytes.Length);


            //Create command String for view
            string sentString = "";
            if (sentString != null)
            {

                foreach (byte sentByte in command)
                {
                    sentString += String.Format("{0} ", sentByte.ToString("X2"));
                }
            }

            if (updateLastSendMsg)
            {
                _slLaserDesk.TCPComm.LastSentMessage = sentString;
            }
            
        }

        public List<Byte> ReadTcpReturn(bool updateLastReceivedMsg)
        { 

            NetworkStream ns = _client.GetStream();
 
            List<Byte> readBytes = new List<Byte>();
         


            int alreadyByteCount = 0;


            while (true)
            {
                try
                {
                    int readInt = ns.ReadByte();
                    Byte readByte = Convert.ToByte(readInt);
                    readBytes.Add(readByte);
                    if (readByte == 0x03 && readBytes[alreadyByteCount - 1] != 0x10)
                    {
                        //Create command String for view
                        string returnString = "";
                        if (readBytes != null)
                        {
                            foreach (byte readBt in readBytes)
                            {
                                returnString += String.Format("{0} ", readBt.ToString("X2"));
                            }
                        }

                        if (updateLastReceivedMsg)
                        {
                            _slLaserDesk.TCPComm.LastReceivedMessage = returnString;
                        }
                        return readBytes;
                    }
                    alreadyByteCount++;
                }
                catch
                {
                    return null;
                }
            } 
          
        }

      

        public List<Byte> BuildCommand(int commNumber,List<Byte> parameterBytes)
        {
            List<Byte> commandByteList = new List<byte>();

            //STX Byte
            commandByteList.Add(STX);

            //Command Lenght Byte
            int paramByteCount = 4;
            if(parameterBytes != null)
            {
                paramByteCount = parameterBytes.Count;
            }
            Byte[] finalCommandLenghtBytes = BitConverter.GetBytes(paramByteCount);
            for (int i = 0; i < finalCommandLenghtBytes.Length; i++)
            {
                commandByteList.Add(finalCommandLenghtBytes[i]);
            }

            //Command Number Byte
            Byte[] finalCommandNumberByte = BitConverter.GetBytes(commNumber);   
            for (int i = 0; i < finalCommandNumberByte.Length; i++)
            {
                commandByteList.Add(finalCommandNumberByte[i]);
            }

            //Parameter Byte
            if (parameterBytes != null)
            {
                foreach (Byte paramByte in parameterBytes)
                {
                    commandByteList.Add(paramByte);
                }
            }

            //CheckSum Byte
            Byte[] dataBytes = new Byte[commandByteList.Count - 1];
            commandByteList.CopyTo(1,dataBytes,0, commandByteList.Count - 1);
            Byte ChecksumByte = GetChecksum(dataBytes);
            commandByteList.Add(ChecksumByte);

            //ETX Byte
            commandByteList.Add(ETX);

            List<Byte> commandAfterDLE = new List<byte>(commandByteList);
            int addDLECount = 0;
            for(int i =0; i< commandByteList.Count; i++ )
            {
                if((i!=0) && (i!= (commandByteList.Count-1)) && 
                    (commandByteList[i] == 0x02 || commandByteList[i] == 0x03 || commandByteList[i] == 0x10))
                {
                    commandAfterDLE.Insert(i+addDLECount, 0x10);
                    addDLECount++;
                }
            }
            return commandAfterDLE;

        }



        public static Byte GetChecksum(Byte[] arr)
        {
            Byte cs = 0;
            int len = arr.Length;
            int i, h = 0, z = 0;
            for (i = 0; i < len; i++) // summarize every byte of array
            {
                h += arr[i];
            }
            do // calculate sum of digits
            {
                z += (h % 10);
                h /= 10;
            }
            while (h > 0); // every digit of byte
            return cs = (byte)(z % 10); // take last digit
        }


        public List<Byte> CovertStringParamToByteList(string stringParam)
        {
            Byte[] stringBytes = Encoding.Unicode.GetBytes(stringParam);
            List<Byte> stringByteList = stringBytes.ToList<Byte>();

            //Add end string
            stringByteList.Add(0x00);
            stringByteList.Add(0x00);

            return stringByteList;
        }

    }
}
