   M S B A M L   `   `   `  ���� K  GMessenger Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:Messenger_ClientMessenger_Client  %    Messenger_Client.FriendsControl   W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     ,cclr-namespace:Messenger_Client     .��� .+���   �� CornerRadius$  10��
������ $��0q�$��Center=�$	��Top=�$	��192��$	��415��$��
5, 0, 0, 0q�._�� .+��� -   $��CurrentSortHolder��
������ $	��Top=�$
��Left=�$��98��$��25��$��10, 4, 0, 0q�._��� -   $��OpenSortDropdown�� � ToolTip$E ?Displays the current sorting method. Click for sorting options.��$��90��$��26��$��Center=�$	��Top=�$��0, -1, 15, 0q����� *CurrentSort+ �� RelativeSource ��  �� Mode$ FindAncestor=� �� AncestorType Control����  �� 
TargetType" ��.R���  ��Name$ 
SortCanvas��
���   �$��75��$��30�� � Triggers :�  :� RoutedEvent$ Canvas.MouseEnter��.[��� .���� .���� 	 �� 
TargetName$	 
SortCanvas��
 �� TargetProperty$1
 +(Canvas.Background).(SolidColorBrush.Color)� �� From	 Black	 �� To #3f4240 g� Duration$ 0D�:� $ Button.MouseLeave��.[��� .���� .���� $	 
SortCanvas��$1
 +(Canvas.Background).(SolidColorBrush.Color)� #3f4240	 Black$ 0D�.+��� $��58, 6, 0, 0q�$��15��$����C:\Users\evanh\Documents\Auto Network Storage\Docker Projects\MessengerProject\C# Client\Messenger Client\Images\ChevronDown28px.png���� $��10, 5, 0, 0q�$��13(�$��Center=�$��Center=�
������ $
��Bold"�
����������� *	Content+ �� $ FindAncestor=� Control� -   $��OptionsPopup��$��Bottom=� � VerticalOffset$	 -31�� � HorizontalOffset$ 0��$	��120��$	��140�� � 	StaysOpen$ False�� � PlacementTarget ��  �� ElementName$ CurrentSortHolder������ *IsSortDropdownVisible+ �� $ FindAncestor=� Control.���� -   $��SortOptionsPanel��.+��� -   $��
ActiveSort��$# Sort by most recently active.��$	��200��$��35��$
��Left=�$	��Top=�$��
0, 0, 0, 0q����� " ��.R��� $ 
UserCanvas��
������ $	��200��$��35�� :� $ Canvas.MouseEnter��.[��� .���� .���� $	 
UserCanvas��$1
 +(Canvas.Background).(SolidColorBrush.Color)� Transparent #3f4240$ 0D�:� $ Button.MouseLeave��.[��� .���� .���� $	 
UserCanvas��$1
 +(Canvas.Background).(SolidColorBrush.Color)� #3f4240 Transparent$ 0D�.+��� $��	UserBlock��$��Active��$��22, 10, 0, 0q�$��13(�$��Center=�$��Center=�
������ $
��Bold"�
��������� -   $��AlphabeticalSort��$  Sort by alphabetical order��$	��200��$��35��$
��Left=�$	��Top=����� " ��.R��� $ 
UserCanvas��
������ $	��200��$��35�� :� $ Canvas.MouseEnter��.[��� .���� .���� $	 
UserCanvas��$1
 +(Canvas.Background).(SolidColorBrush.Color)� Transparent #3f4240$ 0D�:� $ Button.MouseLeave��.[��� .���� .���� $	 
UserCanvas��$1
 +(Canvas.Background).(SolidColorBrush.Color)� #3f4240 Transparent$ 0D�.+��� $��Alphabetical��$��22, 10, 0, 0q�$��13(�$��Center=�$��Center=�
������ $
��Bold"�
��������� -   $��
RecentSort��$3 -Sort by who you've most recently engaged with��$	��200��$��35��$
��Left=�$	��Top=����� " ��.R��� $ 
UserCanvas��
������ $	��200��$��35�� :� $ Canvas.MouseEnter��.[��� .���� .���� $	 
UserCanvas��$1
 +(Canvas.Background).(SolidColorBrush.Color)� Transparent #3f4240$ 0D�:� $ Button.MouseLeave��.[��� .���� .���� $	 
UserCanvas��$1
 +(Canvas.Background).(SolidColorBrush.Color)� #3f4240 Transparent$ 0D�.+��� $��Recent��$��22, 10, 0, 0q�$��13(�$��Center=�$��Center=�
������ $
��Bold"�
��������� -   $��FavoriteSort��$, &Sort by who you interact with the most��$	��200��$��35��$
��Left=�$	��Top=����� " ��.R��� $ 
UserCanvas��
������ $	��200��$��35�� :� $ Canvas.MouseEnter��.[��� .���� .���� $	 
UserCanvas��$1
 +(Canvas.Background).(SolidColorBrush.Color)� Transparent #3f4240$ 0D�:� $ Button.MouseLeave��.[��� .���� .���� $	 
UserCanvas��$1
 +(Canvas.Background).(SolidColorBrush.Color)� #3f4240 Transparent$ 0D�.+��� $��Favorite��$��22, 10, 0, 0q�$��13(�$��Center=�$��Center=�
������ $
��Bold"�
��������� ���#000000$��
2, 2, 2, 2q�$��Right=�$	��Top=�$��26��$��90��$��0, 3.5, 10, 0q�._��� -	   $��	SearchBox��$  Search for friends by name��$��Search�� �� TextWrapping$ NoWrap=����#30302c$��
0, 0, 0, 0q�
������ 
��������� $	��350��$
��Auto=�$��10, 5, 10, 0q�.���� .+��� -
    ��Name$ FriendsDisplay������ .R��� 
���   �$��0q����#3f4240._��� -   $	��200��$��30��$
��Left=�$	��Top=����� *
UserName+
 � Tag �� *ChatID+���� " ��.R��� $ 
UserCanvas��
������ $	��200��$��30�� :� $ Canvas.MouseEnter��.[��� .���� .���� $	 
UserCanvas��$1
 +(Canvas.Background).(SolidColorBrush.Color)� Transparent Gray$ 0D�:� $ Button.MouseLeave��.[��� .���� .���� $	 
UserCanvas��$1
 +(Canvas.Background).(SolidColorBrush.Color)� Gray Transparent$ 0D�.+��� $��	UserBlock��#��z��# z $��10, 5, 0, 0q�$��14(�$��Center=�$��Center=�
������ $
��Bold"�
�������