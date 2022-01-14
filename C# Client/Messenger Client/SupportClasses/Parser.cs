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
    /// The first three characters of a transmission header MUST BE as follows:
    /// 
    /// [Index 0] - Multiple Message Indicator - If T (True), the ConnectionHandler stores the message in a transmission buffer.
    ///  When an F (False) transmission is received following a T transmission or multiple T transmissions, the
    /// buffer contents are sent to be parsed and the buffer is cleared.
    /// 
    /// [Index 1-2] - Opcode - Determines how the remainder of the message is parsed by being assigned to a bitmask.
    /// 
    /// 
    ///
    /// As such, all information in a server message __must__ be in the bitmask appropriate order with the
    /// appropriate sizes, and a field __must__ not be present if not required by the given Opcode.
    ///
    /// Padding of input strings is done using asterisks (*) at this time, using the Parser.Pack function.
    /// 
    /// The bitmask order is as follows:
    /// 00001: UserID - 32 characters - Present for all messages except login and registration. 
    /// 00010: UserName - 32 characters - Present for login, registration, and when adding a friend.
    /// 00100: Password - 128 characters - Present only for login and registration.
    /// 01000: Session ID - 32 characters - Required for all non-login and non-registration interactions. Very weakly verifies connection
    /// integrity.
    /// 10000: Chat ID - 32 characters - Identifies a single chat between one or multiple people.
    ///
    /// The final component of a received transmission, the Message, is whatever remains after the item determined by the bit mask 
    /// are parsed out.
    /// The message component is used to store either user notifications or json objects such as lists of messages and friends.
    /// 
    /// The core server opcodes with their bitmasks are:
    ///
    /// IR (Initial Registration):  00110  /  6
    /// LR (Login Request):  00110  /   6
    /// PF (Pull Friends):  01001  /   9
    /// AF (Add Friend):  01011  / 11
    /// US (User Search): 01011 / 11
    /// PC (Pull User-Chat Pairs) / 01001 / 9
    /// PM (Pull Messages From Chat):  11001    / 25
    /// SM (Send Message):  11001   /   25
    /// HB (Heartbeat): 00000 / 0

    ///
    /// The core client opcodes (What this Parser is responsible for parsing) with their bitmasks are:
    ///
    /// RU (Registration unsuccessful):  00010 / 2
    /// RS (Registration successful):  00010  / 2
    /// LU (Login unsuccessful):	 00010 / 2
    /// LS (Login successful):  01001 / 9
    /// FP (Friend Push): 01001 / 9 
    /// UR (User search Results) : 01001 / 9
    /// CP (User-Chat Pairs Push): 01001 / 9
    /// MP (Message Push for one chat): 11001 / 25
    /// CN  (Chat Notification): 11001 / 25
    /// AM (Administrative Message): 01001 / 9
    /// HB (Heartbeat): 00000 / 0
    ///</summary>

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
            OpcodeMasks.Add("FR", 9);
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
