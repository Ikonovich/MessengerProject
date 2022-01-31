using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger_Client
{


    /// <summary> 
    /// 
    /// This class contains methods that can be used to parse data from incoming messages.
    /// The primary function returns true if the parse is successful, false otherwise, and
    /// sends output to a provided dictionary. 
    /// 
    /// See MessageDefinitions.txt for details on message structure.
    /// </summary>

    static class Parser
    {

        private static Dictionary<string, int> OpcodeMasks;

        private static int debugMask = 8;


        private const int UserNameLength = 32;
        private const int UserIDLength = 32;
        private const int PasswordLength = 128;
        private const int SessionIDLength = 32;
        private const int ChatIDLength = 8;

        static Parser()
        {
            OpcodeMasks = new Dictionary<string, int>();

            OpcodeMasks.Add("RS", 2);
            OpcodeMasks.Add("RU", 2);
            OpcodeMasks.Add("LU", 0);
            OpcodeMasks.Add("LS", 11);
            OpcodeMasks.Add("LO", 9);
            OpcodeMasks.Add("FP", 9);
            OpcodeMasks.Add("RP", 9);
            OpcodeMasks.Add("UR", 9);
            OpcodeMasks.Add("CP", 9);
            OpcodeMasks.Add("MP", 25);
            OpcodeMasks.Add("CN", 25);
            OpcodeMasks.Add("AM", 9);
            OpcodeMasks.Add("HB", 0);

        }

        /// <param name="input">The input string to the parse function</param>
        /// <param name="outputDict">The dictionary holding the output of the parse function</param>
        public static bool Parse(string input, out Dictionary<string, string> outputDict)
        {

            outputDict = new Dictionary<string, string>();

            string opcode = "";
            string remainder = "";

            try
            {
                opcode = input.Substring(0, 2);
                remainder = input.Substring(2);

                outputDict.Add("Opcode", opcode);
            }
            catch
            {
                Debugger.Record("Parser failed at getting opcode.", debugMask + 1);
                return false;
            }

            // Begin the fall through parser.


            int mask = 0;
            if (OpcodeMasks.ContainsKey(opcode))
            {
                mask = OpcodeMasks[opcode];
            }
            else
            {
                return false;
            }

            // Parse out UserID

            if ((mask & 1) > 0)
            {
                try
                {
                    string userID = Unpack(remainder.Substring(0, UserIDLength));
                    outputDict.Add("UserID", userID);
                    remainder = remainder.Substring(UserIDLength);
                }
                catch
                {
                    Debugger.Record("Parser failed at getting UserID, bit 1.", debugMask + 1);
                    return false;
                }
            }

            // Parse out UserName

            if ((mask & 2) > 0)
            {
                try
                {
                    string username = Unpack(remainder.Substring(0, UserNameLength));
                    outputDict.Add("UserName", username);
                    remainder = remainder.Substring(UserNameLength);
                }
                catch
                {
                    Debugger.Record("Parser failed at getting UserName, bit 2.", debugMask + 1);
                    return false;
                }
            }


            // Parse out Password
            if ((mask & 4) > 0)
            {
                try
                {
                    string password = Unpack(remainder.Substring(0, PasswordLength));
                    outputDict.Add("Password", password);
                    remainder = remainder.Substring(PasswordLength);
                }
                catch
                {
                    Debugger.Record("Parser failed at getting Password, bit 3.", debugMask + 1);
                    return false;
                }
            }

            // Parse out session ID.
            if ((mask & 8) == 8)
            {
                try
                {
                    string sessionID = remainder.Substring(0, SessionIDLength);
                    outputDict.Add("SessionID", sessionID);
                    remainder = remainder.Substring(SessionIDLength);
                }
                catch
                {
                    Debugger.Record("Parser failed at getting SessionID, bit 4.", debugMask + 1);
                    return false;
                }

            }

            if ((mask & 16) == 16)
            {
                try
                {
                    string chatID = Unpack(remainder.Substring(0, ChatIDLength));
                    outputDict.Add("ChatID", chatID);
                    remainder = remainder.Substring(ChatIDLength);
                }
                catch
                {
                    Debugger.Record("Parser failed at getting ChatID, bit 5.", debugMask + 1);
                    return false;
                }
            }

            // Whatever remains is the Message component of the transmission.

            outputDict.Add("Message", remainder);
            return true;
        }



        // This method packs a string in a special null character (currently asterisks) to make it fit the size
        // provided, allowing it to be parsed more easily.
        // Any strings longer than the size given will be truncated.
        /// <param name="input">The string to be packed.</param>
        /// <param name="size">The desired size of the string after packing</param>
        public static string Pack(string input, int size)
        {
            if (input.Length > size)
            {
                input = input.Substring(0, size);
            }

            for (int i = input.Length; i < size; i++)
            {
                input += "*";
            }
            return input;
        }

        /// <summary> 
        /// An overload of the Pack function that allows an integer to be packed.
        /// Relies on the default pack function to operate.
        /// </summary>  
        /// <param name="input">The int to be packed.</param>
		/// <param name="size">The size of the final packed string.</param>
        public static string Pack(int input, int size)
        {
            string inputString = input.ToString();

            return Pack(inputString, size);
        }


        // Unpacks a string that has been packed with filler characters (currently asterisks) to fit the transmission space.
        /// <param name="input">The string to be unpacked.</param>
        private static string Unpack(string packedString)
        {
            int packStart = packedString.IndexOf("*");

            if (packStart < 0)
            {
                return packedString;
            }
            string unpackedString = packedString.Substring(0, packStart);

            return unpackedString;
        }
    }
}
