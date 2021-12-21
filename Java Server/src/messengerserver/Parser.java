package messengerserver;
import java.util.HashMap;



//All messages must contain as their first two letters an Operation Code or Opcode. This Opcode tells the server
//how to parse and handle the incoming message. Specifically, each Opcode is mapped to a bit mask, and a fall-through
//parser is used to parse each message based on this mask. 
//
//Padding of input strings is done using asterisks (*) at this time.
//
//As such, all information in a server message __must__ be in the bitmask appropriate order with the
//appropriate sizes, and a field __must__ not be present if not required by the given Opcode.
//
//The bitmask order is as follows:
//
//000001: Opcode - always 1. 
//000010: UserID - 32 characters - Present for all messages except login and registration. 
//000100: UserName - 32 characters - Present for login, registration, and when adding a friend.
//001000: Password - 128 characters - Present only for login and registration.
//010000: Session ID - 32 characters - Required for all non-login and non-registration interactions. Very weakly verifies connection
//integrity.
//100000: Chat ID - 32 characters - Identifies a single chat between one or multiple people.
//
//The final component of a received transmission, the Message, is whatever remains after the item determined by the bit mask are parsed out.
//
//The core opcodes with their bitmasks are:
//
//IR (Initial Registration):  001101  /  13
//LR (Login Request):  001101   /   13
//PF (Pull Friends):  011011  /   27
//AF (Add Friend):  010111   / 23
//PC (Pull Chat):  110011    / 51
//SM (Send Message):  110011   /   51



public class Parser {
	
	// Used to determine the parser behavior.
	
	private static final int userNameLength = 32;
	private static final int userIDLength = 32;
	private static final int passwordLength = 128;
	private static final int sessionIDLength = 32;
	private static final int chatIDLength = 8;
	
	
	
	private static HashMap<String, Integer> opcodeMap;
	
	static {
		
		opcodeMap = new HashMap<String, Integer>();
		
		opcodeMap.put("IR", 13);
		opcodeMap.put("LR", 13);
		opcodeMap.put("PF", 27);
		opcodeMap.put("AF", 23);
		opcodeMap.put("PC", 51);
		opcodeMap.put("SM", 51);
	}
	
	
	public static HashMap<String, String> parse(String input) 
	{
		
		HashMap<String, String> returnMap = new HashMap<String, String>();		
		// Gets the opcode.
		String opcode = "ER";
		String message = input;
		
		try {
			opcode = input.substring(0, 2);
			input = input.substring(2);
		}
		catch(Exception e) {
			
			returnMap.put("Opcode", "ER");
			Debugger.record("Parser failed getting opcode", 9);
			return returnMap;
		}
		
		
		// Getting the bitmask from the opcode.
		int mask = 0;
		if (opcodeMap.containsKey(opcode)) {
			mask = opcodeMap.get(opcode);
		}
		else {
			
			returnMap.put("Opcode", opcode);
			Debugger.record("Parser failed at bit 0 for opcode: " + opcode, 9);
			return returnMap;
		}

		// Begin the fall through parser here.
		
		if ((mask & 2) > 0) 
		{
			
			try {
				returnMap.put("UserID", message.substring(0, userIDLength));
				message = message.substring(userIDLength);
			}
			catch (Exception e) 
			{
				Debugger.record("Parser failed at bit 1 for opcode: " + opcode, 9);
				returnMap.put("Opcode", "ER");
				return returnMap;
			}
			
		}
		if ((mask & 4) > 0) 
		{

			try {
				returnMap.put("UserName", message.substring(0, userNameLength));
				message = message.substring(userNameLength);
			}
			catch (Exception e) 
			{
				Debugger.record("Parser failed at bit 2 for opcode: " + opcode, 9);
				returnMap.put("Opcode", "ER");
				return returnMap;
			}
			
			
		}
		if ((mask & 8) > 0) 
		{

			try {
				returnMap.put("Password", message.substring(0, passwordLength));
				message = message.substring(passwordLength);
			}
			catch (Exception e) 
			{
				Debugger.record("Parser failed at bit 3 for opcode: " + opcode, 9);
				returnMap.put("Opcode", "ER");
				return returnMap;
			}
			
		}
		if ((mask & 16) > 0) 
		{

			try {
				returnMap.put("SessionID", message.substring(0, sessionIDLength));
				message = message.substring(sessionIDLength);
			}
			catch (Exception e) 
			{
				Debugger.record("Parser failed at bit 4 for opcode: " + opcode, 9);
				returnMap.put("Opcode", "ER");
				return returnMap;
			}
			
		}
		if ((mask & 32) > 0) 
		{

			try {
				returnMap.put("ChatID", message.substring(0, chatIDLength));
				message = message.substring(chatIDLength);
			}
			catch (Exception e) 
			{
				Debugger.record("Parser failed at bit 5 for opcode: " + opcode, 9);
				returnMap.put("Opcode", "ER");
				return returnMap;
			}
			
		}
		
		
		if (message.length() > 0) 
		{
			returnMap.put("Message", message);
		}
		
		return returnMap;
	}
}