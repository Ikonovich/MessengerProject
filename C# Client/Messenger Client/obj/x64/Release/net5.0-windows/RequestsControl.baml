   M S B A M L   `   `   `  ���� K  GMessenger Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:Messenger_ClientMessenger_Client  &     Messenger_Client.RequestsControl   W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     0localclr-namespace:Messenger_Client     .��� .+��� $	��350��$
��Auto=�$��10, 0, 10, 0q�.���� .+��� -   $��DisplayMessage��$"��You have 0 requests waiting.��
�������$��Center=�$��0, 0, 0, 21.5q��� -     ��Name$  RequestsDisplay������ .R��� 
���   �$��0q����#3f4240._��� -   $	��200��$��30��$
��Left=�$	��Top=����� *Item1+
 � Tag �� *Item2+����  �� 
TargetType" ��.R���  ��Name$ 
UserCanvas��
������ $	��200��$��30�� � Triggers :�  :� RoutedEvent$ Canvas.MouseEnter��.[��� .���� .����  �� 
TargetName$ 
UserCanvas�� �� TargetProperty$1 +(Canvas.Background).(SolidColorBrush.Color)� �� From Transparent		 �� To	 Gray
 g� Duration$
 0D�:� $ Button.MouseLeave��.[��� .���� .���� $ 
UserCanvas��$1 +(Canvas.Background).(SolidColorBrush.Color)� Gray	 Transparent$
 0D�.+��� $��	UserBlock��#��z��# z $��10, 5, 0, 0q�$��14(�$��Center=�$��Center=�
������ $
��Bold"�
�������