   M S B A M L   `   `   `  ����K  GMessenger Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:Messenger_ClientMessenger_Client  5      )    #Messenger_Client.FindFriendsControl   5      W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:Messenger_Client     5        � 	Resources  5	         ImageButton �� 
TargetType �� 
TargetType �� Property �� Value �� Triggers P� Property P� Value �� Opacity	 �� Color
 �� RelativeSource �� AncestorType��%�  &	        �� 5
   
   " ��6.   .R�5      � $��2q�6   ��5      �� 5      " �6   .�5      ��  � ��
 ���� 5       5      P� 5       �@J�IsMouseOver$
 True��6=   . �5   "   ��  � �� 5   &   �� 5   *   $	 0.7Q�6:   $
	 Gray��6H   6W   5   #   5      5      .+�5      �� $��50��6   ��5      �� *Tag+
 �� *FindAncestor+" ��5      5      5!      .��5"      � .+�5%   
   ��  K� ZIndex$ 2��6   $��Center=�5&      $��Center=�5'      $	��200��5(      $��
0, 0, 0, 0q�5)      y�5*      �� *ConfirmationPopupVisibility+
 �� *FindAncestor+" ����5+      �� 5,      " ��6   .�5-      ��  � ��
 � ���6I   �� 5.       � ��$	 0.5q�68   6F   �� 5/       � �� 50      �� 51      $	 Black��6.   $ .7Q�6<   6L   53      54      ._�57      �  � RowDefinitions 58      �� 59      6&   �� 5:      6&   .+�5<      �� -   5         ��Name$ StorageBlock��5<      $��0��62   $��0��6=   6I   �� 5=      -   5        $��	PopupText��5=      $��30, 50, 30, 0q�5>      $��0��5?      $��DemiBold"�5@      $;��5Would you like to send a friend request to Fake user?��5A      $
��Wrap=�5B      
�������5C   .   �� 5D      -   5         ��Name$ ConfirmationButton��5D      #��[  5E      $��1��5G      $��30, 0, 0, 0q�5H      $��60��5I      $��60��5J       � ToolTip$ Send request��5K      $
��Left=�5L      
 � Tag$� �C:\Users\evanh\Documents\Auto Network Storage\Docker Projects\MessengerProject\C# Client\Messenger Client\Images\Checkmark50px.png��5M      6�   �� 5N      -   5        #��[  5N      $��1��5P      $��0, 0, 30, 0q�5Q      $��60��5R      $��60��5S      $ Send request��5T      $��Right=�5U      $� C:\Users\evanh\Documents\Auto Network Storage\Docker Projects\MessengerProject\C# Client\Messenger Client\Images\Cancel50px.png��5V      6�   5W      5X      �� 5[   
   
���   �$��
2, 2, 2, 2q�5\      $��Right=�5]      $	��Top=�5^      $��25��5_      $	��170��6   $��0, 0, 10, 0q�5`      ._�5a      �� -   5        $��	SearchBox��5a      $3 -Find new friends by part or all of their name��5b      $��Enter part of a username��5c       �� TextWrapping$ NoWrap=�5d      ���#30302c
�������$��
0, 0, 0, 0q�5g      
������ 5h      5i      �� 5j   
   $	��350��6   $
��Auto=�6$   $��0, 32.2, 0, 0q�6G   $	��Top=�6^   .��5k      �� -   5        $��UserResultsPanel��5k      .+�5l      �� -   5         ��Name$ UserResultsDisplay��5l      ��5m      �� 5n      .R�5o      �� 
���   �$��0q�6:   ���#3f4240._�5p   "   �� -   5        $	��200��5p   )   $��30��65   $
��Left=�6�   $	��Top=�6�   ��6A   �� *Item1+ 6[   �� *Item2+��5q   &   �� 5r   *   " ��6:   .R�5s   .   ��  ��Name$ 
UserCanvas��65   
������ $	��200��6b   $��30��6n    � Triggers 5t   2   :� 5u   6    :� RoutedEvent$ Canvas.MouseEnter��6C   .[�5v   :   �� .��5w   >   �� .��5x   B   ��  �� 
TargetName$ 
UserCanvas��5y   E    �� TargetProperty$1 +(Canvas.Background).(SolidColorBrush.Color)�5z   E    �� From Transparent5{   E   	 �� To Gray5|   E    g� Duration$ 0D�5}   E   5~   ?   5   ;   5�   7   :� 5�   6   $ Button.MouseLeave��6C   .[�5�   :   �� .��5�   >   �� .��5�   B   �� $ 
UserCanvas��5�   E   $1 +(Canvas.Background).(SolidColorBrush.Color)�5�   E    Gray5�   E    Transparent5�   E   $ 0D�5�   E   5�   ?   5�   ;   5�   7   .+�5�   2   �� $��	UserBlock��6<   #��z��6M   # z 6n   $��10, 5, 0, 0q�6�   $��14(�6�   $��Center=�6�   $��Center=�6�   
������ $
��Bold"�6�   
�������5�   3   5�   /   5�   +   5�   #   5�      5�      5�      5�      5�      5�      5�      