   M S B A M L   `   `   `  ���� F  BcomPort_wpf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:comPort_wpfcomPort_wpf  O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089I2clr-namespace:System.Collections;assembly=mscorlibSystem.Collections )clr-namespace:comPort_wpfcomPort_wpf      comPort_wpf.MainWindow   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/netfx/2009/xaml/presentation    R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     +localclr-namespace:comPort_wpf     Bcol2clr-namespace:System.Collections;assembly=mscorlib     Aav2http://schemas.microsoft.com/expression/blend/2008     'pclr-namespace:comPort_wpf     -     �Name$  
mainWindow�� � Title$ ComPort tool��$	��300��$	��620�� � 	Resources    ColorBackground �� Color  MyTextBLockStyle �� 
TargetType �� Property �� Value  MyTextBlokStyle  MyComboBoxStyle  MyGridSplitterStyle  MyTextBoxStyle  btn_Up_ToolBar  TuggleBtnConnect �� Triggers P� Property	 P� Value  TuggleBtnDisconnect 	 Button.MouseOver.Background 
 buttonStyle
 �� Angle  Menu_TextBlock_Up_toolBar  Menu_Up_toolBar  MenuItem_Up_toolBar  myListVievStyle  Button.Static.Background  Button.Static.Border  Button.MouseOver.Border  Button.Pressed.Background  Button.Checked.Background  Button.Pressed.Border  Button.Disabled.Background  Button.Disabled.Border  Button.Disabled.Foreground  mComPort m� IsCheckable  mDataBit  
mParityBit  mStopBit��%�  &	        &	      &	 �     &	 i    &	 q    &	 �    &	 �    &	 \    &	 �    &	
 G    &	 W    &	 ,	    &	 ]
    &	 j    &	 S    &	 h    &		 }    &	 �    &	 �    &	 �    &	 �    &	 �    &	 �    &	     &	 #    &	 j    &	 �    &	 {    �� $ 	#FFB1ACAC���� " ��.���  � ��$ 12(���  � ��# �   ��  � ��
 �   ���  �@��LineStackingStrategy$ BlockLineHeight=���  � ��$ CharacterEllipsis=��� " ��.���  � ��$ 12(���  � �� �	#FFC9C2C2��  � ��
 �   ���  � ��$ 2,2,2,4q���  � ��$ Center=���  � ��$ Center=��� " ��.���  � ��$ 10(���  � ��$ Center=���  � ��$ Center=���  � ��$ Center=���  � ��$ 2,0,2,0q���  �@��
IsEditable$
 True����  �@��
IsReadOnly$
 True���� % �@��IsSynchronizedWithCurrentItem$
 TrueY��� " ��.���  � ��$ Stretch=���  �@�Cursor$ Cross����  � �� �	#FF9A9A9A�� " ��.���  � ��$ 12(���  � ��
 �   ���  � �� �	#FF0CE50C��  � ��$ NoWrap=���  �@��LineStackingStrategy$ BlockLineHeight=���  � ��$ CharacterEllipsis=��� " ��.���  � ��$
 Left=���  � ��$ 22����  � ��$	 Top=���  � ��$ 1,0,1,2q���  � ��$
 Bold"���  � ��$ Italic$���  � ��$ 12(��� " d�.���  � ��$ 20����  �@d�	IsChecked$ FalseY���  � ��$ Center=���  � ��$ 12(� P�  �@J�IsMouseOver$
	 True��. ���  � ��
 �   �P�  �@d�	IsChecked$
	 TrueY�. ���  � �� �	#FF414141��  � �� �	#FF0059FF��  � �� �	#FF08F308P�  �@d�	IsChecked$	 FalseY�. ���  � ��
 �������  � ��
 �   ��� " d�.���  � ��$ 20����  �@d�	IsChecked$ FalseY���  � ��$ Center=���  � ��$ 12(� P�  �@d�	IsChecked$
	 TrueY�. ���  � ��# � 	 ��  � �� �	#FFFF0202��  � ��
 �/���P�  �@d�	IsChecked$	 FalseY�. ���  � ��
 �������  � ��
 �   ���  � �� �	#FF340404�� " ��.���  � ��$ 12(���  � ��
 �   ���  � ��
 � � ���  � ��$ 15����  � ��$ 15����  �@J�RenderTransformOrigin$ .5,.5&� P�  �@J�IsMouseOver$
	 True��. ���  � z� �� $
 45Q���  � ��
 �   ��� " ��.���  � ��$ 12(���  � �� �	#FF9DA1A3��  � ��
 �   ���  � ��$ 3,0,0,0q� P�  �@J�IsMouseOver$
	 True��. ���  � �� �	#FF63AFC5��  � ��
 �   ��� " o�.���  � ��$ 12(���  � �� �	#FFCCCBCB��  � ��
 �   ���  � ��$ 2,0,0,0q� P�  �@J�IsMouseOver$
	 True��. ���  � �� �	#FFCCCBCB��  � ��
 �   �P�  �@o�
IsMainMenu$
	 True��. ���  � �� �	#FFCCCBCB��  � ��
 �   ��� " m�.���  � ��$ 12(���  � �� �	#FF9DA1A3��  � ��$ 22����  � ��$ 0,0,1,0q���  � ��$ 0,1,0,0q� P�  �@J�IsMouseOver$
	 True��. ���  � �� �	#FFCCCBCB��  � ��
 �   ���  �@J�Opacity$	 0.9Q��� " ��.���  � �� �	#FF0FCB36��  � �� �	#FF484F5C��  � ��
 �   ���  �@J�OpacityMask �	#FF1C1919��  � ��$ 12(���  � ��$ 
-3,-2,-3,0q���  �@��SelectionMode$ Multiple=��� $ 	#FFDDDDDD���� $ 	#FF707070���� $ 	#FFBEE6FD���� $ 	#FF3C7FB1���� $ 	#FFC4E5F6���� $ SkyBlue���� $ 	#FF2C628B���� $ 	#FFF4F4F4���� $ 	#FFADB2B5���� $ 	#FF838383���� .@�m� $
��COM1��$
 True��m� $
��COM2��$
 True��m� $
��COM3��$
 True��m� $
��COM4��$
 True��m� $
��COM5��$
 True��m� $
��COM6��$
 True��m� $
��COM7��$
 True��m� $
��COM8��$
 True��m� $
��COM9��$
 True��m� $��COM10��$
 True��m� $��COM11��$
 True��m� $��COM12��$
 True��m� $��COM13��$
 True��m� $��COM14��$
 True��m� $��COM15��$
 True��m� $��COM16��$
 True��m� $��COM17��$
 True��m� $��COM18��$
 True��m� $��COM19��$
 True��m� $��COM20��$
 True��m� $��COM21��$
 True��m� $��COM22��$
 True��m� $��COM23��$
 True��m� $��COM24��$
 True��m� $��COM25��$
 True��m� $��COM26��$
 True��m� $��COM27��$
 True��m� $��COM28��$
 True��m� $��COM29��$
 True��m� $��COM30��$
 True���� .@�m� $��5��$
 True��m� $��6��$
 True��m� $��7��$
 True��m� $��8��$
 True���� .@�m� $��нет.��$
 True��m� $��нечёт.��$
 True��m� $��чёт.��$
 True��m� $��	марк.��$
 True��m� $��пробел��$
 True���� .@�m� $��1��$
 True��m� $	��1.5��$
 True��m� $��2��$
 True��.���  � ColumnDefinitions �� $��* ��� $
��Auto ��� $��* � � RowDefinitions �� $
��Auto ��� $��30 ��� �� $��30 ��� $��25 ��� $
��Auto �.+��� $��3��$��0�� �� Orientation$ 
Horizontal=����	#FFCCCBCB$��30��.+�o� $��20��#���  .@�m� $��  Сообщения  ��#���  .@�m� $'��!Редактор ообщений���� m� $��Сообщение 1��m� $��Сообщение 2��m� $��Сообщение 3���� m� $(��"Сообщение из файла���� m� -   $(��"Выход из программы��m� $��  Вид   ��#���   m� InputGestureText$
 true��.@�m� -    m�Name$ checkSignals��$��Сигналы��$
 True�� m� 	IsChecked$
 True��m� -   $ 
checkClock��$%��Контроль времени��$
 True��$
 True��m� -   $ checkToolBar��$+��%Панель инструментов��$
 True��$
 True���� m� -   $ Tx_Data��$>��8Панель отправляемых сообщений��$
 True��$
 True��m� -   $ Rx_Data��$<��6Панель принимаемых сообщений��$
 True��$
 True���� m� $��Журнал��m� -   $��  Настройки  ��#���  m� -	   $��  CommPort   ��#���  m� -
   $�� О программе ��#���  �� -    ��Name$ SatusBarClock��$��3��$��0�����	#FFCCCBCB$��Right=�.@��� �� -    ��Name$ tBlockClock�����	#FF1DEB14
���   �$
��Bold"�$��16(�$��Stretch=�$��Stretch=�$��65�� � IsSharedSizeScope$
 True���� �� -    ��Name$ StecPanel_ToolBar��$��3��$��1��$ 
Horizontal=����	#FFB4B4B4.+�d� -    d�Name$ 	btnConect�� � ToolTip$ Открыть порт��$��3,0,2,0q�$��Connect��#���  d� -   $ btnDisConect��$ Закрыть порт��$��2,0,2,0q�$��
Disconnect��#���  �� -   $��12(�$��Center=�$# Настройка порта��$��2,0,24,0q�$��20��$��ComPort���� -   $��12(�$��Center=�$( "Выход из программы��$��2,0,52,0q�$��20��$
��Exit��� $��2��$��3�����	#FFCCC8C8 �� $��* �$	��300���� $��5 ��� $��* �$	��300�� �� $��* ��� $��20 �.+��� -    ��Name$ TerminalTXList��$��0��$��0��#���    
terminalTx#���   �� View �� .P��� $��#�� �� DisplayMemberBinding �� *Count+�� $��
Время�� �� *Time+�� $��Отправленно�� �� *HEX+�� $��ASCII�� �� *ASCII+ � ContextMenu �� .@�m� $��Копировать�� m� Command$
 Copy���� m� $��Выбрать всё...��$ Select���� m� $'��!Сохранить в файл...��$
 Save��m� $*��$Сохранить всё в HTML...���� m� $��Поиск...��$
 Find���� $��1��$��3�����	#FF8E90A4 �� ShowsPreview$
 True��$��4��$��Stretch=��� -   $ TerminalRXList��$��2��$��0��#���    
terminalRx#���   �� .P��� $��#�� �� *Count+�� $��
Время�� �� *Time+�� $��Принято�� �� *HEX+�� $��ASCII�� �� *ASCII+ �� .@�m� -   $��Копировать���� m� $��Выбрать всё...���� m� $'��!Сохранить в файл...��m� $*��$Сохранить всё в HTML...���� m� $��Поиск...��$
 True���� -   $��0��$��1��$��Очистка��$��Right=�$0 *Очистить окно передачи��$��Center=�d� -   $ 
btnStop_rx��$��2��$��1��$��Стоп Приём��$
��Left=�$W QПриостановить вывод сообщений в окно приёма��$��Center=��� -   $��2��$��1��$��Очистка��$��Right=�$, &Очистить окно приёма��$��Center=��� -   $ 	stbsender��$��30��$��0��$��3��$��3�����	#FF847E7F.@��� -    ��Name$ tBoxMessage_tx��$	��300��$�� ���� �� -    ��Name$ 
btnSend_tx��  J� SnapsToDevicePixels$
  True��$
}�True��$ Подсказка!��$��Center=�$
��Send���� � -   ! �Name$! gridToolBar��$��0��$��4��$��3��" � UseLayoutRounding$" False�����	#FF847E7F �� $��* ��� $��2* ��� $
��Auto �.+��� -   $ toolbar_sostoyanie��$��Stretch=�$
 True��# �� Padding$# 1,0,0,0q����	#FF707070
���   �$��0��$��2��]� $��2��$��0��$��25��$	��Top=�$
��Auto��$��False��$��0,0,4,0q�$
 True��.+��� -   $
 box1��$��80��#���  $ � DataContext$ �� *MyPort+% �� FallbackValue$% PortName���� -   $
 box2��$��90��#���  $ �� *MyPort+$% BaudRate���� -   $
 box3��$��80��#���  $ �� *MyPort+$% DataBits���� -    $
 box4��$	��100��#���  $ �� *MyPort+$% 
ParityBits���� -!   $
 box5��$��95��#���  $ �� *MyPort+$% StopBits���� � -"   $! gridSignals��$��0��$��5��$��3��$" False�����	#FF847E7F$��25��.+��� $ 
Horizontal=�.+��� -#   $
 Xbox��$��40���� -$   $
 Ybox��$��40��