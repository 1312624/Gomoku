   M S B A M L   `   `   `  ����   GomokuOnline    Gomoku.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     �Name$  myWin��$	��700��$
��1000�� � WindowStartupLocation$ CenterScreen=� � Icon$ Icon/icon.png��.��]� ��. .+��� $	��Top=�$	��100��$��40(�
���   �
�������$+��% Gomoku - Five in a row - multiplayer�� �� TextAlignment$ Justify=�$
��Wrap=�$��0,20,0,0q�� $��Right=�$��Right=�$	��350��$��0,20,0,0q�.+��� .+�� .+��� $��Your name: ��$��18(�$
��Bold"�
���   �$��38��$	��105���� $��30��$	��160�� �� CornerRadius$ 4��$��1q�
�������._��� -    ��Name$ tbxName��$��28�� �� TextAlignment$
 Left=�$��18(��� $��10,0,0,0q�$��32��$��70��$��1q�
�������$ 4��._��� -    ��Name$ btnName��$��30��$��Start!��$��18(��� $ 4��$��40��$��0,10,0,0q�
����  �$��2q�._��� $��Chat/History��$��18(�
����ح��� 
����  �$��2q�$ 4��._��� -    ��Name$ 
ScrollChat����. $
��Auto=�.���� -   	 ��Name$	 	ViewPanel��$	��270���� 
����  �$��2q�$ 4��._�� ���	#FFEAE5EA.+��� $ 4��$	��200��$��
20,10,0,10q�$��27��$��2q�
�������._��� -   $ tbxChat��$	��200��$��28��$��18(�$��0q�$
 Left=��� $��28��$��2q����	#FFFFB900$ 4��._��� -   $ btnSend��$��60�����	#FFFFB900$
��Send��
�������$��0q�$��18(�
 J� IsHitTestVisible
 . $��0,-2,0,0q�(�  (� Stretch$ Uniform=�$��	0,10,0,10q�.���� 
���   �$��4q�._�� -    �Name$ 
ChessBroad��$	��600��$	��600�� � RowDefinitions �� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* � � ColumnDefinitions �� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* �