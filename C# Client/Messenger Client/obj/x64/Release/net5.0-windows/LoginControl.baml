   M S B A M L   `   `   `  ���� K  GMessenger Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:Messenger_ClientMessenger_Client  #    Messenger_Client.LoginControl   W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     0localclr-namespace:Messenger_Client     .��� .+��� $��Center=�$��Center=�$��0, 0, 0, 170q�.+��� $��DemiBold"�
�������$��Center=�$��Center=�$X��REnter your username and password to login. If you do not have an account, you can ��$
��Wrap=��� -   $
��None?�  �� TextDecorations$
  None{�.�.��� 
����� �.�
register.�� �� $
��Left=�$��Center=�$
��Wrap=� � ToolTip$L FBetween 8-32 characters. Alphanumeric characters and underscores only.���� 
�������.�	Username:�� -   $��LoginUsername��$��4q�
�������
���   �$��Center=�$��Center=�$	��200��$��20�� �� 	MaxLength$ 32��$L FBetween 8-32 characters. Alphanumeric characters and underscores only.���� $
��Left=�$��Center=�$
��Wrap=�$ Between 8-128 characters.���� 
�������.�	Password:�� -   $��LoginPassword��$��4q�
�������
���   �$��Center=�$��Center=�$	��200��$��20��$	 128��$ Between 8-128 characters.���� $��Right=�$��Center=�$��0, 10, 30, 0q��� -   $
��None?�$
  None{�.�.��� $��LOGIN��$
��Bold"�
������ 
����� � � Triggers :�  :� RoutedEvent$ PreviewMouseDown��:� $ PreviewMouseDown��.[��� .���� .����  �� TargetProperty$4 .(TextBlock.Foreground).(SolidColorBrush.Color)� �� From DarkCyan	 �� To Red g� AutoReverse$
 true��	 g� Duration$	 0:0:0.5D��� 