   M S B A M L   `   `   `  ����K  GMessenger Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:Messenger_ClientMessenger_Client  5   	   !    Messenger_Client.MainWindow   5   	   W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:Messenger_Client     5   	     � AllowsTransparency$
  True��5   	    � WindowStyle$
 None=�5	   	   
������  � Title$ 	Messenger��5   	   $	��450��6   $	��450��6(   .��5      � ��5   
   �� 5       �� Color$ #333336��6   .+�5   
   �� $	��0.4q�6   ��5      �� 5       �� 
TargetType" ��6   .�5      ��  �� Property � �� �� Value
 ���� 6J    �� Triggers 5      P� 5       P� Property �@J�IsMouseOver	 P� Value$
	 True��69   . �5      ��  � ��
 � ���6Q   5      5      ._�5      � .+�5      � 
 � RowDefinitions
 5       �� 5!      $��30 �6(   66   �� 5"      $��1* �6(   66   .+�5$      � $��0��6   
���   �.+�5&      � -   5        $��MessagePopup��5&        � 	StaysOpen$ False��6�   $	��150��6�   $	��200��6�   $��Center=�6�   ��65   �� *IsNotificationPopupOpen+ �� RelativeSource ��  �� Mode$ FindAncestor=� �� AncestorType
 Window.��5'      �� 
���   �$��7q�69   $	��150��6M   $	��200��6Z   ���#3f4240 �� CornerRadius$ 30��6{   ._�5(   "   �� $��Center=�6-   $��Center=�6J   .+�5)   &   �� -   5        $��	PopupText��5)   t   $��Center=�60   
�������$
��Bold"�6a   $	��150��6�   $
��Wrap=�6�   ��6�   �� *NotificationPopupMessage+ �� $ FindAncestor=�
 Window�� 5*   &   -   5        $��ClosePopupButton��5*   -   $��Center=�6E   $��Center=�6b   $��0, 20, 0, 0q�6}   .��5+   *   �� $��Center=�64   $��Center=�6Q   �� 5,   "   .�6'   Okay6-   5-   +   5.   '   5/   #   50      51      � 53      -   5        $��ConfirmationPopup��53       $
 True��55      $	��150��56      $	��200��57      $��Center=�58      ��54      �� *IsSelectionPopupOpen+ �� $ FindAncestor=�
 Window.��59      �� 
���   �$��7q�5:      $	��150��5;      $	��200��5<      ���#3f4240$ 30��5>      ._�5?   "   �� $��Center=�6-   $��Center=�6J   .+�5@   &   �� $��Center=�60   $��0, 0, 0, 15q�5A   $   
�������$
��Bold"�5C   $   $	��150��5D   $   $
��Wrap=�5E   $   ��5F   $   �� *SelectionPopupMessage+ �� $ FindAncestor=�
 Window5G   '   �� 5H   &   $��Center=�61   $��Center=�6N    �� Orientation$ 
Horizontal=�6i   .+�5I   *   �� -   5        $��	ButtonOne��5J   !   $��40��5K   !   $��30��5L   !   $��Right=�5P   !   $	��Top=�5Q   !   ��5M   !   �� *OptionOneText+ �� $ FindAncestor=�
 Window
 � Tag 5N   !   �� *OptionOneTag+ �� $ FindAncestor=�
 Window��5R   .   �� 5S   2    �� 
TargetType" ��6B   .R�5T   6   ��  ��Name$ YesButtonCanvas��6=   
������� � Triggers 5V   :   :� 5W   >    :� RoutedEvent$ Canvas.MouseEnter��6K   .[�5X   B   �� .��5Y   F   �� .��5Z   J   ��  �� 
TargetName$ YesButtonCanvas��5[   A    �� TargetProperty$1 +(Canvas.Background).(SolidColorBrush.Color)�5\   A    �� From Transparent5]   A   	 �� To Gray5^   A    g� Duration$ 0D�5_   A   5`   G   5a   C   5b   ?   :� 5c   >   $ Button.MouseLeave��6K   .[�5d   B   �� .��5e   F   �� .��5f   J   �� $ YesButtonCanvas��5g   =   $1 +(Canvas.Background).(SolidColorBrush.Color)�5h   =    Gray5i   =    Transparent5j   =   $ 0D�5k   =   5l   G   5m   C   5n   ?   .+�5p   :   ��  ��Name$ TextOne��6D   $��10, 5, 0, 0q�5q   8   $��14(�5r   8   $��Center=�5s   8   $��Center=�5t   8   
������ $
��Bold"�5v   8   
���������5x   8   �� *OptionOneText+ �� $ FindAncestor=�
 Window5y   ;   5z   7   5{   3   5}   +   �� 5~   *   -   5        $��	ButtonTwo��5~   1   $��40��5   %   $��30��5�   %   $��Right=�5�   %   $	��Top=�5�   %    5�   %   �� *OptionTwoTag+ �� $ FindAncestor=�
 Window��5�   .   �� 5�   2   " ��6B   .R�5�   6   �� $ NoButtonCanvas��6=   
������  5�   :   :� 5�   >   $ Canvas.MouseEnter��6K   .[�5�   B   �� .��5�   F   �� .��5�   J   �� $ NoButtonCanvas��5�   1   $1 +(Canvas.Background).(SolidColorBrush.Color)�5�   1    Transparent5�   1    Gray5�   1   $ 0D�5�   1   5�   G   5�   C   5�   ?   :� 5�   >   $ Button.MouseLeave��6K   .[�5�   B   �� .��5�   F   �� .��5�   J   �� $ NoButtonCanvas��5�   1   $1 +(Canvas.Background).(SolidColorBrush.Color)�5�   1    Gray5�   1    Transparent5�   1   $ 0D�5�   1   5�   G   5�   C   5�   ?   .+�5�   :   �� $ TextTwo��5�   1   $��10, 5, 0, 0q�5�   1   $��14(�5�   1   $��Center=�5�   1   $��Center=�5�   1   
������ $
��Bold"�5�   1   
���������5�   1   �� *OptionTwoText+ �� $ FindAncestor=�
 Window5�   ;   5�   7   5�   3   5�   +   5�   '   5�   #   5�      5�      �� 5�      $��22��6%   $	��230��62   $��
5, 7, 4, 4q�5�      $
��Left=�5�      $	��Top=�5�      .+�5�      �� -   5        $��UserNameButton��5�   %   $	��230��5�      $��22��6%   $��Center=�5�      $	��Top=�5�      $��
0, 0, 0, 0q�5�      ��5�   "   �� 5�   &   " ��66   .R�5�   *   �� $ 
NameCanvas��61   
������ $	��230��6^   $��22��6j    5�   .   :� 5�   2   $ Canvas.MouseEnter��6?   .[�5�   6   �� .��5�   :   �� .��5�   >   �� $ 
NameCanvas��5�   I   $1 +(Canvas.Background).(SolidColorBrush.Color)�5�   I    Transparent5�   I    #3f42405�   I   $ 0D�5�   I   5�   ;   5�   7   5�   3   :� 5�   2   $ Button.MouseLeave��6?   .[�5�   6   �� .��5�   :   �� .��5�   >   �� $ 
NameCanvas��5�   I   $1 +(Canvas.Background).(SolidColorBrush.Color)�5�   I    #3f42405�   I    Transparent5�   I   $ 0D�5�   I   5�   ;   5�   7   5�   3   .+�5�   .   �� $��
0, 0, 0, 0q�64   $��20��5�   (   $���}C:\Users\evanh\Documents\Auto Network Storage\Docker Projects\MessengerProject\C# Client\Messenger Client\Images\CoolIcon.png��5�   (   5�   /   �� 5�   .   $��25, 0, 0, 0q�5�   ,   $��13(�5�   ,   $��Center=�5�   ,   $��Center=�5�   ,   $
��Bold"�5�   ,   
���������5�   8   �� *DisplayText+ �� $ FindAncestor=�
 Window �� Mode$ TwoWay=�5�   /   5�   +   5�   '   5�      5�      � 5�      -   5        $��UserNamePopup��5�      $��Bottom=�5�       � VerticalOffset$	 -31��5�       � HorizontalOffset$ 0��6)   $	��120��5�      $��35��6    $ False��6,    � PlacementTarget 5�       ��   �� ElementName$  UserNameButton����5�      �� *IsUserNameDropdownVisible+ �� $ FindAncestor=�
 Window.��5�      �� -	   5        $��UserOptionsPanel��5�   )   .+�5�   "   �� -
   5        $��LogoutButton��5�   )   ! � ToolTip$! Log out of your account��5�   !   $	��200��5�   !   $��35��6-   $
��Left=�5�   !   $	��Top=�6=   $��
0, 0, 0, 0q�5�   !   ��5�   &   �� 5�   *   " ��6:   .R�5   .   �� $ 
UserCanvas��65   
������ $	��200��6b   $��35��6n    5  2   :� 5  6   $ Canvas.MouseEnter��6C   .[�5  :   �� .��5  >   �� .��5  B   �� $ 
UserCanvas��5  I   $1 +(Canvas.Background).(SolidColorBrush.Color)�5  I    Transparent5  I    #3f42405	  I   $ 0D�5
  I   5  ?   5  ;   5  7   :� 5  6   $ Button.MouseLeave��6C   .[�5  :   �� .��5  >   �� .��5  B   �� $ 
UserCanvas��5  I   $1 +(Canvas.Background).(SolidColorBrush.Color)�5  I    #3f42405  I    Transparent5  I   $ 0D�5  I   5  ?   5  ;   5  7   .+�5  2   �� $��Log Out��6<   $��22, 10, 0, 0q�6K   $��13(�6b   $��Center=�6p   $��Center=�6�   
������ $
��Bold"�6�   
�������5  3   5  /   5  +   5   #   5!     5"     �� 5$     -   5        $��RequestsButton��5$  !   $! View new requests��5&     " � Cursor$
" Hand��5(     $��300, 0, 0, 0q�5)     $��Center=�5*     $	��Top=�5+     y�5%     �� *RequestVisibility+ �� $ FindAncestor=�
 Window��5,     �� 5-  "   " ��62   .R�5.  &   �� $ RequestCanvas��6-   
������ $��30��50  !   $��30��6,    51  *   :� 52  .   $ Canvas.MouseEnter��6;   .[�53  2   �� .��54  6   �� .��55  :   �� $ RequestCanvas��56  I   $1 +(Canvas.Background).(SolidColorBrush.Color)�57  I    Transparent58  I    #3f424059  I   $ 0D�5:  I   5;  7   5<  3   5=  /   :� 5>  .   $ Button.MouseLeave��6;   .[�5?  2   �� .��5@  6   �� .��5A  :   �� $ RequestCanvas��5B  I   $1 +(Canvas.Background).(SolidColorBrush.Color)�5C  I    #3f42405D  I    Transparent5E  I   $ 0D�5F  I   5G  7   5H  3   5I  /   .+�5K  *   �� $��20��60   $��
5, 5, 0, 0q�5L  !   $���}C:\Users\evanh\Documents\Auto Network Storage\Docker Projects\MessengerProject\C# Client\Messenger Client\Images\alertRed.png��5M  !   5N  +   5O  '   5P  #   5R     �� 5U     -   5        $��30��5U  !   $��30��6,   $��X��68   $��Right=�6Z   $	��Top=�6w   ��5V     �� 5W  "   " ��62   .R�5X  &   �� $ 
ExitCanvas��6-   
������ $��30��6Z   $��30��6e    5Y  *   :� 5Z  .   $ Canvas.MouseEnter��6;   .[�5[  2   �� .��5\  6   �� .��5]  :   �� $ 
ExitCanvas��5^  1   $1 +(Canvas.Background).(SolidColorBrush.Color)�5_  1    Transparent5`  1    Gray5a  1   $ 0D�5b  1   5c  7   5d  3   5e  /   :� 5f  .   $ Button.MouseLeave��6;   .[�5g  2   �� .��5h  6   �� .��5i  :   �� $ 
ExitCanvas��5j  1   $1 +(Canvas.Background).(SolidColorBrush.Color)�5k  1    Gray5l  1    Transparent5m  1   $ 0D�5n  1   5o  7   5p  3   5q  /   .+�5s  *   �� $��10, 5, 0, 0q�64   $��14(�6J   $��Center=�6X   $��Center=�6u   
������ $
��Bold"�6�   
�������#��z��6�   5t  +   5u  '   5v  #   5x     �� 5{     -   5        $��-��5{  !   $��30��6G   $��30��6R   $��Right=�6^   $	��Top=�6{   $��0, 0, 29, 0q�6�   ��5|     �� 5}  "   " ��62   .R�5~  &   �� $ MinimizeCanvas��6-   
������ $��30��6^   $��30��6i    5  *   :� 5�  .   $ Canvas.MouseEnter��6;   .[�5�  2   �� .��5�  6   �� .��5�  :   �� $ MinimizeCanvas��5�  1   $1 +(Canvas.Background).(SolidColorBrush.Color)�5�  1    Transparent5�  1    Gray5�  1   $ 0D�5�  1   5�  7   5�  3   5�  /   :� 5�  .   $ Button.MouseLeave��6;   .[�5�  2   �� .��5�  6   �� .��5�  :   �� $ MinimizeCanvas��5�  1   $1 +(Canvas.Background).(SolidColorBrush.Color)�5�  1    Gray5�  1    Transparent5�  1   $ 0D�5�  1   5�  7   5�  3   5�  /   .+�5�  *   �� $��10, 0, 0, 0q�64   $��25(�6J   $��Center=�6X   $��Center=�6u   
������ 
�������$
��Bold"�6�   #��z��6�   5�  +   5�  '   5�  #   5�     5�     � 5�     $��1��6   $��Stretch=�6(   # � ColumnDefinitions# 5�     �� 5�     $
��0.4* �6/   �� 5�     $��1* �6/   �� 5�     $
��1.7* �6/   .+�5�     �� -   5        $��SegmentSidebar��5�  )   $��0��5�     $��Stretch=�5�     ��5�     �� *SidebarSegment+ �� $ FindAncestor=�
 Window$ TwoWay=�5�     �� 5�     -   5        $��SegmentLeft��5�  )   $��1��5�  !   $��Stretch=�5�  !   ��5�  !   �� *LeftSegment+ �� $ FindAncestor=�
 Window$ TwoWay=�5�  !   �� 5�     -   5        $��SegmentRight��5�  )   $��2��5�     $��Stretch=�5�     ��5�     �� *RightSegment+ �� $ FindAncestor=�
 Window$ TwoWay=�5�     5�     5�     5�     5�     5�     5�     