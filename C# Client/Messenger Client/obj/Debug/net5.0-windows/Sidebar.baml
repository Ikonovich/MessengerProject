   M S B A M L   `   `   `  ����K  GMessenger Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:Messenger_ClientMessenger_Client  5          Messenger_Client.Sidebar   5      W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:Messenger_Client     5        � 	Resources  5	         SidebarButton �� 
TargetType �� 
TargetType �� Property �� Value �� Triggers P� Property P� Value �� Opacity	 �� Color
 �� RelativeSource �� AncestorType  DropdownButton��%�  &	        &	 �    �� 5
   
   " ��60   .R�5      � ��5      �� 5      " �6   .�5      ��  � ��
 ���� 5       5      P� 5       �@J�IsMouseOver$
 True��6=   . �5   "   ��  � �� 5   &   �� 5   *   $	 0.7Q�6:   $
	 Gray��6H   6W   5   #   5      5      .+�5      �� $��40��5      ��5      �� *Tag+
 �� *FindAncestor+" ��5      5      5      �� 5   
   " ��61   .R�5       � ��5!      �� 5"      " �6   .�5#      ��  � ��
 ���� 6M    5$      P� 5%       �@J�IsMouseOver$
 True��6=   . �5&   "   ��  � �� 5'   &   �� 5(   *   $	 0.7Q�6:   $
	 Gray��6H   6W   5*   #   5+      5-      5/      50      51      .��52      � ��53   
   �� 54      $
 0.50Q�6   $	 #393A40��6-   56       � RowDefinitions 58   
   �� 59      $
��0.4* �6   6,   �� 5:      $
��0.4* �6   6,   �� 5;      $
��0.4* �6   6,   �� 5<      $
��2.4* �6   6,   .+�5@   
   �� -   5         ��Name$ FriendsButton��5A      $��0��5@      #��[  5C      $��50��5D      $	��Top=�5E       � ToolTip$ View your friends��5F      
 � Tag$� �C:\Users\evanh\Documents\Auto Network Storage\Docker Projects\MessengerProject\C# Client\Messenger Client\Images\PersonIcon50px.png��5G      5H      �� 5J   
   -   5        $ ChatsButton��5K      $��1��5J      #��[  5M      $��50��5N      $	��Top=�5O      $# View chats you're a member of��5P      $� }C:\Users\evanh\Documents\Auto Network Storage\Docker Projects\MessengerProject\C# Client\Messenger Client\Images\Chat50px.png��5Q      5R      �� 5T   
   -   5        $ FindFriendsButton��5U      $��2��5T      #��[  5W      $��50��5X      $	��Top=�5Y      $ Find and add new friends��5Z      $� �C:\Users\evanh\Documents\Auto Network Storage\Docker Projects\MessengerProject\C# Client\Messenger Client\Images\AddFriend30px.png��5[      5\      5^      5_      