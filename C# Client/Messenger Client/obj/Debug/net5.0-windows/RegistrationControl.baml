   M S B A M L   `   `   `  ����K  GMessenger Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:Messenger_ClientMessenger_Client  5      *    $Messenger_Client.RegistrationControl   5      W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:Messenger_Client     5      .��5	      � .+�5
   
   �� $��Center=�6   $	��Top=�62   $��0, 50, 0, 0q�6J   .+�5      �� $��DemiBold"�6   
�������$
��Left=�5      $��Center=�5      $
��Wrap=�5      86Please fill out the following information to register.�� 5   D   *(If you already have an account, you can �� 5      -   5        $
��None?�5        �� TextDecorations$
  None{�6K   .�6b   .�5      �� 
��� ���.�63   login6:   .�� 6A   5      5      �� 5      $��	Username:��6   $
��Bold"�5      
�������$
��Left=�5      $��Center=�5      $
��Wrap=�5       � ToolTip$L FBetween 8-32 characters. Alphanumeric characters and underscores only.��5      5      �� 5      -   5        $��RegistrationUsername��5"      
��� ���$	��0.5q�5      $��4q�5       $L FBetween 8-32 characters. Alphanumeric characters and underscores only.��5!      
�������
���   �$��Center=�5%      $��Center=�5&      $	��200��5'      $��20��6"    �� 	MaxLength$ 32��5(      �� 5)      $��	Password:��6   $
��Bold"�5*      
�������$
��Left=�5,      $��Center=�5-      $
��Wrap=�5.      $ Between 8-128 characters.��5/      50      �� 51      -   5        $��RegistrationPasswordOne��55      
��� ���$	��0.5q�52      $��4q�53      $ Between 8-128 characters.��54      
�������
���   �$��Center=�58      $��Center=�59      $	��200��5:      $��20��6"   $	 128��5;      �� 5<      $��Reenter Password:��6   $
��Bold"�5=      
�������$
��Left=�5?      $��Center=�5@      $
��Wrap=�5A      $: 4Between 8-128 characters. Must match Password field.��5B      5C      �� 5D      -   5        $��RegistrationPasswordTwo��5H      
��� ���$	��0.5q�5E      $��4q�5F      $: 4Between 8-128 characters. Must match Password field.��5G      
�������
���   �$��Center=�5K      $��Center=�5L      $	��200��5M      $��20��6#   $	 128��5N      �� 5O      $��Right=�6   $��Center=�5P      $��0, 10, 30, 0q�5Q      �� 5R      -   5        $
��None?�5R      $
  None{�6Z   .�6q   .�5S      �� $��REGISTER��6   $
��Bold"�6/   
������ 
������� � Triggers 5T      :� 5U       :� RoutedEvent$ PreviewMouseDown��6+   .[�5V   "   �� .��5W   &   �� .��5X   *   ��  �� TargetProperty$4 .(TextBlock.Foreground).(SolidColorBrush.Color)�5Y   -    �� From	 White5Z   -   	 �� To Red5[   -    g� AutoReverse$
 true��5\   -   	 g� Duration$	 0:0:0.5D�5]   -   5_   '   5`   #   5a      �� 5b   0   5c      5d      5e      5f      5g      5h      