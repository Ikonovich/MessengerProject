   M S B A M L   `   `   `  ����K  GMessenger Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:Messenger_ClientMessenger_Client  5      #    Messenger_Client.LoginControl   5      W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:Messenger_Client     5      .��5	      � $	��500��6   .+�5
   
   �� $��Center=�6   $��Center=�62   $��0, 0, 0, 170q�6M   .+�5      �� $��DemiBold"�6   
�������$��Center=�6A   $��Center=�6^   $X��REnter your username and password to login. If you do not have an account, you can ��6z   $
��Wrap=�6�   �� 5      -   5        $
��None?�5        �� TextDecorations$
  None{�6R   .�6i   .�5      �� 
��� ���.�63   
register6=   .�� 6E   5      5      �� 5      $
��Left=�5      $��Center=�6,   $
��Wrap=�5       � ToolTip$L FBetween 8-32 characters. Alphanumeric characters and underscores only.��5      �� 5      
�������.�6&   	Username:61   5      �� 5      -   5        $��LoginUsername��5      
��� ���$	��0.5q�5      $��4q�5      
�������
���   �$��Center=�5      $��Center=�63   $	��200��5      $��20��6"    �� 	MaxLength$ 32��5      $L FBetween 8-32 characters. Alphanumeric characters and underscores only.��5      �� 5       $
��Left=�6   $��Center=�63   $
��Wrap=�6N   $ Between 8-128 characters.��6b   �� 5!      
�������.�6&   	Password:61   5"      �� 5#      -   5        $��LoginPassword��5(      
��� ���$	��0.5q�5$      $��4q�5%      
�������
���   �$��Center=�5)      $��Center=�63   $	��200��5*      $��20��6"   $	 128��5+      $ Between 8-128 characters.��5,      �� 5.      $��Right=�6   $��Center=�64   $��0, 10, 30, 0q�6P   �� 5/      -   5        $
��None?�5/      $
  None{�6P   .�6g   .�50      �� $��LOGIN��6    $
��Bold"�6-   
������ 
������� � Triggers 51      :� 52       :� RoutedEvent$ PreviewMouseDown��6+   54      :� 55      $ PreviewMouseDown��6+   .[�56   "   �� .��57   &   �� .��58   *   ��  �� TargetProperty$4 .(TextBlock.Foreground).(SolidColorBrush.Color)�59   -    �� From	 White5:   -   	 �� To Red5;   -    g� AutoReverse$
 true��5<   -   	 g� Duration$	 0:0:0.5D�5=   -   5?   '   5@   #   5A      �� 5B   0   5C      5D      5E      5F      5G      5H      