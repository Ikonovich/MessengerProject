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

    /// Items being parsed out are:
    /// Receiving username: 32 chars, filled with asterisks after the final alphanumeric character.
    /// Sending username: 32 chars, filled with asterisks after the final alphanumeric character.
    /// SessionID: 32 alphanumeric chars
    /// Verification code: 16 alphanumeric chars

    /// Available receipt codes are:

    /// RS - Registration Successful - Should be accompanied by verification code and username.
    /// LS - Login Successful - Should be accompanied by verification code, sessionID, and username.
    /// LU - Login Unsuccessful - Should be accompanied by verification code, username, and an error message
    /// PR - Pull Message Request - Sent to indicate that a new message has been sent and a pull request should be made
    /// for a specific user. Accompanied by receiving username, sending username, and session ID.
    /// AM - Administrative message - A generic response code that should be accompanied by a session ID, username, and message.
    ///
    /// </summary>


    static class Parser
    {

        /// <param name="input">The input string to the parse function</param>
        /// <param name="outputDict">The dictionary holding the output of the parse function</param>
        public static bool Parse(string input, out Dictionary<string, string> outputDict)
        {
            outputDict = new Dictionary<string, string>();

            string receiptCode = "";
            string remainder = "";
            try
            {
                receiptCode = input.Substring(0, 2);
                remainder = input.Substring(2);
            }
            catch
            {
                return false;
            }

            switch(receiptCode)
            {
                case "RS":
                    return ParseRS(remainder, out outputDict);
                case "RU":
                    return ParseRU(remainder, out outputDict);
                case "LS":
                    return ParseLS(remainder, out outputDict);
                case "LU":
                    return ParseLU(remainder, out outputDict);
                case "FP":
                    return ParseFP(remainder, out outputDict);
                case "FM":
                    return ParseFM(remainder, out outputDict);
                case "AM":
                    return ParseAM(remainder, out outputDict);

                default:
                    return false;
            }

        }

        // RS - Registration Successful - Should be accompanied by verification code and username.
        /// <param name="input">The input string to the parse function</param>
        /// <param name="outputDict">The dictionary holding the output of the parse function</param>
        /// 

        private static bool ParseRS(string input, out Dictionary<string, string> outputDict)
        {
            outputDict = new Dictionary<string, string>();

            try
            {
                string receiptCode = "RS";
                string verification = input.Substring(0, 16);
                string username = input.Substring(16, 32);
                string message = input.Substring(48);

                outputDict["ReceiptCode"] = receiptCode;
                outputDict["VerificationCode"] = verification;
                outputDict["Username"] = Unpack(username);
                outputDict["Message"] = message;
            }
            catch (IndexOutOfRangeException e)
            {
                return false;
            }
            return true;
        }

        // RU - Registration Unsuccessful - Should be accompanied by verification code, username, and an error message
        /// <param name="input">The input string to the parse function</param>
        /// <param name="outputDict">The dictionary holding the output of the parse function</param>

            private static bool ParseRU(string input, out Dictionary<string, string> outputDict)
        {
            outputDict = new Dictionary<string, string>();

            try
            {
                string receiptCode = "RU";
                string verification = input.Substring(0, 16);
                string username = input.Substring(16, 32);
                string errorMessage = input.Substring(32);

                outputDict["ReceiptCode"] = receiptCode;
                outputDict["VerificationCode"] = verification;
                outputDict["Username"] = Unpack(username);
                outputDict["Message"] = errorMessage;

            }
            catch (IndexOutOfRangeException e)
            {
                return false;
            }
            return true;
        }

        // LS - Login Successful - Should be accompanied by verification code, sessionID, and username.
        /// <param name="input">The input string to the parse function</param>
        /// <param name="outputDict">The dictionary holding the output of the parse function</param>

        private static bool ParseLS(string input, out Dictionary<string, string> outputDict)
        {
            string debug = "Parsing LS message: \n" + input + " \n: End message.\n\n\n";
            Debug.WriteLine(debug);
            outputDict = new Dictionary<string, string>();
            try
            {
                string receiptCode = "LS";
                string verification = input.Substring(0, 16);
                string sessionID = input.Substring(16, 32);
                string username = input.Substring(48, 32);
                string message = input.Substring(80);

                Debug.WriteLine("\nLS Parse: Verification: " + verification + " SessionID: " + sessionID + " Username: " + username + " Message: " + message + "\n");


                outputDict["ReceiptCode"] = receiptCode;
                outputDict["SessionID"] = sessionID;
                outputDict["VerificationCode"] = verification;
                outputDict["Username"] = Unpack(username);
                outputDict["Message"] = message;

            }
            catch (IndexOutOfRangeException e)
            {
                return false;
            }
            return true;
        }


        // LU - Login Unsuccessful - Should be accompanied by verification code, username, and an error message.
        /// <param name="input">The input string to the parse function</param>
        /// <param name="outputDict">The dictionary holding the output of the parse function</param>
        private static bool ParseLU(string input, out Dictionary<string, string> outputDict)
        {
            outputDict = new Dictionary<string, string>();

            try
            {
                string receiptCode = "LU";
                string verification = input.Substring(0, 16);
                string username = input.Substring(16, 32);
                string errorMessage = input.Substring(48);


                outputDict["ReceiptCode"] = receiptCode;
                outputDict["VerificationCode"] = verification;
                outputDict["Username"] = Unpack(username);
                outputDict["Message"] = errorMessage;

            }
            catch (IndexOutOfRangeException e)
            {
                return false;
            }

            return true;
        }

        // FP - A server message indicating that an updated friends list is being sent.
        /// <param name="input">The input string to the parse function</param>
        /// <param name="outputDict">The dictionary holding the output of the parse function</param>

        private static bool ParseFP(string input, out Dictionary<string, string> outputDict)
        {
            outputDict = new Dictionary<string, string>();

            Debug.WriteLine("Parse FP Called on string: " + input + "\n");

            try
            {
                string receiptCode = "FP";

                string verification = input.Substring(0, 16);
                string username = input.Substring(16, 32);
                string sessionID = input.Substring(48, 32);
                string friends = input.Substring(80);


                outputDict["ReceiptCode"] = receiptCode;
                outputDict["VerificationCode"] = verification;
                outputDict["Username"] = Unpack(username);
                outputDict["SessionID"] = sessionID;
                outputDict["Friends"] = friends;


            }
            catch (IndexOutOfRangeException e)
            {
                return false;
            }

            return true;
        }

        // FM - A server message, responding to a Pull Message[PM] request, indicating that
        // messages are being sent.
        /// <param name="input">The input string to the parse function</param>
        /// <param name="outputDict">The dictionary holding the output of the parse function</param>
        private static bool ParseFM(string input, out Dictionary<string, string> outputDict)
        {
            outputDict = new Dictionary<string, string>();

            Debug.WriteLine("Parse FM Called.");

            try
            {
                string receiptCode = "FM";

                string verification = input.Substring(0, 16);
                string username = input.Substring(16, 32);
                string sessionID = input.Substring(48, 32);
                string friends = input.Substring(80);

                outputDict["ReceiptCode"] = receiptCode;
                outputDict["Username"] = Unpack(username);
                outputDict["SessionID"] = sessionID;
                outputDict["Friends"] = friends;

            }
            catch (IndexOutOfRangeException e)
            {
                return false;
            }

            return true;
        }

        // AM - Administrative message - A generic response code that should be accompanied by a session ID, username, and message.
        /// <param name="input">The input string to the parse function</param>
        /// <param name="outputDict">The dictionary holding the output of the parse function</param>
        private static bool ParseAM(string input, out Dictionary<string, string> outputDict)
        {
            outputDict = new Dictionary<string, string>();

            try
            {
                string receiptCode = "AM";
                string username = input.Substring(0, 16);
                string sessionID = input.Substring(16, 32);
                string errorMessage = input.Substring(48);

                outputDict["ReceiptCode"] = receiptCode;
                outputDict["Username"] = Unpack(username);
                outputDict["SessionID"] = sessionID;
                outputDict["Message"] = errorMessage;

            }
            catch (IndexOutOfRangeException e)
            {
                return false;
            }

            return true;
        }


        // This method packs a string in a special null character (currently asterisks) to make it fit the size
        // provided, allowing it to be parsed more easily.
        /// <param name="input">The string to be packed.</param>
        /// <param name="size">The desired size of the string after packing</param>
        public static string Pack(string input, int size)
        {
            for (int i = input.Length; i < size; i++)
            {
                input += "*";
            }
            return input;
        }

        // Unpacks a string that has been packed with filler characters (currently asterisks) to fit the transmission space.
        /// <param name="input">The string to be unpacked.</param>
        private static string Unpack(string packedString)
        {
            int packStart = packedString.IndexOf("*");

            string unpackedString = packedString.Substring(0, packStart);

            Debug.WriteLine("Unpacked " + packedString + " into " + unpackedString + "\n");
            return unpackedString;
        }
    }
}
