   M S B A M L   `   `   `  ����K  GMessenger Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:Messenger_ClientMessenger_Client  5      %    Messenger_Client.FriendsControl   5      W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ,cclr-namespace:Messenger_Client     5        � 	Resources  5	         StyledButton �� 
TargetType �� 
TargetType �� Property �� Value �� Triggers P� Property P� Value �� Opacity	 �� Color
 �� RelativeSource �� AncestorType��%�  &	        �� 5
   
   " ��6/   .R�5      � $��2q�6   ��5      �� 5      " �6   .�5      ��  � ��
 ���� 5       5      P� 5       �@J�IsMouseOver$
 True��6=   . �5   "   ��  � �� 5   &   �� 5   *   $	 0.7Q�6:   $
	 Gray��6H   6W   5   #   5      5      .+�5      �� $��30��6   ��5      �� *Tag+
 �� *FindAncestor+" ��5      5      5      .��5       �  � RowDefinitions 5!   
   �� 5"      $
��0.1* �6   6,   �� 5#      $��1* �6   6*   .+�5%   
   � $��0��6   .+�5&      ��  K� ZIndex$ 0��6   $��DemiBold"�5'      $��Friend List��5(      
�������$��15(�5*      $��15, 8, 0, 0q�5+      5,      � 5-   
   $��40��6   $��Right=�6   ��5.      �� 5/      " �6   .�50      ��  � ��
 ���� 6M    51      P� 52       �@J�IsMouseOver$
 True��69   . �53      ��  � �� 54   "   �� 55   &   $
	 Gray��66   $	 0.7Q�6C   56   '   58      59      5;      .+�5=      � -   5         �Name$ 
SearchArea��5=      $ 0��6'   .+�5>      �� -   5        $��SearchButton��5>       � ToolTip$ Search friends list��5?      #��[  5@      $
��Auto��5B      
 � Tag$� �C:\Users\evanh\Documents\Auto Network Storage\Docker Projects\MessengerProject\C# Client\Messenger Client\Images\SearchIcon30px.png��5C      5D      5E      5F      � 5G      -   5        $ SearchPanel��5G      $ 2��5H      
���   ���5J      ��  �� ElementName$ FriendsDisplay�� �� Path$ ActualWidth�y�5K      �� *SearchPanelVisibility+
 �� *FindAncestor+" ��.+�5L      �� ��5M      �� 5N      " ��6    .�5O      ��  � ��
 � ���6Q   �� 5P       � ��$ 
0, 1, 0, 1q�6@   6U    5Q      P� 5R   "    �@�Tag$
 True��69   . �5S   &   ��  � ��
 � ���6Y   5T   #   5V      ._�5X      �  � ColumnDefinitions 5Y      �� 5Z      $��1* �6/   6<   �� 5[      $
��0.3* �6/   6>   .+�5^      � 
���   � 5_      �� 5`      $	��.2* �6,   6<   �� 5a      $��1* �6,   6;   �� 5b      $	��.2* �6,   6<   .+�5e      �� -   5        $��	SearchBox��5h   "   $��1��5e   "   $��0��5f   "   $��15(�5g   "   $' !Start typing the name of a friend��5i   "   $��0q�5j   "   
������ 
������� �� 
CaretBrush
 �����.��5p   9   5q      5r      �� 5s      -   5        $��CancelSearchButton��5s   !   
��� ���$ Cancel search��5u      $��1��5w      #��[  5x      $� C:\Users\evanh\Documents\Auto Network Storage\Docker Projects\MessengerProject\C# Client\Messenger Client\Images\Cancel50px.png��5y      5z      5{      5|      5}      5~      �� 5   
   $��Stretch=�5�      $��Stretch=�5�      $
��Auto=�5�      $��1��5�      .��5�      �� .+�5�      �� -   5         ��Name$ FriendsDisplay��5�      ��5�      �� 5�      .R�5�      �� 
���   �$��0q�69   ���#3f4240._�5�   "   �� -   5        $��30��5�   )   $��Stretch=�5�   )   $	��Top=�5�   )   ��5�   )   �� *
UserName+ 5�   )   �� *UserID+��5�   &   �� 5�   *   " ��6:   .R�5�   .   ��  ��Name$ 
UserCanvas��65   
������ $	��200��6b   $��30��6n    � Triggers 5�   2   :� 5�   6    :� RoutedEvent$ Canvas.MouseEnter��6C   .[�5�   :   �� .��5�   >   �� .��5�   B   ��  �� 
TargetName$ 
UserCanvas��5�   E    �� TargetProperty$1 +(Canvas.Background).(SolidColorBrush.Color)�5�   E    �� From Transparent5�   E   	 �� To Gray5�   E    g� Duration$ 0D�5�   E   5�   ?   5�   ;   5�   7   :� 5�   6   $ Button.MouseLeave��6C   .[�5�   :   �� .��5�   >   �� .��5�   B   �� $ 
UserCanvas��5�   E   $1 +(Canvas.Background).(SolidColorBrush.Color)�5�   E    Gray5�   E    Transparent5�   E   $ 0D�5�   E   5�   ?   5�   ;   5�   7   .+�5�   2   �  5�   6   �� 5�   :   $��1* �6K   6X   �� 5�   :   $��1* �6K   6X   .+�5�   6   �� $��	UserBlock��6@   $��1��5�   @   #��z��5�   @   # z 5�   @   $��10, 5, 0, 0q�5�   @   $��14(�5�   @   $��Center=�5�   @   $��Center=�5�   @   
������ $
��Bold"�5�   @   
�������5�   7   �� 5�   6   $��0��6<   $��10, 5, 0, 0q�5�   <   $��20��5�   <   $
��Left=�5�   <   $����C:\Users\evanh\Documents\Auto Network Storage\Docker Projects\MessengerProject\C# Client\Messenger Client\Images\PersonIconDefault30px.png��5�   <   6�   5�   3   5�   /   5�   +   5�   #   5�      5�      5�      5�      5�      5�      5�      