   M S B A M L   `   `   `  ���� K  GMessenger Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:Messenger_ClientMessenger_Client  !    Messenger_Client.MainWindow   W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     0localclr-namespace:Messenger_Client       � AllowsTransparency$
  True�� � WindowStyle$
 None=�
������  � Title$ 	Messenger��$	��450��$	��450��.��� ����  �� Color$ #333333��.+��� -   $��WindowBorder��
���   �$��0, 30, 0, 5q�
������ � -   $��MessagePopup�� � 	StaysOpen$ False��$	��150��$	��200��$��Center=����� *IsNotificationPopupOpen+ �� RelativeSource ��  �� Mode$ FindAncestor=� �� AncestorType
 Window.���� 
���   �$��7q�$	��150��$	��200�����#3f4240 �� CornerRadius$ 30��._��� $��Center=�$��Center=�.+��� -   $��	PopupText��$��Center=�
�������$
��Bold"�$	��150��$
��Wrap=����� *NotificationPopupMessage+ �� $ FindAncestor=�
 Window�� -   $��ClosePopupButton��$��Center=�$��Center=�$��0, 20, 0, 0q�.���� $��Center=�$��Center=��� .�Okay� -   $��ConfirmationPopup��$
 True��$	��150��$	��200��$��Center=����� *IsSelectionPopupOpen+ �� $ FindAncestor=�
 Window.���� 
���   �$��7q�$	��150��$	��200�����#3f4240$ 30��._��� $��Center=�$��Center=�.+��� $��Center=�$��0, 0, 0, 15q�
�������$
��Bold"�$	��150��$
��Wrap=����� *SelectionPopupMessage+ �� $ FindAncestor=�
 Window�� $��Center=�$��Center=�	 �� Orientation$	 
Horizontal=�.+��� -   $��	ButtonOne��$��40��$��30��$��Right=�$	��Top=����� *OptionOneText+ �� $ FindAncestor=�
 Window

 � Tag
 �� *OptionOneTag+ �� $ FindAncestor=�
 Window����  �� 
TargetType" ��.R���  ��Name$ YesButtonCanvas��
������� � Triggers :�  :� RoutedEvent$ Canvas.MouseEnter��.[��� .���� .����  �� 
TargetName$ YesButtonCanvas�� �� TargetProperty$1 +(Canvas.Background).(SolidColorBrush.Color)� �� From Transparent	 �� To Gray g� Duration$ 0D�:� $ Button.MouseLeave��.[��� .���� .���� $ YesButtonCanvas��$1 +(Canvas.Background).(SolidColorBrush.Color)� Gray Transparent$ 0D�.+���  ��Name$ TextOne��$��10, 5, 0, 0q�$��14(�$��Center=�$��Center=�
������ $
��Bold"�
����������� *OptionOneText+ �� $ FindAncestor=�
 Window�� -   $��	ButtonTwo��$��40��$��30��$��Right=�$	��Top=�
 �� *OptionTwoTag+ �� $ FindAncestor=�
 Window���� " ��.R��� $ NoButtonCanvas��
������  :� $ Canvas.MouseEnter��.[��� .���� .���� $ NoButtonCanvas��$1 +(Canvas.Background).(SolidColorBrush.Color)� Transparent Gray$ 0D�:� $ Button.MouseLeave��.[��� .���� .���� $ NoButtonCanvas��$1 +(Canvas.Background).(SolidColorBrush.Color)� Gray Transparent$ 0D�.+��� $ TextTwo��$��10, 5, 0, 0q�$��14(�$��Center=�$��Center=�
������ $
��Bold"�
����������� *OptionTwoText+ �� $ FindAncestor=�
 Window�� $��22��$	��230��$��
5, 7, 4, 4q�$
��Left=�$	��Top=�.+��� -   $��UserNameButton��$	��230��$��22��$��Center=�$	��Top=�$��
0, 0, 0, 0q����� " ��.R��� $ 
NameCanvas��
������ $	��230��$��22�� :� $ Canvas.MouseEnter��.[��� .���� .���� $ 
NameCanvas��$1 +(Canvas.Background).(SolidColorBrush.Color)� Transparent #3f4240$ 0D�:� $ Button.MouseLeave��.[��� .���� .���� $ 
NameCanvas��$1 +(Canvas.Background).(SolidColorBrush.Color)� #3f4240 Transparent$ 0D�.+��� $��
0, 0, 0, 0q�$��20��$���}C:\Users\evanh\Documents\Auto Network Storage\Docker Projects\MessengerProject\C# Client\Messenger Client\Images\CoolIcon.png���� $��25, 0, 0, 0q�$��13(�$��Center=�$��Center=�$
��Bold"�
����������� *DisplayText+ �� $ FindAncestor=�
 Window �� Mode$ TwoWay=�� -	   $��UserNamePopup��$��Bottom=� � VerticalOffset$	 -31�� � HorizontalOffset$ 0��$	��120��$��35��$ False�� � PlacementTarget ��  �� ElementName$ UserNameButton������ *IsUserNameDropdownVisible+ �� $ FindAncestor=�
 Window.���� -
   $��UserOptionsPanel��.+��� -   $��LogoutButton�� � ToolTip$ Log out of your account��$	��200��$��35��$
��Left=�$	��Top=�$��
0, 0, 0, 0q����� " ��.R��� $ 
UserCanvas��
������ $	��200��$��35�� :� $ Canvas.MouseEnter��.[��� .���� .���� $ 
UserCanvas��$1 +(Canvas.Background).(SolidColorBrush.Color)� Transparent #3f4240$ 0D�:� $ Button.MouseLeave��.[��� .���� .���� $ 
UserCanvas��$1 +(Canvas.Background).(SolidColorBrush.Color)� #3f4240 Transparent$ 0D�.+��� $��Log Out��$��22, 10, 0, 0q�$��13(�$��Center=�$��Center=�
������ $
��Bold"�
��������� -   $��RequestsButton��$ View new requests�� � Cursor$
 Hand��$��300, 0, 0, 0q�$��Center=�$	��Top=�y��� *RequestVisibility+ �� $ FindAncestor=�
 Window���� " ��.R��� $ RequestCanvas��
������ $��30��$��30�� :� $ Canvas.MouseEnter��.[��� .���� .���� $ RequestCanvas��$1 +(Canvas.Background).(SolidColorBrush.Color)� Transparent #3f4240$ 0D�:� $ Button.MouseLeave��.[��� .���� .���� $ RequestCanvas��$1 +(Canvas.Background).(SolidColorBrush.Color)� #3f4240 Transparent$ 0D�.+��� $��20��$��
5, 5, 0, 0q�$���}C:\Users\evanh\Documents\Auto Network Storage\Docker Projects\MessengerProject\C# Client\Messenger Client\Images\alertRed.png���� -   $��30��$��30��$��X��$��Right=�$	��Top=����� " ��.R��� $ 
ExitCanvas��
������ $��30��$��30�� :� $ Canvas.MouseEnter��.[��� .���� .���� $ 
ExitCanvas��$1 +(Canvas.Background).(SolidColorBrush.Color)� Transparent Gray$ 0D�:� $ Button.MouseLeave��.[��� .���� .���� $ 
ExitCanvas��$1 +(Canvas.Background).(SolidColorBrush.Color)� Gray Transparent$ 0D�.+��� $��10, 5, 0, 0q�$��14(�$��Center=�$��Center=�
������ $
��Bold"�
�������#��z���� -   $��-��$��30��$��30��$��Right=�$	��Top=�$��0, 0, 29, 0q����� " ��.R��� $ MinimizeCanvas��
������ $��30��$��30�� :� $ Canvas.MouseEnter��.[��� .���� .���� $ MinimizeCanvas��$1 +(Canvas.Background).(SolidColorBrush.Color)� Transparent Gray$ 0D�:� $ Button.MouseLeave��.[��� .���� .���� $ MinimizeCanvas��$1 +(Canvas.Background).(SolidColorBrush.Color)� Gray Transparent$ 0D�.+��� $��10, 0, 0, 0q�$��25(�$��Center=�$��Center=�
������ 
�������$
��Bold"�#��z���� -   $��LeftView��$��0, 30, 0, 0q�$
��Left=�$	��190��.+��� -   $��CurrentViewHolder��
���   �$	��120��$��30��$��0, 10, 0, 10q�y��� *IsCurrentViewVisible+ �� $ FindAncestor=�
 Window._��� -   $	��120��$��35��$��Center=�$	��Top=�$��
0, 0, 0, 0q����� " ��.R��� $ 
UserCanvas��
������ $	��200��$��35�� :� $ Canvas.MouseEnter��.[��� .���� .���� $ 
UserCanvas��$1 +(Canvas.Background).(SolidColorBrush.Color)� Transparent #3f4240$ 0D�:� $ Button.MouseLeave��.[��� .���� .���� $ 
UserCanvas��$1 +(Canvas.Background).(SolidColorBrush.Color)� #3f4240 Transparent$ 0D�.+��� $��95, 6, 0, 0q�$��18��$����C:\Users\evanh\Documents\Auto Network Storage\Docker Projects\MessengerProject\C# Client\Messenger Client\Images\ChevronDown28px.png���� $��18, 5, 0, 0q�$��13(�$��Center=�$��Center=�
������ $
��Bold"�
����������� *CurrentLeftView+ �� $ FindAncestor=�
 Window� -   $��OptionsPopup��$��Bottom=�$	 -31��$ 0��$	��120��$	��105��$ False�� �� $ CurrentViewHolder������ *IsOptionsDropdownVisible+ �� $ FindAncestor=�
 Window.���� -   $��ViewOptionsPanel��.+��� -   $��FriendsListView��$$ Display a list of your friends��$	��200��$��35��$
��Left=�$	��Top=�$��
0, 0, 0, 0q����� " ��.R��� $ 
UserCanvas��
������ $	��200��$��35�� :� $ Canvas.MouseEnter��.[��� .���� .���� $ 
UserCanvas��$1 +(Canvas.Background).(SolidColorBrush.Color)� Transparent #3f4240$ 0D�:� $ Button.MouseLeave��.[��� .���� .���� $ 
UserCanvas��$1 +(Canvas.Background).(SolidColorBrush.Color)� #3f4240 Transparent$ 0D�.+��� $��	UserBlock��$��Friends List��$��22, 10, 0, 0q�$��13(�$��Center=�$��Center=�
������ $
��Bold"�
��������� -   $��	ChatsView��$	��200��$��35��$
��Left=�$	��Top=����� " ��.R��� $ 
UserCanvas��
������ $	��200��$��35�� :� $ Canvas.MouseEnter��.[��� .���� .���� $ 
UserCanvas��$1 +(Canvas.Background).(SolidColorBrush.Color)� Transparent #3f4240$ 0D�:� $ Button.MouseLeave��.[��� .���� .���� $ 
UserCanvas��$1 +(Canvas.Background).(SolidColorBrush.Color)� #3f4240 Transparent$ 0D�.+��� $��	UserBlock��$��Chats��$��22, 10, 0, 0q�$��13(�$��Center=�$��Center=�
������ $
��Bold"�
��������� -   $��FindFriendsView��$ Search for friends��$	��200��$��35��$
��Left=�$	��Top=����� " ��.R��� $ 
UserCanvas��
������ $	��200��$��35�� :� $ Canvas.MouseEnter��.[��� .���� .���� $ 
UserCanvas��$1 +(Canvas.Background).(SolidColorBrush.Color)� Transparent #3f4240$ 0D�:� $ Button.MouseLeave��.[��� .���� .���� $ 
UserCanvas��$1 +(Canvas.Background).(SolidColorBrush.Color)� #3f4240 Transparent$ 0D�.+��� $��	UserBlock��$��Find Friends��$��22, 10, 0, 0q�$��13(�$��Center=�$��Center=�
������ $
��Bold"�
��������� -   $��SegmentLeft��$	��194��$	��450��$��Bottom=�$
��Left=����� *LeftSegment+ �� $ FindAncestor=�
 Window$ TwoWay=��� -   $��SegmentRight��$	��251��$	��450��$��Bottom=�$��0,0,5,0q����� *RightSegment+ �� $ FindAncestor=�
 Window$ TwoWay=����� *RightSegmentAlignment+ �� $ FindAncestor=�
 Window$ TwoWay=�