   M S B A M L   `   `   `  ����K  GПопытка 1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:Попытка_1Попытка_1  5      !    Попытка_1.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      _ [MaterialDesignThemes.Wpf, Version=4.2.1.2, Culture=neutral, PublicKeyToken=df2a72020bd7962ac _MaterialDesignThemes.MahApps, Version=0.1.9.2, Culture=neutral, PublicKeyToken=df2a72020bd7962aHmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes   5      ,localclr-namespace:Попытка_1   5        � Title$0  *Городская поликлиника.��5
      $	��450��6    $	��800��6-   $	��800��69   $	��450��6H    J� InputBindings 5      �� 5   
   
 �� Key$ Enter��5       �� Command 5      �� *Enter+ �� CommandParameter 5      ��  �� ElementName$ Password��5      .��5      � 
������� � RowDefinitions 5   
   �� 5      $��0.75* �6   6.   �� 5      $��0.75* �6   6-   �� 5      $��0.75* �6   6-   �� 5      $��1.75* �6   6-    � ColumnDefinitions 5   
   �� 5      6!   �� 5      6!   �� 5      6!   �� 5      6!   �� 5      6!   .+�5!   
   �� $��1��6   $��0��6$   $��3��61   $��Авторизация��6E   $��Center=�6X   $��Center=�6s   $��24(�6�   �� 5"   
   $��1��6   $��1��6%   $��3��62    �� Orientation$ Vertical=�6F   $��Center=�6]   $
��16,0q�6x   .+�5#      �� $ 
Horizontal=�6   .+�5$      �� $ ��Введите логин:��6   $��20(�62   $
��Left=�6@   $��Center=�6[   5%      �� 5&      $��20(�6   $��2,8,0,0q�6`   $
��Left=�6q   $	��440��6�   	 �� 
CaretBrush	 �	#FF49695A
 �� SelectionBrush
 �	#FF49695A��6$   �� *Login+ �� UpdateSourceTrigger$ PropertyChanged=�6�   5'      �� 5(   
   $��1��6   $��2��6%   $��3��62   $ Vertical=�6F   $��Center=�6]   $
��16,0q�6x   .+�5)      �� $ 
Horizontal=�6   .+�5*      �� $"��Введите пароль:��6   $��20(�63   $
��Left=�6A   $��Center=�6\   5+      G� 5,      -   5        $��Password��5,   (   $��20(�6   $��
0,8,-0.4,0q�69    G� 
CaretBrush �	#FF49695A G� SelectionBrush �	#FF49695A6d   5-      �� 5.   
   $��1��6   $��3��6%   $��3��62   .+�5/      �� $��Center=�6a   $	��Top=�6~   $��
Войти��6�   $��20(�6�   $��0,8,0,0q�6�   ���	#FFFFF0F5���	#FFFFF0F5
���   ���6   �� *Enter+��6/   �� $ Password��6�   �� 50      $��18(�6   
���  ��$��Center=�67   $��Center=�6T   $��0,4,0,0q�6o   ��51      �� 52       �� 
TargetType" ��6    �� Triggers 53      �� 54       �� Binding 6*   �� *Wrong+ �� Value 06D   .a�55   "   ��  �� Property � y� �� Value$ 	Collapsed=�6?   56      �� 57       6*   �� *Wrong+ 16D   .a�58   "   ��  � ��$K EНе найден пользователь с этим логином��69   59      �� 5:       6*   �� *Wrong+ 26D   .a�5;   "   ��  � ��$. (Пароль введен неверно��69   5<      5>      5@      5A      5C      5D      