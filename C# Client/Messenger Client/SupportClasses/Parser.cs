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
    /// All messages must contain as their first two letters an Operation Code or Opcode. This Opcode tells the server
    /// how to parse and handle the incoming message. Specifically, each Opcode is mapped to a bit mask, and a fall-through
    /// parser is used to parse each message based on this mask. 
    ///
    /// Padding of input strings is done using asterisks (*) at this time.
    ///
    /// As such, all information in a server message __must__ be in the bitmask appropriate order with the
    /// appropriate sizes, and a field __must__ not be present if not required by the given Opcode.
    ///
    /// The bitmask order is as follows:
    ///
    /// 000001: Opcode - always 1. Determines how the remainder of the message is parsed by being assigned to a bitmask.
    /// 000010: UserID - 32 characters - Present for all messages except login and registration. 
    /// 000100: UserName - 32 characters - Present for login, registration, and when adding a friend.
    /// 001000: Password - 128 characters - Present only for login and registration.
    /// 010000: Session ID - 32 characters - Required for all non-login and non-registration interactions. Very weakly verifies connection
    /// integrity.
    /// 100000: Chat ID - 32 characters - Identifies a single chat between one or multiple people.
    ///
    /// The final component of a received transmission, the Message, is whatever remains after the item determined by the bit mask are parsed out.
    /// The message component is used
    /// 
    /// The core server opcodes with their bitmasks are:
    ///
    /// IR (Initial Registration):  001101  /  13
    /// LR (Login Request):  001101   /   13
    /// PF (Pull Friends):  011011  /   27
    /// AF (Add Friend):  010111   / 23
    /// PC (Pull User-Chat Pairs) / 010011 / 19
    /// PM (Pull Messages From Chat):  110011    / 51
    /// SM (Send Message):  110011   /   51
    ///
    /// The core client opcodes (What this Parser is responsible for parsing) with their bitmasks are:
    ///
    /// RU (Registration unsuccessful):  000101 / 5
    /// RS (Registration successful):  000101 / 5
    /// LU (Login unsuccessful):	 000101 / 5
    /// LS (Login successful):  010111 / 7
    /// FP (Friend Push): 010011 / 19
    /// CP (User-Chat Pairs Push): 010011 / 19
    /// MP (Message Push for one chat): 110011 / 51
    /// AM (Administrative Message): 010011 / 19
    ///</summary>

    static class Parser
    {

        private static Dictionary<string, int> OpcodeMasks;

        private static int debugMask = 8;

        static Parser()
        {
            OpcodeMasks = new Dictionary<string, int>();

            OpcodeMasks.Add("RS", 5);
            OpcodeMasks.Add("RU", 5);
            OpcodeMasks.Add("LU", 1);
            OpcodeMasks.Add("LS", 23);
            OpcodeMasks.Add("FP", 19);
            OpcodeMasks.Add("CP", 19);
            OpcodeMasks.Add("MP", 51);
            OpcodeMasks.Add("AM", 19);

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

            if ((mask & 2) == 2)
            {
                try
                {
                    string userID = Unpack(remainder.Substring(0, 32));
                    outputDict.Add("UserID", userID);
                    remainder = remainder.Substring(32);
                }
                catch
                {
                    Debugger.Record("Parser failed at getting UserID, bit 1.", debugMask + 1);
                    return false;
                }
            }

            // Parse out UserName

            if ((mask & 4) == 4)
            {
                try
                {
                    string username = Unpack(remainder.Substring(0, 32));
                    outputDict.Add("UserName", username);
                    remainder = remainder.Substring(32);
                }
                catch
                {
                    Debugger.Record("Parser failed at getting UserName, bit 2.", debugMask + 1);
                    return false;
                }
            }


            // Parse out Password
            if ((mask & 8) == 8)
            {
                try
                {
                    string password = Unpack(remainder.Substring(0, 32));
                    outputDict.Add("Password", password);
                    remainder = remainder.Substring(32);
                }
                catch
                {
                    Debugger.Record("Parser failed at getting Password, bit 3.", debugMask + 1);
                    return false;
                }
            }

            // Parse out session ID.
            if ((mask & 16) == 16)
            {
                try
                {
                    string sessionID = remainder.Substring(0, 32);
                    outputDict.Add("SessionID", sessionID);
                    remainder = remainder.Substring(32);
                }
                catch
                {
                    Debugger.Record("Parser failed at getting SessionID, bit 4.", debugMask + 1);
                    return false;
                }

            }

            if ((mask & 32) == 32)
            {
                try
                {
                    string chatID = Unpack(remainder.Substring(0, 32));
                    outputDict.Add("ChatID", chatID);
                    remainder = remainder.Substring(32);
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

            Debug.WriteLine("Unpacking " + packedString);


            if (packStart < 0)
            {
                return packedString;
            }
            string unpackedString = packedString.Substring(0, packStart);

            return unpackedString;
        }
    }
}
