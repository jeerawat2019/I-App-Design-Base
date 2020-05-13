using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Xml.Serialization;

namespace YamahaRCX221_222
{
    public enum CommunicationType
    {
        Serial,
        TCP
    }

    [Serializable]
    public class YamahaRCX221_222Config
    {
        public YamahaRCX221_222Config()
        {
            this.Axis1Name = "Axis 1";
            this.Axis2Name = "Axis 2";
            this.CommunicationType = YamahaRCX221_222.CommunicationType.TCP;
            this.SerialPortSetting = new SerialPortSetting();
            this.IpAddress = "127.0.0.1";
            this.Port = 23;
        }

        public string Axis1Name { get; set; }
        public string Axis2Name { get; set; }
        public CommunicationType CommunicationType { get; set; }
        public SerialPortSetting SerialPortSetting { get; set; }
        public string IpAddress { get; set; }
        public int Port { get; set; }
        public double Axis1DefaultSpeed { get; set; }
        public double Axis1DefaultAcc { get; set; }
        public double Axis1DefaultDec { get; set; }
        public double Axis2DefaultSpeed { get; set; }
        public double Axis2DefaultAcc { get; set; }
        public double Axis2DefaultDec { get; set; }
        public double AxesDefaultVectorSpeed { get; set; }
        public EncoderType Axis1EncoderType { get; set; }
        public EncoderType Axis2EncoderType { get; set; }
    }
}
