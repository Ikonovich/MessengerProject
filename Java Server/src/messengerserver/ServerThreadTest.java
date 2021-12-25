package messengerserver;

import static org.junit.jupiter.api.Assertions.*;

import org.junit.BeforeClass;
import org.junit.jupiter.api.*;

import java.util.HashMap;

class ServerThreadTest
{
    private static ServerThread server;

    @BeforeAll
    public static void setUp() {
        server = new ServerThread();
    }

    @Test
    public void loginWithPremadeUserShouldReturnCorrectMessage() {

        String username = "testdude";
        String userpass = "testpass";

        HashMap<String, String> userMap = new HashMap<>();

        userMap.put("UserName", username);
        userMap.put("Password", userpass);

        String loginResult = server.login(userMap);

        assertEquals("LS", loginResult.substring(0, 2));
        assertEquals("testdude", loginResult.substring(loginResult.length() - 8));
    }


}