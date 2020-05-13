using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace MCore.Comp.IOSystem
{
	class MEDAQLibDLL
	{
		public enum ERR_CODE
		{
			ERR_NOERROR = 0,
			ERR_FUNTION_NOT_SUPPORTED = -1,
			ERR_CANNOT_OPEN = -2,
			ERR_NOT_OPEN = -3,
			ERR_APPLYING_PARAMS = -4,
			ERR_SEND_CMD_TO_SENSOR = -5,
			ERR_CLEARUNG_BUFFER = -6,
			ERR_HW_COMMUNICATION = -7,
			ERR_TIMEOUT_READING_FROM_SENSOR = -8,
			ERR_READING_SENSOR_DATA = -9,
			ERR_INTERFACE_NOT_SUPPORTED = -10,
			ERR_ALREADY_OPEN = -11,
			ERR_CANNOT_CREATE_INTERFACE = -12,
			ERR_NO_SENSORDATA_AVAILABLE = -13,
			ERR_UNKNOWN_SENSOR_COMMAND = -14,
			ERR_UNKNOWN_SENSOR_ANSWER = -15,
			ERR_SENSOR_ANSWER_ERROR = -16,
			ERR_SENSOR_ANSWER_TOO_SHORT = -17,
			ERR_WRONG_PARAMETER = -18,
			ERR_NOMEMORY = -19,
			ERR_NO_ANSWER_RECEIVED = -20,
			ERR_SENSOR_ANSWER_DOES_NOT_MATCH_COMMAND = -21,
			ERR_BAUDRATE_TOO_LOW = -22,
			ERR_OVERFLOW = -23,
			ERR_INSTANCE_NOT_EXTST = -24,
			ERR_NOT_FOUND = -25,
		}

		public enum ME_SENSOR
		{
			SENSOR_ILD1401 = 1,
			SENSOR_ILD1700 = 2,
			SENSOR_ILD1800 = 3,
			SENSOR_ILD2000 = 4,
			SENSOR_ILD2200 = 5,
			SENSOR_IFD2400 = 6,
			SENSOR_IFD2430 = 7,
			SENSOR_ODC2500 = 8,
			SENSOR_ODC2600 = 9,
			ENCODER_IF2004 = 10,
            SENSOR_ILD1402 = 23,
		};

		// functions provided by MEDAQLib.dll
		[DllImport("MEDAQlib.dll")]
		public static extern UInt32 CreateSensorInstance(ME_SENSOR sensor);

        [DllImport("MEDAQlib.dll", EntryPoint = "ClearAllParameters")]
        private static extern Int32 MEDAQLib_ClearAllParameters(UInt32 instanceHandle);
        public static ERR_CODE ClearAllParameters(UInt32 instanceHandle)
        {
            return (ERR_CODE)MEDAQLib_ClearAllParameters(instanceHandle);
        }

        [DllImport("MEDAQlib.dll", EntryPoint = "ReleaseSensorInstance")]
		private static extern Int32 MEDAQLib_ReleaseSensorInstance(UInt32 instanceHandle);
		public static ERR_CODE ReleaseSensorInstance(UInt32 instanceHandle)
		{
			return (ERR_CODE)MEDAQLib_ReleaseSensorInstance(instanceHandle);
		}

		[DllImport("MEDAQlib.dll", EntryPoint = "SetParameterInt")]
		private static extern Int32 MEDAQLib_SetParameterInt(
			UInt32 instanceHandle,
			String paramName,
			Int32 paramValue);
		public static ERR_CODE SetParameterInt(
			UInt32 instanceHandle,
			String paramName,
			Int32 paramValue)
		{
			return (ERR_CODE)MEDAQLib_SetParameterInt(instanceHandle, paramName, paramValue);
		}

		[DllImport("MEDAQlib.dll", EntryPoint = "SetParameterDWORD")]
		private static extern Int32 MEDAQLib_SetParameterDWORD(
			UInt32 instanceHandle,
			String paramName,
			UInt32 paramValue);
		public static ERR_CODE SetParameterDWORD(
			UInt32 instanceHandle,
			String paramName,
			UInt32 paramValue)
		{
			return (ERR_CODE)MEDAQLib_SetParameterDWORD(instanceHandle, paramName, paramValue);
		}

		[DllImport("MEDAQlib.dll", EntryPoint = "SetParameterDouble")]
		private static extern Int32 MEDAQLib_SetParameterDouble(
			UInt32 instanceHandle, 
			String paramName, 
			Double paramValue);
		public static ERR_CODE SetParameterDouble(
			UInt32 instanceHandle,
			String paramName,
			Double paramValue)
		{
			return (ERR_CODE)MEDAQLib_SetParameterDouble(instanceHandle, paramName, paramValue);
		}

		[DllImport("MEDAQlib.dll", EntryPoint = "SetParameterString")]
		private static extern Int32 MEDAQLib_SetParameterString(
			UInt32 instanceHandle, 
			String paramName, 
			String paramValue);
		public static ERR_CODE SetParameterString(
			UInt32 instanceHandle,
			String paramName,
			String paramValue)
		{
			return (ERR_CODE)MEDAQLib_SetParameterString(instanceHandle, paramName, paramValue);
		}

		[DllImport("MEDAQlib.dll", EntryPoint = "GetParameterInt")]
		private static extern Int32 MEDAQLib_GetParameterInt(
			UInt32 instanceHandle, 
			String paramName, 
			ref Int32 paramValue);
		public static ERR_CODE GetParameterInt(
			UInt32 instanceHandle,
			String paramName,
			ref Int32 paramValue)
		{
			return (ERR_CODE)MEDAQLib_GetParameterInt(instanceHandle, paramName, ref paramValue);
		}

		[DllImport("MEDAQlib.dll", EntryPoint = "GetParameterDWORD")]
		private static extern Int32 MEDAQLib_GetParameterDWORD(
			UInt32 instanceHandle, 
			String paramName, 
			ref UInt32 paramValue);
		public static ERR_CODE GetParameterDWORD(
			UInt32 instanceHandle,
			String paramName,
			ref UInt32 paramValue)
		{
			return (ERR_CODE)MEDAQLib_GetParameterDWORD(instanceHandle, paramName, ref paramValue);
		}

		[DllImport("MEDAQlib.dll", EntryPoint = "GetParameterDouble")]
		private static extern Int32 MEDAQLib_GetParameterDouble(
			UInt32 instanceHandle, 
			String paramName, 
			ref Double paramValue);
		public static ERR_CODE GetParameterDouble(
			UInt32 instanceHandle,
			String paramName,
			ref Double paramValue)
		{
			return (ERR_CODE)MEDAQLib_GetParameterDouble(instanceHandle, paramName, ref paramValue);
		}

		[DllImport("MEDAQlib.dll", EntryPoint = "GetParameterString")]
		private static extern Int32 MEDAQLib_GetParameterString(
			UInt32 instanceHandle, 
			String paramName, 
			StringBuilder paramValue, 
			ref UInt32 maxLen);
		public static ERR_CODE GetParameterString(
			UInt32 instanceHandle,
			String paramName,
			StringBuilder paramValue,
			ref UInt32 maxLen)
		{
			return (ERR_CODE)MEDAQLib_GetParameterString(instanceHandle, paramName, paramValue, ref maxLen);
		}

		[DllImport("MEDAQlib.dll", EntryPoint = "OpenSensor")]
		private static extern Int32 MEDAQLib_OpenSensor(UInt32 instanceHandle);
		public static ERR_CODE OpenSensor(UInt32 instanceHandle)
		{
			return (ERR_CODE)MEDAQLib_OpenSensor(instanceHandle);
		}

		[DllImport("MEDAQlib.dll", EntryPoint = "CloseSensor")]
		private static extern Int32 MEDAQLib_CloseSensor(UInt32 instanceHandle);
		public static ERR_CODE CloseSensor(UInt32 instanceHandle)
		{
			return (ERR_CODE)MEDAQLib_CloseSensor(instanceHandle);
		}

		[DllImport("MEDAQlib.dll", EntryPoint = "SensorCommand")]
		private static extern Int32 MEDAQLib_SensorCommand(UInt32 instanceHandle);
		public static ERR_CODE SensorCommand(UInt32 instanceHandle)
		{
			return (ERR_CODE)MEDAQLib_SensorCommand(instanceHandle);
		}

		[DllImport("MEDAQlib.dll", EntryPoint = "DataAvail")]
		private static extern Int32 MEDAQLib_DataAvail(UInt32 instanceHandle, ref Int32 avail);
		public static ERR_CODE DataAvail(UInt32 instanceHandle, ref Int32 avail)
		{
			return (ERR_CODE)MEDAQLib_DataAvail(instanceHandle, ref avail);
		}

		[DllImport("MEDAQlib.dll", EntryPoint = "TransferData")]
		private static extern Int32 MEDAQLib_TransferData(
			UInt32 instanceHandle,
			[MarshalAs(UnmanagedType.LPArray)] Int32[] rawData,
			[MarshalAs(UnmanagedType.LPArray)] Double[] scaledData,
			Int32 maxValues,
			ref Int32 read);
		public static ERR_CODE TransferData(
			UInt32 instanceHandle,
			Int32[] rawData,
			Double[] scaledData,
			Int32 maxValues,
			ref Int32 read)
		{
			return (ERR_CODE)MEDAQLib_TransferData(instanceHandle, rawData, scaledData, maxValues, ref read);
		}

		[DllImport("MEDAQlib.dll", EntryPoint = "Poll")]
		private static extern Int32 MEDAQLib_Poll(
			UInt32 instanceHandle,
			[MarshalAs(UnmanagedType.LPArray)] Int32[] rawData,
			[MarshalAs(UnmanagedType.LPArray)] Double[] scaledData,
			Int32 maxValues);
		public static ERR_CODE Poll(
			UInt32 instanceHandle,
			Int32[] rawData,
			Double[] scaledData,
			Int32 maxValues)
		{
			return (ERR_CODE)MEDAQLib_Poll(instanceHandle, rawData, scaledData, maxValues);
		}

		[DllImport("MEDAQLib.dll", EntryPoint = "GetError")]
		private static extern Int32 MEDAQLib_GetError(
			UInt32 instanceHandle, 
			StringBuilder errText, 
			UInt32 maxLen);
		public static ERR_CODE GetError(
			UInt32 instanceHandle,
			StringBuilder errText,
			UInt32 maxLen)
		{
			return (ERR_CODE)MEDAQLib_GetError(instanceHandle, errText, maxLen);
		}

        public const UInt32 WAIT_TIMEOUT = 0x00000102;
        public const UInt32 WAIT_FAILED = 0xFFFFFFFF;
        public const UInt32 WAIT_OBJECT_0 = 0x00000000;
        [DllImport("kernel32", EntryPoint="WaitForSingleObject", CharSet= CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern UInt32 WaitForSingleObject(UInt32 handle, UInt32 dwMiliseconds);
        [DllImport("kernel32", EntryPoint = "SetEvent", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SetEvent(UInt32 handle);
    }
}