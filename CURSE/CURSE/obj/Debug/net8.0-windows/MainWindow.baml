   M S B A M L   `   `   `  ����@  <CURSE, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:CURSECURSE  5          CURSE.MainWindow   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      %localclr-namespace:CURSE     5        � Title$  
MainWindow��5   	   $	��500��6   $	��500��6)    � WindowStyle$
 None=�5	   	    � WindowStartupLocation$ CenterScreen=�5
   
    � 
ResizeMode$ NoResize=�5   	    � AllowsTransparency$
 True��5       � Icon$ Resources/Icon.png��5       � 	Resources 5       �� DataType   CURSE.LogIn � ShowGridLines	 � RowDefinitions
 � ColumnDefinitions �� RelativeSource �� AncestorType �� Orientation   CURSE.Register��%Z  (��       5      *, +)(�� L    5[      *, +)�� 5   
   "  6   .R�5      � $ false��6   	 5      �� 5      �� 5      �� 5      �� 5      �� 5      �� 5      �� 5      �� 5      �� 5      �� 5      �� 5       �� 5!      �� 5"      �� 5#      �� 5$      �� 5%      �� 5&      �� 5'      �� 5(      �� 5)      �� 5*      
 5,      �� 5-      �� 5.      �� 5/      �� 50      �� 51      �� 52      �� 53      �� 54      �� 55      �� 56      �� 57      �� 58      �� 59      �� 5:      �� 5;      �� 5<      �� 5=      �� 5>      �� 5?      �� 5@      �� 5A      .+�5G      �� $��Resources/background.png��6   $��21��6:   $��21��6O   $��0��6a   $��0��6q   �� 5I      $��7��6   $��5��6'   $��11��67   $��3��6L   $��	0,10,0,20q�6]   �� 5J      $��10��6   $��5��6(   $��11��68   $��3��6M   $��	0,10,0,20q�6^   �� 5K      $��8��6t   $��14��6�   $��5��6�   $��2��6�   ��6   �� *DataContext.Enter+ �� 
 Window.��6�   ХУЙ6�   �� 5L      -   5        $��8��5L      $��5��6,   $��16��6@   регистрация5N      �� 5P      -   5        
���**��$��21��5P   ,   $��2��6A   
���**��6n   �� 5Q      -   5        $��MyNote��5Q      
�������$��Center=�6=   $��10��6Y   $��2��6n   $��20(�6�   $��0 0 0 4 q�6�   �� 5R      $ 
Horizontal=�6   $��Right=�66   $��21��6S   $��2��6h   .+�5S      �� -   5        
���**��$��-��5S   0   $��40(�6<   $��2��6K   
���**��6g   �� 5T      -   5        
���**��$��X��5T   0   $��40(�6<   $��2��6J   
���**��6f   5U      �� 5W      $��3��6   $��7��6)   $��7��69   $��4��6M   
���   �$��60(�6q   $��15 0 0 0q�6   
Вход6�   5X      5Y      �� 5[   
   "  6   .R�5\      � $ False��6   	 5]      �� 5^      �� 5_      �� 5`      �� 5a      �� 5b      �� 5c      �� 5d      �� 5e      �� 5f      �� 5g      �� 5h      �� 5i      �� 5j      �� 5k      �� 5l      �� 5m      �� 5n      �� 5o      �� 5p      �� 5q      �� 5r      
 5t      �� 5u      �� 5v      �� 5w      �� 5x      �� 5y      �� 5z      �� 5{      �� 5|      �� 5}      �� 5~      �� 5      �� 5�      �� 5�      �� 5�      �� 5�      �� 5�      �� 5�      �� 5�      �� 5�      �� 5�      �� 5�      .+�5�      �� ���#fce584$��21��6.   $��21��6C   $��0��6U   $��0��6e   �� 5�      $��6��6   $��5��6'   $��11��67   $��3��6L   $��	0,10,0,20q�6]   �� 5�      $��9��6   $��5��6'   $��11��67   $��3��6L   $��	0,10,0,20q�6]   �� 5�      $��12��6   $��5��6(   $��11��68   $��3��6M   $��	0,10,0,20q�6^   �� 5�      $��8��6t   $��15��6�   $��5��6�   $��2��6�   ��6   �� *DataContext.Enter+ �� 
 Window.��6�   ХУЙ6�   �� 5�      $��8��6{   $��5��6�   $��2��6�   ���#fce584���#fce584$��17��6�   ��6   �� *DataContext.LogInCommand+ �� 
 Window.��6�   
вход6�   �� 5�      -   5        
���**��$��21��5�   ,   $��2��6A   6S   �� 5�      -   5        $��MyNote��5�      
�������$��Center=�6=   $��0,0,0,0q�6X   $��10��6i   6~   �� 5�      $ 
Horizontal=�6   $��Right=�66   $��Center=�6R   $��21��6m   .+�5�      �� -   5        
���**��$	��−��5�   0   $��3��6<   $��2��6L   $��20��6]   $��0,0,0,0q�6r   6�   �� 5�      -	   5        
���**��$��X��5�   0   $��30��6<   $��30��6G   6S   5�      �� 5�      $��2��6   $��2��6)   $��17��69   $��4��6N   
���   �$��60(�6r   $��15 0 0 0q�6�   Регистрация6�   5�      5�      5�      .��5�      �� ��6   �� *CurrentView+5�      