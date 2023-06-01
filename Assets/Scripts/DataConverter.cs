using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
///用于数据帧与命令帧数据转换与生成的类
///</summary>
namespace MyTools
{
    public class Crc
    {

        /// <summary>
        /// 判断数据中crc是否正确
        /// </summary>
        /// <param name="datas">传入的数据后两位是crc</param>
        /// <returns></returns>
        public static bool IsCrcOK(byte[] datas)
        {
            int length = datas.Length - 2;

            byte[] bytes = new byte[length];
            Array.Copy(datas, 0, bytes, 0, length);
            byte[] getCrc = GetModbusCrc16(bytes);

            if (getCrc[0] == datas[length] && getCrc[1] == datas[length + 1])
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 传入数据添加两位crc
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public static byte[] GetCRCDatas(byte[] datas)
        {
            int length = datas.Length;
            byte[] crc16 = GetModbusCrc16(datas);
            byte[] crcDatas = new byte[length + 2];
            Array.Copy(datas, crcDatas, length);
            Array.Copy(crc16, 0, crcDatas, length, 2);
            return crcDatas;
        }
        private static byte[] GetModbusCrc16(byte[] bytes)
        {
            byte crcRegister_H = 0xFF, crcRegister_L = 0xFF;// 预置一个值为 0xFFFF 的 16 位寄存器

            byte polynomialCode_H = 0xA0, polynomialCode_L = 0x01;// 多项式码 0xA001

            for (int i = 0; i < bytes.Length; i++)
            {
                crcRegister_L = (byte)(crcRegister_L ^ bytes[i]);

                for (int j = 0; j < 8; j++)
                {
                    byte tempCRC_H = crcRegister_H;
                    byte tempCRC_L = crcRegister_L;

                    crcRegister_H = (byte)(crcRegister_H >> 1);
                    crcRegister_L = (byte)(crcRegister_L >> 1);
                    // 高位右移前最后 1 位应该是低位右移后的第 1 位：如果高位最后一位为 1 则低位右移后前面补 1
                    if ((tempCRC_H & 0x01) == 0x01)
                    {
                        crcRegister_L = (byte)(crcRegister_L | 0x80);
                    }

                    if ((tempCRC_L & 0x01) == 0x01)
                    {
                        crcRegister_H = (byte)(crcRegister_H ^ polynomialCode_H);
                        crcRegister_L = (byte)(crcRegister_L ^ polynomialCode_L);
                    }
                }
            }

            return new byte[] { crcRegister_L, crcRegister_H };
        }

    }

    public class Converter
    {
        /// <summary>
        /// 16进制字符串转字节数组
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static byte[] StringToBytes(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return new byte[0];
            }
            //将" "删除，即用""替代
            //string s = str.Replace(" ", "");
            //int count = s.Length / 2;
            int count = str.Length / 2;
            var result = new byte[count];
            for (int i = 0; i < count; i++)
            {
                var tempBytes = Byte.Parse(str.Substring(2 * i, 2), System.Globalization.NumberStyles.HexNumber);
                result[i] = tempBytes;
            }
            return result;
        }


            /// <summary>
            /// 16进制字符串转字节数组
            /// </summary>
            /// <param name="str">字符串</param>
            /// <returns></returns>
            public static byte[] StringToBytes1(string str)
            {
                if (string.IsNullOrEmpty(str))
                {
                    return new byte[0];
                }
                //将" "删除，即用""替代
                string s = str.Replace(" ", "");
                int count = s.Length / 2;
                //int count = str.Length / 2;
                var result = new byte[count];
                for (int i = 0; i < count; i++)
                {
                    var tempBytes = Byte.Parse(s.Substring(2 * i, 2), System.Globalization.NumberStyles.HexNumber);
                    result[i] = tempBytes;
                }
                return result;
            }

            /// <summary>
            /// 字节数组转16进制字符串
            /// </summary>
            /// <param name="bytes">字节数组</param>
            /// <returns></returns>
            public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }

        public static string GetHexChar(string value)
        {
            string sReturn = string.Empty;
            switch (value)
            {
                case "10":
                    sReturn = "A";
                    break;
                case "11":
                    sReturn = "B";
                    break;
                case "12":
                    sReturn = "C";
                    break;
                case "13":
                    sReturn = "D";
                    break;
                case "14":
                    sReturn = "E";
                    break;
                case "15":
                    sReturn = "F";
                    break;
                default:
                    sReturn = value;
                    break;
            }
            return sReturn;
        }

        /// <summary>
        /// 字符串转换为16进制字符串
        /// </summary>
        /// <param name="value">字符串</param>
        /// <returns>16进制字符串</returns>
        public static string ConvertHex(string value)
        {
            string sReturn = string.Empty;
            try
            {

                while (ulong.Parse(value) >= 16)
                {
                    ulong v = ulong.Parse(value);
                    sReturn = GetHexChar((v % 16).ToString()) + sReturn;
                    value = Math.Floor(Convert.ToDouble(v / 16)).ToString();
                }
                sReturn = GetHexChar(value) + sReturn;
                for (int i = 0; i < 2 - sReturn.Length; i++)
                {
                    sReturn = "0" + sReturn;
                }

            }
            catch
            {
                sReturn = "###Valid Value!###";
            }
            return sReturn;
        }
    }
}
