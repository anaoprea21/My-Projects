   M S B A M L   `   `   `  ����D  @Notepad++, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:Notepad__	Notepad__  5   	       Notepad__.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:Notepad__     5   	   -   5          � Title$  	Notepad++��5	   	   $	��490��6   $	��800��6(    � DataContext 5
        Notepad__.TabModel  5   
   .��5      � -   5         �Name$	 grd��5      .+�5   
   o� $	��Top=�6   @�5      m� 5      $
��File��6   .@�5      m� $	��New��6    m� Command 6,   ��  �� Path$ 	newTabCom�m� 5      $
��Open��6    6-   �� $ openFileCom�m� 5      $
��Save��6    6-   �� $ saveFileCom�m� 5      $��
Save as...��6    63   �� $ saveAsFileCom�5      m� 5      $��Search��6   .@�5      m� $
��Find��6    6-   �� $ findCom�m� 5      $��
Replace...��6    63   �� $ 
replaceCom�m� 5      $��Replace all...��6    67   �� $ replaceAllCom�5      m� 5      $
��Help��6   .@�5      m� -   5        $��About��5      6.   5      5      �� 5    
   $��
235,20,0,0q�6   ��6)   �� *tabs+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=� �� SelectedIndex 5!      �� *index+$ TwoWay=�$ PropertyChanged=���5"      �� 5#      .R�5$      ��  �� Orientation$ 
Horizontal=�6!   .+�5%      �� ��6$   �� *header+�� 5&      $��X��6!   ��6-   �� *DataContext.removeTabCom+	 �� RelativeSource	 �� *FindAncestor+
 �� AncestorType"
 ��5'      5(       �� ContentTemplate 5*      �� 5+      .R�5,      �� $��0,0,0,0q�6    �� AcceptsReturn . ��6D   �� *	content+$ TwoWay=�$ PropertyChanged=�5-      5/      R� 50   
   -   5        $��
folderView��50   '   $��
0,22,550,0q�6    � 	Resources 51       �� 
TargetType �� Property �� Value��%  'Q�       �� 52      " Q�6   .�53      ��  � �� 54      �� 55      .R�56   "   �� $ 
Horizontal=�6-   .+�57   &   �� $��Images/folder.png��6,   $��20��6G   $��3q�6R   �� 58   &   $��Center=�60   ��6K   �� 59   #   5:      5<      5=      5>      5@      5A      5B      