   M S B A M L   `   `   `  ����U  QWave Pathfinding Algorithm, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:Wave_Pathfinding_AlgorithmWave_Pathfinding_Algorithm  5   	   +    %Wave_Pathfinding_Algorithm.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   _ [MaterialDesignThemes.Wpf, Version=5.0.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   :local(clr-namespace:Wave_Pathfinding_Algorithm     5   	     � Title$'  !Волновой алгоритм��5
   	   $	��450��6#   $	��920��60   .��5      �  � RowDefinitions 5   
   �� 5      $��2* �6   �� 5      $	��150 �6   .+�5   
   �� $��/bin/Debug/background.png��6   $��2��63   $��UniformToFill=�6D   �� 5   
   -   5         ��Name$ AboutBtn��5      $��О программе��6#   $��Right=�69   $	��Top=�6U   6m   �� 5   
   $��10q�6   
�������$��2q�60   
������$	��120��6]    �� CornerRadius$ 50��6j   $��1��6|   ._�5   
   � .+�5      �� $	��Top=�6    �� Orientation$ Vertical=�6.   $��Center=�6F   $��10q�6c   .+�5      �� $!��Размер матрицы��6   $��40,0,0,0q�61   �� 5      $ 
Horizontal=�6   $��40,0,0,0q�66   .+�5      �� -   5         ��Name$ XSizeBox��5      $��50��60   �� 5      $��x��6    $��0,0,5,0q�6)   �� 5      -   5        $ NSizeBox��5      $��50��60   5      �� 5       -   5        $ GenerateMatrixButton��5    h      MaterialDesignToolButton#��[  6   
������$/��)Сгенерировать матрицу��6�   $��10,0,0,0q�6�   5!      �� 5"      $��10q�6   .+�5#      �� $/��)Метод распространения��6   �� 5$      -   5        $ 
OrtogonBtn��5$   o    ' #MaterialDesignRaisedSecondaryButton#��[ 6   
���/���$ ��Ортогональный��6�   {�.  6�   �� 5%      -   5        $ 	DiagonBtn��5%   p   #��[ 6   
���/���$'��!Орто-диагональный��6�   6�   5&      �� 5'      $��Center=�6   $��Center=�66   .+�5(      �� -   5        $ 
WallButton��5(   K   #��[  6   $-��'Добавить препятствие��6_   $��30��6~   $	��160��6�   6�   �� 5)      -   5        $ RemoveWallButton��5)   c    " MaterialDesignRaisedDarkButton#��[ 6   
���  ��$!��Режим удаления��6}   $��30��6�   $	��160��6�   6�   5*      �� 5+      $��Center=�6   $��Center=�66   $��10q�6Q   .+�5,      �� -	   5        $ StartBtn��5,   K   #��[  6   $!��Выбрать начало��6]   6v   �� 5-      -
   5        $ EndBtn��5-   K   #��[  6   $��Выбрать конец��6[   6s   5.      �� 50      $��Center=�6   $��Center=�66   .+�51      �� -   5        $��AnimCheckBox��51        MaterialDesignLightCheckBox#��[ 63   
������$��Анимация��6�   $��50,0,0,0q�6�    d� 	IsChecked$
 TrueY�6�   6�   �� 52      -   5        $ TraceBtn��52      $/��)Выполнить трассировку��6/   6O   �� 53      -   5        $ ResetBtn��53      $��
Сброс��6+   6;   54      56      57      �� 59   
   $��0��6   $��20,20,20,20q�6$   .��5:      �� -   5        $��matrixContentPresenter��5:      5;      5=      5>      