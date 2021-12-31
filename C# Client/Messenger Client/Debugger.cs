using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger_Client
{
    class Debugger
    {

		// Determines what is printed in the console. 
		// Bits assigned to incoming message type for this application are:
		// Bit 0 = Error message flag. - 1
		// Bit 1 = Controller handler message flag. 
		// Bit 2 = ConnectionHandler thread message flag. 
		// Bit 3 = Parser thread message flag. 
		// Bit 4 = Security-related message flag.
		// Bit 5 - Support class related messages.
		// Bit 6 - FriendControl messages.


		private static int printMask = 127;

		public static void Record(string message, int bitmask)
        {


			Debug.WriteLine(message);

			if ((printMask & bitmask) == printMask)
			{
			}

        }
    }
}
