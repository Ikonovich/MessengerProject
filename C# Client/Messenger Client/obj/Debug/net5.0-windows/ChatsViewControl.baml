   M S B A M L   `   `   `  ����K  GMessenger Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:Messenger_ClientMessenger_Client  5      '    !Messenger_Client.ChatsViewControl   5      W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:Messenger_Client     5        � 	Resources  5	         SmallImageButton �� 
TargetType �� 
TargetType �� Property �� Value �� Triggers P� Property P� Value �� Opacity	 �� Color
 �� RelativeSource �� AncestorType��%�  &	        �� 5
   
   " ��63   .R�5      � $��2q�6   ��5      �� 5      " �6   .�5      ��  � ��
 ���� 5       5      P� 5       �@J�IsMouseOver$
 True��6=   . �5   "   ��  � �� 5   &   �� 5   *   $	 0.7Q�6:   $
	 Gray��6H   6W   5   #   5      5      .+�5      �� $��20��6   ��5      �� *Tag+
 �� *FindAncestor+" ��5      5      5      .��5       �  � RowDefinitions 5!   
   �� 5"      $
��0.1* �6   6,   �� 5#      $��1* �6   6*   .+�5%   
   � $��0��6    � ColumnDefinitions 5&      �� 5'      $��1* �6#   60   �� 5(      $
��0.3* �6#   62   .+�5*      ��  K� ZIndex$ 0��6   $��0��5+      $��DemiBold"�5,      $��	Chat List��5-      
�������$��15(�5/      $��15, 8, 0, 0q�50      51      �� 52      -   5        $��NewChatButton��52      $��1��53       � ToolTip$ Start a new chat��54      #��[  55      $
��Auto��57      
 � Tag$� |C:\Users\evanh\Documents\Auto Network Storage\Docker Projects\MessengerProject\C# Client\Messenger Client\Images\Add30px.png��58      59      5:      �� 5;   
   $ 0��6   $
��Auto=�5<      $��10, 5, 10, 0q�5=      $��1��5>      .��5?      �� .+�5@      �� -   5         ��Name$ ChatsDisplay��5@      ��5A      �� 5B      .R�5C      �� 
���   �$��0q�69   ���#3f4240._�5D   "   �� -   5        $	��200��5D   )   $��30��65   $
��Left=�6�   $	��Top=�6�   ��6A   �� *
ChatName+ 6^   �� *ChatID+��5E   &   �� 5F   *   " ��6:   .R�5G   .   ��  ��Name$ 
UserCanvas��65   
������ $	��200��6b   $��30��6n    � Triggers 5H   2   :� 5I   6    :� RoutedEvent$ Canvas.MouseEnter��6C   .[�5J   :   �� .��5K   >   �� .��5L   B   ��  �� 
TargetName$ 
UserCanvas��5M   E    �� TargetProperty$1 +(Canvas.Background).(SolidColorBrush.Color)�5N   E    �� From Transparent5O   E   	 �� To Gray5P   E    g� Duration$ 0D�5Q   E   5R   ?   5S   ;   5T   7   :� 5U   6   $ Button.MouseLeave��6C   .[�5V   :   �� .��5W   >   �� .��5X   B   �� $ 
UserCanvas��5Y   E   $1 +(Canvas.Background).(SolidColorBrush.Color)�5Z   E    Gray5[   E    Transparent5\   E   $ 0D�5]   E   5^   ?   5_   ;   5`   7   .+�5b   2   �� $��ChatNameBlock��6<   $
��Wrap=�5c   <   #��z��5d   <   # z 5e   <   # z��5f   <   $��10, 5, 0, 0q�5g   <   $��14(�5h   <   $��Center=�5i   <   $��Center=�5j   <   
������ $
��Bold"�5l   <   
�������5n   3   5o   /   5p   +   5r   #   5s      5t      5v      5w      5x      � 5y   
   $��1��6   $ 1��5z      ���#333336y�5|      �� *CreateChatVisibility+
 �� *FindAncestor+" �� 5}      �� 5~      $
��0.1* �6    60   �� 5      $��0.07* �6    61   �� 5�      $
��0.3* �6    60   �� 5�      $
��0.5* �6    60   �� 5�      $	��.7* �6    6/   .+�5�      �� $��1��6   $��Creating a new chat...��5�      $��
5, 0, 0, 0q�5�      $
��Bold"�5�      
�������5�      � 5�      $��2��6   .+�5�      �� $��5q�6   $
��Left=�5�      $��Center=�67   $
��Wrap=�5�      $L FBetween 8-32 characters. Alphanumeric characters and underscores only.��5�      �� 5�      
�������.�6*   Chat Title:67   5�      �� 5�      -   5        $��ChatTitleEntry��5�      $��5, 45, 5, 0q�5�      
�������
���   �$��Stretch=�5�      $��Center=�64   $��20��5�       �� 	MaxLength$ 32��5�      $L FBetween 8-32 characters. Alphanumeric characters and underscores only.��5�      5�      � 5�      $��3��6   .+�5�      �� $��Right=�6   $��10, 10, 0, 0q�68   �� 5�      -   5        $
��None?�5�        �� TextDecorations$
 None{�6X   .�6o   .�5�      �� 
�������.�62   Cancel6:   .�� 6B   5�      5�      �� 5�      $
��Left=�6   $��10, 10, 0, 0q�67   �� 5�      -   5        $
��None?�5�       $
 None{�6R   .�6i   .�5�      �� 
�������.�62   Create6:   .�� 6B   5�      5�      5�      5�      5�      5�      